using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    public partial class EnvrionmentConstant : ConstantBase<EnvrionmentConstant> {

        public static readonly EnvrionmentConstant ODCDev = new EnvrionmentConstant("ODCDev","ODC Dev envrionment");
        public static readonly EnvrionmentConstant ODCSIT = new EnvrionmentConstant("ODCSIT","ODC SIT envrionment");
        public static readonly EnvrionmentConstant CDNDev = new EnvrionmentConstant("CDNDev","CDN Dev envrionment");
        public static readonly EnvrionmentConstant CDNSIT = new EnvrionmentConstant("CDNSIT","CDN SIT envrionment");
        public static readonly EnvrionmentConstant UAT = new EnvrionmentConstant("UAT","UAT envrionment");
        public static readonly EnvrionmentConstant PROD = new EnvrionmentConstant("PROD","Production envrionment");

        private EnvrionmentConstant(string code, string description)
         : base(code, description) {
        }
    }
}
