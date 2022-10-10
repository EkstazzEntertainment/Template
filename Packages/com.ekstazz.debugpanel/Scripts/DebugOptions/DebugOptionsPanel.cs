namespace Ekstazz.Debug.DebugOptions
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using Zenject;

    
    public abstract class DebugOptionsPanel<TOption> : MonoBehaviour where TOption : IDebugOption
    {
        [Inject] private DiContainer DiContainer { get; set; }
        
        [SerializeField] private Transform footer;
        [SerializeField] private RectTransform panelRect;

        protected abstract DebugOptions<TOption> DebugOptions { get; }
        protected abstract DebugOptionView<TOption> DebugOptionView { get; }

        private bool HasOptions => DebugOptions.Options.Any();

        private List<DebugOptionView<TOption>> currentOptions = new List<DebugOptionView<TOption>>();

        
        protected void Start()
        {
            if (!HasOptions)
            {
                Destroy(gameObject);
            }
            else
            {
                SetupOptions();
            }
        }

        private void SetupOptions()
        {
            CreateOptions();
            ResizeView();
            PutFooterInTheEnd();
        }

        private void CreateOptions()
        {
            currentOptions.Clear();
            foreach (var runtimeOption in DebugOptions.Options)
            {
                var option = CreateOption();
                option.Init(runtimeOption);
                currentOptions.Add(option);
            }
        }

        private DebugOptionView<TOption> CreateOption()
        {
            return DiContainer.InstantiatePrefabForComponent<DebugOptionView<TOption>>(DebugOptionView, transform);
        }

        private void ResizeView()
        {
            var optionHeight = DebugOptionView.GetComponent<RectTransform>().rect.height;

            var size = panelRect.sizeDelta;
            size.y += optionHeight * DebugOptions.Options.Count;
            panelRect.sizeDelta = size;
        }

        private void PutFooterInTheEnd()
        {
            footer.SetAsLastSibling();
        }

        public void Reset()
        {
            foreach (var optionView in currentOptions)
            {
                optionView.ResetToDefault();
            }
        }
    }
}