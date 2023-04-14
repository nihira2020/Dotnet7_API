using firstapi.Helpter;
using firstapi.Repos;
using firstapi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace firstapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly dbfirstcontext dbfirstcontext;
        private readonly IEmailService emailService;
        public CustomerController(dbfirstcontext dbfirstcontext,IEmailService service) { 
            this.dbfirstcontext = dbfirstcontext;
            this.emailService = service;
        }

        [HttpGet("Getall")]
        public ActionResult Index()
        {
            return Ok(this.dbfirstcontext.TblCustomers.ToList());
        }

        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail()
        {
            try
            {
                Mailrequest mailrequest = new Mailrequest();
                mailrequest.ToEmail = "nihiratechiees@gmail.com";
                mailrequest.Subject = "Welcome to NihiraTechiees";
                mailrequest.Body = GetHtmlcontent();
                await emailService.SendEmailAsync(mailrequest);
                return Ok();
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        private string GetHtmlcontent()
        {
            string Response = "<div style=\"width:100%;background-color:lightblue;text-align:center;margin:10px\">";
            Response += "<h1>Welcome to Nihira Techiees</h1>";
            Response += "<img src=\"https://yt3.googleusercontent.com/v5hyLB4am6E0GZ3y-JXVCxT9g8157eSeNggTZKkWRSfq_B12sCCiZmRhZ4JmRop-nMA18D2IPw=s176-c-k-c0x00ffffff-no-rj\" />";
            Response += "<h2>Thanks for subscribed us</h2>";
            Response += "<a href=\"https://www.youtube.com/channel/UCsbmVmB_or8sVLLEq4XhE_A/join\">Please join membership by click the link</a>";
            Response += "<div><h1> Contact us : nihiratechiees@gmail.com</h1></div>";
            Response += "</div>";
            return Response;
        }
    }
}
