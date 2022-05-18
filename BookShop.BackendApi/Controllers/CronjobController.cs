using BookShop.BackendApi.Provider;
using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BookShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CronjobController : ControllerBase
    {
        private readonly ICronjob _cronjob;
        private readonly IUserApiService _userApiService;

        public CronjobController(ICronjob Cronjob, IUserApiService userApiService)
        {
            _cronjob = Cronjob;
            _userApiService = userApiService;
        }

        //[HttpGet("/fire-and-forget")]
        //public JsonResult FireAndForget()
        //{
        //    var jobId = BackgroundJob.Enqueue( () => _cronjob.SendEmailAccount());
        //    return new JsonResult(new { StatusCode = 200, msg = $"{jobId}" });
        //}

        ////delay
        //[HttpGet("/discount-mail")]
        //public JsonResult SendDiscount()
        //{
        //    var jobId = BackgroundJob.Schedule(() => _cronjob.SendDiscount(), TimeSpan.FromMinutes(2));
        //    return new JsonResult(new { StatusCode = 200, msg = $"Send Delay Email Successfull to {jobId}" });
        //}

        //RecurringJob
        [HttpGet("/recurring-job")]
        public IActionResult SendEmail()
        {
            RecurringJob.AddOrUpdate(() => _cronjob.SendEmailEveryday(), Cron.Minutely, TimeZoneInfo.Utc);
            return new JsonResult(new { StatusCode = 200, msg = "Send Email Successfull" });
        }

        //Continuations
        [HttpGet("/continuations")]
        public IActionResult Continuations([FromQuery] string Email)
        {
            var jobId = BackgroundJob.Enqueue(() => _cronjob.Continuations());
            BackgroundJob.ContinueJobWith(jobId, () => _userApiService.UnsubscribeEmail(Email));
            return new JsonResult(new { StatusCode = 200, msg = $"Send Unsub Email Successfull to {jobId}" });
        }

        [HttpGet("/remove-recurring-job")]
        public JsonResult RemoveRecurringJob()
        {
            using (var connection = JobStorage.Current.GetConnection())
            {
                foreach (var recurringJob in connection.GetRecurringJobs())
                {
                    RecurringJob.RemoveIfExists(recurringJob.Id);
                }
            }
            return new JsonResult(new { StatusCode = 200, msg = "remove successfull" });
        }
    }
}