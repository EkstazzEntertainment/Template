namespace Ekstazz.Saves
{
    using System.Collections.Generic;
    using Conflicts;
    using Converters;
    using Core.Modules;
    using Debug.DebugOptions;
    using Packers;
    using Parsers;
    using Worker;
    using UnityEngine;
    using Zenject;

    
    [AutoInstalledModule]
    public class SavesInstaller : ModuleInstaller
    {
        public override IModuleInitializer ModuleInitializer => new Initializer();
        public override string Name => "Ekstazz.Saves";

        
        public override void InstallBindings()
        {
            Container.Bind<ISaveablesRegistry>().To<SaveablesRegistry>().AsSingle();
            Container.Bind<SaveHolder>().AsSingle();

            Container.Bind<IAutoConflictSolver>().To<AutoConflictSolver>().AsSingle();
            Container.Bind<SavePipeline>().AsSingle();
            Container.Bind<ISaver>().To<Saver>().AsSingle();

            Container.Bind<ISaveContext>().To<SaveContext>().AsSingle();
            Container.BindInterfacesTo<SaveScheduler>().AsSingle();
            Container.Bind<ISaveWorker>().To<SaveWorker>().AsSingle();

            Container.Bind<ISavePacker>().To<SavePacker>().AsSingle();
            Container.Bind<ISaveParser>().To<SaveParser>().AsSingle();

            Container.Bind<ISaveIoWorker>().WithId(SaveOrigin.Remote).To<LocalSaveWorker>().AsCached();
            Container.Bind<ISaveIoWorker>().WithId(SaveOrigin.Local).To<LocalSaveWorker>().AsCached();

            var saveInjector = new SaveInjector();
            saveInjector.AutoRegisterSaves();
            Container.Bind<ISaveInjector>().FromInstance(saveInjector);

            var saveConverter = new SaveConverter();
            Container.Bind<ISaveConverter>().FromInstance(saveConverter);
            saveConverter.AddConverter(new DropOlderSavesConverter(Serialization.MinimumVersion));
        }

        
        protected override IEnumerable<ModuleVerificationResult> FindModuleErrors()
        {
            var serializationVersion = Resources.Load<SerializationVersion>($"Settings/{nameof(SerializationVersion)}");
            if (serializationVersion == null)
            {
                yield return ModuleVerificationResult.HasError("Please, add SerializationVersion asset to Resources/Settings via Create menu (Ekstazz/Saves/Serialization Version)");
            }

            if (serializationVersion != null)
            {
                if (serializationVersion.currentVersion < serializationVersion.minimumVersion)
                {
                    yield return ModuleVerificationResult.HasError("Current serialization version is less than minimum");
                }

                if (serializationVersion.currentVersion <= 0 || serializationVersion.minimumVersion <= 0)
                {
                    yield return ModuleVerificationResult.HasError("Serialization version is less than or equal to 0. Please make sure version is positive number");
                }
            }
        }

        
        private class Initializer : IModuleInitializer
        {
            [Inject] private ToggleOptions ToggleOptions { get; set; }
            [Inject] private ISaveContext SaveContext { get; set; }

            
            public void Prepare()
            {
                ToggleOptions.AddOption(new ToggleOption
                {
                    Name = "Enable saves",
                    Default = true,
                    OnValueChanged = ChangeSaveBehaviour
                });
            }

            private void ChangeSaveBehaviour(bool value)
            {
                if (value)
                {
                    EnableSaves();
                }
                else
                {
                    DisableSaves();
                }
            }

            private void DisableSaves()
            {
                SaveContext.ApplyBehaviour(SaveBehaviour.SaveDisabled, SaveBlockingContext.Debug);
            }

            private void EnableSaves()
            {
                if (SaveContext.SaveBlockers.Contains(SaveBlockingContext.Debug))
                {
                    SaveContext.ApplyBehaviour(SaveBehaviour.SaveEnabled, SaveBlockingContext.Debug);
                }
            }
        }
    }
}