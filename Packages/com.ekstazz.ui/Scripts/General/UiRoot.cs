namespace Ekstazz.Ui
{
    using UnityEngine;

    
    public class UiRoot : MonoBehaviour
    {
        [SerializeField] private Transform windowsParent;
        [SerializeField] private Transform popUpsParent;
        [SerializeField] private UiRootType type;
        [SerializeField] private UiBackground back;
        
        public UiRootType Type => type;
        public Transform WindowsParent => windowsParent;
        public Transform PopUpsParent => popUpsParent;

        
        public UiBackground SpawnBackground(Transform parent)
        {
            return Instantiate(back, parent, false);
        }
    }

    public enum UiRootType
    {
        Global,
        Local
    }
}
