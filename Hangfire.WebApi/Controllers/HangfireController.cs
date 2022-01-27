using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Hangfire.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        [HttpPost]
        [Route("welcome")]
        public IActionResult Welcome(string userName)
        {
            var jobId = BackgroundJob.Enqueue(() => SendWelcomeMail(userName));
            return Ok($"Job Id {jobId} Completed. Welcome Mail Sent!");
        }

        public void SendWelcomeMail(string userName)
        {
            //Logic to mail the User
            System.Console.WriteLine($"Welcome to our application, {userName}");
        }

        [HttpPost]
        [Route("delayedWelcome")]
        public IActionResult DelayedWelcome(string userName)
        {
            var jobId = BackgroundJob.Schedule(() => SendDelayedWelcomeMail(userName), TimeSpan.FromMinutes(2));
            return Ok($"Job Id {jobId} Completed. Delayed Welcome Mail Sent!");
        }

        public void SendDelayedWelcomeMail(string userName)
        {
            // Logic to mail the user
            Console.WriteLine($"Welcome to our application, {userName}");
        }

        [HttpPost]
        [Route("invoice")]
        public IActionResult Invoice(string userName)
        {
            RecurringJob.AddOrUpdate(() => SendInvoiceMail(userName), Cron.Monthly);
            return Ok($"Recurring Job Scheduled. Invoice will be mailed Monthly for {userName}!");
        }
        public void SendInvoiceMail(string userName)
        {
            //Logic to Mail the user
            Console.WriteLine($"Here is your invoice, {userName}");
        }

        [HttpPost]
        [Route("unsubscribe")]
        public IActionResult Unsubscribe(string userName)
        {
            var jobId = BackgroundJob.Enqueue(() => UnsubscribeUser(userName));
            BackgroundJob.ContinueJobWith(jobId, () => Console.WriteLine($"Sent Confirmation Mail to {userName}"));
            return Ok($"Unsubscribed");
        }
        public void UnsubscribeUser(string userName)
        {
            //Logic to Unsubscribe the user
            Console.WriteLine($"Unsubscribed {userName}");
        }
    }
}
