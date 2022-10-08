namespace Ekstazz.ProtoGames.Level.World
{
    using System;
    using System.Collections.Generic;
    using Cameras;
    using LevelBased.Flow.Signals;
    using Logic;
    using UnityEngine;
    using UnityEngine.Rendering;

    
    [CreateAssetMenu(fileName = "World_", menuName = "BlobPlanetGames/WorldTheme")]
    public class WorldTheme : ScriptableObject
    {
        public string id = "World_";

        [Header("The less num the higher priority")]
        public int priority = 1;

        [Header("Environment")]
        public SkyboxSettings[] skyboxSettings;
        public EnvironmentSettings environmentSettings;
        
        [Header("Camera")] 
        public List<CameraOverrideInfo> cameraOverrideInfos;
        
        
        public void ApplyEnvironmentSettings()
        {
            var levelStyleAdjuster = new LevelStyleAdjuster<ILevelReadyToStart>();
            levelStyleAdjuster.SetUpEnvironmentExecuteMode(environmentSettings, skyboxSettings[0]);
        }
    }
    
    [Serializable]
    public class SkyboxSettings
    {
        public Material sky;
        public FogSettings fog;
        public LightSettings light;
    }
    
    [Serializable]
    public class FogSettings
    {
        public bool isActive;
        public FogMode mode;
        public Color color;
        public float startDistance;
        public float endDistance;
        public float density = 0.01f;
    }

    [Serializable]
    public class LightSettings
    {
        public Color color;
        public float intensity;
    }
    
    [Serializable]
    public class EnvironmentSettings
    {
        public Light sunSource;
        public Color realTimeShadowColor = Color.white;
        public Color skyColor = Color.white;
        public Color equatorColor = Color.white;
        public Color groundColor = Color.white;
        public DefaultReflectionMode reflectionsMode = DefaultReflectionMode.Skybox;
        public int reflectionsResolution = 128;
        public int reflectionIntensity;
        public int reflectionBounces;
    }
}