namespace Ekstazz.ProtoGames.Level.World
{
    using System.Collections.Generic;
    using System.Linq;
    using Config;
    using Ekstazz.LevelBased.Logic;
    using LevelBased.Logic;
    using UnityEngine;
    using Zenject;

    public interface IWorldThemeProvider
    {
        WorldTheme GetDefaultWorldTheme();
        WorldTheme GetWorldTheme(LevelGameInfo levelGameInfo);
        int GetLevelIndexInWorldTheme(LevelGameInfo levelGameInfo);
    }

    public class WorldThemeProvider : IInitializable, IWorldThemeProvider
    {
        [Inject] private WorldLevelsConfig worldLevelsConfig;

        private readonly string defaultThemeName = "World_Default";
        
        private List<WorldTheme> worldThemeResources;
        private WorldTheme defaultTheme;

        protected virtual string WorldThemesPath { get; set; } = "WorldThemes";


        public void Initialize()
        {
            worldThemeResources = Resources.LoadAll<WorldTheme>(WorldThemesPath).OrderBy(w=> w.priority).ToList();
            worldThemeResources.Remove(worldThemeResources.FirstOrDefault(i=> i.id == defaultThemeName));
            defaultTheme = Resources.Load<WorldTheme>($"{WorldThemesPath}/{defaultThemeName}");
        }

        public WorldTheme GetDefaultWorldTheme()
        {
            return defaultTheme;
        }

        public WorldTheme GetWorldTheme(LevelGameInfo levelGameInfo)
        {
            return worldThemeResources.FirstOrDefault(x => x.id == GetWorldId(levelGameInfo));
        }

        public WorldTheme GetNextWorldTheme(LevelGameInfo levelGameInfo)
        {
            var currentWorldId = GetWorldId(levelGameInfo);
            var currentWorldIndex = worldThemeResources
                .IndexOf(
                    worldThemeResources
                        .First(e => e.id == currentWorldId)
                    );

            var nextWorldIndex =
                (currentWorldIndex == worldThemeResources.Count - 1)
                    ? 0
                    : currentWorldIndex + 1;
            return worldThemeResources[nextWorldIndex];
        }

        public int GetLevelIndexInWorldTheme(LevelGameInfo levelGameInfo)
        {
            return GetWorldLevels(levelGameInfo).IndexOf(levelGameInfo.sceneId);
        }

        private string GetWorldId(LevelGameInfo levelGameInfo)
        {
            var currentGameWorldThemes = worldLevelsConfig.WorldThemeLevelsConfig.GameWorldThemeInfos
                .First(gameWorldThemes => gameWorldThemes.GameID == levelGameInfo.gameID);
            
            var worldID = currentGameWorldThemes.Configurations
                .FirstOrDefault(e => e.Levels.Contains(levelGameInfo.sceneId))?.WorldId;

            if (worldID != null)
            {
                return worldID;
            }

            Debug.Log($"<color=red>World id with level {levelGameInfo.sceneId} was not found</color>");
            return defaultTheme.id;
        }

        private List<string> GetWorldLevels(LevelGameInfo levelGameInfo)
        {
            var currentGameWorldThemes = worldLevelsConfig.WorldThemeLevelsConfig.GameWorldThemeInfos
                .First(gameWorldThemes => gameWorldThemes.GameID == levelGameInfo.gameID);
            
            return currentGameWorldThemes.Configurations
                .First(e => e.WorldId == GetWorldId(levelGameInfo)).Levels;
        }
    }
}