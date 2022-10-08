namespace Ekstazz.Ui
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    
    public static class Icons
    {
        private static readonly Dictionary<Type, ScriptableObject> IconsObjects = new Dictionary<Type, ScriptableObject>();

        public static void Register<T>() where T : ScriptableObject
        {
            IconsObjects[typeof(T)] = Resources.Load<T>($"Ui/Icons/{typeof(T).Name}");
        }

        public static T Get<T>() where T : ScriptableObject
        {
            return (T) IconsObjects[typeof(T)];
        }

        public static Sprite NullImage => Get<DefaultIcons>()?.nullIcon;
    }
}