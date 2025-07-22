using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
            var operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            var targetProgress = 0f;
            var fakeProgress = 0f;

            while (fakeProgress < 0.9f)
            {
                fakeProgress += Time.deltaTime * 0.3f;
                slider.value = fakeProgress;
                yield return null;
            }

            while (operation.progress < 0.9f) yield return null;
            while (slider.value < 1f)
            {
                slider.value += Time.deltaTime * 0.5f;
                yield return null;
            }

            yield return new WaitForSeconds(0.2f);
            operation.allowSceneActivation = true;
            yield return new WaitForSeconds(0.3f);
            UIManager.Instance.ShowView(ScreenType.Title);
        }
    }
}