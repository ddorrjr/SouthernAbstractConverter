using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SouthernAbstractConverter.Model
{
    class PropertyDetail
    {
        public int PropertyDetailID { get; set; }
        public int PropertyID { get; set; }
        public int EventTypeID { get; set; }
        public int EventYear { get; set; }
        public string Debtor { get; set; }
        public int PurchaserID { get; set; }
        public DateTime TranxDate { get; set; }
        public string COB { get; set; }
        public string Notes { get; set; }
        
    }
}
