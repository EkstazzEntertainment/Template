namespace Ekstazz.Debug.DebugOptions
{
    using UnityEngine;
    using Zenject;

    
    public class ToggleOptionsPanel : DebugOptionsPanel<ToggleOption>
    {
        [Inject] private ToggleOptions ToggleOptions { get; set; }

        [SerializeField] private ToggleOptionView toggleOptionViewPrefab;

        protected override DebugOptions<ToggleOption> DebugOptions => ToggleOptions;
        protected override DebugOptionView<ToggleOption> DebugOptionView => toggleOptionViewPrefab;
    }
}