using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Utility
{
    public static class UtTransform
    {
        public static void Lerp(this Transform position, Vector3 v3, float speed)
        {
            position.position = Vector3.Lerp(position.position, v3, Time.deltaTime * speed);
        }

        public static void Slerp(this Transform position, Vector3 v3, float speed)
        {
            position.position = Vector3.Slerp(position.position, v3, Time.deltaTime * speed);
        }

        public static void LerpEvenly(this Transform position, Vector3 v3, float speed)
        {
            var dist = (position.position - v3).magnitude;
            position.position = Vector3.Lerp(position.position, v3, (Time.deltaTime * speed) / dist);
        }

        public static void SlerpEvenly(this Transform position, Vector3 v3, float speed)
        {
            var dist = (position.position - v3).magnitude;
            position.position = Vector3.Slerp(position.position, v3, (Time.deltaTime * speed) / dist);
        }

        public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 normal)
        {
            return Mathf.Atan2(Vector3.Dot(normal, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
        }
    }
}
