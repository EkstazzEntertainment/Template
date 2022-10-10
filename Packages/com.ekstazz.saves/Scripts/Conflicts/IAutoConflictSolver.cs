namespace Ekstazz.Saves.Conflicts
{
    using Data;

    
    internal interface IAutoConflictSolver
    {
        ConflictSolveResult Solve(SaveData local, SaveData remote);
    }

    internal class ConflictSolveResult
    {
        public ConflictSolveStatus Status { get; set; }
        public SaveData Result { get; set; }
    }

    internal enum ConflictSolveStatus
    {
        Solved,
        Forced,
        NotSolved,
        BothCorrupted,
        BothEmpty
    }
}