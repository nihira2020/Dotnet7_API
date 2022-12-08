using firstapi.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace firstapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly dbfirstcontext dbfirstcontext;
        public CustomerController(dbfirstcontext dbfirstcontext) { 
            this.dbfirstcontext = dbfirstcontext;
        }

        [HttpGet("Getall")]
        public ActionResult Index()
        {
            return Ok(this.dbfirstcontext.TblCustomers.ToList());
        }
    }
}
