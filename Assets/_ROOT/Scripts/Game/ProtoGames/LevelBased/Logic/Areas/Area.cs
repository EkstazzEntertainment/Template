namespace Ekstazz.LevelBased.Logic
{
    using UnityEngine;

    public class Area : MonoBehaviour
    {
        [field: SerializeField] public string AreaId { get; private set; } = "Area_";

        public void SetAreaId(string id)
        {
            AreaId = id;
        }

        private void OnValidate()
        {
            gameObject.name = AreaId;
        }
    }
}
