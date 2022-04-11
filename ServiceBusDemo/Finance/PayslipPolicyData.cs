using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Messages;

namespace Finance
{
    public class PayslipPolicyData : ContainSagaData
    {
        public string UserPayDate { get; set; }
        public int UserId { get; set; }
        public DateTime PayslipDate { get; set; }
        public string letter { get; set; }
        public decimal Amount { get; set; }
        public bool IsPayslipIssued { get; set; }
        public bool IsPayslipDebited { get; set; }
    }
}
