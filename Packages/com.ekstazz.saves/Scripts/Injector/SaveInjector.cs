namespace Ekstazz.Saves
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    internal class SaveInjector : ISaveInjector
    {
        private readonly List<SaveComponentInjector> injectors = new List<SaveComponentInjector>();
        
        [Obsolete("This method is empty to avoid compile errors with old version.\r\n Method AutoRegisterSaves registers all saves marked dy SaveComponentAttribute automaticaly")]
        public static void Register<T>() where T : class, ISaveComponent
        {
            
        }

        [Obsolete("method AutoRegisterSaves register all saves marked by SaveComponentAttribute automaticaly")]
        public void RegisterSave<T>() where T :class, ISaveComponent
        {
            if (injectors.Any(injector => injector.GetType() == typeof(SaveComponentInjector<T>)))
            {
                Debug.LogError($"trying to add duplicate injector: {typeof(T)}, ignoring");
                return;
            }
            injectors.Add(new SaveComponentInjector<T>());
        }

        public void AutoRegisterSaves()
        {
            var autoSaveTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.GetTypes())
                    .Where(HasSaveAttributeAndIsSaveComponent);
            var log = "Register saves:\n";
            foreach (var type in autoSaveTypes)
            {
                if (!typeof(ISaveComponent).IsAssignableFrom(type))
                {
                    Debug.LogError($"trying to add injectors that not implements IComponentSave interface: {type}, ignoring");
                    continue;
                }
                var injectorType = typeof(SaveComponentInjector<>).MakeGenericType(type);
                if (injectors.Any(injector => injector.GetType() == injectorType))
                {
                    Debug.LogError($"trying to add duplicate injector: {type}, ignoring");
                    continue;
                }
                var injectorInstance = Activator.CreateInstance(injectorType) as SaveComponentInjector;
                injectors.Add(injectorInstance);
                log += $"{type} save are successfully registed\n";
            }
            Debug.Log(log);
        }

        private bool HasSaveAttributeAndIsSaveComponent(Type type)
        {
            return Attribute.IsDefined(type, typeof(SaveComponentAttribute)) && typeof(ISaveComponent).IsAssignableFrom(type);
        }

        public bool TryInjectSaves(List<ISaveComponent> saves, ISaveable persistent, DateTime lastSaveTime)
        {
            var injector = injectors.FirstOrDefault(inj => inj.CanWorkWith(persistent));
            if (injector == null)
            {
                throw new Exception($"No save injector found for type {persistent} - probably, you forget to register type");
            }
            var save = injector.GetSaveToInject(saves);
            if (save == null)
            {
                persistent.PrepareInitial();
                return true;
            }

            return injector.TryInject(save, persistent, lastSaveTime);
        }

        public ISaveComponent GetSave(ISaveable persistent)
        {
            var injector = injectors.FirstOrDefault(inj => inj.CanWorkWith(persistent));
            if (injector == null)
            {
                throw new Exception($"No save injector found for type {persistent} - probably, you forget to register type");
            }

            var componentSave = injector.ExtractSave(persistent);
            if (componentSave == null)
            {
                throw new Exception($"Null came from {persistent}");
            }
            return componentSave;
        }
    }
}