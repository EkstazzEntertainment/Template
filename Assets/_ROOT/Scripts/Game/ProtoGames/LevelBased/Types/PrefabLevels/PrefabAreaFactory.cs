namespace Ekstazz.LevelBased.Prefabs
{
    using Ekstazz.LevelBased.Logic;
    using UnityEngine;
    using Zenject;

    public class AreaFactory : IFactory<Area, Transform, Area>
    {
        private readonly DiContainer container;

        public AreaFactory(DiContainer container)
        {
            this.container = container;
        }

        public Area Create(Area prefab, Transform parent)
        {
            return container.InstantiatePrefabForComponent<Area>(prefab, parent);
        }
    }
}
