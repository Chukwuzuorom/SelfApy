using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace Greeting.Controllers
{
    [ApiController]
   // [Route("[controller]")]
    public class GreetingController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };



        public class Model 
        {
            public string message { get; set; }
           // public string Pretext { get; set; }
        }
        private readonly ILogger<GreetingController> _logger;

        public GreetingController(ILogger<GreetingController> logger)
        {
            _logger = logger;
        }



       // [Authorize]
        [HttpPost]
        [Route("Greeting")]
        public string Greeting([FromBody] Model model)
        {

            string certificatePath = "/etc/ssl/certs/tls.crt";
            string privateKeyPath = "/etc/ssl/certs/tls.key";
            //string password = "<password>"; // if the certificate is password-protected
            X509Certificate2 certificate = new X509Certificate2(certificatePath);
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ClientCertificateOptions = ClientCertificateOption.Manual;
            httpClientHandler.SslProtocols = SslProtocols.Tls12;
            httpClientHandler.ClientCertificates.Add(certificate);

            
            HttpClient client = new HttpClient(httpClientHandler);
            client.BaseAddress = new Uri("https://greeter.default.svc.cluster.local/Greeting");

            //HttpResponseMessage response = await client.GetAsync("<your API endpoint>");
            //string responseString = await response.Content.ReadAsStringAsync();
            if (model.message!=null)
            {
                string greeting =  "Hello" + " " + model.message;
               // HttpResponseMessage respons = response.Content.ToString();
                return greeting;
            }
            else
            {
                return "Please input a valid mesage";
            }
            
        }

       
    }
}