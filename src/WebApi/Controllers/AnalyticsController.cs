using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CasoNaranjitoSac.Models;
using CasoNaranjitoSac.Models.Views;
using System;
using System.Collections.Generic;
using System.Linq;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Session>>> GetSessions()
        {
            var results = await _context.Session
            .Select(e => new Session
            {
                IdSession = e.IdSession,
                Uuid = e.Uuid,
                UrlOrigin = e.UrlOrigin,
                Created = e.Created,
                Page = e.Page
                .Select(
                    page => new Page
                    {
                        IdPage = page.IdPage,
                        IdSession = page.IdSession,
                        Initial = page.Initial,
                        Ended = page.Ended,
                        UrlVisit = page.UrlVisit
                    }).ToList(),
                Link = e.Link
                .Select(
                    link => new Link
                    {
                        IdLink = link.IdLink,
                        IdSession = link.IdSession,
                        UrlLink = link.UrlLink,
                        Created = link.Created,
                    }).ToList()
            })
            .OrderByDescending(e => e.IdSession)
            .ToListAsync();

            return results;
        }

        [HttpGet("{uuid}")]
        public async Task<ActionResult<Session>> GetSession(string uuid)
        {
            var session = await _context.Session
            .Include(o => o.Page).FirstOrDefaultAsync(m => m.Uuid == uuid);


            if (session != null)
            {
                session.Page = session.Page.Select(
                    page => new Page
                    {
                        IdPage = page.IdPage,
                        IdSession = page.IdSession,
                        Initial = page.Initial,
                        Ended = page.Ended,
                        UrlVisit = page.UrlVisit
                    }).ToList();

                session.Link = session.Link.Select(
                    link => new Link
                    {
                        IdLink = link.IdLink,
                        IdSession = link.IdSession,
                        UrlLink = link.UrlLink,
                        Created = link.Created,
                    }).ToList();

                return session;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("report")]
        public async Task<IActionResult> GetReport()
        {
            const string query = @"SELECT t0.uuid Uuid, 
                                COUNT(t1.urlVisit) Pages, 
                                COUNT(t2.urlLink) Links,
                                TIMESTAMPDIFF(SECOND,MIN(t1.initial),MAX(t1.ended)) Seconds
                                FROM SESSION t0 
                                INNER JOIN page t1 ON t1.idSession = t0.idSession
                                INNER JOIN link t2 ON t2.idSession = t0.idSession
                                GROUP BY t0.uuid 
                                ORDER BY t0.idSession DESC;";

            var conn = _context.Database.GetDbConnection();
  
            await conn.OpenAsync();
            var command = conn.CreateCommand();
            command.CommandText = query;
            var reader = await command.ExecuteReaderAsync();

            Analytics analytics = null; 

            if (await reader.ReadAsync())
            {
                analytics = new Analytics(){
                    Uuid = reader.GetString(0),
                    Pages = reader.GetString(1),
                    Links = reader.GetString(2),
                    Seconds = int.Parse(reader.GetString(3))
                };
            }

            return new ObjectResult(analytics);
        }
    }
}