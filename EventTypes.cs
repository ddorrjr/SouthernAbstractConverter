using System.Collections.Generic;
using System.Linq;

namespace SouthernAbstractConverter
{
    public class EventTypes
    {
        public static List<string> EventNames = new List<string> {"Quitclaim","Final Certificate","SEE:", "NOTE:","NO REDEMPTION FOUND REGISTERED." }.Select(item => item.ToLower()).ToList();
        
        public static string Quitclaim => EventNames[0];
        public static string FinalCertif => EventNames[1];
        public static string SEEBits => EventNames[2];
        public static string NOTEBits => EventNames[3];
        public static string REDEMPT => EventNames[4];
    }
}