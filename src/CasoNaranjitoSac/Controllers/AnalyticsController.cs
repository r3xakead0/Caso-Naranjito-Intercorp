using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasoNaranjitoSac.Models;
using System;

namespace CasoNaranjitoSac.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly AnalyticsContext _context;

        public AnalyticsController(AnalyticsContext context)
        {
            _context = context;
        }

        [HttpGet("session/{url}")]
        public async Task<IActionResult> GetSession(string url)
        {
            var session = new Session()
            {
                Uuid = Guid.NewGuid().ToString(),
                UrlOrigin = url
            };

            _context.Session.Add(session);
            await _context.SaveChangesAsync();

            _context.Page.Add(new Page() { IdSession = session.IdSession, UrlVisit = session.UrlOrigin });
            await _context.SaveChangesAsync();

            return Ok(new
            {
                uuid = session.Uuid
            });
        }

        [HttpGet("link/{uuid}/{url}")]
        public async Task<IActionResult> Getlink(string uuid, string url)
        {
            var session = await _context.Session.SingleOrDefaultAsync(m => m.Uuid == uuid);
            if (session == null)
            {
                return NotFound();
            }
            else
            {
                var link = new Link()
                {
                    IdSessionNavigation = session,
                    UrlLink = url
                };
                _context.Link.Add(link);
                await _context.SaveChangesAsync();

                return Ok();
            }
        }


    }
}