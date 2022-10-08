namespace Ekstazz.Shared.Ui.Components
{
    using System.Collections.Generic;
    using System.Linq;
    using DG.Tweening;
    using TMPro;
    using UnityEngine;

    public class NotificationIcon : MonoBehaviour
    {
        private TMP_Text text;

        private bool currentValue;

        private int value;

        private List<(string, int)> values = new List<(string, int)>();

        private void Awake()
        {
            text = GetComponentInChildren<TMP_Text>();
            currentValue = gameObject.activeSelf;
        }

        public void QuickHide()
        {
            gameObject.SetActive(false);
            currentValue = false;
        }

        public int Value
        {
            get => value;
            set => Set("_", value);
        }

        public void Set(string key, int val)
        {
            values = values.Where(tuple => tuple.Item1 != key).ToList();
            values.Add((key, val));
            var sum = values.Sum(tuple => tuple.Item2);
            value = sum;
            SetVisibleTo(sum != 0, sum.ToString());
        }

        private void SetVisibleTo(bool value, string msg = "!")
        {
            if (value == currentValue && text.text == msg)
            {
                return;
            }

            if (value == currentValue)
            {
                text.SetText(msg);
                return;
            }

            if (value)
            {
                Show(msg);
            }
            else
            {
                Hide();
            }
        }

        private void Show(string value = "!")
        {
            currentValue = true;
            text.SetText(value);
            gameObject.SetActive(true);
            transform.DOKill();
            transform.localScale = Vector3.one;
            transform.DOScale(Vector3.zero, 0.2f).From().SetEase(Ease.OutBack);
        }

        private void Hide()
        {
            currentValue = false;
            transform.DOKill();
            transform.localScale = Vector3.one;
            transform.DOScale(Vector3.zero, 0.2f);
        }
    }
}
