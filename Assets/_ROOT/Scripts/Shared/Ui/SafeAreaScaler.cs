namespace Ekstazz.Ui
{
    using UnityEngine;

    
    public class SafeAreaScaler : MonoBehaviour
    {
        private RectTransform Panel;

        public enum SimDevice
        {
            None, IPhoneX
        }
        
        [SerializeField] private SimDevice simulateDevice;
        
        
        private void Awake() 
        {
            Panel = GetComponent<RectTransform> ();
            var safeArea = GetSafeArea();
            ScaleTo(safeArea);
        }
        
        public Rect GetSafeArea()
        {
            var safeArea = Screen.safeArea;

            if (!Application.isEditor || simulateDevice == SimDevice.None)
            {
                return CropRect(safeArea);
            }

            var nsa = new Rect (0, 0, Screen.width, Screen.height);
            switch (simulateDevice)
            {
                case SimDevice.IPhoneX:
                    nsa = new Rect (0f, 102f / 2436f, 1f, 2202f / 2436f);
                    break;
            }

            safeArea = new Rect (Screen.width * nsa.x, Screen.height * nsa.y, Screen.width * nsa.width, Screen.height * nsa.height);

            return CropRect(safeArea);
        }

        private Rect CropRect(Rect safeArea)
        {
            var yPos = Mathf.Min(40, safeArea.yMin);
            var delta = safeArea.yMin - yPos;
            return new Rect(safeArea.xMin, yPos, safeArea.width, safeArea.height + delta);
        }

        private void ScaleTo(Rect r)
        {
            // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
            var anchorMin = r.position;
            var anchorMax = r.position + r.size;
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;
            Panel.anchorMin = anchorMin;
            Panel.anchorMax = anchorMax;

//            Debug.Log($"New safe area applied to {name}: x={r.x}, y={r.y}, w={r.width}, h={r.height} on full extents w={Screen.width}, h={Screen.height}");
        }
    }
}