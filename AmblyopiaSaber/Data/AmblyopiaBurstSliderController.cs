using AmblyopiaSaber.Config;
using AmblyopiaSaber.Utils;
using SiraUtil.Objects;
using UnityEngine;
using Zenject;

namespace AmblyopiaSaber.Data
{
    public class AmblyopiaBurstSliderController : MonoBehaviour, INoteControllerDidInitEvent
    {
        private PluginConfig _pluginConfig;

        protected Transform noteCube;
        private BurstSliderGameNoteController _burstSliderGameNoteController;

        protected GameObject activeNote;
        protected SiraPrefabContainer container;
        protected SiraPrefabContainer.Pool activePool;

        [Inject]
        internal void Init(PluginConfig pluginConfig)
        {
            _pluginConfig = pluginConfig;
            _burstSliderGameNoteController = GetComponent<BurstSliderGameNoteController>();

            _burstSliderGameNoteController.didInitEvent.Add(this);

            noteCube = _burstSliderGameNoteController.gameObject.transform.Find("NoteCube");
        }

        public void HandleNoteControllerDidInit(NoteControllerBase noteController)
        {
            NoteUtils.ApplyConfigToNote(_pluginConfig, noteController, true);
        }
    }
}
