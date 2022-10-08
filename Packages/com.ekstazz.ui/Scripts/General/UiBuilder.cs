namespace Ekstazz.Ui
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Windows;
    using UnityEngine;
    using Zenject;
    using Object = UnityEngine.Object;

    
    public class UiBuilder : IWindowsChecker
    {
        [Inject] private DiContainer container;

        private static readonly IDictionary<string, Window> Cache = new Dictionary<string, Window>();

        private readonly List<Window> currentWindows = new List<Window>();

        protected virtual string Directory => "Common/";
        
        public bool IsAnyWindowsOpened => currentWindows.Any();

        public List<Window> CurrentWindows => currentWindows.ToList();

        
        public T CreateWindowWithOptions<T>(IWindowOptions options, UiRootType rootType = UiRootType.Local, bool withBackground = false) where T : Window
        {
            return CreateWindow<T>(rootType, options, withBackground);
        }

        public virtual T CreateWindow<T>(UiRootType rootType = UiRootType.Local, IWindowOptions options = null, bool withBackground = false) where T : Window
        {
            //We are searching for parent every time, because UiBuilder exists between scenes and roots may change
            var root = Object.FindObjectsOfType<UiRoot>().FirstOrDefault(r => r.Type == rootType);
            if (root == null)
            {
                throw new Exception($"Can not find UiRoot of type {rootType}");
            }

            var name = typeof(T).Name;
            Cache.Clear();
            Cache.TryGetValue(name, out var prefab);
            if (prefab == null)
            {
                var path = $"{Directory}UI/{name}";
                prefab = Resources.Load<T>(path);
                if (prefab == null)
                {
                    prefab = Resources.Load<T>($"UI/{name}");
                }
                Debug.Log($"loading UI/{name}");
                Cache[name] = prefab;
            }

            if (prefab == null)
            {
                throw new WindowNotFoundException($"Can't find window prefab for: {name}");
            }

            var transform = prefab.IsPopup ? root.PopUpsParent : root.WindowsParent;
            var popup = container.InstantiatePrefabForComponent<Window>(prefab, transform);
            var window = popup.GetComponent<T>();

            var showBack = withBackground || window.ShowBack;

            if (showBack)
            {
                var back = root.SpawnBackground(transform);
                back.transform.SetSiblingIndex(window.transform.GetSiblingIndex());
                if (window.CloseOnBackgroundClick)
                {
                    back.ClickAction = () => window.Close();
                }

                if (window.TransparentBack)
                {
                    back.MakeTransparent();
                }

                window.Closing += () => Object.Destroy(back.gameObject);
            }

            if (options != null)
            {
                window.ShowWithOptions(options);
            }
            else
            {
                window.Show();
            }

            window.Closing += () => currentWindows.Remove(window);

            currentWindows.Add(window);
            return window;
        }

        public void ShowInfoPopUp(string text)
        {
            CreateWindowWithOptions<InfoPopUp>(new InfoPopUpOptions
            {
                Text = text,
            });
        }
    }

    public class WindowNotFoundException : Exception
    {
        public WindowNotFoundException(string s) : base(s)
        {
        }
    }
}