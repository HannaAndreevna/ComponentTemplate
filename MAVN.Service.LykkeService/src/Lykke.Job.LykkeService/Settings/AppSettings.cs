using Lykke.Sdk.Settings;
using MAVN.Job.LykkeService.Settings.JobSettings;

namespace MAVN.Job.LykkeService.Settings
{
    public class AppSettings : BaseAppSettings
    {
        public LykkeServiceJobSettings LykkeServiceJob { get; set; }
    }
}
