using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common {
    public sealed class DataType : ConstantBase<DataType> {
        public static readonly DataType Integer = new DataType("Integer", "Integer");
        public static readonly DataType Decimal = new DataType("Decimal", "Decimal");
        public static readonly DataType DateTime = new DataType("DateTime", "DateTime");
        public static readonly DataType Text = new DataType("String", "String");

        private DataType(string code, string description)
            : base(code, description) {
        }
    }
}
