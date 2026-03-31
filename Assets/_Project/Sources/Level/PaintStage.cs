using Assets.Sources.Audio;
using Assets.Sources.ColorizerScripts;
using Assets.Sources.Pause;
using Assets.Sources.PlayerScripts;
using Assets.Sources.Save;
using Assets.Sources.Table;
using Assets.Sources.UI;
using Assets.Sources.Utils;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using YG;

namespace Assets.Sources.Level
{
    public class PaintStage : Stage
    {
        [SerializeField] private Colorizer _colorizer;
        [SerializeField] private Validator _validator;
        [SerializeField] private ColoringPositionHandler _positionHandler;
        [SerializeField] private ColorReferenceViewHandler _referenceViewer;
        [SerializeField] private Toggle _autoPaint;
        [SerializeField] private GameObject _aim;
        [SerializeField] private FixedJoystick _joystick;
        [SerializeField] private Button _toNextLevel;
        [SerializeField] private MenuWindow _toNextLevelAnimator;
        [SerializeField] private MenuWindow _tutorialStartWindow;
        [SerializeField] private Button _tutorialAccept;
        [SerializeField] private Button _tutorialDecline;
        [SerializeField] private HoldButton _paint;
        [SerializeField] private SinglePressButton _reset;
        [SerializeField] private HoleMover _hole;
        [SerializeField] private SmoothedFade[] _interfacesFade;
        [SerializeField] private Scroller _scroller;
        [SerializeField] private ScoreView _scoreView;
        [SerializeField] private TextMeshProUGUI _finalScore;
        [SerializeField] private AudioSource _backgroundMusic;

        private TemplateColorReference _colorReference;
        private Template _template;
        private Tutorial _tutorial;
        private bool _isFinished;
        private bool _isTutorialAccepted;
        private string _nextStageName = string.Empty;

        public bool IsReferenceShowing { get; private set; }
        public IReadOnlyList<IReadonlyTemplateCube> TemplateCubes => _template.TemplateCubes;
        public KeyCode Paint => Bindings.Paint;
        public KeyCode ResetCube => Bindings.ResetCube;

        private protected override void OnEnable()
        {
            base.OnEnable();
            _validator.Updated += OnFinished;
            _referenceViewer.IsShowing += OnShowing;
            _autoPaint.onValueChanged.AddListener(OnAutoPaint);
            _toNextLevel.onClick.AddListener(OnNextApplied);
            _tutorialAccept.onClick.AddListener(OnTutorialAccept);
            _tutorialDecline.onClick.AddListener(OnTutorialDecline);
        }

        private protected override void OnDisable()
        {
            base.OnDisable();
            _validator.Updated -= OnFinished;
            _referenceViewer.IsShowing -= OnShowing;
            _autoPaint.onValueChanged.RemoveListener(OnAutoPaint);
            _toNextLevel.onClick.RemoveListener(OnNextApplied);
            _tutorialAccept.onClick.RemoveListener(OnTutorialAccept);
            _tutorialDecline.onClick.RemoveListener(OnTutorialDecline);
        }

        private void Start()
        {
            StartCoroutine(RefreshEventSystem(()=> _backgroundMusic.Play()));
        }

        public override void Init(PauseHandler pauseHandler, AudioPlayerSpawner audioPlayerSpawner)
        {
            base.Init(pauseHandler, audioPlayerSpawner);
            _colorizer.SetStage(this, CurrentColors);
            _colorizer.Init(pauseHandler);
            _colorizer.SetResetButton(_reset);
            _validator.Init(pauseHandler);
            _validator.SetStage(this);
            _referenceViewer.Init(pauseHandler);
            _positionHandler.SetPaintStage(this);
            _positionHandler.Init(pauseHandler);
            _positionHandler.SetJoystick(_joystick, _paint);
            _scoreView.Init(pauseHandler);
            _scoreView.SetStartScore(CurrentScore);
            _scroller.Init(pauseHandler);
            _toNextLevel.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }

        public void SetTemplate(Template template, TemplateColorReference colorReference)
        {
            _template = template;
            _template.Init();
            _colorReference = colorReference;
            _colorReference.ResetEntriesCurrentIndex();
        }

        public IReadonlyTemplateCube GetCubeByColor(Color color)
        {
            while (_colorReference.HasFreeIndex(color))
            {
                if (_colorReference.TryGetIndexByColor(color, out int index))
                {
                    IReadonlyTemplateCube templateCube = TemplateCubes.First(tc => tc.Index == index);

                    if (templateCube != null && templateCube.IsMarked == false)
                        return templateCube;
                }
                else
                {
                    break;
                }
            }

            return null;
        }

        public IReadonlyTemplateCube GetCube(int index)
        {
            return _template.GetCube(index);
        }

        public Color GetColor(int index)
        {
            return _colorReference.GetColor(index);
        }

        public override void SetTutorialObject(GameObject tutorialObject)
        {
            base.SetTutorialObject(tutorialObject);
            _tutorial = TutorialObject.GetComponent<Tutorial>();
            _tutorial.Closed += OnTutorialClosed;
            _tutorialStartWindow.Closed += OnTutorialStartWindowClosed;
        }

        public override void Begin()
        {
            if (Progress.IsTutorial)
            {
                PauseHandler.Pause();
                Pause.interactable = false;
                _tutorialStartWindow.Show();
                return;
            }

            base.Begin();
        }

        private void OnTutorialClosed()
        {
            _tutorial.Closed -= OnTutorialClosed;
            base.Begin();
            PauseHandler.Resume();
            Pause.interactable = true;
        }

