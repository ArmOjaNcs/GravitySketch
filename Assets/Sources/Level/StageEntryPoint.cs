using Assets.Sources.Audio;
using Assets.Sources.Pause;
using Assets.Sources.UI;
using Assets.Sources.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Level
{
    public abstract class StageEntryPoint : MonoBehaviour
    {
        [SerializeField] private protected Stage Stage;
        [SerializeField] private PauseInput _pauseInput;
        [SerializeField] private protected List<PauseableObject> Objects;
        [SerializeField] private protected AudioPlayerSpawner AudioPlayerSpawner;
        [SerializeField] private PauseMenu _pauseMenu;
        [SerializeField] private LoadWindow _loadWindow;

        private protected PauseHandler PauseHandler;

        private void OnEnable()
        {
            Stage.Finished += OnStageFinished;
            
        }

        private void OnDisable()
        {
            Stage.Finished -= OnStageFinished;
        }

        private void Start()
        {
            Enter();
        }

        private protected void Enter() => StartCoroutine(StartStage());

        private IEnumerator StartStage()
        {
            PauseHandler = new PauseHandler();
            _pauseMenu.Init(PauseHandler, _pauseInput);
            _loadWindow.Init(PauseHandler);
            _loadWindow.Updated += OnLoadWindowUpdated;
            AudioPlayerSpawner.SetPauseHandler(PauseHandler);
            Initialize();

            yield return new WaitForSeconds(UserUtils.LoadTime);
            _loadWindow.FadeOut();
        }

        private protected void StopPauseInput() => _pauseInput.StopInput();

        private protected virtual void OnLoadWindowUpdated() => _pauseInput.StartInput();

        private protected abstract void Initialize();
        private void OnStageFinished() => StopPauseInput();
    }
}