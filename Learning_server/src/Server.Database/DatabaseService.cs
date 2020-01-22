using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Server.Database.Context;
using ChickenAPI.Core.Logging;
using Server.Core.Logging;
using System.Threading;
using System.Threading.Tasks;
using Pomelo.EntityFrameworkCore.MySql;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace Server.Database
{
    public static class DatabaseService
    {
        private static readonly ILogger Log = Logger.GetLogger("DatabaseService");

        public static bool InitializeSqlDb(DatabaseConfiguration databaseConfiguration)
        {
            try
            {
                using var db = GetSqlDatabase(databaseConfiguration);
                db.Database.Migrate();
                Log.Info("SqlDatabase initialized successfully");
            }
            catch (Exception ex)
            {
                switch (ex)
                {
                    case DbUpdateException sql:
                        return false;
                }

                Log.Error("[DB_INIT]", ex);
                return false;
            }
            return true;
        }

        public static FunatDbContext GetSqlDatabase(DatabaseConfiguration databaseConfiguration)
        {
            var builder = new DbContextOptionsBuilder<FunatDbContext>();
            var options = databaseConfiguration.DbType switch
            {
                /*case DatabaseType.MongoDB:
                return new MongoDatabaseClient(databaseConfiguration);*/
                DatabaseType.MySQL => builder.UseMySql(databaseConfiguration.ToString()),
                DatabaseType.PostgreSQL => builder.UseNpgsql(databaseConfiguration.ToString()),
                DatabaseType.MSSQL => builder.UseSqlServer(databaseConfiguration.ToString()),
                _ => throw new NotImplementedException()
            };
            return new FunatDbContext(options.Options);
            //StartupDb(db);
        }

        private static void StartupDb(DbContext db)
        {
            CheckLoopAndWait(db);
            db.Database.Migrate();
            Task.Run(() => CheckConnection(db));
        }

        private static void CheckConnection(DbContext db)
        {
            while (true)
            {
                Thread.Sleep(10_000);
                CheckLoopAndWait(db);
            }
        }

        private static void CheckLoopAndWait(DbContext db)
        {
            if (!db.Database.CanConnect())
            {
                Log.Warn("Inability to connect to the SqlDatabase. Started re-connection check loop!");
                Thread.Sleep(10_000);
                while (!db.Database.CanConnect())
                    Thread.Sleep(10_000);
                Log.Info("Connection with the SqlDatabase re-established!");
            }
        }
    }
}
