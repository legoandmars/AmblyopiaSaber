using AmblyopiaSaber.Config;
using SiraUtil.Interfaces;
using Zenject;
using SiraUtil.Objects.Beatmap;
using SiraUtil.Extras;
using AmblyopiaSaber.Data;

namespace AmblyopiaSaber.Installers
{
    internal class AmblyopiaSaberCoreInstaller : Installer<PluginConfig, AmblyopiaSaberCoreInstaller>
    {
        private readonly PluginConfig _pluginConfig;

        public AmblyopiaSaberCoreInstaller(PluginConfig pluginConfig)
        {
            _pluginConfig = pluginConfig;
            if (!_pluginConfig.LoadedYet)
            {
                _pluginConfig.LoadedYet = true;
                _pluginConfig.LeftNoteData.RightEyeVisibility = 0.5f;
                _pluginConfig.RightNoteData.LeftEyeVisibility = 0.5f;
            }
        }
        public override void InstallBindings()
        {
            Container.BindInstance(_pluginConfig).AsSingle();
            Container.RegisterRedecorator(new BasicNoteRegistration(DecorateNote));
            Container.RegisterRedecorator(new BurstSliderHeadNoteRegistration(DecorateNote));
            Container.RegisterRedecorator(new BurstSliderNoteRegistration(DecorateSlider));
        }

        public GameNoteController DecorateNote(GameNoteController original)
        {
            original.gameObject.AddComponent<AmblyopiaController>();
            return original;
        }

        public BurstSliderGameNoteController DecorateSlider(BurstSliderGameNoteController original)
        {
            original.gameObject.AddComponent<AmblyopiaBurstSliderController>();
            return original;
        }
    }
}
