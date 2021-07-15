using AmblyopiaSaber.Managers;
using AmblyopiaSaber.UI;
using SiraUtil;
using Zenject;

namespace AmblyopiaSaber.Installers
{
    internal class AmblyopiaSaberMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.Bind<AmblyopiaSettingsView>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<AmblyopiaFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();

            Container.BindInterfacesTo<MenuButtonManager>().AsSingle();
        }
    }
}
