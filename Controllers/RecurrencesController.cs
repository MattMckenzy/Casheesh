using Casheesh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casheesh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecurrencesController : ControllerBase
    {
        private readonly CasheeshContext _context;

        public RecurrencesController(CasheeshContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recurrence>>> GetRecurrences()
        {
            return await _context.Recurrences.ToListAsync();
        }

        [HttpPut("{accountName}/{name}")]
        public async Task<IActionResult> PutRecurrence(string accountName, string name, Recurrence recurrence)
        {
            if (accountName != recurrence.AccountName || name != recurrence.Name)
            {
                return BadRequest();
            }

            _context.Entry(recurrence).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecurrenceExists(accountName, name))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Recurrence>> PostRecurrence(Recurrence recurrence)
        {
            _context.Recurrences.Add(recurrence);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (RecurrenceExists(recurrence.AccountName, recurrence.Name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return recurrence;
        }

        [HttpDelete("{accountName}/{name}")]
        public async Task<IActionResult> DeleteRecurrence(string accountName, string name)
        {
            var recurrence = await _context.Recurrences.FindAsync(accountName, name);
            if (recurrence == null)
            {
                return NotFound();
            }

            _context.Recurrences.Remove(recurrence);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RecurrenceExists(string accountName, string name)
        {
            return _context.Recurrences.Any(e => e.AccountName == accountName && e.Name == name);
        }
    }
}
