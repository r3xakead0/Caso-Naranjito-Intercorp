using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasoNaranjitoSac.Models;

namespace CasoNaranjitoSac.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly AnalyticsContext _context;

        public AnalyticsController(AnalyticsContext context)
        {
            _context = context;
        }

        [HttpGet("session/{url}")]
        public async Task<ActionResult<string>> GetSession(string url)
        {
            var session = new Session(){
                Uuid = "",
                UrlOrigin = url
            };

            _context.Session.Add(session);
            await _context.SaveChangesAsync();
            
            _context.Page.Add(new Page() { IdSession = session.IdSession, UrlVisit = session.UrlOrigin });
            await _context.SaveChangesAsync();

            return session.Uuid;

        }

        [HttpPost("page/{uuid}")]
        public async Task<ActionResult<Session>> GetSessio1n(string uuid, string url)
        {
            var session = new Session(){
                Uuid = "",
                UrlOrigin = url
            };

            _context.Session.Add(session);
            await _context.SaveChangesAsync();
            
            _context.Page.Add(new Page() { IdSession = session.IdSession, UrlVisit = session.UrlOrigin });
            await _context.SaveChangesAsync();

            return session;

        }

        [HttpPost]
        public async Task<ActionResult<Session>> PostAnalytics(string url)
        {
            
            var session = new Session(){
                UrlOrigin = url
            };

            _context.Session.Add(session);
            await _context.SaveChangesAsync();

            return session;
        }


    }
}