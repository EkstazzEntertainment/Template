namespace Ekstazz.Debug.DebugOptions
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class RuntimeOptionView : DebugOptionView<RuntimeOption>
    {
        [SerializeField]
        private Slider slider;

        [SerializeField]
        private TMP_Text optionValue;

        [SerializeField]
        private TMP_Text min;

        [SerializeField]
        private TMP_Text max;

        public override void ResetToDefault()
        {
            slider.value = Option.Default;
        }

        private void OnValueChanged(float value)
        {
            Option.UpdateWith(value);
            optionValue.text = $"{value:F2}";
        }

        public override void Init(RuntimeOption runtimeOption)
        {
            base.Init(runtimeOption);
            
            slider.minValue = runtimeOption.Min;
            slider.maxValue = runtimeOption.Max;

            min.text = $"{runtimeOption.Min:F}";
            max.text = $"{runtimeOption.Max:F}";
            
            slider.onValueChanged.AddListener(OnValueChanged);
            slider.value = runtimeOption.Value;
            OnValueChanged(runtimeOption.Value);
        }
    }
}