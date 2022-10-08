namespace Ekstazz.Debug.DebugOptions
{
    using UnityEngine;
    using Zenject;

    public class InvokableOptionsPanel : DebugOptionsPanel<InvokableOption>
    {
        [Inject]
        public InvokableOptions InvokableOptions { get; set; }

        [SerializeField]
        private InvokableOptionView invokableOptionViewPrefab;

        protected override DebugOptions<InvokableOption> DebugOptions => InvokableOptions;
        protected override DebugOptionView<InvokableOption> DebugOptionView => invokableOptionViewPrefab;
    }
}