using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace SBabchuk.UI
{
    public class ViewportRayCastTargetController : MonoBehaviour
    {
        Image img;

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
        }

        private void Awake()
        {
            img = GetComponent<Image>();
        }

        private void Start()
        {
            SetRaycastTarget(false);
        }

        void SetRaycastTarget(bool _value)
        {
            img.raycastTarget = _value;
        }
    }
}
