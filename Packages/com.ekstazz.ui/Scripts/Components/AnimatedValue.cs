namespace Ekstazz.Ui
{
    using System;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;

    public class AnimatedValue : MonoBehaviour
    {
        private int amount;

        private int curValue;

        [SerializeField]
        [Range(0.1f, 5)]
        private float speed = 1f;

        private Text text;

        public float TimeToAnimate => speed;

        public int Value
        {
            get { return amount; }
            set { Add(value - amount); }
        }

        public event Action AnimationComplete = delegate { };

        private void Awake()
        {
            text = gameObject.GetComponent<Text>();
        }

        public void Add(int value)
        {
            if (this == null)
            {
                return;
            }
            amount += value;
            if (DOTween.IsTweening(gameObject))
            {
                DOTween.Complete(gameObject);
            }
            DOTween.To(OnUpdate, curValue, amount, speed)
                .SetEase(Ease.OutCubic)
                .SetId(gameObject)
                .OnComplete(() =>
                {
                    text.text = amount.ToString();
                    AnimationComplete();
                });
        }

        private void OnUpdate(float val)
        {
            curValue = (int)val;
            text.text = curValue.ToString();
        }
    }
}
