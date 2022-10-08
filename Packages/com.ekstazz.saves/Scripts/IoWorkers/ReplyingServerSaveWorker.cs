#pragma warning disable 4014
namespace Ekstazz.Saves
{
//    using System;
//    using System.Threading.Tasks;
//    using MyGym.Core;
//    using MyGym.Core.Analytics;
//    using MyGym.Core.Network;
//    using MyGym.Core.Network.Core;
//    using MyGym.Core.Strange.Signals;
//    using MyGym.Data;
//    
//    using View.UI;
//
//    /// <summary>
//    /// Reliable deliver of savefile to server is transport task
//    /// (like TCP)
//    /// </summary>
//    public class ReplyingServerSaveWorker : ISaveIoWorker
//    {
//        [Inject]
//        public IServerApi ServerApi { get; set; }
//
//        [Inject]
//        public NetworkSettings NetworkSettings { get; set; }
//
//        [Inject]
//        public Session Session { get; set; }
//
//        [Inject]
//        public ReloadGameCompletely ReloadGameCompletly { get; set; }
//
//        [Inject]
//        public IAnalytics Analytics { get; set; }
//
//        [Inject]
//        public IModalConfirmWindow ModalConfirmWindow { get; set; }
//
//        private string pendingRequest;
//
//        [PostConstruct]
//        public void Init()
//        {
//            StartUploading();
//        }
//
//        private async Task StartUploading()
//        {
//            while (true)
//            {
//                await Task.Delay((int)NetworkSettings.Data.saveGameUploadRate * 1000);
//                if (!Session.SessionStarted || pendingRequest == null)
//                {
//                    continue;
//                }
//
//                var closure = pendingRequest;
//                var result = await ServerApi.PostProfileAsync(closure);
//
//                if (result.Status == SendProfileDataStatus.Conflict)
//                {
//                    NotifyAboutServerConflict();
//                    break;
//                }
//
//
//                if (pendingRequest == closure)
//                {
//                    pendingRequest = null;
//                }
//            }
//        }
//
//        public async Task<byte[]> Read(string key)
//        {
//            var res = await ServerApi.GetProfileAsync();
//            var packed = res.Result?.save;
//            if (packed == null)
//            {
//                return null;
//            }
//
//            return Convert.FromBase64String(packed);
//        }
//
//        public Task<bool> Write(string key, byte[] data)
//        {
//            //prevent sending profile before login on any conditions
//            var packed = Convert.ToBase64String(data);
//            pendingRequest = packed;
//            return Task.FromResult(true);
//        }
//
//
//        private void NotifyAboutServerConflict()
//        {
//            Analytics.GameEvent("Save Upload Conflict: In-Game");
//            ModalConfirmWindow.QueueModalConfirmMessage(
//                Loc.Get("InGameSyncSaveConflictHeader"),
//                Loc.Get("InGameSyncSaveConflictMessage"), () => ReloadGameCompletly.Dispatch(),
//                Loc.Get("idRestart"));
//        }
//
//    }
}