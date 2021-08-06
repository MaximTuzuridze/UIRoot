using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    public abstract class UIWindowElement : MonoBehaviour
    {
        public bool IsDebugElement = false;

        public string ElementId => Id();

        [HideInInspector]
        public UIWindow Parent;

        private string elementName;

        public virtual void OnShowParentWindow()
        {
        }

        public virtual void OnInitialize()
        {
        }

        public void Initialize(UIWindow parent)
        {
            Parent = parent;
            OnInitialize();
            if(IsDebugElement && !UIRoot.Instance.IsDebugModeActive)
                gameObject.SetActive(false);
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        private string Id()
        {
            if (!string.IsNullOrEmpty(elementName))
                return elementName;
            else
            {
                elementName = this.name;
            }

            return elementName;
        }
    }
}

