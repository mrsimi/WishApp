using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Twilio.TwiML;
using Twilio.AspNet.Core;
using System.Linq;
using System;
using Newtonsoft.Json;

namespace WishAppAzFunction
{
    public static class WishPlayer
    {
        [FunctionName("WishPlayer")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            AppDbContext context = new AppDbContext();

            log.LogInformation(JsonConvert.SerializeObject(context.WishItems.ToList()));

            int savedMessages = context.WishItems.Count();
            string wish = string.Empty;
            if(savedMessages > 0)
            {
                var rand = new Random().Next(context.WishItems.Count());
                wish = context.WishItems.AsEnumerable().ElementAt(rand).Message;
            }
            else 
            {
                wish = "No wishes present yet, check back later. Happy holidays!";
            }

            var response = new VoiceResponse();
            response.Say($"Reading out a wish for you", voice: "alice");
            response.Say(wish);

            return new TwiMLResult(response);
        }
    }
}
