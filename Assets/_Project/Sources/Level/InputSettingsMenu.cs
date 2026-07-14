using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Sources.Save;
using Assets.Sources.UI;
using Assets.Sources.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Enum = System.Enum;

namespace Assets.Sources.Level
{
    public class InputSettingsMenu : MonoBehaviour, IDisposable
    {
        [Header("ControlButtons")]
        [SerializeField] private Button _moveUpButton;
        [SerializeField] private Button _moveDownButton;
        [SerializeField] private Button _moveLeftButton;
        [SerializeField] private Button _moveRightButton;
        [SerializeField] private Button _boostButton;
        [SerializeField] private Button _shieldButton;
        [SerializeField] private Button _rotateLeftButton;
        [SerializeField] private Button _rotateRightButton;

        [Header("PaintButtons")]
        [SerializeField] private Button _paintButton;
        [SerializeField] private Button _resetCubeButton;

        [Header("Options")]
        [SerializeField] private Toggle _useMouseRotationToggle;
        [SerializeField] private Toggle _virtualJoystickToggle;
        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _backButton;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _messageText;
        [SerializeField] private VerticalScrollContent _content;

        [Header("Visuals")]
        [SerializeField] private Color _waitingColor = Color.yellow;

        private InputBindings _bindings;
        private Button _waitingButton;
        private Coroutine _currentRebindRoutine;
        private bool _isStarted;
        private Color _originalColor;
        private Dictionary<ButtonType, TextMeshProUGUI> _buttonsText = new ();

        private void Awake()
        {
            _buttonsText.Add(ButtonType.MoveUp, _moveUpButton.GetComponentInChildren<TextMeshProUGUI>());
            _buttonsText.Add(ButtonType.MoveDown, _moveDownButton.GetComponentInChildren<TextMeshProUGUI>());
            _buttonsText.Add(ButtonType.MoveLeft, _moveLeftButton.GetComponentInChildren<TextMeshProUGUI>());
            _buttonsText.Add(ButtonType.MoveRight, _moveRightButton.GetComponentInChildren<TextMeshProUGUI>());
            _buttonsText.Add(ButtonType.Boost, _boostButton.GetComponentInChildren<TextMeshProUGUI>());
            _buttonsText.Add(ButtonType.Defend, _shieldButton.GetComponentInChildren<TextMeshProUGUI>());
            _buttonsText.Add(ButtonType.RotateLeft, _rotateLeftButton.GetComponentInChildren<TextMeshProUGUI>());
            _buttonsText.Add(ButtonType.RotateRight, _rotateRightButton.GetComponentInChildren<TextMeshProUGUI>());
            _buttonsText.Add(ButtonType.Paint, _paintButton.GetComponentInChildren<TextMeshProUGUI>());
            _buttonsText.Add(ButtonType.ResetCube, _resetCubeButton.GetComponentInChildren<TextMeshProUGUI>());
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Rebuild()
        {
            if (_bindings == null)
                _bindings = SaveSystem.LoadInputBindings();

            Subscribe();
            UpdateLabels();
            _content.Rebuild();
        }

        public void Dispose()
        {
            if (_isStarted == false)
                return;

            UnSubscribe();
        }

        private void OnMoveUp() => StartRebind(_moveUpButton, ButtonType.MoveUp);

        private void OnMoveDown() => StartRebind(_moveDownButton, ButtonType.MoveDown);

        private void OnMoveLeft() => StartRebind(_moveLeftButton, ButtonType.MoveLeft);

        private void OnMoveRight() => StartRebind(_moveRightButton, ButtonType.MoveRight);

        private void OnBoost() => StartRebind(_boostButton, ButtonType.Boost);

        private void OnShield() => StartRebind(_shieldButton, ButtonType.Defend);

        private void OnRotateLeft() => StartRebind(_rotateLeftButton, ButtonType.RotateLeft);

        private void OnRotateRight() => StartRebind(_rotateRightButton, ButtonType.RotateRight);

        private void OnPaint() => StartRebind(_paintButton, ButtonType.Paint);

        private void OnResetCube() => StartRebind(_resetCubeButton, ButtonType.ResetCube);

        private void UpdateLabels()
        {
            foreach (var (buttonType, textComponent) in _buttonsText)
                textComponent.SetText(FormatKey(_bindings.GetKeyCode(buttonType)));
        }

        private string FormatKey(KeyCode key)
        {
            if (key == KeyCode.None)
                return "—";

            return Translator.GetKey(key);
        }

        private void StartRebind(Button button, ButtonType buttonType)
        {
            if (_currentRebindRoutine != null)
                StopCoroutine(_currentRebindRoutine);

            _waitingButton = button;

            EventSystem.current.SetSelectedGameObject(null);

            _originalColor = _waitingButton.image.color;
            _waitingButton.image.color = _waitingColor;
            _waitingButton.interactable = false;
            _messageText.enabled = true;

            _currentRebindRoutine = StartCoroutine(WaitForKey(buttonType));
        }

        private IEnumerator WaitForKey(ButtonType buttonType)
        {
            yield return null;

            while (Input.anyKeyDown == false)
                yield return null;

            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    if (key == KeyCode.Escape || key == KeyCode.LeftWindows
                        || key == KeyCode.RightWindows || key == KeyCode.LeftCommand)
                    {
                        _waitingButton.interactable = true;
                        _messageText.enabled = false;
                        _waitingButton.image.color = _originalColor;
                        UpdateLabels();
                        yield break;
                    }

                    AssignKey(buttonType, key);
                    SaveSystem.SaveInputBindings(_bindings);
                    break;
                }
            }

