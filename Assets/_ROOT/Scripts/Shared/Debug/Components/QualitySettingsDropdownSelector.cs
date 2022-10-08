namespace Ekstazz.Shared.Debug
{
    using TMPro;
    using UnityEngine;

    public class QualitySettingsDropdownSelector : MonoBehaviour
    {
        [Header("Structure")]
        [SerializeField] private TMP_Dropdown dropdownList;

        private int currentQualityIndex;

        private void Awake()
        {
            currentQualityIndex = QualitySettings.GetQualityLevel();

            dropdownList.ClearOptions();
            foreach (var qualitySettingsName in QualitySettings.names)
            {
                var qualityDropdownOption = new TMP_Dropdown.OptionData(qualitySettingsName);
                dropdownList.options.Add(qualityDropdownOption);
            }
            dropdownList.value = currentQualityIndex;

            dropdownList.onValueChanged.AddListener(OnDropdownItemSelected);
        }

        private void OnDropdownItemSelected(int newQualitySettingsIndex)
        {
            Debug.Log($"<color=green>Changing quality settings: </color>" +
                      $"from: \"{GetQualityNameByIndex(currentQualityIndex)}\"({currentQualityIndex}) " +
                      $"to: \"{GetQualityNameByIndex(newQualitySettingsIndex)}\"({newQualitySettingsIndex})");

            QualitySettings.SetQualityLevel(newQualitySettingsIndex, true);
            currentQualityIndex = newQualitySettingsIndex;
        }

        private static string GetQualityNameByIndex(int index) => QualitySettings.names[index];

        private void OnDestroy()
        {
            dropdownList.onValueChanged.RemoveAllListeners();
        }
    }
}