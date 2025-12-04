using Assets.Sources.Level;
using Assets.Sources.Utils;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Sources.UI
{
    public class LoadWindow : LevelScore
    {
        [SerializeField] private Image _loadImage;
        [SerializeField] private Sprite[] _loadSprites;
        [SerializeField] private TextMeshProUGUI _endText;
        [SerializeField] private float _animationDuration;
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

            float elapsedTime = 0;
            float progress = 0;

            while (_operation.progress < 0.9f)
            {
                float target = _operation.progress / 0.9f;
                elapsedTime += Time.deltaTime;
                progress = elapsedTime / UserUtils.LoadTime;
                float startValue = _loadProgress.value;
                _loadProgress.value = Mathf.MoveTowards(startValue, target, progress);

                yield return null;
            }

            elapsedTime = 0;
            progress = 0;

            while (_loadProgress.value < 1f)
            {
                elapsedTime += Time.deltaTime;
                progress = elapsedTime / UserUtils.ThirdOfUnit;

                _loadProgress.value = Mathf.MoveTowards(_loadProgress.value, 1f, progress);

                yield return null;
            }

            yield return WaitForAnyKey();
        }

        private IEnumerator WaitForAnyKey()
        {
            yield return null;

            _loadProgress.value = 1f;
            float elapsedTime = 0;
            float progress = 0;

            while (_canvasGroup.alpha < 1)
            {
                elapsedTime += Time.deltaTime;
                progress = elapsedTime / UserUtils.One;
                float startValue = _canvasGroup.alpha;
                _canvasGroup.alpha = Mathf.MoveTowards(startValue, 1, progress);
                yield return null;
            }

            while (Input.anyKeyDown == false)
                yield return null;

            _operation.allowSceneActivation = true;
        }
    }
}