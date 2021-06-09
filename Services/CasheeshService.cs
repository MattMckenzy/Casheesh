using Casheesh.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Casheesh.Services
{
    public partial class CasheeshService : BackgroundService
    {
        #region Private Properties

        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CasheeshService> _logger;

        #endregion

        #region Constructor and Entry Point

        public CasheeshService(IServiceScopeFactory scopeFactory, ILogger<CasheeshService> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Information ({DateTime.Now}) - Casheesh background service started!");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using IServiceScope scope = _scopeFactory.CreateScope();
                    CasheeshContext context = scope.ServiceProvider.GetRequiredService<CasheeshContext>();

                    foreach (Recurrence recurrence in context.Recurrences)
                    {
                        if (DateTime.Now.Date >= (recurrence.LastApplied + recurrence.TimeSpan).Date)
                        {
                            context.Transactions.Add(new Transaction
                            {
                                Number = !recurrence.Account.Transactions.Any() ? 1 : recurrence.Account.Transactions.OrderByDescending(transaction => transaction.Timestamp).First().Number + 1,
                                AccountName = recurrence.AccountName,
                                Value = recurrence.Value,
                                Description = $"Recurring transaction: {recurrence.Name} - {recurrence.Description}",
                                Timestamp = DateTime.Now
                            });
                            recurrence.LastApplied = DateTime.Now;
                            recurrence.Account.CurrentBalance += recurrence.Value;
                        }
                    }

                    foreach (Account account in context.Accounts)
                    {
                        Balance latestBalance = account.Balances.OrderByDescending(balance => balance.Id).FirstOrDefault();
                        if (latestBalance == null || (DateTime.Now.Date < latestBalance.Timestamp && latestBalance.Value != account.CurrentBalance))
                        {
                            context.Balances.Add(new Balance
                            {
                                AccountName = account.Name,
                                Id = !account.Balances.Any() ? 1 : latestBalance.Id + 1,
                                Value = account.CurrentBalance,
                                Timestamp = DateTime.Now                                
                            });
                        }
                    }

                    await context.SaveChangesAsync(stoppingToken);

                    await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                }
                catch (TaskCanceledException)
                {
                    _logger.LogInformation($"Information ({DateTime.Now}) - Casheesh service is stopping.");
                }
                catch (Exception exception)
                {
                    _logger.LogCritical($"Critical ({DateTime.Now}) - Exception during casheesh processing: {exception.Message}{Environment.NewLine}{exception.StackTrace}");
                }
            }


            _logger.LogInformation($"Information ({DateTime.Now}) - Casheesh background service stopped!");
        }

        #endregion
    }
}