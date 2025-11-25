using Assets.Sources.Save;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        [Header("Visuals")]
        [SerializeField] private Color _waitingColor = Color.yellow;

        private InputBindings _bindings;
        private Button _waitingButton;
        private string _waitingField;
        private Coroutine _currentRebindRoutine;
        private bool _isStarted;
        private Color _originalColor;

        private void OnEnable()
        {
            _bindings = SaveSystem.LoadInputBindings();

            _moveUpButton.onClick.AddListener(() => StartRebind(_moveUpButton, "MoveUp"));
            _moveDownButton.onClick.AddListener(() => StartRebind(_moveDownButton, "MoveDown"));
            _moveLeftButton.onClick.AddListener(() => StartRebind(_moveLeftButton, "MoveLeft"));
            _moveRightButton.onClick.AddListener(() => StartRebind(_moveRightButton, "MoveRight"));
            _boostButton.onClick.AddListener(() => StartRebind(_boostButton, "Boost"));
            _shieldButton.onClick.AddListener(() => StartRebind(_shieldButton, "Defend"));
            _rotateLeftButton.onClick.AddListener(() => StartRebind(_rotateLeftButton, "RotateLeft"));
            _rotateRightButton.onClick.AddListener(() => StartRebind(_rotateRightButton, "RotateRight"));

            _paintButton.onClick.AddListener(() => StartRebind(_paintButton, "Paint"));
            _resetCubeButton.onClick.AddListener(() => StartRebind(_resetCubeButton, "ResetCube"));

            _resetButton.onClick.AddListener(OnResetDefaults);
            _backButton.onClick.AddListener(OnBack);

            _useMouseRotationToggle.isOn = _bindings.UseMouseRotation;
            _useMouseRotationToggle.onValueChanged.AddListener(OnMouseToggleChanged);
            _virtualJoystickToggle.isOn = _bindings.UseJoystick;
            _virtualJoystickToggle.onValueChanged.AddListener(OnJoystickToggleChanged);

            _messageText.enabled = false;
            UpdateLabels();
            _isStarted = true;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_isStarted == false)
                return;

            _moveUpButton.onClick.RemoveListener(() => StartRebind(_moveUpButton, "MoveUp"));
            _moveDownButton.onClick.RemoveListener(() => StartRebind(_moveDownButton, "MoveDown"));
            _moveLeftButton.onClick.RemoveListener(() => StartRebind(_moveLeftButton, "MoveLeft"));
            _moveRightButton.onClick.RemoveListener(() => StartRebind(_moveRightButton, "MoveRight"));
            _boostButton.onClick.RemoveListener(() => StartRebind(_boostButton, "Boost"));
            _shieldButton.onClick.RemoveListener(() => StartRebind(_shieldButton, "Defend"));
            _rotateLeftButton.onClick.RemoveListener(() => StartRebind(_rotateLeftButton, "RotateLeft"));
            _rotateRightButton.onClick.RemoveListener(() => StartRebind(_rotateRightButton, "RotateRight"));

            _paintButton.onClick.RemoveListener(() => StartRebind(_paintButton, "Paint"));
            _resetCubeButton.onClick.RemoveListener(() => StartRebind(_resetCubeButton, "ResetCube"));

            _resetButton.onClick.RemoveListener(OnResetDefaults);
            _backButton.onClick.RemoveListener(OnBack);

            _useMouseRotationToggle.onValueChanged.RemoveListener(OnMouseToggleChanged);
            _virtualJoystickToggle.onValueChanged.RemoveListener(OnJoystickToggleChanged);
        }

        private void UpdateLabels()
        {
            _moveUpButton.GetComponentInChildren<TextMeshProUGUI>().text = FormatKey(_bindings.MoveUp);
            _moveDownButton.GetComponentInChildren<TextMeshProUGUI>().text = FormatKey(_bindings.MoveDown);
            _moveLeftButton.GetComponentInChildren<TextMeshProUGUI>().text = FormatKey(_bindings.MoveLeft);
            _moveRightButton.GetComponentInChildren<TextMeshProUGUI>().text = FormatKey(_bindings.MoveRight);
            _boostButton.GetComponentInChildren<TextMeshProUGUI>().text = FormatKey(_bindings.Boost);
            _shieldButton.GetComponentInChildren<TextMeshProUGUI>().text = FormatKey(_bindings.Defend);
            _rotateLeftButton.GetComponentInChildren<TextMeshProUGUI>().text = FormatKey(_bindings.RotateLeft);
            _rotateRightButton.GetComponentInChildren<TextMeshProUGUI>().text = FormatKey(_bindings.RotateRight);
            _paintButton.GetComponentInChildren<TextMeshProUGUI>().text = FormatKey(_bindings.Paint);
            _resetCubeButton.GetComponentInChildren<TextMeshProUGUI>().text = FormatKey(_bindings.ResetCube);
        }

        private string FormatKey(KeyCode key)
        {
            return key == KeyCode.None ? "—" : key.ToString();
        }

        private void StartRebind(Button button, string fieldName)
        {
            if (_currentRebindRoutine != null)
                StopCoroutine(_currentRebindRoutine);

            _waitingButton = button;
            _waitingField = fieldName;
            _originalColor = _waitingButton.image.color;
            _waitingButton.image.color = _waitingColor;
            _messageText.enabled = true;

            _currentRebindRoutine = StartCoroutine(WaitForKey());
        }

        private IEnumerator WaitForKey()
        {
            yield return null;

            while (Input.anyKeyDown == false)
                yield return null;

            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    if (key == KeyCode.Escape)
                    {
                        _messageText.enabled = false;
                        _waitingButton.image.color = _originalColor;
                        UpdateLabels();
                        yield break;
                    }

                    AssignKey(_waitingField, key);
                    UpdateLabels();
                    SaveSystem.SaveInputBindings(_bindings);
                    break;
                }
            }

            _waitingButton.image.color = _originalColor;
            _messageText.enabled = false;
            _currentRebindRoutine = null;
        }

        private void AssignKey(string field, KeyCode key)
        {
            string[] controlGroup =
            {
                "MoveUp", "MoveDown", "MoveLeft", "MoveRight",
                "Boost", "Defend",
                "RotateLeft", "RotateRight"
            };
            
            string[] paintGroup =
            {
                "Paint", "ResetCube"
            };

            string[] activeGroup;

            if (Array.Exists(controlGroup, f => f == field))
                activeGroup = controlGroup;
            else
                activeGroup = paintGroup;

            foreach (string f in activeGroup)
            {
                var fieldInfo = typeof(InputBindings).GetField(f);

                if (fieldInfo != null &&
                    fieldInfo.FieldType == typeof(KeyCode) &&
                    (KeyCode)fieldInfo.GetValue(_bindings) == key)
                {
                    fieldInfo.SetValue(_bindings, KeyCode.None);
                }
            }

            var targetField = typeof(InputBindings).GetField(field);

            if (targetField != null && targetField.FieldType == typeof(KeyCode))
                targetField.SetValue(_bindings, key);
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
    }
}