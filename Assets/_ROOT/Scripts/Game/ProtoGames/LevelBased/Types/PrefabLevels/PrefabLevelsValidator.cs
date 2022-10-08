namespace Ekstazz.LevelBased.PrefabLevels
{
    using System.Collections.Generic;
    using System.Linq;
    using Logic;
    using Ekstazz.LevelBased.Logic;
    using UnityEngine;

    
    public class PrefabLevelsValidator : ILevelsValidator
    {
        private const string AreasDirectory = "Levels/Generated";
        
        public string Validate(LevelConfig[] configs)
        {
            return VerifyConfigurationIds(LoadAreasPrefabs(), configs);
        }
        
        private List<Area> LoadAreasPrefabs()
        {
            return Resources.LoadAll<Area>(AreasDirectory).ToList();
        }
        
        private string VerifyConfigurationIds(List<Area> areas, LevelConfig[] configs)
        {
            var verificationResult = $"<color=yellow>Level configs to areas prefabs matching verification:</color>\n";
            verificationResult += $"Amount of area prefabs loaded: {areas.Count};\n";
            verificationResult += $"Amount of levels loaded: {configs.Length}\n";

            var levelAreasUsed = Enumerable.Repeat<bool>(false, areas.Count).ToList();
            foreach (var levelConfig in configs)
            {
                var matchIndex = -1;
                for (var i = 0; i < areas.Count; i++)
                {
                    if (levelConfig.LevelGamesQueue.ToList().Exists(x => x.sceneId == areas[i].AreaId))
                    {
                        levelAreasUsed[i] = true;
                        if (matchIndex != -1)
                        {
                            verificationResult += $" <color=red>Error</color> Level config with id: <color=red>{levelConfig.LevelId}</color> has multiple areas id matching!\n";
                        }
                        matchIndex = i;
                    }
                }

                if (matchIndex == -1)
                {
                    verificationResult += $" <color=red>Error</color> Level config with id: <color=red>{levelConfig.LevelId}</color> hasn't matching areas prefab with id <color=red>{levelConfig.LevelGamesQueue}</color>!\n";
                }
            }

            for (var i = 0; i < levelAreasUsed.Count; i++)
            {
                if (levelAreasUsed[i] == false)
                {
                    verificationResult += $" <color=yellow>Warning</color> Area prefab with id <color=yellow>{areas[i].AreaId}</color> wasn't used in current levels list.\n";
                }
            }

            return verificationResult;
        }
    }
}