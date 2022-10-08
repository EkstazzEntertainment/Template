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
        /// <summary>
        /// Solver sucessfully decide save to use
        /// </summary>
        Solved,
        
        /// <summary>
        /// Solver was force to choose one variant
        /// </summary>
        Forced,

        /// <summary>
        /// Means that solver can not decide which save to use
        /// </summary>
        NotSolved,

        /// <summary>
        /// Error, both saves can not be used
        /// </summary>
        BothCorrupted,
        
        /// <summary>
        /// Two empty saves were passed
        /// </summary>
        BothEmpty
    }
}