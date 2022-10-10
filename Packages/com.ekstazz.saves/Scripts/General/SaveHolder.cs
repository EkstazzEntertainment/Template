namespace Ekstazz.Saves
{
    using System;
    using System.Threading.Tasks;
    using Data;


    internal class SaveHolder
    {
        private SaveData snapshot;
        private TaskCompletionSource<SaveData> tcs = new TaskCompletionSource<SaveData>();

        
        public async Task<SaveData> RetreiveAsync()
        {
            var res =  await tcs.Task;
            tcs = new TaskCompletionSource<SaveData>();
            return res;
        }

        public void SetSave(SaveData save)
        {
            tcs.SetResult(save);
        }
        
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