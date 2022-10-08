namespace Ekstazz.Sounds
{
    using UnityEngine;
    using UnityEngine.UI;
    
    [RequireComponent(typeof(Toggle))]
    public class TapSoundToggle : TapSound
    {
        private void Start()
        {
            var toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(_ => Tap());
        }
    }
}