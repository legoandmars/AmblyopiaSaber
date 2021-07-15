using System;
using UnityEngine;

namespace AmblyopiaSaber.Data
{
    [Serializable]
    public class NoteEyeData
    {
        public float LeftEyeVisibility = 1f;
        public float RightEyeVisibility = 1f;
        public Vector3 LeftEyeAdjust = Vector3.zero;
        public Vector3 RightEyeAdjust = Vector3.zero;
    }
}
