using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace Core.Utility
{
    public class UtStorage : MonoBehaviour
    {
        private ArrayList utPatameters;

        public void Initialize() => utPatameters = new ArrayList();

        public void AddParameter<T>(string parameterName, T defaultValue = default)
        {
            var patameter = new UtParameter<T>(parameterName,defaultValue);
            utPatameters.Add(patameter);
        }

        public void ParameterUpdate<T>(string name) => SetParameter(name, GetParameter<T>(name));

        public T GetParameter<T>(string name) => FindParameter<T>(name).Parameter;

        public void SetParameter<T>(string name, T newValue) => FindParameter<T>(name).SetValue(newValue);

        public void OnParameterChange<T>(string name, Action<T> onChange) => FindParameter<T>(name).OnParameterChange += onChange;

        private UtParameter<T> FindParameter<T>(string name)
        {
            var parameter = utPatameters.OfType<UtParameter<T>>().ToList().Find(x => x.Name == name);
            return parameter;
        }
    }    
}