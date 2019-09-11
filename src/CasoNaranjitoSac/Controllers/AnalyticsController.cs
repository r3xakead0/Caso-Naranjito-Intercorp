using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CasoNaranjitoSac.Models;

namespace AnalyticsController.Controllers
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

        // GET: api/Analytics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Analytics>>> GetAnalyticsItems()
        {
            return await _context.AnalyticsItems.ToListAsync();
        }

        // GET: api/Analytics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Analytics>> GetAnalyticsItem(string guid)
        {
            var AnalyticsItem = await _context.AnalyticsItems.FindAsync(guid);

            if (AnalyticsItem == null)
            {
                return NotFound();
            }

            return AnalyticsItem;
        }

        // POST: api/Analytics
        [HttpPost]
        public async Task<ActionResult<Analytics>> PostAnalyticsItem(Analytics item)
        {
            _context.AnalyticsItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnalyticsItem), new { id = item.guid }, item);
        }

        // PUT: api/Analytics/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnalyticsItem(string guid, Analytics item)
        {
            if (guid != item.guid)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Analytics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnalyticsItem(string guid)
        {
            var AnalyticsItem = await _context.AnalyticsItems.FindAsync(guid);

            if (AnalyticsItem == null)
            {
                return NotFound();
            }

            _context.AnalyticsItems.Remove(AnalyticsItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}