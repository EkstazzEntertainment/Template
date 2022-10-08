namespace Ekstazz.Saves
{
    using UnityEngine;
    using Worker;
    using Zenject;

    internal class SaveScheduler : ISaveScheduler, ILateTickable
    {
        [Inject]
        internal ISaveContext SaveContext { get; set; }

        [Inject]
        internal ISaveWorker SaveWorker { get; set; }

        [Inject]
        internal ISaver Saver { get; set; }

        private bool frameSaveRequested;

        public void ScheduleSave()
        {
            frameSaveRequested = true;
        }

        public void LateTick()
        {
            CheckForSave();
        }

        private void CheckForSave()
        {
            if (!frameSaveRequested)
            {
                return;
            }

            frameSaveRequested = false;
            PerformSave();
        }

        private void PerformSave()
        {
            if (SaveContext.IsBlocked)
            {
                //if save is disabled of game hasn't started, or already start reloading, return
                Debug.LogWarning($"Save was not performed - {string.Join(",", SaveContext.SaveBlockers)}");
                return;
            }

            SaveWorker.Write(Saver.CreateSave());
        }
    }
}