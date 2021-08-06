using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Utility
{
    public class UtHolder : MonoBehaviour
    {
        public GameObject AddUtility(string utName)
        {
           return Instantiate(new GameObject(utName),gameObject.transform) as GameObject;
        }
    }
}
