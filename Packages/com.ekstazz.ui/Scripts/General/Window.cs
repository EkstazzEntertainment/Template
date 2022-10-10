namespace Ekstazz.Ui
{
    using System;
    using Faders;
    using UnityEngine;

    
    public abstract class Window : UiElement
    {
        [SerializeField] private bool showBackground;
        
        public event Action Opening;
        public event Action Opened;
        public event Action Closing;
        public event Action Closed;

        public bool ShowBack => showBackground;
        public virtual bool CloseOnBackgroundClick => false;
        public virtual bool TransparentBack => false;
        public virtual bool IsStatic => false;
        public virtual bool IsPopup => false;

        protected UiFader Fader;

        
        private void Awake()
        {
            Fader = GetComponent<UiFader>() ?? gameObject.AddComponent<DefaultFader>();
        }

        public void Show()
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            Opening?.Invoke();
            Fader.FadeIn(() =>
            {
                IsHidden = false;
                Opened?.Invoke();
                OnWindowOpened();
            });
        }

        protected virtual void OnWindowOpened()
        {
        }

        public virtual void Hide()
        {
            IsHidden = true;
            Fader.FadeOut(() => { gameObject.SetActive(false); });
        }

        public void Close()
        {
            if (IsStatic || !this)
            {
                return;
            }
            BeforeClosing();
            Closing?.Invoke();
            Fader.FadeOut(() =>
            {
                Closed?.Invoke();
                if (this)
                {
                    Destroy(gameObject);
                }
            });
        }

        protected virtual void BeforeClosing()
        {
        }

        public virtual void ShowWithOptions(IWindowOptions options)
        {
            Debug.LogError($"Window {this} can not parse any options, use Window<T> instead");
        }

        public void AnchorToPoint(Vector3 anchor)
        {
            var screenPoint = Camera.current.WorldToScreenPoint(anchor);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GetComponentInParent<Canvas>().GetComponent<RectTransform>(),
                screenPoint, Camera.main, out var p);
            GetComponent<RectTransform>().anchoredPosition = new Vector3(p.x, p.y, 0);
        }
    }

    public abstract class Window<TOptions> : Window where TOptions : class, IWindowOptions, new()
    {
        protected virtual TOptions DefaultOptions => new TOptions();
        protected TOptions Options;
        protected virtual bool ViewModelRequired => true;

        
        public override void ShowWithOptions(IWindowOptions options)
        {
            if (!TryGetTypedOptions(options, out var typedOptions))
            {
                return;
            }

            Options = typedOptions;
            Show();

            Prepare();
        }

        protected abstract void Prepare();

        protected bool TryGetTypedOptions(IWindowOptions options, out TOptions typedOptions)
        {
            if (options == null)
            {
                options = DefaultOptions;
            }

            typedOptions = options as TOptions;
            if (typedOptions == null)
            {
                Debug.LogError($"Error while opening window {this}: options must have type {typeof(TOptions)} but have {options.GetType()}");
                return false;
            }

            return true;
        }
    }

    public interface IWindowOptions
    {
    }
}
