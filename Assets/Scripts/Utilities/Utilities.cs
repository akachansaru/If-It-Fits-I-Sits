using UnityEngine;
using System.Collections;

namespace UtilityFunctions
{
    public class Utilities
    {
        public static float DistanceZ(Transform t1, Transform t2)
        {
            return Mathf.Abs(t1.position.z - t2.position.z);
        }

        public static float DistanceZ(Vector3 v1, Vector3 v2)
        {
            return Mathf.Abs(v1.z - v2.z);
        }
    }
}
