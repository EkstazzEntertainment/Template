namespace Ekstazz.Shared.Ui.Components
{
    using Ekstazz.Ui;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    [RequireComponent(typeof(Button))]
    public abstract class WindowOpener<T> : MonoBehaviour where T : Window
    {
        [Inject]
        public UiBuilder UiBuilder { get; set; }

        protected virtual IWindowOptions Options { get; }

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                if (Options == null)
                {
                    UiBuilder.CreateWindow<T>();
                }
                else
                {
                    UiBuilder.CreateWindowWithOptions<T>(Options);
                }
            });
        }
    }
}
