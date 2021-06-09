using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Casheesh.Models;

namespace Casheesh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalancesController : ControllerBase
    {
        private readonly CasheeshContext _context;

        public BalancesController(CasheeshContext context)
        {
            _context = context;
        }

        // GET: api/Balances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Balance>>> GetBalances()
        {
            return await _context.Balances.ToListAsync();
        }

        // GET: api/Balances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Balance>> GetBalance(string id)
        {
            var balance = await _context.Balances.FindAsync(id);

            if (balance == null)
            {
                return NotFound();
            }

            return balance;
        }

        // PUT: api/Balances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBalance(string id, Balance balance)
        {
            if (id != balance.AccountName)
            {
                return BadRequest();
            }

            _context.Entry(balance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BalanceExists(id))
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

        // POST: api/Balances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Balance>> PostBalance(Balance balance)
        {
            _context.Balances.Add(balance);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BalanceExists(balance.AccountName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetBalance", new { id = balance.AccountName }, balance);
        }

        // DELETE: api/Balances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBalance(string id)
        {
            var balance = await _context.Balances.FindAsync(id);
            if (balance == null)
            {
                return NotFound();
            }

            _context.Balances.Remove(balance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BalanceExists(string id)
        {
            return _context.Balances.Any(e => e.AccountName == id);
        }
    }
}
