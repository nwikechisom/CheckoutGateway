using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CheckoutGateway.BankSimulator.Api;

public class Extensions
{
    private async Task RunMigration<T>(T db) where T : DbContext
    {
        var pendingMigrations = db.Database.GetPendingMigrations().ToList();
        if (pendingMigrations.Any())
        {
            var migrator = db.Database.GetService<IMigrator>();
            foreach (var targetMigration in pendingMigrations) migrator.Migrate(targetMigration);
        }

        await Task.CompletedTask;
    }
}
