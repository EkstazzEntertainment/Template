namespace Ekstazz.Game.Flow
{
    using TMPro;
    using UnityEngine;

    public class BuildVersionView : MonoBehaviour
    {
        [SerializeField] private TMP_Text buildVersion;

        private void Awake()
        {
            buildVersion.text = $"v{Application.version}";
        }
    }
}