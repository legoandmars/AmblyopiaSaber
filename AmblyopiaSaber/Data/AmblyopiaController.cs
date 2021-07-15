using AmblyopiaSaber.Config;
using SiraUtil.Objects;
using UnityEngine;
using Zenject;

namespace AmblyopiaSaber.Data
{
    public class AmblyopiaController : MonoBehaviour, INoteControllerNoteWasCutEvent, INoteControllerNoteWasMissedEvent, INoteControllerDidInitEvent, INoteControllerNoteDidDissolveEvent
    {
        private PluginConfig _pluginConfig;

        protected Transform noteCube;
        private GameNoteController _gameNoteController;
        // private CustomNoteColorNoteVisuals _customNoteColorNoteVisuals;

        protected GameObject activeNote;
        protected SiraPrefabContainer container;
        protected SiraPrefabContainer.Pool activePool;

        [Inject]
        internal void Init(PluginConfig pluginConfig)
        {
            _pluginConfig = pluginConfig;
            gameObject.GetComponent<ColorNoteVisuals>().didInitEvent += AmblyopiaController_didInitEvent;
            _gameNoteController = GetComponent<GameNoteController>();

            _gameNoteController.didInitEvent.Add(this);
            _gameNoteController.noteWasCutEvent.Add(this);
            _gameNoteController.noteWasMissedEvent.Add(this);
            _gameNoteController.noteDidDissolveEvent.Add(this);

            noteCube = _gameNoteController.gameObject.transform.Find("NoteCube");
        }

        private void AmblyopiaController_didInitEvent(ColorNoteVisuals arg1, NoteControllerBase noteController)
        {

        }

        public void HandleNoteControllerNoteWasMissed(NoteController nc)
        {

        }

        public void HandleNoteControllerDidInit(NoteControllerBase noteController)
        {
            foreach (Transform child in noteController.gameObject.transform?.GetChild(0))
            {
                if (child != null && child.name.ToLower().StartsWith("customnote"))
                {
                    // found the thing
                    foreach(Transform child2 in child?.GetChild(0)?.GetChild(0))
                    {
                        if (child2 == null) continue;
                        Material mat = child2.GetComponent<Renderer>()?.sharedMaterial;

                        float visibility = -1;
                        Vector3 transformPosition = Vector3.zero;

                        if (child2.name == "NoteCube")
                        {
                            if (noteController.noteData.colorType == ColorType.ColorA)
                            {
                                // Left note, left eye
                                visibility = _pluginConfig.LeftNoteData.LeftEyeVisibility;
                                transformPosition = _pluginConfig.LeftNoteData.LeftEyeAdjust;
                            }
                            else if (noteController.noteData.colorType == ColorType.ColorB)
                            {
                                // Right note, right eye
                                visibility = _pluginConfig.RightNoteData.RightEyeVisibility;
                                transformPosition = _pluginConfig.RightNoteData.RightEyeAdjust;
                            }
                        }
                        else if(child2.name == "NoteCube2")
                        {
                            if (noteController.noteData.colorType == ColorType.ColorA)
                            {
                                // Left note, right eye
                                visibility = _pluginConfig.LeftNoteData.RightEyeVisibility;
                                transformPosition = _pluginConfig.LeftNoteData.RightEyeAdjust;
                            }
                            else if (noteController.noteData.colorType == ColorType.ColorB)
                            {
                                // Right note, left eye
                                visibility = _pluginConfig.RightNoteData.LeftEyeVisibility;
                                transformPosition = _pluginConfig.RightNoteData.LeftEyeAdjust;
                            }
                        }

                        if(visibility != -1)
                        {
                            mat.SetFloat("_Glow", visibility);
                            child2.localPosition = Vector3.zero;
                            child2.position += transformPosition;
                            foreach(Transform innerChild in child2) if(innerChild != null && innerChild.gameObject.activeSelf) innerChild.GetComponent<Renderer>().sharedMaterial.SetFloat("_Glow", visibility);
                        }
                    }
                }
            }
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
