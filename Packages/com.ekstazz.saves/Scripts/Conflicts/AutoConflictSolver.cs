namespace Ekstazz.Saves.Conflicts
{
    using Data;

    internal class AutoConflictSolver : IAutoConflictSolver
    {
        public ConflictSolveResult Solve(SaveData local, SaveData remote)
        {
            //if both saves are corrupted, something went wrong
            if (local.HasErrors && remote.HasErrors)
            {
                return new ConflictSolveResult
                {
                    Status = ConflictSolveStatus.BothCorrupted 
                };
            }
            
            //if both saves are empty, saves are not enabled yet
            if (local.IsEmpty && remote.IsEmpty)
            {
                return new ConflictSolveResult
                {
                    Status = ConflictSolveStatus.BothEmpty
                };
            }

            //if one of saves is null, return another
            if (local.IsEmpty)
            {
                return Choose(remote);
            }

            if (remote.IsEmpty)
            {
                return Choose(local);
            }
            
            var mostRecent = local.Result.TimeStamp >= remote.Result.TimeStamp ? local : remote;
            
            var localMeta = local.Result.Meta;
            var remoteMeta = remote.Result.Meta;

            if (localMeta is null)
            {
                return Choose(remote);
            }
            
            //if progress is similar, return newer one
            if (localMeta.IsEqualTo(remoteMeta))
            {
                return Choose(mostRecent);
            }

            var bestMeta = localMeta.CompareTo(remoteMeta);
            
            var withBestProgression = bestMeta.AreEqual ? local : bestMeta.BestOne == localMeta? local : remote;
            //if one of saves is newer and have better progression, return it
            if (mostRecent == withBestProgression)
            {
                return Choose(withBestProgression);
            }

            return Choose(withBestProgression);
        }

        private ConflictSolveResult Choose(SaveData result)
        {
            return new ConflictSolveResult
            {
                Status = ConflictSolveStatus.Solved,
                Result = result
            };
        }
    }

    internal class RemoteOnlyConflictSolver : IAutoConflictSolver
    {
        public ConflictSolveResult Solve(SaveData local, SaveData remote)
        {
            return new ConflictSolveResult()
            {
                Result = remote,
                Status = ConflictSolveStatus.Forced
            };
        }
    }
    
    internal class LocalOnlyConflictSolver : IAutoConflictSolver
    {
        public ConflictSolveResult Solve(SaveData local, SaveData remote)
        {
            return new ConflictSolveResult()
            {
                Result = local,
                Status = ConflictSolveStatus.Forced
            };
        }
    }
}