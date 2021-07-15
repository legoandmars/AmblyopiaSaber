using AmblyopiaSaber.Config;
using AmblyopiaSaber.Data;
using AmblyopiaSaber.Utils;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.ViewControllers;
using IPA.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace AmblyopiaSaber.UI
{
    internal class AmblyopiaSettingsView : BSMLResourceViewController
    {
        public override string ResourceName => "AmblyopiaSaber.UI.Views.settings.bsml";
        private PluginConfig _pluginConfig;

        private GameplaySetupViewController _gameplaySetupViewController = null;

        [Inject]
        public void Construct(PluginConfig pluginConfig, GameplaySetupViewController gameplaySetupViewController)
        {
            _pluginConfig = pluginConfig;
            _gameplaySetupViewController = gameplaySetupViewController;
        }

        [UIParams]
        internal BeatSaberMarkupLanguage.Parser.BSMLParserParams parserParams = null;

        [UIAction("update-notes")]
        public void UpdateNotes(float input)
        {
            ApplySettingsToNotes();
        }

        private bool ignoringWarnings = false;
        async void Popup(int ms)
        {
            await Task.Run(() => Task.Delay(ms));
            if(CustomNotes.Utilities.LayerUtils.pluginConfig != null)
            {
                Plugin.Log.Info("LAST NOTE IS..");
                Plugin.Log.Info(CustomNotes.Utilities.LayerUtils.pluginConfig.LastNote);
                if (!ignoringWarnings && !CustomNotes.Utilities.LayerUtils.pluginConfig.LastNote.ToLower().StartsWith("seperate"))
                {
                    parserParams.EmitEvent("open-warningModal");
                    // warning msg to say to apply the note
                }
            }
        }

        [UIAction("warningDontShowPressed")]
        internal void WarningDontShowPressed()
        {
            ignoringWarnings = true;
            parserParams.EmitEvent("close-warningModal");
        }

        [UIAction("warningOKPressed")]
        internal void WarningOKPressed()
        {
            parserParams.EmitEvent("close-warningModal");
        }

        // preview code from modeldownloader, which is a modified version of customnotes preview code
        // https://i.imgur.com/v6O57ll.png

        private GameObject _previewHolder;

        internal void ClearData()
        {
            if (_previewHolder != null)
            {
                Destroy(_previewHolder);
                _previewHolder = null;
            }
        }
        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
            CreateNotePreview(NoteUtils.notePreview);
            Popup(1000);
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            ClearData();
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
        }

        private void ApplySettingsToNotes()
        {
            ApplySettingToSubNotes(noteLeft, _pluginConfig.LeftNoteData, false);
            ApplySettingToSubNotes(noteDotLeft, _pluginConfig.LeftNoteData, false);
            ApplySettingToSubNotes(noteRight, _pluginConfig.RightNoteData, true);
            ApplySettingToSubNotes(noteDotRight, _pluginConfig.RightNoteData, true);
        }

        private void ApplySettingToSubNotes(GameObject subNote, NoteEyeData data, bool isRight)
        {
            foreach (Transform child in subNote.transform.GetChild(0))
            {
                if(child.name == "NoteCube")
                {
                    if (isRight) ApplySettingToNote(child.gameObject, data.RightEyeVisibility, data.RightEyeAdjust);
                    else ApplySettingToNote(child.gameObject, data.LeftEyeVisibility, data.LeftEyeAdjust);
                }
                else if(child.name == "NoteCube2")
                {
                    if (!isRight) ApplySettingToNote(child.gameObject, data.RightEyeVisibility, data.RightEyeAdjust);
                    else ApplySettingToNote(child.gameObject, data.LeftEyeVisibility, data.LeftEyeAdjust);
                }
            }
        }

        private void ApplySettingToNote(GameObject noteToApply, float visibility, Vector3 offset)
        {
            Material mat = noteToApply.GetComponent<Renderer>()?.sharedMaterial;
            mat.SetFloat("_Glow", visibility);
            noteToApply.transform.localPosition = Vector3.zero;
            noteToApply.transform.position += offset;
            foreach (Transform innerChild in noteToApply.transform) if (innerChild.gameObject.activeSelf) innerChild.GetComponent<Renderer>().sharedMaterial.SetFloat("_Glow", visibility);
        }

        GameObject noteLeft;
        GameObject noteDotLeft;
        GameObject noteRight;
        GameObject noteDotRight;

        // NOTE PREVIEW UTILS
        private void CreateNotePreview(GameObject note)
        {
            _previewHolder = new GameObject();

            Vector3 leftDotPos = new Vector3(-7f, 0f, 0.0f);
            Vector3 leftArrowPos = new Vector3(-7f, 3f, 0.0f);
            Vector3 rightDotPos = new Vector3(7f, 0f, 0.0f);
            Vector3 rightArrowPos = new Vector3(7f, 3f, 0.0f);

            GameObject NoteLeft = note.transform.Find("NoteLeft").gameObject;
            GameObject NoteRight = note.transform.Find("NoteRight").gameObject;
            Transform NoteDotLeftTransform = note.transform.Find("NoteDotLeft");
            Transform NoteDotRightTransform = note.transform.Find("NoteDotRight");
            GameObject NoteDotLeft = NoteDotLeftTransform != null ? NoteDotLeftTransform.gameObject : NoteLeft;
            GameObject NoteDotRight = NoteDotRightTransform != null ? NoteDotRightTransform.gameObject : NoteRight;
            //_previewHolder.transform.Rotate(0.0f, 60.0f, 0.0f);
            _previewHolder.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            //_previewHolder.transform.position = new Vector3(2.90f, 0.9f, 1.85f);
            _previewHolder.transform.position = new Vector3(0f, 0.5f, 3.5f);

            noteLeft = CreatePreviewNote(NoteLeft, transform, leftArrowPos);
            noteDotLeft = CreatePreviewNote(NoteDotLeft, transform, leftDotPos);
            noteRight = CreatePreviewNote(NoteRight, transform, rightArrowPos);
            noteDotRight = CreatePreviewNote(NoteDotRight, transform, rightDotPos);

            ColorizeCustomNote(_gameplaySetupViewController.colorSchemesSettings.GetSelectedColorScheme().saberAColor, 1, noteLeft);
            ColorizeCustomNote(_gameplaySetupViewController.colorSchemesSettings.GetSelectedColorScheme().saberAColor, 1, noteDotLeft);
            ColorizeCustomNote(_gameplaySetupViewController.colorSchemesSettings.GetSelectedColorScheme().saberBColor, 1, noteRight);
            ColorizeCustomNote(_gameplaySetupViewController.colorSchemesSettings.GetSelectedColorScheme().saberBColor, 1, noteDotRight);

            ApplySettingsToNotes();
        }
        private GameObject CreatePreviewNote(GameObject note, Transform transform, Vector3 localPosition)
        {
            GameObject noteObject = InstantiateGameObject(note, transform);
            PositionPreviewNote(localPosition, noteObject);
            return noteObject;
        }

        private GameObject InstantiateGameObject(GameObject gameObject, Transform transform = null)
        {
            if (gameObject)
            {
                return transform ? Instantiate(gameObject, transform) : Instantiate(gameObject);
            }
            return null;
        }
        private void PositionPreviewNote(Vector3 vector, GameObject noteObject)
        {
            if (noteObject && vector != null)
            {
                noteObject.transform.SetParent(_previewHolder.transform);
                noteObject.transform.localPosition = vector;
                noteObject.transform.localScale = new Vector3(1, 1, 1);
                //noteObject.transform.Rotate(0.0f, 60f, 0.0f);
                //noteObject.transform.localRotation = Quaternion.identity;
            }
        }
        public static void ColorizeCustomNote(Color color, float colorStrength, GameObject noteObject)
        {
            Type disableNoteColorType = null;
            disableNoteColorType = PluginManager.EnabledPlugins.Where(x => x.Name == "CustomNotes").FirstOrDefault().Assembly.GetType("CustomNotes.DisableNoteColorOnGameobject");
            if (!noteObject || color == null)
            {
                return;
            }

            Color noteColor = color * colorStrength;

            IEnumerable<Transform> childTransforms = noteObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform childTransform in childTransforms)
            {
                bool colorDisabled = false;

                if (disableNoteColorType != null)
                {
                    colorDisabled = childTransform.GetComponent(disableNoteColorType);
                }

                if (!colorDisabled)
                {
                    Renderer childRenderer = childTransform.GetComponent<Renderer>();
                    if (childRenderer)
                    {
                        childRenderer.material.SetColor("_Color", noteColor);
                    }
                }
            }
        }


        // pain and suffering

        [UIValue("left-note-left-eye-visibility")]
        public float leftNoteLeftEyeVisibility
        {
            get { return _pluginConfig.LeftNoteData.LeftEyeVisibility; }
            set
            {
                _pluginConfig.LeftNoteData.LeftEyeVisibility = value;
            }
        }

        [UIValue("left-note-right-eye-visibility")]
        public float leftNoteRightEyeVisibility
        {
            get { return _pluginConfig.LeftNoteData.RightEyeVisibility; }
            set
            {
                _pluginConfig.LeftNoteData.RightEyeVisibility = value;
            }
        }

        [UIValue("right-note-left-eye-visibility")]
        public float rightNoteLeftEyeVisibility
        {
            get { return _pluginConfig.RightNoteData.LeftEyeVisibility; }
            set
            {
                _pluginConfig.RightNoteData.LeftEyeVisibility = value;
            }
        }

        [UIValue("right-note-right-eye-visibility")]
        public float rightNoteRightEyeVisibility
        {
            get { return _pluginConfig.RightNoteData.RightEyeVisibility; }
            set
            {
                _pluginConfig.RightNoteData.RightEyeVisibility = value;
            }
        }

        [UIValue("ll-x")]
        public float llx
        {
            get { return _pluginConfig.LeftNoteData.LeftEyeAdjust.x; }
            set
            {
                _pluginConfig.LeftNoteData.LeftEyeAdjust.x = value;
            }
        }

        [UIValue("ll-y")]
        public float lly
        {
            get { return _pluginConfig.LeftNoteData.LeftEyeAdjust.y; }
            set
            {
                _pluginConfig.LeftNoteData.LeftEyeAdjust.y = value;
            }
        }

        [UIValue("ll-z")]
        public float llz
        {
            get { return _pluginConfig.LeftNoteData.LeftEyeAdjust.z; }
            set
            {
                _pluginConfig.LeftNoteData.LeftEyeAdjust.z = value;
            }
        }

        [UIValue("lr-x")]
        public float lrx
        {
            get { return _pluginConfig.LeftNoteData.RightEyeAdjust.x; }
            set
            {
                _pluginConfig.LeftNoteData.RightEyeAdjust.x = value;
            }
        }

        [UIValue("lr-y")]
        public float lry
        {
            get { return _pluginConfig.LeftNoteData.RightEyeAdjust.y; }
            set
            {
                _pluginConfig.LeftNoteData.RightEyeAdjust.y = value;
            }
        }

        [UIValue("lr-z")]
        public float lrz
        {
            get { return _pluginConfig.LeftNoteData.RightEyeAdjust.z; }
            set
            {
                _pluginConfig.LeftNoteData.RightEyeAdjust.z = value;
            }
        }

        [UIValue("rl-x")]
        public float rlx
        {
            get { return _pluginConfig.RightNoteData.LeftEyeAdjust.x; }
            set
            {
                _pluginConfig.RightNoteData.LeftEyeAdjust.x = value;
            }
        }

        [UIValue("rl-y")]
        public float rly
        {
            get { return _pluginConfig.RightNoteData.LeftEyeAdjust.y; }
            set
            {
                _pluginConfig.RightNoteData.LeftEyeAdjust.y = value;
            }
        }

        [UIValue("rl-z")]
        public float rlz
        {
            get { return _pluginConfig.RightNoteData.LeftEyeAdjust.z; }
            set
            {
                _pluginConfig.RightNoteData.LeftEyeAdjust.z = value;
            }
        }

        [UIValue("rr-x")]
        public float rrx
        {
            get { return _pluginConfig.RightNoteData.RightEyeAdjust.x; }
            set
            {
                _pluginConfig.RightNoteData.RightEyeAdjust.x = value;
            }
        }

        [UIValue("rr-y")]
        public float rry
        {
            get { return _pluginConfig.RightNoteData.RightEyeAdjust.y; }
            set
            {
                _pluginConfig.RightNoteData.RightEyeAdjust.y = value;
            }
        }

        [UIValue("rr-z")]
        public float rrz
        {
            get { return _pluginConfig.RightNoteData.RightEyeAdjust.z; }
            set
            {
                _pluginConfig.RightNoteData.RightEyeAdjust.z = value;
            }
        }

    }
}
