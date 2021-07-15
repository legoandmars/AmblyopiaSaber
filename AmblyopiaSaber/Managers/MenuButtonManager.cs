using AmblyopiaSaber.UI;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using HMUI;
using System;
using Zenject;

namespace AmblyopiaSaber.Managers
{
    internal class MenuButtonManager : IInitializable, IDisposable
    {
        private readonly MenuButton _menuButton;
        private readonly MainFlowCoordinator _mainFlowCoordinator;
        private readonly AmblyopiaFlowCoordinator _amblyopiaFlowCoordinator;
        private FlowCoordinator _parentFlowCoordinator;
        private readonly GameplaySetupViewController _gameplaySetupViewController;

        public MenuButtonManager(MainFlowCoordinator mainFlowCoordinator, AmblyopiaFlowCoordinator amblyopiaFlowCoordinator, GameplaySetupViewController gameplaySetupViewController)
        {
            _mainFlowCoordinator = mainFlowCoordinator;
            _amblyopiaFlowCoordinator = amblyopiaFlowCoordinator;
            _gameplaySetupViewController = gameplaySetupViewController;

            _menuButton = new MenuButton("Amblyopia Settings", "Change Amblyopia-related settings here!", ShowFlow, true);
        }

        public void Initialize()
        {
            MenuButtons.instance.RegisterButton(_menuButton);
        }

        public void Dispose()
        {
            if (MenuButtons.IsSingletonAvailable)
            {
                MenuButtons.instance.UnregisterButton(_menuButton);
            }
        }

        private void ShowFlow()
        {
            _parentFlowCoordinator = _mainFlowCoordinator.YoungestChildFlowCoordinatorOrSelf();
            _parentFlowCoordinator.PresentFlowCoordinator(_amblyopiaFlowCoordinator);
        }
    }
}
