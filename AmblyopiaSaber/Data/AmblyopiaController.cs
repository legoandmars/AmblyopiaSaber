using AmblyopiaSaber.Config;
using AmblyopiaSaber.Utils;
using SiraUtil.Objects;
using UnityEngine;
using Zenject;

namespace AmblyopiaSaber.Data
{
    public class AmblyopiaController : MonoBehaviour, INoteControllerDidInitEvent
    {
        private PluginConfig _pluginConfig;

        protected Transform noteCube;
        private GameNoteController _gameNoteController;

        protected GameObject activeNote;
        protected SiraPrefabContainer container;
        protected SiraPrefabContainer.Pool activePool;

        [Inject]
        internal void Init(PluginConfig pluginConfig)
        {
            _pluginConfig = pluginConfig;
            _gameNoteController = GetComponent<GameNoteController>();

            _gameNoteController.didInitEvent.Add(this);

            noteCube = _gameNoteController.gameObject.transform.Find("NoteCube");
        }

        public void HandleNoteControllerDidInit(NoteControllerBase noteController)
        {
            NoteUtils.ApplyConfigToNote(_pluginConfig, noteController, false);
        }
    }
}
