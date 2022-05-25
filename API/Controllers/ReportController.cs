using Microsoft.AspNetCore.Mvc;
using Telerik.Reporting.Services.AspNetCore;
using Telerik.Reporting.Services;

namespace API {
    [Route("reports")]
    [ApiController]
    [AccessCodeAuthorize("RA01")]
    [IgnoreAntiforgeryToken]
    public class ReportsController : ReportsControllerBase {
        public ReportsController(IReportServiceConfiguration reportServiceConfiguration)
                 : base(reportServiceConfiguration) {
        }
    }
}