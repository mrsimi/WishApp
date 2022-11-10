using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Twilio.TwiML;
using Twilio.AspNet.Core;
using Newtonsoft.Json;
using System.Linq;

namespace WishAppAzFunction
{
    public static class IncomingMessage
    {
        [FunctionName("IncomingMessage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            AppDbContext dbContext = new AppDbContext();
           var form = await req.ReadFormAsync();
           var body = form["Body"];
           var sender =form["From"];

           dbContext.WishItems.Add(new WishItem{Message = body, Sender = sender});
           dbContext.SaveChanges();
           log.LogInformation(JsonConvert.SerializeObject(dbContext.WishItems.ToList()));
           var response = new MessagingResponse();
           response.Message("Thank you for the wish. Happy Holidays!");

           return new TwiMLResult(response);
        }
    }
}
