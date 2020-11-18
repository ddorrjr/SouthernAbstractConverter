using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SouthernAbstractConverter.Model
{
    class Property
    {
        public int PropertyID { get; set; }
        public int SubdivisionID { get; set; }
        public string LotDesc { get; set; }
        public int TSYear { get; set; }
        public string OtherInfo { get; set; }
        public string SQDesc { get; set; }
    }
}
