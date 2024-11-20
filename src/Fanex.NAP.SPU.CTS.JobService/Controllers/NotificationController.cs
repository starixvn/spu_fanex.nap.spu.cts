namespace Fanex.NAP.SPU.CTS.JobService.Controllers
{
    using Fanex.NAP.SPU.CTS.JobService.Services.Notification;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationJobService _jobService;

        public NotificationController(INotificationJobService jobService)
        {
            _jobService = jobService;
        }

        /// <summary>
        /// Scan & notification via telegram
        /// </summary>
        /// <returns>This function is unused</returns>
        [HttpPost("ScanDataConsistencyMonitoring")]
        public async Task<bool> ScanDataConsistencyMonitoring()
        {
            return await _jobService.ScanDataConsistencyMonitoring();
        }
    }
}
