using System;
using UnityEngine;

namespace AmblyopiaSaber.Data
{
    [Serializable]
    public class EyeData
    {
        public float Visibility;
        public Vector3 PositionAdjust;

        public EyeData(float _visibility)
        {
            Visibility = _visibility;
            PositionAdjust = new Vector3(0,0,0);
        }
    }
}
