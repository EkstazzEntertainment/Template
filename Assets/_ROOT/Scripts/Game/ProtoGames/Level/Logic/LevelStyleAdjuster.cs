namespace Ekstazz.ProtoGames.Level.Logic
{
    using System.Linq;
    using Ekstazz.LevelBased.Logic;
    using LevelBased.Flow.Signals;
    using UnityEngine;
    using World;
    using Zenject;

    
    public class LevelStyleAdjuster<T> : IInitializable where T : ILevelReadyToStart
    {
        [Inject] private ILevelProvider levelProvider;
        [Inject] private IWorldThemeProvider worldThemeProvider;
        [Inject] private SignalBus signalBus;
        [Inject] private LazyInject<LightAdjuster> lightAdjuster;
        
        
        public void Initialize()
        {
            signalBus.Subscribe<T>(SetCurrentLevelSkybox);
        }

        private void SetCurrentLevelSkybox()
        {
            var currentLevelIndex = worldThemeProvider.GetLevelIndexInWorldTheme(levelProvider.CurrentLevelGameInfo);
            var skyboxSettings = worldThemeProvider
                .GetWorldTheme(levelProvider.CurrentLevelGameInfo).skyboxSettings.ToList();
            var environmentSettings = worldThemeProvider
                .GetWorldTheme(levelProvider.CurrentLevelGameInfo).environmentSettings;
            
            if (skyboxSettings.Count == 0)
            {
                skyboxSettings = worldThemeProvider.GetDefaultWorldTheme().skyboxSettings.ToList();
            }
            var currentWorldSetting = skyboxSettings[currentLevelIndex % skyboxSettings.Count];
            
            ConfigureFog(currentWorldSetting);
            ConfigureLight(currentWorldSetting);
            ConfigureRenderSettings(environmentSettings, currentWorldSetting);
        }

        private void ConfigureFog(SkyboxSettings currentWorldSettings)
        {
            if (!currentWorldSettings.fog.isActive)
            {
                return;
            }
            
            RenderSettings.fog = true;
            RenderSettings.fogMode = currentWorldSettings.fog.mode;
            RenderSettings.fogColor = currentWorldSettings.fog.color;
            RenderSettings.fogStartDistance = currentWorldSettings.fog.startDistance;
            RenderSettings.fogEndDistance = currentWorldSettings.fog.endDistance;
            RenderSettings.fogDensity = currentWorldSettings.fog.density;
        }

        private void ConfigureRenderSettings(EnvironmentSettings environmentSettings, SkyboxSettings skyboxSettings)
        {
            RenderSettings.skybox = skyboxSettings.sky;
            RenderSettings.sun = environmentSettings.sunSource;
            RenderSettings.ambientLight = environmentSettings.realTimeShadowColor;
            RenderSettings.subtractiveShadowColor = environmentSettings.realTimeShadowColor;
            RenderSettings.ambientSkyColor = environmentSettings.skyColor;
            RenderSettings.ambientEquatorColor = environmentSettings.equatorColor;
            RenderSettings.ambientGroundColor = environmentSettings.groundColor;
            RenderSettings.defaultReflectionMode = environmentSettings.reflectionsMode;
            RenderSettings.defaultReflectionResolution = environmentSettings.reflectionsResolution;
            RenderSettings.reflectionIntensity = environmentSettings.reflectionIntensity;
            RenderSettings.reflectionBounces = environmentSettings.reflectionBounces;
        }

        private void ConfigureLight(SkyboxSettings currentWorldSettings)
        {
            lightAdjuster.Value.SetLightColor(currentWorldSettings.light.color);
            lightAdjuster.Value.SetLightIntensity(currentWorldSettings.light.intensity);
        }
        
        public void SetUpEnvironmentExecuteMode(EnvironmentSettings environmentSettings, SkyboxSettings skyboxSettings)
        {
            ConfigureFog(skyboxSettings);
            ConfigureRenderSettings(environmentSettings, skyboxSettings);
        }
    }
}
