using AmblyopiaSaber.Config;
using System.Reflection;
using UnityEngine;

namespace AmblyopiaSaber.Utils
{
    public static class NoteUtils
    {
        public static GameObject notePreview;
        public static void LoadEmbeddedBundles()
        {
            // cursed one liner
            notePreview = AssetBundle.LoadFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("AmblyopiaSaber.Resources.embedded_notes.bloq")).LoadAsset<GameObject>("assets/_customnote.prefab");
            UnityEngine.Object.DontDestroyOnLoad(notePreview);
           
        }

        public static void ApplyConfigToNote(PluginConfig _pluginConfig, NoteControllerBase noteController, bool isChain = false)
        {
            foreach (Transform child in noteController.gameObject.transform?.GetChild(0))
            {
                if (child != null && child.name.ToLower().StartsWith("customnote"))
                {
                    // found the thing
                    foreach (Transform child2 in isChain ? child?.GetChild(0)?.GetChild(0)?.GetChild(0) : child?.GetChild(0)?.GetChild(0))
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
                        else if (child2.name == "NoteCube2")
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

                        if (visibility != -1)
                        {
                            mat.SetFloat("_Glow", visibility);
                            child2.localPosition = Vector3.zero;
                            child2.position += transformPosition;
                            foreach (Transform innerChild in child2) if (innerChild != null && innerChild.gameObject.activeSelf) innerChild.GetComponent<Renderer>().sharedMaterial.SetFloat("_Glow", visibility);
                        }
                    }
                }
            }
        }
    }
}
