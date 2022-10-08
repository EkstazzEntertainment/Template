namespace Ekstazz.Shared.Debug
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ekstazz.Sounds;
    using MoreMountains.NiceVibrations;
    using TMPro;
    using UnityEngine;
    using Zenject;

    
    public class VibrationsTesterComponent : MonoBehaviour
    {
        [Inject] private GameSounds gameSounds;

        [Header("Structure")]
        [SerializeField] private TMP_Dropdown dropdownList;

        private List<HapticTypes> availableTypes;

        
        private void Awake()
        {
            dropdownList.ClearOptions();

            availableTypes = Enum.GetValues(typeof(HapticTypes)).Cast<HapticTypes>().ToList();
            foreach (var availableType in availableTypes)
            {
                var dropdownOption = new TMP_Dropdown.OptionData(availableType.ToString());
                dropdownList.options.Add(dropdownOption);
            }

            dropdownList.value = availableTypes.IndexOf(HapticTypes.None);
        }

        public void InvokeSelectedHaptic()
        {
            var hapticToInvoke = availableTypes[dropdownList.value];
            Debug.Log($"<color=green>Debug:</color> Invoking HapticType: {hapticToInvoke}");
            gameSounds.Haptic(hapticToInvoke);
        }
    }
}