using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public class LoadingView : UIBaseView
    {
        [SerializeField] private Slider slider;

        private void Awake()
        {
            slider.value = 0f;
        }

        public override void OnShow()
        {
            base.OnShow();
            StartCoroutine(LoadAsync("Gameplay"));
        }

        private IEnumerator LoadAsync(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                slider.value = progress;
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);

            UIManager.Instance.ShowView(ViewConstants.TitleView);
        }
    }
}