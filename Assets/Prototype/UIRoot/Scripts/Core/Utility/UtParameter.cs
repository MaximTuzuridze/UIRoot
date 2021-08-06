using System;

namespace Core.Utility {
    [System.Serializable]
    public class UtParameter<T>
    {
        private const string ParameterPlayerPrefKey = "PARAMETER_";
        private const string ParameterExist = "PARAM_EXIST";
        public T Parameter;
        public string Name;

        public Action<T> OnParameterChange;

        private bool isParameterExistInStorage;

        public UtParameter(string name, T defaultValue = default)
        {
            Name = name;
            isParameterExistInStorage = UtPlayerPrefs.Load<bool>(ParameterExist + Name);
            if (isParameterExistInStorage)
                Load();
            else
                Parameter = defaultValue;
        }

        public T GetValue()
        {
            return Parameter;
        }

        public void SetValue(T value)
        {
            Parameter = value;
            OnParameterChange?.Invoke(value);
            Save();
        }

        private void Save()
        {
            isParameterExistInStorage = true;
            isParameterExistInStorage.Save(ParameterExist + Name);
            Parameter.Save(ParameterPlayerPrefKey + Name);
        }

        private void Load()
        {
            SetValue(UtPlayerPrefs.Load<T>(ParameterPlayerPrefKey + Name));
        }
    }
}