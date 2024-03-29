﻿using Casheesh.Models;
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

                    foreach (Bounty bounty in context.Bounties)
                    {
                        if (DateTime.Now.Date >= (bounty.LastApplied + bounty.TimeSpan).Date)
                        {
                            double bountyIncrement = bounty.IsRate ? bounty.Value * bounty.IncrementValue : bounty.IncrementValue;
                            bounty.Value = Math.Min(bounty.Value + bountyIncrement, bounty.MaxValue);
                            bounty.LastApplied = DateTime.Now;
                        }
                    }

                    foreach (Recurrence recurrence in context.Recurrences)
                    {
                        if (DateTime.Now.Date >= (recurrence.LastApplied + recurrence.TimeSpan).Date)
                        {
                            double recurrenceValue = recurrence.IsRate ? recurrence.Account.CurrentBalance * recurrence.Value : recurrence.Value;
                            context.Transactions.Add(new Transaction
                            {
                                Number = !recurrence.Account.Transactions.Any() ? 1 : recurrence.Account.Transactions.OrderByDescending(transaction => transaction.Timestamp).First().Number + 1,
                                AccountName = recurrence.AccountName,
                                Account = recurrence.Account,
                                Value = recurrenceValue,
                                Description = $"{(recurrence.IsRate ? "Applied rate" : "Recurring transaction")}: {recurrence.Name} - {recurrence.Description}",
                                Timestamp = DateTime.Now
                            });
                            recurrence.LastApplied = DateTime.Now;
                            recurrence.Account.CurrentBalance += recurrenceValue;
                        }
                    }

                    Account? netWorthAccount = await context.Accounts.FindAsync(new object[] { "Net Worth" }, cancellationToken: stoppingToken);
                    if (netWorthAccount != null)
                    {
                        context.RemoveRange(netWorthAccount.Transactions);
                        netWorthAccount.CurrentBalance = context.Accounts.Sum(account => account.Name == "Net Worth" ? 0 : account.CurrentBalance);
                    }

                    foreach (Account account in context.Accounts)
                    {
                        Balance? latestBalance = account.Balances.OrderByDescending(balance => balance.Id).FirstOrDefault();
                        if (latestBalance == null || (DateTime.Now.Date > latestBalance.Timestamp.Date && latestBalance.Value != account.CurrentBalance))
                        {
                            context.Balances.Add(new Balance
                            {
                                AccountName = account.Name,
                                Account = account,
                                Id = (latestBalance?.Id ?? 0) + 1,
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