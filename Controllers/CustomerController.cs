using ClosedXML.Excel;
using firstapi.Helpter;
using firstapi.Repos;
using firstapi.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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

        [HttpGet("ExportExcel")]
        public ActionResult ExportExcel()
        {
            var _empdata = GetEmpdata();
            var _cusdata = GetCustomerdata();
            using (XLWorkbook wb = new XLWorkbook())
            {
                var sheet1 = wb.AddWorksheet(_empdata, "Employee Records");
                wb.AddWorksheet(_cusdata);

                sheet1.Column(1).Style.Font.FontColor = XLColor.Red;

                sheet1.Columns(2,4).Style.Font.FontColor = XLColor.Blue;

                sheet1.Row(1).CellsUsed().Style.Fill.BackgroundColor = XLColor.Black;
                //sheet1.Row(1).Cells(1,3).Style.Fill.BackgroundColor = XLColor.Yellow;
                sheet1.Row(1).Style.Font.FontColor = XLColor.White;

                sheet1.Row(1).Style.Font.Bold = true;
                sheet1.Row(1).Style.Font.Shadow = true;
                sheet1.Row(1).Style.Font.Underline = XLFontUnderlineValues.Single;
                sheet1.Row(1).Style.Font.VerticalAlignment = XLFontVerticalTextAlignmentValues.Superscript;
                sheet1.Row(1).Style.Font.Italic = true;

                sheet1.Rows(2, 3).Style.Font.FontColor = XLColor.AshGrey;

                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Sample.xlsx");
                }
            }
        }

        [NonAction]
        private DataTable GetEmpdata()
        {
            DataTable dt = new DataTable();
            dt.TableName = "Empdata";
            dt.Columns.Add("Code", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns.Add("Designation", typeof(string));

            var _list = this.dbfirstcontext.TblEmployees.ToList();
            if(_list.Count > 0 )
            {
                _list.ForEach(item =>
                {
                    dt.Rows.Add(item.Code,item.Name,item.Email,item.Phone,item.Designation);
                });
            }

            return dt;
        }

        [NonAction]
        private DataTable GetCustomerdata()
        {
            DataTable dt = new DataTable();
            dt.TableName = "Customerdata";
            dt.Columns.Add("Code", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns.Add("Credit Limit", typeof(int));

            var _list = this.dbfirstcontext.TblCustomers.ToList();
            if (_list.Count > 0)
            {
                _list.ForEach(item =>
                {
                    dt.Rows.Add(item.Id, item.Name, item.Email, item.Phone, item.CreditLimit);
                });
            }

            return dt;
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
