using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace NetExtensions.Infra.Sqlite.InMemoryDb
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqliteInMemoryDb<TContext>(this IServiceCollection services) where TContext : DbContext, new()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            services.AddSingleton(sp => connection);
            var options = new DbContextOptionsBuilder<TContext>()
                .UseSqlite(connection)
                .Options;
            using var context = CreateContext(options);
            context.Database.EnsureCreated();

            services.AddSingleton(sp => options);
            return services;
        }

        private static TContext CreateContext<TContext>(DbContextOptions<TContext> options) where TContext : DbContext, new() => (TContext)Activator.CreateInstance(typeof(TContext), options);
    }
}
