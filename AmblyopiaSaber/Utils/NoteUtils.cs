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
    }
}
