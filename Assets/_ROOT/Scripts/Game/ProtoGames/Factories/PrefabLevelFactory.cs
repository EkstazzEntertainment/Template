namespace Ekstazz.ProtoGames
{
    using LevelBased.Logic;
    using UnityEngine;
    using Zenject;

    public class PrefabLevelFactory : PlaceholderFactory<Area, Transform, Area>
    {
        private DiContainer container;

        public PrefabLevelFactory(DiContainer container)
        {
            this.container = container;
        }

        public override Area Create(Area prefab, Transform parent)
        {
            return container.InstantiatePrefabForComponent<Area>(prefab, parent);
        }
    }
}