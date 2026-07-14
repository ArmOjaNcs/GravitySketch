using System.Collections;
using Assets.Sources.Level;
using Assets.Sources.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class LoadWindow : LevelScore
    {
        [SerializeField] private Image _loadImage;
        [SerializeField] private Sprite[] _loadSprites;
        [SerializeField] private Slider _loadProgress;
        [SerializeField] private CanvasGroup _canvasGroup;

        private AsyncOperation _operation;

        private void Start()
        {
            _canvasGroup.alpha = 0;
            int random = Random.Range(0, _loadSprites.Length);
            _loadImage.sprite = _loadSprites[random];
            StartCoroutine(LoadAsync());
        }

        private IEnumerator LoadAsync()
        {
            switch (Progress.SceneType)
            {
                case SceneType.Main:
                    _operation = SceneManager.LoadSceneAsync(UserUtils.Main);
                    break;

                case SceneType.Collect:
                    _operation = SceneManager.LoadSceneAsync(UserUtils.Collect);
                    break;

                case SceneType.Paint:
                    _operation = SceneManager.LoadSceneAsync(UserUtils.Paint);
                    break;
            }

            _operation.allowSceneActivation = false;

            float visualProgress = 0f;

            while (_operation.progress < 0.9f)
            {
                float target = _operation.progress / 0.9f;

                visualProgress = Mathf.MoveTowards(
                    visualProgress,
                    target,
                    Time.deltaTime);

                _loadProgress.value = visualProgress;
                yield return null;
            }

            while (visualProgress < 1f)
            {
                visualProgress = Mathf.MoveTowards(
                    visualProgress,
                    1f,
                    Time.deltaTime);

                _loadProgress.value = visualProgress;
                yield return null;
            }

            yield return FadeIn();

            while (Input.anyKey == false && Input.GetMouseButton(0) == false && Input.GetMouseButton(1) == false)
                yield return null;

            _operation.allowSceneActivation = true;
        }

        private IEnumerator FadeIn()
        {
            float elapsedTime = 0f;

            while (_canvasGroup.alpha < 1f)
            {
                elapsedTime += Time.deltaTime * 2.5f;
                _canvasGroup.alpha = Mathf.Clamp01(elapsedTime);
                yield return null;
            }
        }
    }
}