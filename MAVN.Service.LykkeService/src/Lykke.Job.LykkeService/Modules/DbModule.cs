﻿using Autofac;
#if (aztrepos)
using AzureStorage.Tables;
#endif
using JetBrains.Annotations;
#if (aztrepos)
using Lykke.Common.Log;
#endif
#if (mssqlrepos)
using Lykke.Common.MsSql;
#endif
using Lykke.SettingsReader;
#if (aztrepos)
using MAVN.LykkeType.LykkeService.AzureRepositories;
#endif
using MAVN.LykkeType.LykkeService.Domain.Repositories;
#if (mssqlrepos)
using MAVN.LykkeType.LykkeService.MsSqlRepositories;
using MAVN.LykkeType.LykkeService.MsSqlRepositories.Repositories;
#endif
using MAVN.Job.LykkeService.Settings;

namespace MAVN.Job.LykkeService.Modules
{
    [UsedImplicitly]
    public class DbModule : Module
    {
#if (aztrepos)
        private readonly IReloadingManager<AppSettings> _appSettings;
#endif
#if (mssqlrepos)
        private readonly string _connectionString;
#endif

        public DbModule(IReloadingManager<AppSettings> appSettings)
        {
#if (aztrepos)
            _appSettings = appSettings;
#endif
#if (mssqlrepos)
            _connectionString = appSettings.CurrentValue.LykkeServiceJob.Db.SqlDbConnString;
#endif
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LykkeServiceRepository>()
                .As<ILykkeServiceRepository>()
                .SingleInstance();
#if (aztrepos)

            builder.Register(ctx =>
                AzureTableStorage<LykkeServiceEntity>.Create(
                    _appSettings.ConnectionString(x => x.LykkeServiceService.Db.AzureDbConnString),
                    "LykkeService",
                    ctx.Resolve<ILogFactory>()))
            .SingleInstance();
#endif
#if (mssqlrepos)

            builder.RegisterMsSql(
                _connectionString,
                connString => new LykkeServiceContext(connString, false),
                dbConn => new LykkeServiceContext(dbConn));
#endif
        }
    }
}
