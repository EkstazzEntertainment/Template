namespace TwoSquares.Shared.Utils
{
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    
    public class OneClickButton : Button
    {
        private bool pressed;

        
        public void ResetButton()
        {
            pressed = false;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (pressed)
            {
                return;
            }

            pressed = true;
            base.OnPointerClick(eventData);
        }
    }
}
