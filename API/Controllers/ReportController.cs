using Microsoft.AspNetCore.Mvc;
using Telerik.Reporting.Services.AspNetCore;
using Telerik.Reporting.Services;

namespace API {
    [Route("reports")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class ReportsController : ReportsControllerBase {
        public ReportsController(IReportServiceConfiguration reportServiceConfiguration)
                 : base(reportServiceConfiguration) {
        }
    }
}