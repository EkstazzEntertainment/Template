namespace Ekstazz.Saves.Flow
{
    using System;
    using System.Threading.Tasks;
    using UnityEngine;
    using Zenject;
    using Zenject.Extensions.Commands;

    public class WaitAndApplySaveCommand : Command
    {
        [Inject]
        internal ISaver Saver { get; set; }

        [Inject]
        internal SaveHolder SaveHolder { get; set; }

        public override async Task Execute()
        {
            var save = await SaveHolder.RetreiveAsync();
            try
            {
                Saver.LoadFromSave(save);
            }
            catch (Exception e)
            {
                Debug.LogError($"Critical error occured during save applying");
                Debug.LogError(e);
            }
        }
    }
}