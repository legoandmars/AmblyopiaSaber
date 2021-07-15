using BeatSaberMarkupLanguage;
using HMUI;
using Zenject;

namespace AmblyopiaSaber.UI
{
    internal class AmblyopiaFlowCoordinator : FlowCoordinator
    {
        private MainFlowCoordinator _mainFlow;
        private AmblyopiaSettingsView _ambylopiaSettingsView;

        [Inject]
        public void Construct(MainFlowCoordinator mainFlow, AmblyopiaSettingsView ambylopiaSettingsView)
        {
            _mainFlow = mainFlow;
            _ambylopiaSettingsView = ambylopiaSettingsView;
        }
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if (firstActivation)
            {
                SetTitle("Amblyopia Settings");
                showBackButton = true;
            }
            ProvideInitialViewControllers(_ambylopiaSettingsView);
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            _mainFlow.DismissFlowCoordinator(this, null);
        }
    }
}
