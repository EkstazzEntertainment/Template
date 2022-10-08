namespace Ekstazz.Debug.DebugOptions
{
    using TMPro;
    using UnityEngine;

    public abstract class DebugOptionView<TOption> : MonoBehaviour where TOption : IDebugOption
    {
        [SerializeField]
        protected TMP_Text optionNameText;
        
        protected TOption Option;

        public abstract void ResetToDefault();

        public virtual void Init(TOption runtimeOption)
        {
            Option = runtimeOption;
            optionNameText.SetText(runtimeOption.Name);
        }
    }
}