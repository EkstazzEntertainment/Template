namespace Ekstazz.Debug.DebugOptions
{
    using UnityEngine;
    using UnityEngine.UI;

    public class ToggleOptionView : DebugOptionView<ToggleOption>
    {
        [SerializeField]
        private Toggle toggle;

        [SerializeField]
        private Animator animator;

        private static readonly int ToggleAnimation = Animator.StringToHash("Toggle");

        public override void ResetToDefault()
        {
            toggle.isOn = Option.Default;
        }

        public override void Init(ToggleOption runtimeOption)
        {
            base.Init(runtimeOption);
            
            toggle.isOn = runtimeOption.Value;
            toggle.onValueChanged.AddListener(OnValueChanged);
            SetAnimatorState(runtimeOption.Value);
        }

        private void SetAnimatorState(bool value)
        {
            animator.Play(value ? "On" : "Off", 0, 1);
        }

        private void OnValueChanged(bool value)
        {
            Option.UpdateWith(value);
            animator.SetTrigger(ToggleAnimation);
        }
    }
}