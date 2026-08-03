using System;
using Assets.Sources.Level;
using Assets.Sources.Pause;
using Assets.Sources.Table;
using Assets.Sources.UI;
using UnityEngine;

namespace Assets.Sources.ColorizerScripts
{
    public class ColoringPositionHandler : PauseableObject
    {
        [Header("Gameplay")]
        [SerializeField] private Aim _aim;

        [Header("Input")]
        [SerializeField] private Camera _playerCamera;
        [SerializeField] private LayerMask _cubeLayer;
        [SerializeField] private float _maxRayDistance = 1000f;

        [Header("Joystick Control")]
        [SerializeField] private bool _useJoystick = false;
        [SerializeField] private RectTransform _cursorUI;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private float _cursorSpeed = 100f;

        private FixedJoystick _paintJoystick;
        private HoldButton _paintButton;
        private PaintStage _stage;

        private IReadonlyTemplateCube _currentHighlighted;
        private bool _isAutoPaint;
        private bool _isColoring;
        private bool _isStarted;
        private Color _paintColor;

        private Vector2 _cursorCurrentPos;

        public event Action<IReadonlyTemplateCube> PositionApplied;

        private void Start()
        {
            _cursorCurrentPos = _cursorUI.anchoredPosition;
        }

        private void Update()
        {
            if (IsPaused || IsInitialized == false || _isStarted == false)
                return;

            if (_useJoystick)
                HandleJoystickInput();
            else
                _isColoring = Input.GetKey(_stage.Paint);

            if (_isAutoPaint || _stage.IsReferenceShowing)
            {
                _aim.SetColor(Color.clear);
                return;
            }

            HandleHoverAndPaint();
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            _aim.Init(pauseHandler);
            _aim.StartAnimaton();

            if (_stage == null)
                return;

            IsInitialized = true;
        }

        public void EnableJoystickControl(bool value)
        {
            _useJoystick = value;
            _cursorUI.gameObject.SetActive(value);

            if (value)
                _cursorUI.anchoredPosition = _cursorCurrentPos;
            else
                _cursorCurrentPos = _cursorUI.anchoredPosition;
        }

        public void SetJoystick(FixedJoystick joystick, HoldButton paintButton)
        {
            _paintJoystick = joystick;
            _paintButton = paintButton;
        }

        public void SetPaintColor(Color color)
        {
            _paintColor = color;
            _aim.SetColor(_paintColor);
        }

        public void SetPaintStage(PaintStage paintStage) => _stage = paintStage;

        public void SetAutoPaint(bool isAutoPaint) => _isAutoPaint = isAutoPaint;

        public void StartStage() => _isStarted = true;

        private void HandleJoystickInput()
        {
            if (_paintJoystick == null)
                return;

            Vector2 move = new Vector2(
                _paintJoystick.Horizontal,
                _paintJoystick.Vertical);

            if (move.sqrMagnitude > 0.0001f)
            {
                _cursorUI.anchoredPosition += move * _cursorSpeed * Time.deltaTime;
                ClampCursor();
            }

            if (_paintButton.IsHeld)
                _isColoring = true;
            else
                _isColoring = false;
        }

        private void ClampCursor()
        {
            RectTransform canvasRect = _canvas.GetComponent<RectTransform>();
            Vector2 pos = _cursorUI.anchoredPosition;

            float maxX = canvasRect.sizeDelta.x / 2f;
            float maxY = canvasRect.sizeDelta.y / 2f;

            pos.x = Mathf.Clamp(pos.x, -maxX, maxX);
            pos.y = Mathf.Clamp(pos.y, -maxY, maxY);

            _cursorUI.anchoredPosition = pos;
        }

        private bool IsHitCube(out IReadonlyTemplateCube cube)
        {
            Vector3 screenPos;

            if (_useJoystick)
                screenPos = _cursorUI.position;
            else
                screenPos = Input.mousePosition;

            Ray ray = _playerCamera.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out RaycastHit hit, _maxRayDistance, _cubeLayer))
            {
                cube = hit.collider.GetComponent<TemplateCube>();

                if (cube.Type == CubeType.In)
                    return true;
            }

            cube = null;
            return false;
        }

        private void HandleHoverAndPaint()
        {
            if (IsHitCube(out IReadonlyTemplateCube cube) == false)
                return;

            if (cube != _currentHighlighted)
                _currentHighlighted = cube;

            if (_currentHighlighted.IsColored)
            {
                _aim.SetColor(Color.red);
                _aim.SetPosition(_currentHighlighted.Position + Vector3.up);
            }
            else
            {
                if (_aim.Color != _paintColor)
                    _aim.SetColor(_paintColor);

                _aim.SetPosition(_currentHighlighted.Position);
            }

            if (IsCanApplyPosition())
                PositionApplied?.Invoke(_currentHighlighted);
        }

        private bool IsCanApplyPosition()
        {
            return IsHitCube(out IReadonlyTemplateCube cube) &&
                   _currentHighlighted != null &&
                   _isColoring;
        }
    }
}