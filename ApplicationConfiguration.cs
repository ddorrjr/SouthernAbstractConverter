using System;
using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;

namespace SouthernAbstractConverter
{
    public class ApplicationConfiguration
    {
        public static List<string> CobValues = new List<string>();
        public static List<string> ExtraDateFormatStrings = new List<string>();
        public static List<string> SpecialNonHeaderLines = new List<string>();
        public static List<string> DateFixes = new List<string>();
        public static List<string> PreProcesorChecks = new List<string>();
        public static List<string> TaxLikeOption = new List<string>();
        public static List<string> KnownValidEvents = new List<string>();
        public static void Datas(string filePath)
        {
            var fileData = File.ReadAllText(filePath);
            var fileBuffer = Encoding.UTF8.GetBytes(fileData);
            using (var jsonReader = JsonReaderWriterFactory.CreateJsonReader(fileBuffer, new System.Xml.XmlDictionaryReaderQuotas()))
            {
                // For that you will need to add reference to System.Xml and System.Xml.Linq
                var root = XElement.Load(jsonReader);
                CobValues.AddRange(root.XPathSelectElement("//CobLocs").Descendants().Select(S => S.Value).ToList());
                ExtraDateFormatStrings.AddRange(root.XPathSelectElement("//ExtraDateFormats").Descendants().Select(S => S.Value).ToList());
                SpecialNonHeaderLines.AddRange(root.XPathSelectElement("//SpecialNonHeaderLines").Descendants().Select(S => S.Value).ToList());
                DateFixes.AddRange(root.XPathSelectElement("//DateFixes").Descendants().Select(S => S.Value).ToList());
                PreProcesorChecks.AddRange(root.XPathSelectElement("//preprocessorChecks").Descendants().Select(S => S.Value).ToList());
                TaxLikeOption.AddRange(root.XPathSelectElement("//TaxSaleLikeOptions").Descendants().Select(S => S.Value).ToList());
                KnownValidEvents.AddRange(root.XPathSelectElement("//KnownValidEventTypes").Descendants().Select(S => S.Value).ToList());
            }
        }
    }
}