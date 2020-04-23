using Autofac;
#if azurequeuesub
using Autofac.Extensions.DependencyInjection;
#endif
#if timeperiod
using Common;
#endif
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Sdk.Health;
using Lykke.SettingsReader;
using MAVN.Job.LykkeService.Services;
using MAVN.Job.LykkeService.Settings;
#if timeperiod
using MAVN.Job.LykkeService.PeriodicalHandlers;
#endif
#if azurequeuesub
using MAVN.JobTriggers.Extenstions;
using MAVN.JobTriggers.Triggers;
#endif

namespace MAVN.Job.LykkeService.Modules
{
    [UsedImplicitly]
    public class JobModule : Module
    {
        private readonly AppSettings _settings;
        private readonly IReloadingManager<AppSettings> _settingsManager;

        public JobModule(IReloadingManager<AppSettings> settingsManager)
        {
            _settings = settingsManager.CurrentValue;
            _settingsManager = settingsManager;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // NOTE: Do not register entire settings in container, pass necessary settings to services which requires them

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>()
                .SingleInstance();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>()
                .AutoActivate()
                .SingleInstance();
#if azurequeuesub

            RegisterAzureQueueHandlers(builder);
#endif
#if timeperiod

            RegisterPeriodicalHandlers(builder);
#endif

            // TODO: Add your dependencies here
        }
#if azurequeuesub

        private void RegisterAzureQueueHandlers(ContainerBuilder builder)
        {
            builder.Register(ctx =>
            {
                var scope = ctx.Resolve<ILifetimeScope>();
                var host = new TriggerHost(new AutofacServiceProvider(scope));
                return host;
            }).As<TriggerHost>()
            .SingleInstance();
    
            // NOTE: You can implement your own poison queue notifier for azure queue subscription.
            // See https://github.com/LykkeCity/JobTriggers/blob/master/readme.md
            // builder.Register<PoisionQueueNotifierImplementation>().As<IPoisionQueueNotifier>();

            builder.AddTriggers(
                pool =>
                {
                    pool.AddDefaultConnection(_settingsManager.Nested(s => s.LykkeServiceJob.AzureQueue.ConnectionString));
                });
        }
#endif
#if timeperiod

        private void RegisterPeriodicalHandlers(ContainerBuilder builder)
        {
            // TODO: You should register each periodical handler in DI container as IStartable singleton and autoactivate it

            builder.RegisterType<MyPeriodicalHandler>()
                .As<IStartable>()
                .As<IStopable>()
                .SingleInstance();
        }
#endif
    }
}
