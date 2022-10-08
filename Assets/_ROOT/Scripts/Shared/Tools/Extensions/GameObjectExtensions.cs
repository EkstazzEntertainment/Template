namespace Ekstazz.Tools
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    
    public static class GameObjectExtensions
    {
        public static Bounds GetBounds(this GameObject gameObject)
        {
            var bounds = new Bounds();
            var renderers = gameObject
                .GetRenderers()
                .Where(r => r.enabled)
                .ToArray();
            if (renderers.Length > 0)
            {
                bounds = renderers.First().bounds;
                foreach (var renderer in renderers.Skip(1))
                {
                    bounds.Encapsulate(renderer.bounds);
                }
            }

            return bounds;
        }

        public static IEnumerable<Renderer> GetRenderers(this GameObject gameObject)
        {
            return gameObject.GetComponentsInChildren<Renderer>();
        }

        public static void SetSceneDirty(this GameObject gameObject, bool log = false)
        {
            var targetScene = gameObject.scene;            
#if UNITY_EDITOR
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(targetScene);
            if (log)
            {
                Debug.Log($"Scene {targetScene} that contains gameobject {gameObject.name} was marked as dirty.".ToGreen());
            }
#else
            if (log)
            {
                Debug.Log($"Cannot mark scene {targetScene} with gameobject {gameObject.name} as dirty in non-editor environment!".ToYellow());
            }
#endif
        }
    }
}