namespace Ekstazz.ProtoGames.Level.Logic
{
    using UnityEngine;
    using Zenject.Extensions.Lazy;
    
    public class LightAdjuster : InjectableView<LightAdjuster>
    {
        [SerializeField] private Light sceneLight;

        public void SetLightColor(Color color)
        {
            sceneLight.color = color;
        }

        public void SetLightIntensity(float intensity)
        {
            sceneLight.intensity = intensity;
        }
    }
}
