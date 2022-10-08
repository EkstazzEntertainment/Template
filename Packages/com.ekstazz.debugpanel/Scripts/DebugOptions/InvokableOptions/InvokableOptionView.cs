namespace Ekstazz.Debug.DebugOptions
{
    using UnityEngine;

    public class InvokableOptionView : DebugOptionView<InvokableOption>
    {
        [SerializeField]
        private NamedButton invokableButtonPrefab;

        [SerializeField]
        private Transform buttonsParent;

        public override void ResetToDefault()
        {
            // No state reset required
        }

        public override void Init(InvokableOption runtimeOption)
        {
            base.Init(runtimeOption);
            
            foreach (var namedInvocation in runtimeOption.InvocationsList)
            {
                var button = GetNewButton();
                button.InitWith(namedInvocation.Name, namedInvocation.OnInvoked);
            }
        }

        private NamedButton GetNewButton()
        {
            return Instantiate(invokableButtonPrefab, buttonsParent, false);
        }
    }
}