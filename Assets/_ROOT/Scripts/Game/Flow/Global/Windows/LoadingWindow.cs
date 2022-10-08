namespace Ekstazz.Game.Flow
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Ui;
    using UnityEngine;
    using UnityEngine.UI;


    public class LoadingWindow : Window
    {
        [SerializeField] private Slider loadingProgress;
        [SerializeField] private List<FakeProgressStep> progressSteps;
        [SerializeField] private float finalAwait;

        public event Action ProgressFinished;
        public bool IsFinished { get; private set; }
        
        private Coroutine coroutine;
        
        
        protected override void OnWindowOpened()
        {
            base.OnWindowOpened();

            coroutine = StartCoroutine(FillFakeProgress());
        }

        private IEnumerator FillFakeProgress()
        {
            for (int i = 0; i < progressSteps.Count; i++)
            {
                yield return new WaitForSeconds(progressSteps[i].time);
                loadingProgress.value = progressSteps[i].step;
            }

            yield return new WaitForSeconds(finalAwait);
            
            ProgressFinished?.Invoke();
            IsFinished = true;
        }
    }

    
    [Serializable]
    public class FakeProgressStep
    {
        public float step;
        public float time;
    }
}
