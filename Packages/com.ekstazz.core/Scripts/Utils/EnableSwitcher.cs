namespace Ekstazz.Utils
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class EnableSwitcher : MonoBehaviour
    {
        [SerializeField]
        private string id;

        [SerializeField]
        private bool onlyInDebug;

        [SerializeField]
        private List<GameObject> listToSwitch;

        private PlayerPrefsStoredValue<bool> isEnabled;

        public void Start()
        {
#if !DEBUG
            if (onlyInDebug)
            {
                SwitchTo(false);
                gameObject.SetActive(false);
                return;
            }
#endif

            if (listToSwitch == null || !listToSwitch.Any())
            {
                return;
            }

            isEnabled = new PlayerPrefsStoredValue<bool>($"Switcher{id}", true);
            GetComponent<Button>().onClick.AddListener(ToggleObjects);
            SwitchTo(isEnabled.Value);
        }

        private void ToggleObjects()
        {
            var first = listToSwitch.First();
            SwitchTo(!first.activeSelf);
        }

        public void SwitchTo(bool value)
        {
            foreach (var o in listToSwitch)
            {
                o.SetActive(value);
            }
            isEnabled.Value = value;
        }
    }
}
