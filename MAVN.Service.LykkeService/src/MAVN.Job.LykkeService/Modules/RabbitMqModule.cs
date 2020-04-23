﻿using Autofac;
using JetBrains.Annotations;
#if rabbitpub
using Lykke.RabbitMqBroker.Publisher;
#endif
#if rabbitsub
using Lykke.RabbitMqBroker.Subscriber;
#endif
using Lykke.SettingsReader;
#if rabbitpub
using MAVN.Job.LykkeService.Contract;
#endif
#if rabbitsub
using MAVN.LykkeType.LykkeService.DomainServices.RabbitSubscribers;
#endif
using MAVN.Job.LykkeService.Settings;
using MAVN.Job.LykkeService.Settings.JobSettings;

namespace MAVN.Job.LykkeService.Modules
{
    [UsedImplicitly]
    public class RabbitMqModule : Module
    {
#if rabbitpub
        private const string PubExchangeName = "REPLACE THIS WITH PROPER EXCHANGE NAME"; // TODO pass proper exchange name
#endif
#if rabbitsub
        private const string SubExchangeName = "REPLACE THIS WITH PROPER EXCHANGE NAME"; // TODO pass proper exchange name
#endif

        private readonly RabbitMqSettings _settings;

        public RabbitMqModule(IReloadingManager<AppSettings> settingsManager)
        {
            _settings = settingsManager.CurrentValue.LykkeServiceJob.Rabbit;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // NOTE: Do not register entire settings in container, pass necessary settings to services which requires them
#if rabbitpub

            RegisterRabbitMqPublishers(builder);
#endif
#if rabbitsub

            RegisterRabbitMqSubscribers(builder);
#endif
        }
#if rabbitpub

        // registered publishers could be esolved by IRabbitPublisher<TMessage> interface
        private void RegisterRabbitMqPublishers(ContainerBuilder builder)
        {
            builder.RegisterJsonRabbitPublisher<MyPublishedMessage>(
                _settings.Publishers.ConnectionString,
                PubExchangeName);
        }
#endif
#if rabbitsub

        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            builder.RegisterJsonRabbitSubscriber<RabbitSubscriber, object>( // TODO replace object with proper message type
                _settings.Subscribers.ConnectionString,
                SubExchangeName,
                nameof(LykkeService).ToLower()); // this could be changed if needed
        }
#endif
    }
}
