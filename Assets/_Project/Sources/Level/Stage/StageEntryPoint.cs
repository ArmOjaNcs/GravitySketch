using System.Collections;
using System.Collections.Generic;
using Audio;
using Pause;
using UnityEngine;

namespace Level.StageScripts
{
    public abstract class StageEntryPoint : MonoBehaviour
    {
        [SerializeField] private protected Stage Stage;
        [SerializeField] private protected List<PauseableObject> Objects;
        [SerializeField] private protected AudioPlayerSpawner AudioPlayerSpawner;

        private protected PauseHandler PauseHandler;
        private protected GameObject Prefab;

        private void Start()
        {
            StartStage();
        }

        private protected virtual void Begin() => Stage.Begin();

        private protected abstract void Initialize();

        private void StartStage()
        {
            PauseHandler = new PauseHandler();
            AudioPlayerSpawner.SetPauseHandler(PauseHandler);
            Initialize();
        }
    }
}