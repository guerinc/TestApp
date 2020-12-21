using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using MimeKit;
using MimeKit.Text;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using WebApplication2.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication2.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {

        private readonly MovieContext _context;

        public MovieController(MovieContext context)
        {
            _context = context;
        }

        // GET: api/<MovieController>
        [Authorize]
        [HttpGet]
        public IEnumerable<Movie> Get(int page, int pageSize)
        {
            return _context.Movie.Skip((page - 1) * pageSize).Take(pageSize);
        }

        // GET api/<MovieController>/5
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult Get(int id)
        {
            IActionResult response = BadRequest();
            try
            {
                var _movie = _context.Movie.Where(x => x.Id == id);
                return Ok(_movie);
            }
            catch (Exception)
            {
                return response;
            }
        }

        // POST api/<MovieController>
        [HttpPost]
        [Authorize]
        public IActionResult Post(string name, string des, int score)
        {
            IActionResult response = BadRequest();
            try
            {
                var _movie = _context.Movie.Add(new Models.Movie { MovieDescription = des, MovieName = name, Score = score });
                _context.SaveChanges();
                return Ok();

            }
            catch (Exception e)
            {
                return response;
            }
        }

        // PUT api/<MovieController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MovieController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpPost]
        [Route("send-email")]
        [Authorize]
        public void SendEmail(string mail)
        {
            // create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("from_address@example.com"));
            email.To.Add(MailboxAddress.Parse(mail));
            email.Subject = "Test Email Subject";
            email.Body = new TextPart(TextFormat.Plain) { Text = "Example Plain Text Message Body" };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("[USERNAME]", "[PASSWORD]");
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        [HttpGet]
        [Route("GetPeriodList")]
        public string GetPeriodList()
        {
            var URL = "https://api.themoviedb.org/3/list/1?api_key=5ef14fa95becd959c3ed576f50e81346&language=en-US";

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(string));
            WebClient syncClient = new WebClient();
            string content = syncClient.DownloadString(URL);

            //var list = new List<Movie>();
            //using (MemoryStream memo = new MemoryStream(Encoding.Unicode.GetBytes(content)))
            //{
            //    list = (List<Movie>)serializer.ReadObject(memo);
            //}

            return content;
        }


    }
}
