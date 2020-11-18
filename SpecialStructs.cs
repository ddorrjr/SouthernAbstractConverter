using System;
using System.Collections.Generic;

namespace SouthernAbstractConverter
{
    public partial class SpecialStructs
    {
        public struct progressData
        {
            public bool UseFileProgressBar;
            public int progressValue;
            public int TotalValue;
            public string UserData;
        }
        public struct Globals
        {
            public const Int32 BUFFER_SIZE = 512; // Unmodifiable
            public static String FILE_NAME = "Output.txt"; // Modifiable
            public static readonly String CODE_PREFIX = "US-"; // Unmodifiable
            public static int globalSubdivisionID = 0;
            public static int globalPropertyID = 0;
            public static int globalEventID = 0;
            public static string globalDeptor = "Shawn";
            public static int globalPurchaserID = 0;
            public static DateTime globalDate;
            public static string EventAbb;
            public static List<string> rowArray;
            public static List<string> globalNotes = new List<string>();
        }
    }
}