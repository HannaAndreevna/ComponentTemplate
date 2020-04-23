using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace MAVN.Service.LykkeService.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class LykkeServiceSettings
    {
        public DbSettings Db { get; set; }
#if (rabbitsub || rabbitpub)

        public RabbitMqSettings Rabbit { get; set; }
#endif
    }
}
