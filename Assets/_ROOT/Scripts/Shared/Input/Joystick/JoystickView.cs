namespace Ekstazz.Input
{
    using System.Collections;
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;
    using Zenject.Extensions.Lazy;

    
    public class JoystickView : InjectableView<JoystickView>
    {
        [Inject] private Joystick joystick;

        [Header("Setup")]
        [SerializeField] private float size;
        [SerializeField] private float threshold;

        [Header("Visibility")]
        [SerializeField] public bool isVisible = true;

        [Header("UI Elements")]
        [SerializeField] private RectTransform originMarker;

        [SerializeField] private RectTransform span;
        [SerializeField] private Image[] images;
        [SerializeField] private float spanCoef = 4;

        private Tween scaling;
        private Tween fade;

        
        private void Start()
        {
            joystick.Size = size;
            joystick.Threshold = threshold;
            joystick.OnStart += OnStart;
            joystick.OnEnd += OnEnd;
            ChangeAlpha(0);
        }

        private void OnStart()
        {
            if (!isVisible)
            {
                return;
            }
            scaling?.Kill();
            fade?.Kill();
            originMarker.position = joystick.StartPosition;
            span.position = joystick.StartPosition;
            StartCoroutine(nameof(UpdateView));
            ChangeAlpha(1);
        }

        private IEnumerator UpdateView()
        {
            while (true)
            {
                if (Joystick.IsFloating)
                {
                    UpdatePosition();
                }
                UpdateScale();
                UpdateRotation();
                yield return null;
            }
        }

        private void ChangeAlpha(float alpha)
        {
            foreach (var image in images)
            {
                var color = image.color;
                color.a = alpha;
                image.color = color;
            }
        }
        
        private void UpdatePosition()
        {
            originMarker.position = joystick.StartPosition;
            span.position = joystick.StartPosition;
        }

        private void UpdateScale()
        {
            var scale = joystick.Distance / span.rect.height * spanCoef;
            scale = Mathf.Max(scale, 1);
            span.localScale = new Vector3(scale, span.localScale.y, span.localScale.z);
        }

        private void UpdateRotation()
        {
            var direction = new Vector3(joystick.Position.x, joystick.Position.y) - originMarker.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180;
            span.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            originMarker.localRotation = Quaternion.AngleAxis(angle - 180, Vector3.forward);
        }

        private void OnEnd()
        {
            if (!isVisible)
            {
                return;
            }
            StopCoroutine(nameof(UpdateView));
            scaling = span.DOScaleX(1, 0.2f);
            fade = DOVirtual.Float(1, 0, 0.2f, ChangeAlpha);
        }

        private void OnDestroy()
        {
            if (joystick != null)
            {
                joystick.OnStart -= OnStart;
                joystick.OnEnd -= OnEnd;
            }
        }
    }
}