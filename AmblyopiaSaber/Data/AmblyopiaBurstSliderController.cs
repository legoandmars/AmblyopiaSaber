using AmblyopiaSaber.Config;
using AmblyopiaSaber.Utils;
using SiraUtil.Objects;
using UnityEngine;
using Zenject;

namespace AmblyopiaSaber.Data
{
    public class AmblyopiaBurstSliderController : MonoBehaviour, INoteControllerNoteWasCutEvent, INoteControllerNoteWasMissedEvent, INoteControllerDidInitEvent, INoteControllerNoteDidDissolveEvent
    {
        private PluginConfig _pluginConfig;

        protected Transform noteCube;
        private BurstSliderGameNoteController _burstSliderGameNoteController;
        // private CustomNoteColorNoteVisuals _customNoteColorNoteVisuals;

        protected GameObject activeNote;
        protected SiraPrefabContainer container;
        protected SiraPrefabContainer.Pool activePool;

        [Inject]
        internal void Init(PluginConfig pluginConfig)
        {
            _pluginConfig = pluginConfig;
            gameObject.GetComponent<ColorNoteVisuals>().didInitEvent += AmblyopiaController_didInitEvent;
            _burstSliderGameNoteController = GetComponent<BurstSliderGameNoteController>();

            _burstSliderGameNoteController.didInitEvent.Add(this);
            _burstSliderGameNoteController.noteWasCutEvent.Add(this);
            _burstSliderGameNoteController.noteWasMissedEvent.Add(this);
            _burstSliderGameNoteController.noteDidDissolveEvent.Add(this);

            noteCube = _burstSliderGameNoteController.gameObject.transform.Find("NoteCube");
        }

        private void AmblyopiaController_didInitEvent(ColorNoteVisuals arg1, NoteControllerBase noteController)
        {

        }

        public void HandleNoteControllerNoteWasMissed(NoteController nc)
        {

        }

        public void HandleNoteControllerDidInit(NoteControllerBase noteController)
        {
            NoteUtils.ApplyConfigToNote(_pluginConfig, noteController, true);
        }

        public void HandleNoteControllerNoteWasCut(NoteController nc, in NoteCutInfo _)
        {
            HandleNoteControllerNoteWasMissed(nc);
        }

        public void HandleNoteControllerNoteDidDissolve(NoteController noteController)
        {
            HandleNoteControllerNoteWasMissed(noteController);
        }


    }
}
