namespace Ekstazz.Shared.Debug
{
    using TMPro;
    using UnityEngine;

    public class BuildVersionDebug : MonoBehaviour
    {
        [SerializeField] private TMP_Text versionPlaceholder;

        private void Start()
        {
            versionPlaceholder.SetText(Application.version);
        }
    }
}