            _waitingButton.image.color = _originalColor;
            _waitingButton.interactable = true;
            _messageText.enabled = false;
            _currentRebindRoutine = null;
        }

        private void AssignKey(ButtonType buttonType, KeyCode key)
        {
            bool isControlGroup = IsControlGroup(buttonType);

            foreach (ButtonType type in Enum.GetValues(typeof(ButtonType)))
            {
                if (IsControlGroup(type) == isControlGroup)
                {
                    if (_bindings.GetKeyCode(type) == key)
                    {
                        _bindings.SetKey(type, KeyCode.None);
                    }
                }
            }

            _bindings.SetKey(buttonType, key);
            UpdateLabels();
        }

        private void OnMouseToggleChanged(bool value)
        {
            _bindings.UseMouseRotation = value;
            SaveSystem.SaveInputBindings(_bindings);
        }

        private void OnJoystickToggleChanged(bool value)
        {
            _bindings.UseJoystick = value;
            SaveSystem.SaveInputBindings(_bindings);
        }

        private void OnResetDefaults()
        {
            _bindings = InputBindings.GetDefault();
            SaveSystem.SaveInputBindings(_bindings);
            UpdateLabels();
            _useMouseRotationToggle.isOn = _bindings.UseMouseRotation;
            _virtualJoystickToggle.isOn = _bindings.UseJoystick;
            _messageText.enabled = false;
        }

        private void OnBack()
        {
            SaveSystem.SaveInputBindings(_bindings);
            _messageText.enabled = false;
        }

        private bool IsControlGroup(ButtonType type)
        {
            return type <= ButtonType.RotateRight;
        }

        private void Subscribe()
        {
            _moveUpButton.onClick.AddListener(OnMoveUp);
            _moveDownButton.onClick.AddListener(OnMoveDown);
            _moveLeftButton.onClick.AddListener(OnMoveLeft);
            _moveRightButton.onClick.AddListener(OnMoveRight);
            _boostButton.onClick.AddListener(OnBoost);
            _shieldButton.onClick.AddListener(OnShield);
            _rotateLeftButton.onClick.AddListener(OnRotateLeft);
            _rotateRightButton.onClick.AddListener(OnRotateRight);

            _paintButton.onClick.AddListener(OnPaint);
            _resetCubeButton.onClick.AddListener(OnResetCube);

            _resetButton.onClick.AddListener(OnResetDefaults);
            _backButton.onClick.AddListener(OnBack);

            _useMouseRotationToggle.isOn = _bindings.UseMouseRotation;
            _useMouseRotationToggle.onValueChanged.AddListener(OnMouseToggleChanged);
            _virtualJoystickToggle.isOn = _bindings.UseJoystick;
            _virtualJoystickToggle.onValueChanged.AddListener(OnJoystickToggleChanged);

            _messageText.enabled = false;
            _isStarted = true;
        }

        private void UnSubscribe()
        {
            _moveUpButton.onClick.RemoveListener(OnMoveUp);
            _moveDownButton.onClick.RemoveListener(OnMoveDown);
            _moveLeftButton.onClick.RemoveListener(OnMoveLeft);
            _moveRightButton.onClick.RemoveListener(OnMoveRight);
            _boostButton.onClick.RemoveListener(OnBoost);
            _shieldButton.onClick.RemoveListener(OnShield);
            _rotateLeftButton.onClick.RemoveListener(OnRotateLeft);
            _rotateRightButton.onClick.RemoveListener(OnRotateRight);

            _paintButton.onClick.RemoveListener(OnPaint);
            _resetCubeButton.onClick.RemoveListener(OnResetCube);

            _resetButton.onClick.RemoveListener(OnResetDefaults);
            _backButton.onClick.RemoveListener(OnBack);

            _useMouseRotationToggle.onValueChanged.RemoveListener(OnMouseToggleChanged);
            _virtualJoystickToggle.onValueChanged.RemoveListener(OnJoystickToggleChanged);
        }
    }
}