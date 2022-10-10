namespace Ekstazz.Debug.DebugOptions
{
    using System;
    using UnityEngine;

    
    public class RuntimeOption : IDebugOption
    {
        public string Name { get; set; }
        public Action<float> OnValueChanged { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }
        public float Default { get; set; }
        public float Value { get; private set; }

        
        public void Init()
        {
            if (PlayerPrefs.HasKey(Name))
            {
                UpdateWith(PlayerPrefs.GetFloat(Name));
            }
            else
            {
                UpdateWith(Default);
            }
        }

        public void UpdateWith(float value)
        {
            Value = Mathf.Clamp(value, Min, Max);
            OnValueChanged?.Invoke(Value);
            PlayerPrefs.SetFloat(Name, Value);
        }
    }
}