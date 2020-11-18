using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using SouthernAbstractConverter.Model;

namespace SouthernAbstractConverter.Service

{
    partial class SADataService
    {
        
        public static string accessDB =
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\SoutherAbs_data\SouthernTaxSaleData64\SouthernTaxSaleData64.accdb;Persist Security Info=False;";



        public static IEnumerable<string> FixPossiblyErronousNewLines(string filepath)
        {
            var Replacements = ApplicationConfiguration.PreProcesorChecks.Select(s => s.Split(new[] {"::"}, StringSplitOptions.RemoveEmptyEntries));
            var block = new HashSet<string>();
            var BlockTest = File.ReadLines(filepath).Aggregate(new HashSet<HashSet<string>>(),
                (myout, Line) =>
                {
                    //add all lines to a "block" as long as they arent empty
                    //hashtables are used to avoid duplicats
                    if (Line.Trim() == string.Empty)
                    {
                        myout.Add(block);
                        block = new HashSet<string>();
                    }

                    block.Add(Line);
                    return myout;
                }).Append(block).Select(blockm =>
            {
                //apply the preprocessor replacements to the joined block of lines 
                return Replacements
                    .Aggregate(string.Join("\n", blockm), (s, s1) => s.Replace(s1[0], s1[1]));
                //remove empty lines and make sure all new lines are clrf
            }).Where(s => s != string.Empty).Select(s => s.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n"));
            
            //var Output = BlockTest.Select(s => string.Join("\n\n", s)).Aggregate((a,b) => a + $"\n{b}\n").Replace("\n\n\n","\n\n").Replace("\n\n\n\n","\n");
            //var Output = BlockTest.Select(s => s.Replace("\n\n\n", "\n\n").Replace("\n\n\n\n", "\n"));
            //Output.Replace("\n\n\n","\n")
            return BlockTest;
        }

        public static void UpdateDatabaseString(string DatabasePath)
        {
            accessDB = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={DatabasePath};Persist Security Info=False;";
        }
        public string ConvertText(string sourceFile,Action<SpecialStructs.progressData>outputcallback)
        {
            string textRows = "";
            if (ThreadGlobals.ShouldPreprocess)
            {
                File.WriteAllLines(sourceFile,FixPossiblyErronousNewLines(sourceFile).ToList());
            }
            
            Regex reSale = new Regex("sale", RegexOptions.IgnoreCase);

            if (!File.Exists(sourceFile))
            {
                //outputcallback($"{sourceFile} not found");
                //Console.WriteLine($"{sourceFile} not found");
                return textRows;
            }

            var totalLines = File.ReadLines(sourceFile).Count();
            using (StreamReader rdr = File.OpenText(sourceFile))
            {
                int i = 1;
                string textRow = string.Empty;

                while ((textRow = rdr.ReadLine()) != null)
                {
                    if (ThreadGlobals.ShouldCancel)
                    {
                        return null;
                    }

                    if (textRow.Length > 0)
                    {
                        var i1 = i;

                        void Callback(string outme)
                        {
                            outputcallback(new SpecialStructs.progressData
                            {
                                progressValue = i1, TotalValue = totalLines,
                                UserData = $"Status From Line {i1} of file {sourceFile}: {outme} "
                            });
                        }

                        ProcessRowData(textRow, Callback);
                        outputcallback(new SpecialStructs.progressData
                            {progressValue = i, TotalValue = totalLines, UserData = ""});
                        //outputcallback(new frmConverter.progressData{ progressValue = i,TotalValue = totalLines,UserData = $"Parsed Line {i} of file {sourceFile}"});
                    }


                    //textRows += textRow;

                    //outputcallback(textRow);
                    Debug.WriteLine(textRow);
                    // txtBoxOutput.Text = textRow;

                    i++;
                }
            }


            return textRows;
        }

        private void ProcessRowData(string textRow,Action<string> callback)
        {
            //string[] rowArray = textRow.Split('\t');

            //List<Model.PropertyDetail> propertyDetail = new List<Model.PropertyDetail>();
            Model.PropertyDetail propertyDtls = new Model.PropertyDetail();
            try
            {
                if (textRow.Substring(0, 1) != "\t" && textRow.Trim().Length > 0 && DataExtraction.CheckIfHeaderLine(textRow) )
                {
                    HeaderRow(textRow, callback);
                }
                else if(textRow.Trim().Length > 0 && !DataExtraction.CheckIfHeaderLine(textRow))
                {
                    PropertyInfo(textRow);
                    EventData(textRow,callback);
                    PropertyDetails(textRow, propertyDtls,callback);
                }
            }
            catch (Exception e)
            {
                callback(e.ToString());
                //throw;
            }
        }

        private static void PropertyInfo(string textRow)
        {
            var mstring = string.Join("\t", SpecialStructs.Globals.rowArray);
            Model.Property property = new Model.Property();
            //Set Property Columns default values (sometimes these fields don't have a value)
            property.LotDesc = " ";
            property.OtherInfo = " ";
            property.SQDesc = " ";
            property.TSYear = 0;

            //Set the Subdivision ID for the Property Subdivision ID
            property.SubdivisionID = SpecialStructs.Globals.globalSubdivisionID;

            //  string TSyear = property.TSYear.ToString();
            
            property.LotDesc = DataExtraction.GetLotInfo(mstring);

            if (SpecialStructs.Globals.rowArray.Count > 2)
            {
                if (SpecialStructs.Globals.EventAbb == "BASE")
                {
                    //Expecting TSYear but got BASE so move to Other Info
                    property.OtherInfo = SpecialStructs.Globals.rowArray[2];
                }
                else
                {
                    property.TSYear = DataExtraction.ExtractTaxSaleYear(mstring);
                }
            }

            property.OtherInfo = DataExtraction.GetPlotInfo(mstring, property.TSYear, property.LotDesc);

            property.SQDesc = DataExtraction.GetSquareData(mstring);


            //  propertyDetail.Add("Shawn");
            //Insert Property
            //int propertyID = InsertProperty(subdivisionID, TSyear, property.LotDesc, property.OtherInfo, property.SQDesc ); = InsertProperty(subdivisionID, TSyear, property.LotDesc, property.OtherInfo, property.SQDesc );
            int propertyID = DataInsertion.InsertProperty(property);

            //Set the PropertyID to a global variable so it can be accessed by the PropertyDetails
            SpecialStructs.Globals.globalPropertyID = propertyID;
        }
        private static void HeaderRow(string textRow,Action<string> callback)
        {
            //parent row
            SpecialStructs.Globals.rowArray = textRow.Split('\t').ToList();
            SpecialStructs.Globals.EventAbb = DataExtraction.ExtractEventAbb(textRow);
            Model.Subdivision subdivision = new Model.Subdivision();
            //Model.Client client = new Model.Client();
            //Model.EventType eventType = new Model.EventType();
            //Model.Property property = new Model.Property();
            
            //Rows in file that do not begin with TAB is the Subdivision Name
            subdivision.SubdivisionName = DataExtraction.ExtractSubdivisionName(textRow);

            //Insert Subdivision Name
            SpecialStructs.Globals.globalSubdivisionID = DataInsertion.InsertSubdivision(subdivision.SubdivisionName);
        }

        private static void EventData(string textRow, Action<string> callback)
        {
            var fixedRow = DataExtraction.textRowWithoutNotes(textRow.Trim());
            var fixedRowLower = fixedRow.ToLower();
            var DateVal = DateTime.MinValue;
            var EventName = "";
            if (fixedRowLower.StartsWith(EventTypes.Quitclaim))
            {
                HandleQuitClain(callback, fixedRow, EventName, ref DateVal);
            } else if(fixedRowLower.StartsWith(EventTypes.FinalCertif))
            {
                EventName = "Final Certificate";
                var SimplifiedRow = fixedRow.Replace(EventName + ",", "");
                SimplifiedRow = SimplifiedRow.Replace(EventTypes.FinalCertif, "");
                DateVal = DataExtraction.ExtractDateField(SimplifiedRow, callback);
            }
            else if(fixedRowLower.StartsWith(EventTypes.SEEBits))
            {
                EventName = "SEE:";
                var SimplifiedRow = fixedRow.Replace(EventName, "");
                DataExtraction.GetNotes(SimplifiedRow).Split(new string[]{"::"},StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(SpecialStructs.Globals.globalNotes.Add);
            }
            else if (fixedRowLower.StartsWith(EventTypes.NOTEBits))
            {
                EventName = "NOTE:";
                var SimplifiedRow = fixedRow.Replace(EventName, "");
                SpecialStructs.Globals.globalNotes.Add(SimplifiedRow);
            }
            else
            {
                EventName = DataExtraction.textRowWithoutNotes(fixedRow);
                EventName = EventName.Split(',')[0].Trim();
                var TEventName = ApplicationConfiguration.KnownValidEvents.Find(item =>
                    Utils.TryToFixSpelling(item, EventName));
                if (TEventName != null)
                {
                    EventName = TEventName;
                }

                if (EventName.Length == 1)
                {
                    EventName = "";
                }
                
                var SimplifiedRow = fixedRow.Replace(EventName + ",", "");
                //tax like options
                if (ApplicationConfiguration.TaxLikeOption.Any(s=> s == EventName))
                {
                    var clients = DataExtraction.ExtractClientsTax(SimplifiedRow);
                    if (clients.Count == 2)
                    {
                        SpecialStructs.Globals.globalDeptor = clients[0];
                        SpecialStructs.Globals.globalPurchaserID = DataInsertion.InsertPurchaser(clients[1]);
                    }
                    else if( clients.Count == 1)
                    {
                        SpecialStructs.Globals.globalDeptor = "";
                        SpecialStructs.Globals.globalPurchaserID = DataInsertion.InsertPurchaser(clients[0]);
                        callback($"The code was unable to find both clients please check the line for missing `to` also please check the tblPurchaser table id {SpecialStructs.Globals.globalPurchaserID} for what was extracted for manual fixing");
                    }
                    else
                    {
                        callback("The code was unable to find both clients please check the line for missing `to`");
                    }
                }
                if(!ApplicationConfiguration.SpecialNonHeaderLines.Any(textRow.Contains))
                {
                    DateVal = DataExtraction.ExtractDateField(SimplifiedRow, callback);
                }

                if (fixedRowLower.StartsWith(EventTypes.REDEMPT))
                {
                    EventName = "NO REDEMPTION FOUND REGISTERED.";
                }
            }
            SpecialStructs.Globals.globalEventID = DataInsertion.InsertEventType(new EventType {EventAbb = SpecialStructs.Globals.EventAbb, EventTypeName = EventName});
            SpecialStructs.Globals.globalDate = DateVal;
        }

        private static void HandleQuitClain(Action<string> callback, string fixedRow, string EventName, ref DateTime DateVal)
        {
            var EventSplit = fixedRow.IndexOf("by");
            var CleanedRow = "";
            if (EventSplit != -1)
            {
                EventName = fixedRow.Substring(0, EventSplit).Trim();
                CleanedRow = fixedRow.Replace(EventName + " by", "");
            }
            else
            {
                EventSplit = fixedRow.IndexOf("to");
                EventName = fixedRow.Substring(0, EventSplit);
                CleanedRow = fixedRow.Replace(EventName," ");
            }

            var clients = DataExtraction.BaseClientExtractor(CleanedRow);
            SpecialStructs.Globals.globalDeptor = clients[0];
            var ClientTwo = clients[1];
            ClientTwo = ClientTwo.Substring(0, DataExtraction.FindAbbMonthOrNumber(ClientTwo)).Trim().TrimEnd(',');
            SpecialStructs.Globals.globalPurchaserID = DataInsertion.InsertPurchaser(ClientTwo);
            DateVal = DataExtraction.ExtractDateField(clients[1].Replace(ClientTwo, ""), callback);
            Console.WriteLine();
        }

        private static void PropertyDetails(string textRow, PropertyDetail propertyDtls, Action<string> callback)
        {
            if (ThreadGlobals.ShouldCancel)
            {
                return;
            }

            var fixedRow = textRow.Trim();
            var LinewithoutNotes = DataExtraction.textRowWithoutNotes(fixedRow);
            //************************************************************
                //child row Property Details
                //add child row to last parent
                Model.SubdivisionEvent subdivision2Event = new Model.SubdivisionEvent();
                string[] rowArray = textRow.Split(',');

                //Handle Transaction Date
                propertyDtls.TranxDate = SpecialStructs.Globals.globalDate;
                //Handle Event Year
                if (DataExtraction.GetEventYear(LinewithoutNotes,out var myvar))
                {
                    propertyDtls.EventYear = myvar;
                }
                else
                {
                    propertyDtls.EventYear = propertyDtls.TranxDate.Year;
                }
                //Set Property Columns default values (sometimes these fields don't have a value)
                propertyDtls.PropertyID = 3;
                propertyDtls.EventTypeID = SpecialStructs.Globals.globalEventID;
                
                propertyDtls.Debtor = SpecialStructs.Globals.globalDeptor;
                propertyDtls.PurchaserID = SpecialStructs.Globals.globalPurchaserID;
                
                propertyDtls.COB = DataExtraction.GetCOB(LinewithoutNotes,callback);
                propertyDtls.Notes = string.Join(" ",SpecialStructs.Globals.globalNotes.Distinct());
                SpecialStructs.Globals.globalNotes = new List<string>();
                //Set the subdivisionID to a global variable to be used in the Property.SubdivisionID
                if (SpecialStructs.Globals.globalPropertyID != 0)
                {
                    //Set the Subdivision ID for the Property Subdivision ID
                    propertyDtls.PropertyID = SpecialStructs.Globals.globalPropertyID;
                }
                else
                {
                    //Set the Subdivision ID for the Property Subdivision ID
                    propertyDtls.PropertyID = SpecialStructs.Globals.globalPropertyID;
                }

                if (rowArray.Length > 0)
                {
                    /*subdivision2Event.EventType = rowArray[0].Trim();
                                        propertyDtls.EventTypeID = 0; //will event type be added later?
                                        propertyDtls.EventYear = rowArray[2].Trim();
                                        propertyDtls.Debtor = rowArray[3].Trim();
                                        propertyDtls.PurchaserID = 0;//will this be added later?
                                        propertyDtls.TranxDate = rowArray[5].Trim();
                                        propertyDtls.COB = rowArray[6].Trim();
                                        propertyDtls.Notes = rowArray[7].Trim();*/
                }

                /*if (reSale.IsMatch(subdivision2Event.EventType))
                                    {
                                        if (rowArray.Length > 1)
                                        {
                                            subdivision2Event.Description = rowArray[1].Trim();
                                        }

                                        if (rowArray.Length > 2)
                                        {
                                            subdivision2Event.EventYear = rowArray[2];
                                        }
                                    }
                                    else
                                    {

                                    }*/
                int propertyDtlsID = DataInsertion.InsertPropertyDetail(propertyDtls);
                // subdivisionEvent.Last().Event.Add(subdivisionEvent);
        }
    }
}