namespace Ekstazz.Debug.DebugOptions
{
    using UnityEngine;
    using Zenject;

    public class RuntimeOptionsPanel : DebugOptionsPanel<RuntimeOption>
    {
        [Inject]
        private RuntimeOptions RuntimeOptions { get; set; }

        [SerializeField]
        private RuntimeOptionView runtimeOptionViewPrefab;

        protected override DebugOptions<RuntimeOption> DebugOptions => RuntimeOptions;
        protected override DebugOptionView<RuntimeOption> DebugOptionView => runtimeOptionViewPrefab;
    }
}