/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Contributors:    https://github.com/Tayx94/graphy/graphs/contributors
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            15-Dec-17
 * Studio:          Tayx
 *
 * Git repo:        https://github.com/Tayx94/graphy
 *
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;

namespace Tayx.Graphy.Fps
{
    using System.Collections;
    using System.Collections.Generic;
    using Ekstazz.Tools.BST;

    public class G_FpsMonitor : MonoBehaviour
    {
        #region Variables -> Private

        private const int CurrentFpsSamplesCount = 10;
        private readonly Queue<float> currentFrameDurationsQueue = new(CurrentFpsSamplesCount);
        private float smallQueueSum;

        private const int AvgFpsSamplesCount = 1200;
        private readonly Queue<float> avgFrameDurationsQueue = new(AvgFpsSamplesCount);
        private readonly RankedBST<float> avgFrameDurationsBST = new();
        private float bigQueueSum;

        #endregion

        #region Properties -> Public

        public short CurrentFPS => (short) (CurrentFpsSamplesCount / smallQueueSum);
        public float CurrentMs => smallQueueSum / CurrentFpsSamplesCount * 1000;
        
        public bool AreStatsAccumulated { get; private set; }
        public short AverageFPS => (short) (AvgFpsSamplesCount / bigQueueSum);

        public short _80PercentileFPS => (short) (1 / avgFrameDurationsBST.FindKthSmallest(Mathf.RoundToInt(AvgFpsSamplesCount * 0.8f)));
        
        public short _95PercentileFPS => (short) (1 / avgFrameDurationsBST.FindKthSmallest(Mathf.RoundToInt(AvgFpsSamplesCount * 0.95f)));

        #endregion

        #region Methods -> Unity Callbacks

        private void Awake()
        {
            Init();
        }

        private IEnumerator FillSmallQueue()
        {
            for (var i = 0; i < CurrentFpsSamplesCount; i++)
            {
                yield return null;
                var unscaledDeltaTime = Time.unscaledDeltaTime;
                smallQueueSum += unscaledDeltaTime;
                currentFrameDurationsQueue.Enqueue(unscaledDeltaTime);
            }

            StartCoroutine(UpdateCurrentFps());
        }

        private IEnumerator UpdateCurrentFps()
        {
            while (true)
            {
                var lastFrameDuration = currentFrameDurationsQueue.Dequeue();
                smallQueueSum -= lastFrameDuration;

                var unscaledDeltaTime = Time.unscaledDeltaTime;
                smallQueueSum += unscaledDeltaTime;
                currentFrameDurationsQueue.Enqueue(unscaledDeltaTime);
                yield return null;
            }
        }

        private IEnumerator FillBigQueue()
        {
            for (var i = 0; i < AvgFpsSamplesCount; i++)
            {
                yield return null;
                var unscaledDeltaTime = Time.unscaledDeltaTime;
                bigQueueSum += unscaledDeltaTime;
                avgFrameDurationsQueue.Enqueue(unscaledDeltaTime);
                avgFrameDurationsBST.Insert(unscaledDeltaTime);
            }

            AreStatsAccumulated = true;
            StartCoroutine(UpdateAverageFps());
        }

        private IEnumerator UpdateAverageFps()
        {
            while (true)
            {
                var lastFrameDuration = avgFrameDurationsQueue.Dequeue();
                avgFrameDurationsBST.Delete(lastFrameDuration);
                bigQueueSum -= lastFrameDuration;

                var unscaledDeltaTime = Time.unscaledDeltaTime;
                bigQueueSum += unscaledDeltaTime;
                avgFrameDurationsQueue.Enqueue(unscaledDeltaTime);
                avgFrameDurationsBST.Insert(unscaledDeltaTime);
                
                yield return null;
            }
        }

        #endregion

        #region Methods -> Private

        private void Init()
        {
            StartCoroutine(FillSmallQueue());
            StartCoroutine(FillBigQueue());
        }

        #endregion
    }
}