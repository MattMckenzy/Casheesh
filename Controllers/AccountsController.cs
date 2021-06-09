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
    public class AccountsController : ControllerBase
    {
        private readonly CasheeshContext _context;

        public AccountsController(CasheeshContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Account>> GetAccount(string name)
        {
            var account = await _context.Accounts.FindAsync(name);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> PutAccount(string name, Account account)
        {
            if (name != account.Name)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(name))
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
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            account.Order = !_context.Accounts.Any() ? 1 : _context.Accounts.OrderByDescending(account => account.Order).First().Order + 1;
            _context.Accounts.Add(account);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AccountExists(account.Name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return account;
        }

        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteAccount(string name)
        {
            var account = await _context.Accounts.FindAsync(name);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccountExists(string name)
        {
            return _context.Accounts.Any(e => e.Name == name);
        }
    }
}
