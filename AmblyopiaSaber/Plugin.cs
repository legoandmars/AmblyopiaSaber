using AmblyopiaSaber.Config;
using AmblyopiaSaber.Installers;
using AmblyopiaSaber.Utils;
using IPA;
using IPA.Config.Stores;
using IPA.Loader;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;

namespace AmblyopiaSaber
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        /// <summary>
        /// Use to send log messages through BSIPA.
        /// </summary>
        internal static IPALogger Log { get; private set; }

        [Init]
        public Plugin(IPALogger logger, IPA.Config.Config config, Zenjector zenjector)
        {
            Instance = this;
            Log = logger;
            zenjector.OnApp<AmblyopiaSaberCoreInstaller>().WithParameters(config.Generated<PluginConfig>());
            zenjector.OnMenu<AmblyopiaSaberMenuInstaller>();
        }

        [OnStart]
        public void OnApplicationStart()
        {
            PluginMetadata customNotesPlugin = PluginManager.GetPluginFromId("Custom Notes");
            if (customNotesPlugin != null)
            {
                //Plugin.Log.Info("Custom notes installed.");
            }
            NoteUtils.LoadEmbeddedBundles();
        }
    }
}
