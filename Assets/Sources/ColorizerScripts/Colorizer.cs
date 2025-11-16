using Assets.Sources.Level;
using Assets.Sources.Pause;
using Assets.Sources.Table;
using Assets.Sources.UI;
using Assets.Sources.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Sources.ColorizerScripts
{
    public class Colorizer : PauseableObject
    {
        [SerializeField] private ColoringPositionHandler _positionHandler;
        [SerializeField] private float _autoRate;

        private SinglePressButton _resetButton;
        private PaintStage _stage;
        private Queue<Color> _availableColors = new();
        private bool _isAutoPaint;
        private float _currentTime;
        private bool _useJoystick;
        private bool _isReseting;

        public event Action<IEnumerable<Color>> QueueChanged;
        public event Action<IReadonlyTemplateCube, Color, bool> PaintApplied;
        public event Action Reseted;

        public int ColorsCount => _availableColors.Count;

        private IEnumerable<Color> Colors
        {
            get => _availableColors.Take(_availableColors.Count >= UserUtils.ColorizerBarCount
                                  ? UserUtils.ColorizerBarCount
                                  : _availableColors.Count);
        }

        public int AutoMatchCount { get; private set; }

        private void OnEnable()
        {
            _positionHandler.PositionApplied += Paint;
        }

        private void OnDisable()
        {
            _positionHandler.PositionApplied -= Paint;
        }

        private void Update()
        {
            if (_stage == null || IsPaused || IsInitialized == false)
                return;

            if (_isAutoPaint == false)
            {
                if(_useJoystick && _resetButton != null)
                    _isReseting = _resetButton.IsPressed;
                else
                    _isReseting = Input.GetMouseButtonDown(1);

                if (_isReseting)
                {
                    _availableColors.Dequeue();
                    QueueChanged?.Invoke(Colors);
                    Reseted?.Invoke();
                }
            }

            if (_isAutoPaint && _stage.IsReferenceShowing == false)
            {
                _currentTime += Time.deltaTime;

                if (_currentTime > _autoRate && _availableColors.Count > 0)
                {
                    _currentTime = 0;
                    Color color = _availableColors.Peek();
                    var templateCube = _stage.GetCubeByColor(color);

                    if (templateCube == null)
                    {
                        _availableColors.Dequeue();
                        QueueChanged?.Invoke(Colors);
                    }
                    else
                        Paint(templateCube);
                }
            }
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);
            QueueChanged?.Invoke(Colors);
            IsInitialized = true;
        }

        public void SetStage(PaintStage paintStage, IReadOnlyList<Color> colors)
        {
            _stage = paintStage;
            SetPaintMaterials(colors);
        }

        public void EnableJoystickControl(bool value) => _useJoystick = value;
        public void SetAutoPaint(bool isAutoPaint) => _isAutoPaint = isAutoPaint;
        public void SetResetButton(SinglePressButton resetButton) => _resetButton = resetButton;

        private void Paint(IReadonlyTemplateCube cube)
        {
            if (_availableColors.Count == 0 || cube.Type != CubeType.In || cube.IsMarked)
                return;

            Color paintColor = _availableColors.Dequeue();

            cube.Mark();

            if (_isAutoPaint)
                AutoMatchCount++;

            QueueChanged?.Invoke(Colors);

            PaintApplied?.Invoke(cube, paintColor, _isAutoPaint);
        }

        private void SetPaintMaterials(IEnumerable<Color> colors)
        {
            _availableColors = new Queue<Color>(colors);
        }
    }
}