namespace Ekstazz.Ui.Windows
{
    using System.Linq;
    using DG.Tweening;
    using TMPro;
    using UnityEngine;

    [WindowMeta("InfoPopUp")]
    public class InfoPopUp : Window<InfoPopUpOptions>
    {
        [SerializeField]
        private TMP_Text text;

        [SerializeField, Range(1, 15)]
        private float lifetime = 3;

        protected override InfoPopUpOptions DefaultOptions => new InfoPopUpOptions();

        public override bool IsPopup => true;

        protected override void Prepare()
        {
            var other = FindObjectsOfType<InfoPopUp>();
            if (other.Any(up => up != this && up.Options.Text == Options.Text))
            {
                Destroy(gameObject);
            }

            text.SetText(Options.Text);

            DOVirtual.DelayedCall(lifetime, Close);
        }
    }

    public class InfoPopUpOptions : IWindowOptions
    {
        public string Text { get; set; }
    }
}