        private protected override void OnVirtualJoystickValueChanged(bool value)
        {
            _paint.gameObject.SetActive(value);
            _reset.gameObject.SetActive(value);
            _joystick.gameObject.SetActive(value);
            _positionHandler.EnableJoystickControl(value);
            _colorizer.EnableJoystickControl(value);
            _hole.EnableJoystickControl(value);
            base.OnVirtualJoystickValueChanged(value);
        }

        private void OnFinished()
        {
            if (_isFinished == false)
            {
                StartCoroutine(FinishRoutine());
                _isFinished = true;
            }
        }

        private void OnShowing(bool isShowing)
        {
            IsReferenceShowing = isShowing;
          
            if (isShowing)
                _colorReference.HighlightAllCubes(TemplateCubes);
            else
                foreach (var cube in TemplateCubes)
                    cube.StopHighlight();
        }

        private void OnAutoPaint(bool isAutoPaint)
        {
            AudioPlayerSpawner.GetAudioPlayer()?.SetUI()?.SetAudioClip(ToggleSound)?.Play();
            _colorizer.SetAutoPaint(isAutoPaint);
            _positionHandler.SetAutoPaint(isAutoPaint);
            _referenceViewer.SetAutoPaint(isAutoPaint);
        }

        private IEnumerator FinishRoutine()
        {
            Finish();
            _paint.gameObject.SetActive(false);
            _reset.gameObject.SetActive(false);
            Pause.interactable = false;
            Transform referenceAndAutoPaint = _autoPaint.transform.parent;
            referenceAndAutoPaint.gameObject.SetActive(false);
            _aim.SetActive(false);

            if (UserUtils.TryGetNextStageName(StageName, out string nextStageName))
            {
                _toNextLevel.gameObject.SetActive(true);
                _nextStageName = nextStageName;
            }
            else
            {
                _toNextLevel.gameObject.SetActive(false);
            }

            int finalScore = _validator.MatchScore + CurrentScore;
            Progress.UpdateLevelScore(UserUtils.GetCollectStageName(StageName), finalScore);
            _finalScore.text = finalScore.ToString();
            SaveSystem.SavePlayerProgress(Progress);

            yield return new WaitForSeconds(UserUtils.Unit);

            AudioPlayerSpawner.GetAudioPlayer()?.SetUI()?.SetAudioClip(FinalSound)?.Play();
            _template.DropDown(PauseHandler);
            _interfacesFade[0].Updated += OnInterfaceClosed;

            foreach (SmoothedFade fade in _interfacesFade)
                fade.FadeOut(UserUtils.HalfOfUnit);

            yield return new WaitForSeconds(UserUtils.Unit);

            _hole.SetStarted();
        }

        private void OnInterfaceClosed()
        {
            _interfacesFade[0].Updated -= OnInterfaceClosed;

            if (YG2.isTimerAdvCompleted)
            {
                YG2.onCloseInterAdv += OnInterAdvClosed;
                YG2.onErrorInterAdv += OnInterAdvClosed;
                YG2.InterstitialAdvShow();
            }
            else
            {
                ShowFinalWindow();
            }
        }

        private void ShowFinalWindow()
        {
            foreach (MenuWindow buttonAnimator in Buttons)
                buttonAnimator.MoveToFinalPosition();

            TextWindow.Opened += OnTextWindowOpened;
            TextWindow.Show();
        }

        private void OnInterAdvClosed()
        {
            YG2.onCloseInterAdv -= OnInterAdvClosed;
            YG2.onErrorInterAdv -= OnInterAdvClosed;
            StartCoroutine(RefreshEventSystem(ShowFinalWindow));
        }

        private void OnTextWindowOpened()
        {
            TextWindow.Opened -= OnTextWindowOpened;

            if (_nextStageName != string.Empty)
                _toNextLevelAnimator.Show();

            ShowButtons();
        }

        private protected override void HideButtons()
        {
            if (TextWindow.IsShown && _toNextLevelAnimator.IsShown)
                _toNextLevelAnimator.Hide();

            base.HideButtons();
        }

        private protected override void OnMainMenuApplied()
        {
            if (_isFinished)
            {
                if (_nextStageName != string.Empty)
                    Progress.SetStageName(_nextStageName);
            }
            else
            {
                Progress.SetStageName(UserUtils.GetCollectStageName(StageName));
            }

            Progress.SetSceneType(SceneType.Main);
            SaveSystem.SavePlayerProgress(Progress);

            base.OnMainMenuApplied();
        }

        private void OnNextApplied()
        {
            if (IsTutorial)
                Progress.SetTutorial(false);

            Progress.SetStageName(_nextStageName);
            Progress.SetSceneType(SceneType.Collect);
            SaveSystem.SavePlayerProgress(Progress);
            AudioPlayerSpawner.GetAudioPlayer()?.SetUI()?.SetAudioClip(ButtonSound)?.Play();
            HideButtons();
            TextWindow.Closed += LoadNext;
            TextWindow.Hide();
        }

        private void LoadNext()
        {
            TextWindow.Closed -= LoadNext;
            SceneManager.LoadScene(UserUtils.Load);
        }

        private void OnTutorialStartWindowClosed()
        {
            _tutorialStartWindow.Closed -= OnTutorialStartWindowClosed;

            if (_isTutorialAccepted)
                _tutorial.Show();
            else
                OnTutorialClosed();
        }

        private void OnTutorialAccept()
        {
            _isTutorialAccepted = true;
            _tutorialStartWindow.Hide();
        }

        private void OnTutorialDecline() => _tutorialStartWindow.Hide();
    }
}