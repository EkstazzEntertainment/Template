namespace Ekstazz.Saves.Flow
{
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Conflicts;
    using Data;
    using Worker;
    using UnityEngine;
    using UnityEngine.Profiling;
    using Zenject;
    using Zenject.Extensions.Commands;

    
    public class StartProcessingSavesCommand : Command
    {
        [Inject(Id = SaveOrigin.Remote)] internal ISaveIoWorker RemoteWorker { get; set; }
        [Inject(Id = SaveOrigin.Local)] internal ISaveIoWorker LocalWorker { get; set; }
        [Inject] internal IAutoConflictSolver AutoConflictSolver { get; set; }
        [Inject] internal SavePipeline SavePipeline { get; set; }
        [Inject] internal SaveHolder SaveHolder { get; set; }

        
        public override async Task Execute()
        {
            Profiler.BeginSample("StartProcessingSaves");
            var remoteSave = new RemoteSaveData(await SavePipeline.GetSaveModelAsync(RemoteWorker));
            var localSave = new LocalSaveData(await SavePipeline.GetSaveModelAsync(LocalWorker));

            var autoSolveResult = AutoConflictSolver.Solve(localSave, remoteSave);

            if (ShouldPrepareInitialSaveData(autoSolveResult))
            {
                var initialSaveData = new InitialSaveData();

                SaveHolder.SetSave(initialSaveData);
                return;
            }

            WriteLoadedSaveAsJson(autoSolveResult.Result);

            SaveHolder.SetSave(autoSolveResult.Result);
            Profiler.EndSample();
        }

        private bool ShouldPrepareInitialSaveData(ConflictSolveResult autoSolveResult)
        {
            var status = autoSolveResult.Status;
            return status == ConflictSolveStatus.BothEmpty ||
                   status == ConflictSolveStatus.BothCorrupted ||
                   status == ConflictSolveStatus.Forced && autoSolveResult.Result.IsEmpty;
        }

        [Conditional("DEBUG")]
        private void WriteLoadedSaveAsJson(SaveData result)
        {
            File.WriteAllText($"{Application.persistentDataPath}/savefile.loaded.json", SavePipeline.ToJson(result.Result));
        }
    }
}