namespace Ekstazz.Saves
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    internal class SaveContext : ISaveContext
    {
        public bool IsBlocked => SaveBlockers.Any();

        public List<SaveBlockingContext> SaveBlockers { get; } = new List<SaveBlockingContext>()
        {
            SaveBlockingContext.Initial //from the beginning, saves are blocked
        };

        public void ApplyBehaviour(SaveBehaviour saveBehaviour, SaveBlockingContext context)
        {
            switch (saveBehaviour)
            {
                case SaveBehaviour.SaveDisabled:
                    SaveBlockers.Add(context);
                    break;
                case SaveBehaviour.SaveEnabled:
                {
                    if (!SaveBlockers.Contains(context))
                    {
                        Debug.LogError($"trying to remove {context} blocking context when it wasn't blocking!");
                    }

                    SaveBlockers.Remove(context);
                    break;
                }
            }
        }
    }
}