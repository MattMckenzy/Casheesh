using Casheesh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Casheesh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly CasheeshContext _context;

        public TransactionsController(CasheeshContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            Account account = _context.Find<Account>(transaction.AccountName);
            if (account == null)
                return BadRequest("The given account was not found!");

            account.Balance += transaction.Value;
            transaction.Number = account.Transactions.Count == 0 ? 1 : account.Transactions.OrderByDescending(transaction => transaction.Timestamp).First().Number + 1;
            transaction.Timestamp = DateTime.Now;
            _context.Transactions.Add(transaction);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TransactionExists(transaction.AccountName, transaction.Number))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return transaction;
        }

        [HttpDelete("{accountName}/{number}")]
        public async Task<IActionResult> DeleteTransaction(string accountName, int number)
        {
            var transaction = await _context.Transactions.FindAsync(accountName, number);
            if (transaction == null)
            {
                return NotFound();
            }

            transaction.Account.Balance -= transaction.Value;
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TransactionExists(string accountName, int number)
        {
            return _context.Transactions.Any(e => e.AccountName == accountName && e.Number == number);
        }
    }
}
