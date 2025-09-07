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
        [SerializeField] private protected List<PauseableObject> Objects;
        [SerializeField] private protected AudioPlayerSpawner AudioPlayerSpawner;
        [SerializeField] private LoadWindow _loadWindow;

        private protected PauseHandler PauseHandler;

        private void Start()
        {
            Enter();
        }

        private protected void Enter() => StartCoroutine(StartStage());

        private IEnumerator StartStage()
        {
            PauseHandler = new PauseHandler();
            _loadWindow.Init(PauseHandler);
            _loadWindow.Updated += OnLoadWindowUpdated;
            AudioPlayerSpawner.SetPauseHandler(PauseHandler);
            Initialize();

            yield return new WaitForSeconds(UserUtils.LoadTime);
            _loadWindow.FadeOut();
        }

        private protected virtual void OnLoadWindowUpdated() => Stage.Begin();

        private protected abstract void Initialize();
    }
}