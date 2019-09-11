using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost("session")]
        public async Task<IActionResult> GetSession(PostBody data)
        {
            var session = new Session()
            {
                Uuid = Guid.NewGuid().ToString(),
                UrlOrigin = data.Url
            };

            _context.Session.Add(session);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                uuid = session.Uuid
            });
        }

        [HttpPost("link")]
        public async Task<IActionResult> PostLink(PostBody data)
        {
            var session = await _context.Session.SingleOrDefaultAsync(m => m.Uuid == data.Uuid);
            if (session == null)
            {
                return NotFound();
            }
            else
            {
                var link = new Link()
                {
                    IdSessionNavigation = session,
                    UrlLink = data.Url
                };
                _context.Link.Add(link);
                await _context.SaveChangesAsync();

                return Ok();
            }
        }

        [HttpPost("page/init")]
        public async Task<IActionResult> PostInitPage(PostBody data)
        {
            var session = await _context.Session.SingleOrDefaultAsync(m => m.Uuid == data.Uuid);
            if (session == null)
            {
                return NotFound();
            }
            else
            {
                var page = new Page()
                {
                    IdSessionNavigation = session,
                    UrlVisit = data.Url
                };
                _context.Page.Add(page);
                await _context.SaveChangesAsync();

                return Ok();
            }
        }

        [HttpPost("page/ended")]
        public async Task<IActionResult> PostEndedPage(PostBody data)
        {
            var session = await _context.Session.SingleOrDefaultAsync(m => m.Uuid == data.Uuid);
            if (session == null)
            {
                return NotFound();
            }
            else
            {
                Page page = null;

                page = await _context.Page.SingleOrDefaultAsync(m => m.IdSession == session.IdSession && m.Ended == null);
                if (page != null)
                {
                    page.Ended = DateTime.Now;
                    _context.Entry(page).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
        }

    }
}