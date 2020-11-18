using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace SouthernAbstractConverter.Service
{
    public class DataExtraction
    {
        private static readonly Dictionary<string, Regex> Regexes = new Dictionary<string, Regex>()
        {
            //Event abbreviation
            {
                "EventAbb", new Regex(
                    "(TS-RED|TS-CANC|TS-CONF|TS-h|TS-([0-9]+ Y.?)|TS-([0-9]+Y.?)|TS-SEE|TS-PAT|TS-|-RED|-CANC|-CONF|-h|-SEE|-PAT|BASE(?!.*BASE))",
                    RegexOptions.Multiline)
            },
            //Subdivision Name
            {"SubDivName", new Regex(@"^((?!TS-|-RED|-CANC|-CONF|-h|-SEE|-PAT|BASE)[A-Za-z0-9\.\-\(\)_\/\&\#\, ]+ ?)+")},
            //Tax location used to extract index
            {
                "TaxLocation",
                new Regex(
                    "[0-9][0-9][0-9][0-9] t|[0-9][0-9][0-9] [0-9] t|[0-9][0-9][0-9] t")
            },
            //Year of TaxSale (1933, 2008)etc
            {"TaxSaleYear", new Regex("(\t[0-9]{4}|\t[0-9]{4} | [0-9]{4}\t|[0-9]{4})([^a-zA-Z0-9]|)*")},
            {
                "PlotUnit",
                new Regex(
                    @"(?!\d{4})?((?!SQ|SQ\.|BASE|\bLOTS?\b)[A-Z0-9-/'()\.\&\#\|\% ]*)?\(?(\bPLOTS?\b|\bUNITS?|\bALL ?\b|\bPARCELS?\b|\bSQUARES?\b|PTN\.|PT\.|AREA|PORTIONS?|\bVARIOUS\b|SEC\.?|\bSECTION\b|RESUB)\)? ?((?!SQ|SQ\.|BASE)[A-Z0-9-/'()\.\&\#\|\% ]*)?\t?\t?((?!SQ|SQ\.|BASE)[A-Z0-9-/'()\.\&\#\|\% ])*")
            },
            {
                "SQuareData",
                new Regex(@"(\bSQ\b|\bSQUARES?\b$)([A-Z0-9-/'()\.\&\#\|\% ]*)?([A-Z0-9-/'()\.\&\#\|\% ]*|\bTRACT\b)?")
            },
            {"LotsInfo", new Regex(@"(\bLOTS?\b) ?([A-Z0-9\-\/\(\) &\.'])*\t?(\([A-Z0-9 \.]*\))?")},
            {"TestParentheses",new Regex(@"\(((?>[^()]+|\((?<n>)|\)(?<-n>))+(?(n)(?!)))\)")}
        };

        public static bool CheckIfHeaderLine(string textRow)
        {
            var CheckAbbrev = Regexes["EventAbb"].IsMatch(textRow);
            var CheckCaptialsAvg =   (double) textRow.Count(char.IsUpper)/(double) textRow.Length;
            var NumberOfCapsOver50percent = CheckCaptialsAvg > 0.5;
            var CheckForSpecialLines = ApplicationConfiguration.SpecialNonHeaderLines.Any(textRow.Contains);
            var Results = (CheckAbbrev || (NumberOfCapsOver50percent && !CheckForSpecialLines));
            return Results;
        }

        public static string ExtractEventAbb(string textRow)
        {
            return Regexes["EventAbb"].Match(textRow).Value;
        }

        public static string ExtractSubdivisionName(string textRow)
        {
            var matches = Regexes["SubDivName"].Matches(textRow).Cast<Match>().Select(element => element.Value);
            var match = string.Join(" ",matches);
            if (match.Contains("TS-"))
            {
                match = match.Substring(0, match.IndexOf("TS-"));
            }
            if (match.Contains("-RED"))
            {
                match = match.Substring(0, match.IndexOf("-RED"));
            }

            return match;
        }
        //this is the base code used to extract the deptor and purchaser it searches for the last location of " to " find where to split the deptor and purchaser
        //However if that fails it falls back to finding the first to or to. or t. in essence handing typos.
        public static List<string> BaseClientExtractor(string textrow)
        {
            var mainbody = textrow.Trim();
            var closingParen = textrow.IndexOf(") to", StringComparison.Ordinal);
            var clients = new List<string>();
            var LastOfIndex = textrow.LastIndexOf(" to ");
            if (LastOfIndex != -1)
            {
                var firstClient = textrow.Substring(0, LastOfIndex);
                var SecondClient = textrow.Substring(LastOfIndex).Replace("to ","");
                return new List<string>() {firstClient, SecondClient};
            }

            if (closingParen >= 0)
            {
                clients = Regex.Split(textrow, @"\) to").ToList();
                clients[0] += ")";
            }
            else
            {
                clients = mainbody.Split(new[] {" to "}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(mstr => mstr.Trim()).ToList();
            }

            if (clients.Count <= 1)
            {
                clients = mainbody.Split(new[] {" to. "}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(mstr => mstr.Trim()).ToList();
            }
            if (clients.Count <= 1)
            {
                clients = mainbody.Split(new[] {" t "}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(mstr => mstr.Trim()).ToList();
            }
            return clients;
        }
        //tax sale specific parts of the client extraction code
        public static List<string> ExtractClientsTax(string textrow)
        {
            var taxBody = string.Join("::", BaseClientExtractor(textrow));
            taxBody = taxBody.Trim().Replace("Tax Sale,", "").Replace("Tax Sale ,", "").Trim();
            var closingParen = textrow.IndexOf(") to", StringComparison.Ordinal);
            var taxIndex = Regexes["TaxLocation"].Match(taxBody).Index;
            if (taxIndex != 0)
            {
                taxBody = taxBody.Substring(0, taxIndex).TrimEnd(' ', ',');
            }
            else
            {
                //Use the month abbreviation to find the beginning of the date string
                var MonthIndex = FindAbbMonthOrNumber(taxBody);
                if (MonthIndex != -1)
                {
                     taxBody = taxBody.Substring(0,MonthIndex).Trim().TrimEnd(',');
                }
            }
            return taxBody.Split(new[] {"::"}, StringSplitOptions.RemoveEmptyEntries).ToList();
        }

        public static int ExtractTaxSaleYear(string textrow)
        {
            var TaxYearString = Regexes["TaxSaleYear"].Match(textrow).Value;
            var TaxYaearStringInts = string.Join("", TaxYearString.Trim().Where(char.IsDigit).ToArray());
            if (int.TryParse(TaxYaearStringInts, out var myint))
            {
                return myint;
            }

            return 0;
        }

        public static int GetCOBLocation(string textrow)
        {
            var incobLoc = -1;
            var ComparisonInt = ApplicationConfiguration.CobValues.FindIndex(textrow.Contains);
            if (ComparisonInt == -1) return incobLoc;
            incobLoc = textrow.IndexOf(ApplicationConfiguration.CobValues[ComparisonInt], StringComparison.Ordinal);
            return incobLoc;
        }
        public static DateTime ExtractDateField(string textrow, Action<string> callback)

        {
            var mydate = DateTime.MinValue;
            if (ApplicationConfiguration.SpecialNonHeaderLines.Any(textrow.Contains))
            {
                return mydate;
            }
            try
            {
                if (DateTimeExtraction(textrow, out var extractDateField)) return extractDateField;
            }
            catch (Exception e)
            {
                callback($"No Date Found or incorrectly formatted :{e} ");
                return DateTime.MinValue;
            }

            callback($"No Date Found or incorrectly formatted");
            return DateTime.MinValue;
        }

        private static bool DateTimeExtraction(string textrow, out DateTime extractDateField)
        {
            DateTime mydate;
            var DateSubstring = textrow.Trim().TrimStart(',');
            var DateFixes= ApplicationConfiguration.DateFixes.Select(s => s.Split(new[]{"::"},StringSplitOptions.RemoveEmptyEntries));
            DateSubstring = DateFixes.Aggregate(DateSubstring, (current, note) => current.Replace(note[0], note[1])).Trim().TrimEnd(',');
            var incobLoc = GetCOBLocation(DateSubstring);
            //special handling for Tax Sale
            if (Regexes["TaxLocation"].IsMatch(DateSubstring) && !DateSubstring.Contains("taxes)"))
            {
                var taxStr = Regexes["TaxLocation"].Match(DateSubstring).Value;
                var taxIndex = DateSubstring.IndexOf(taxStr, StringComparison.Ordinal);
                if (incobLoc == -1 || taxIndex == -1)
                {
                    Console.WriteLine();
                    {
                        extractDateField = DateTime.MinValue;
                        return false;
                    }
                }
                //extract the date by using location of the taxes string as the beginning and the location of the in COB string as the end
                // 1998 taxes, Sept. 13, 2001, in COB
                // ^                           ^
                // Visual example of the prefered locations of taxIndex and incob index
                DateSubstring = DateSubstring.Substring(taxIndex, (incobLoc - taxIndex)).Replace(taxStr, "").Trim()
                    .TrimStart(' ', ',').TrimEnd(',');
                if (DateTime.TryParse(DateSubstring, out mydate))
                {
                    {
                        extractDateField = mydate;
                        return true;
                    }
                }
            }

            if (incobLoc == -1)
            {
                Console.WriteLine();
                {
                    extractDateField = DateTime.MinValue;
                    return false;
                }
            }

            if (DateSubstring.Length > incobLoc)
            {
                //remove COB and everything past it to get a smaller string and also fix september while we are at it.
                var substr = DateSubstring.Substring(0, incobLoc);
                if (DateTime.TryParse(substr, out mydate))
                {
                    {
                        extractDateField = mydate;
                        return true;
                    }
                }

                DateSubstring = substr;
            }
            
            if (DateTime.TryParse(DateSubstring, out mydate))
            {
                {
                    extractDateField = mydate;
                    return true;
                }
            }

            //Use the month abbreviation to find the beginning of the date string
            var MonthIndex = FindAbbMonthOrNumber(DateSubstring);
            if (MonthIndex != -1)
            {
                var DateStr = DateSubstring.Substring(MonthIndex);
                if (DateTime.TryParse(DateStr, out mydate))
                {
                    {
                        extractDateField = mydate;
                        return true;
                    }
                }


                if (DateTime.TryParseExact(DateStr, ApplicationConfiguration.ExtraDateFormatStrings.ToArray(), new CultureInfo("en-US"),
                    DateTimeStyles.AllowWhiteSpaces, out mydate))
                {
                    {
                        extractDateField = mydate;
                        return true;
                    }
                }

                //fix date string where L was used instead of 1
                DateStr = DateStr.Replace("l,", "1,");
                if (DateTime.TryParseExact(DateStr, ApplicationConfiguration.ExtraDateFormatStrings.ToArray(),
                    new CultureInfo("en-US"), DateTimeStyles.AllowWhiteSpaces, out mydate))
                {
                    {
                        extractDateField = mydate;
                        return true;
                    }
                }

                DateStr = DateStr.Substring(0, DateStr.LastIndexOf(","));
                //try removing the last instance of a comma to trim the date down to size.
                if (DateTime.TryParse(DateStr, out mydate))
                {
                    {
                        extractDateField = mydate;
                        return true;
                    }
                }
            }

            extractDateField = DateTime.MinValue;
            return false;
        }

        public static int FindAbbMonthOrNumber(string StringToCheck)
        {
            var FoundMonth = DateTimeFormatInfo.CurrentInfo?.AbbreviatedMonthNames
                .Where(S => StringToCheck.Contains(S) && S != "").ToList();
            if (FoundMonth.Count() == 1)
            {
                var MonthIndex = StringToCheck.IndexOf(FoundMonth[0], StringComparison.Ordinal);
                return MonthIndex;
            }
            //handle dates with /
            if (StringToCheck.Contains("/"))
            {
                return StringToCheck.IndexOf(",");
            }
            return -1;
        }

        public static string GetNotes(string textRow)
        {
            var matches = Regexes["TestParentheses"].Matches(textRow).Cast<Match>().Select(S => S.Value);
            return string.Join("::",matches);
        }

        public static string textRowWithoutNotes(string textRow)
        {
            var fixedRow = textRow.Trim();
            var Notes = GetNotes(fixedRow).Split(new[]{"::"},StringSplitOptions.RemoveEmptyEntries);

            fixedRow = Notes.Aggregate(fixedRow, (current, note) => current.Replace(note, ""));
            var OpenParan = fixedRow.IndexOf("(");
            if (OpenParan != -1)
            {
                var UnfinishedNote = fixedRow.Substring(OpenParan);
                fixedRow = fixedRow.Replace(UnfinishedNote, "");
                Notes = Notes.Append(UnfinishedNote).ToArray();
            }
            Notes.ToList().ForEach(SpecialStructs.Globals.globalNotes.Add);
            return fixedRow;
        }
        public static string GetCOB(string textRow,Action<string> callback)
        {
            var cobLoc = GetCOBLocation(textRow);
            if (cobLoc != -1)
            {
                return textRow.Substring(cobLoc)
                    .Replace("in","")
                    .Replace("COB","");
            }

            if (ApplicationConfiguration.SpecialNonHeaderLines.Any(textRow.Contains))
            {
                return "";
            }
            callback("COB not found or misidentified");
            return "";
        }

        public static bool GetEventYear(string textRow, out int myint)
        {
            return int.TryParse(
                string.Join("",
                    DataExtraction.Regexes["TaxLocation"].Match(textRow).Value.Trim().Where(char.IsDigit).ToArray()), out myint);
        }

        public static string GetSquareData(string textRow)
        {
            if (DataExtraction.Regexes["SQuareData"].IsMatch(textRow))
            {
                return  DataExtraction.Regexes["SQuareData"].Match(textRow).Value;
            }

            return "";
        }

        public static string GetLotInfo(string textRow)
        {
            if (DataExtraction.Regexes["LotsInfo"].IsMatch(textRow))
            {
                return DataExtraction.Regexes["LotsInfo"].Match(textRow).Value;
            }

            return "";
        }

        public static string GetPlotInfo(string textRow,int TSYear,string LotDesc)
        {
            var OtherInfo = "";
            if (Regexes["PlotUnit"].IsMatch(textRow))
            {
                if (TSYear != 0)
                {
                    OtherInfo = Regexes["PlotUnit"].Match(textRow).Value.Replace(TSYear.ToString(),"");
                }
                else
                {
                    OtherInfo = Regexes["PlotUnit"].Match(textRow).Value;
                }

                if (LotDesc != "")
                {
                    OtherInfo = OtherInfo.Replace(LotDesc, "");
                }
            }

            return OtherInfo;
        }
    }
}