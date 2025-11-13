using Assets.Sources.Save;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Sources.Level
{
    public class InputSettingsMenu : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _moveUpButton;
        [SerializeField] private Button _moveDownButton;
        [SerializeField] private Button _moveLeftButton;
        [SerializeField] private Button _moveRightButton;
        [SerializeField] private Button _boostButton;
        [SerializeField] private Button _shieldButton;
        [SerializeField] private Button _rotateLeftButton;
        [SerializeField] private Button _rotateRightButton;
        [SerializeField] private Button _resetButton;
        [SerializeField] private Button _backButton;

        [Header("Options")]
        [SerializeField] private Toggle _useMouseRotationToggle;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _messageText;

        private InputBindings _bindings;
        private Button _waitingButton;
        private string _waitingField;
        private Coroutine _currentRebindRoutine;

        private void Start()
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

            _resetButton.onClick.AddListener(OnResetDefaults);
            _backButton.onClick.AddListener(OnBack);

            _useMouseRotationToggle.isOn = _bindings.UseMouseRotation;
            _useMouseRotationToggle.onValueChanged.AddListener(OnMouseToggleChanged);

            _messageText.enabled = false;
            UpdateLabels();
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
        }

        private string FormatKey(KeyCode key)
        {
            return key == KeyCode.None ? "Ч" : key.ToString();
        }

        private void StartRebind(Button button, string fieldName)
        {
            // если уже идЄт ожидание, прерываем старое
            if (_currentRebindRoutine != null)
                StopCoroutine(_currentRebindRoutine);

            _waitingButton = button;
            _waitingField = fieldName;

            _messageText.enabled = true;

            _currentRebindRoutine = StartCoroutine(WaitForKey());
        }

        private IEnumerator WaitForKey()
        {
            yield return null;

            while (!Input.anyKeyDown)
                yield return null;

            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    if (key == KeyCode.Escape)
                    {
                        _messageText.enabled = false;
                        UpdateLabels();
                        yield break;
                    }

                    AssignKey(_waitingField, key);
                    UpdateLabels();
                    SaveSystem.SaveInputBindings(_bindings);
                    break;
                }
            }

            _messageText.enabled = false;
            _currentRebindRoutine = null;
        }

        private void AssignKey(string field, KeyCode key)
        {
            foreach (var f in typeof(InputBindings).GetFields())
            {
                if (f.FieldType != typeof(KeyCode))
                    continue;

                if ((KeyCode)f.GetValue(_bindings) == key)
                    f.SetValue(_bindings, KeyCode.None);
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

        private void OnResetDefaults()
        {
            _bindings = InputBindings.GetDefault();
            SaveSystem.SaveInputBindings(_bindings);
            UpdateLabels();
            _useMouseRotationToggle.isOn = _bindings.UseMouseRotation;
            _messageText.enabled = false;
        }

        private void OnBack()
        {
            SaveSystem.SaveInputBindings(_bindings);
            _messageText.enabled = false;
        }
    }
}