using AmblyopiaSaber.Data;

namespace AmblyopiaSaber.Config
{
    public class PluginConfig
    {
        public virtual NoteEyeData LeftNoteData { get; set; } = new NoteEyeData();
        public virtual NoteEyeData RightNoteData { get; set; } = new NoteEyeData();

        public virtual bool LoadedYet { get; set; } = false;
    }
}
