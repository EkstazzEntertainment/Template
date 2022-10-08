namespace Ekstazz.Saves
{
    using System;
    using System.Threading.Tasks;
    using Data;

    /// <summary>
    /// Save can be loaded before or after moment of main scene loading,
    /// so we need to keep it somewhere till someone's request
    /// </summary>
    internal class SaveHolder
    {
        private SaveData snapshot;

        private TaskCompletionSource<SaveData> tcs = new TaskCompletionSource<SaveData>();

        /// <summary>
        /// Get keeped save and clean up
        /// </summary>
        /// <returns></returns>
        public async Task<SaveData> RetreiveAsync()
        {
            var res =  await tcs.Task;
            //reset tcs after getting
            tcs = new TaskCompletionSource<SaveData>();
            return res;
        }

        public void SetSave(SaveData save)
        {
            tcs.SetResult(save);
        }

        /// <summary>
        /// in some cases, you want to rememeber savedata and restore it after some time
        /// examples: current save when switching to friends gym, or reloading scene when you already 
        /// know loaded and unpaced save
        /// </summary>
        public void MakeSnapshot(SaveData save)
        {
            snapshot = save;
        }

        public void RestoreSnapshot()
        {
            if (snapshot == null)
            {
                throw new Exception("Trying to restore null snapshot!");
            }
            var tmp = snapshot;
            snapshot = null;
            SetSave(tmp);
        }
    }
}