namespace Ekstazz.Ui
{
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class UiElement : MonoBehaviour, IPointerClickHandler
    {
        public virtual bool IsBlockingDrag => true;

        public bool IsHidden { get; protected set; }

        public void OnPointerClick(PointerEventData eventData)
        {
        }
    }
}
