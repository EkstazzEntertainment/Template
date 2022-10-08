namespace Ekstazz.Sounds
{
    using UnityEngine;
    using UnityEngine.UI;

    [RequireComponent(typeof(Button))]
    public class TapSoundButton : TapSound
    {
        private void Start()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(Tap);
        }
    }
}