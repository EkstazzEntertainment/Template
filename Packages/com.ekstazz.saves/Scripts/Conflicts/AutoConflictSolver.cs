namespace Ekstazz.Saves.Conflicts
{
    using Data;

    
    internal class AutoConflictSolver : IAutoConflictSolver
    {
        public ConflictSolveResult Solve(SaveData local, SaveData remote)
        {
            if (local.HasErrors && remote.HasErrors)
            {
                return new ConflictSolveResult
                {
                    Status = ConflictSolveStatus.BothCorrupted 
                };
            }
            
            if (local.IsEmpty && remote.IsEmpty)
            {
                return new ConflictSolveResult
                {
                    Status = ConflictSolveStatus.BothEmpty
                };
            }

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
            
            if (localMeta.IsEqualTo(remoteMeta))
            {
                return Choose(mostRecent);
            }

            var bestMeta = localMeta.CompareTo(remoteMeta);
            var withBestProgression = bestMeta.AreEqual ? local : bestMeta.BestOne == localMeta? local : remote;

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