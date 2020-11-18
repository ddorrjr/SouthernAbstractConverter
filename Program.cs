using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SouthernAbstractConverter.Service
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ApplicationConfiguration.Datas(Path.Combine(Path.GetDirectoryName(Application.ExecutablePath),"appconfig.json"));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args?.Length > 0)
            {
                Application.Run(new frmConverter(args));
            }
            else
            {
                Application.Run(new frmConverter());
            }
            Service.SADataService saData = new Service.SADataService();

           // saData.ConvertText(@"c:\data\ACT_SAMPLE.txt");

           /* int clientID = saData.InsertClient("new client");
            int eventTypeID = saData.InsertEventType(new Model.EventType { EventAbb = "SL", EventTypeName = "SALE TEST EVENT" });
            int subdivisionID = saData.InsertSubdivision("test subdivision");
            int propertyID = saData.InsertProperty(new Model.Property { SubdivisionID = 1, TSYear = 2007, LotDesc = "test data", OtherInfo = "other test", SQDesc = "SQ 10 TEST" });
            int purchaserID = saData.InsertPurchaser("David Benton");
            int propertyDetailID = saData.InsertPropertyDetail(new Model.PropertyDetail { PropertyID = 1, EventTypeID = 1, EventYear = 2016, Debtor = "new debtor 2", PurchaserID = 1, TranxDate = "12/16/2019", COB = "809", Notes = "notes here" });
            */
            //saData = null;

        }
    }
}
