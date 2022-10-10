namespace Ekstazz.Debug.DebugOptions
{
    using System;
    using UnityEngine;

    
    public class ToggleOption : IDebugOption
    {
        public string Name { get; set; }
        public bool Default { get; set; }
        public bool Value { get; set; }
        public Action<bool> OnValueChanged { get; set; }
        
        
        public void Init()
        {
            if (PlayerPrefs.HasKey(Name))
            {
                var isOn = PlayerPrefs.GetInt(Name) == 1;
                UpdateWith(isOn);
            }
            else
            {
                UpdateWith(Default);
            }
        }

        public void UpdateWith(bool value)
        {
            Value = value;
            OnValueChanged?.Invoke(Value);
            PlayerPrefs.SetInt(Name, value ? 1 : 0);
        }
    }
}