using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.LykkeService.Settings
{
    public class RabbitMqSettings
    {
#if rabbitsub
        public RabbitMqExchangeSettings Subscribers { get; set; }
#endif
#if (rabbitpub)
        public RabbitMqExchangeSettings Publishers { get; set; }
#endif
    }

    public class RabbitMqExchangeSettings
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }
    }
}
