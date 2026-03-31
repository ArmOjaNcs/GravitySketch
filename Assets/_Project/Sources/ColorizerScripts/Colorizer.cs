using Assets._Project._Sources.UI;
using Assets.Sources.Level;
using Assets.Sources.Pause;
using Assets.Sources.Table;
using Assets.Sources.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Sources.ColorizerScripts
{
    public class Colorizer : PauseableObject
    {
        [SerializeField] private ColorData _colorDataPrefab;
        [SerializeField] private VerticalScrollContent _scrollContent;
        [SerializeField] private RectTransform _contentRect;
        [SerializeField] private ColoringPositionHandler _positionHandler;
        [SerializeField] private float _autoRate;

        private SinglePressButton _resetButton;
        private PaintStage _stage;
        private List<ColorData> _availableColors = new();
        private ColorData _currentColorData;
        private bool _isAutoPaint;
        private float _currentTime;
        private bool _useJoystick;
        private bool _isReseting;

        public event Action<Color, int> ColorsCountChanged;
        public event Action<IReadonlyTemplateCube, Color, bool> PaintApplied;
        public event Action Reseted;

        public int ColorsCount => _availableColors.Sum(c => c.Count);

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
            if (_availableColors.Count <= 0)
                return;

            if (_stage == null || IsPaused || IsInitialized == false)
                return;

            if (_isAutoPaint == false && _currentColorData != null)
            {
                if(_useJoystick && _resetButton != null)
                    _isReseting = _resetButton.IsPressed;
                else
                    _isReseting = Input.GetKeyDown(_stage.ResetCube);

                if (_isReseting && _currentColorData.Count > 0)
                {
                    _currentColorData.ReduceCount();

                    if (_currentColorData.Count > 0)
                        ColorsCountChanged?.Invoke(_currentColorData.Color, _currentColorData.Count);
                    else
                        OnColorDataZeroCount(_currentColorData);

                    Reseted?.Invoke();
                }
            }

            if (_isAutoPaint && _stage.IsReferenceShowing == false)
            {
                _currentTime += Time.deltaTime;

                if (_currentTime > _autoRate)
                {
                    _currentTime = 0;

                    if (_currentColorData == null)
                    {
                        if(TrySetRandomColorData() == false)
                            return;
                    }
                   
                    Color color = _currentColorData.Color;
                    var templateCube = _stage.GetCubeByColor(color);

                    if (templateCube == null)
                        OnColorDataZeroCount(_currentColorData);
                    else
                        Paint(templateCube);
                }
            }
        }

        public override void Init(PauseHandler pauseHandler)
        {
            base.Init(pauseHandler);

            ColorsCountChanged?.Invoke(Color.clear, 0);
            _positionHandler.SetPaintColor(Color.clear);
            IsInitialized = true;
        }

        public override void Pause()
        {
            base.Pause();
            DeselectAllColorData();
        }

        public override void Resume()
        {
            base.Resume();

            if (_isAutoPaint)
                return;

            SetInteractableAllColorData();

            if (_currentColorData != null && _currentColorData.Count > 0)
                _currentColorData.Select();
        }

        public void SetStage(PaintStage paintStage, IReadOnlyList<Color> colors)
        {
            _stage = paintStage;
            SetPaintMaterials(colors);
        }

        public void EnableJoystickControl(bool value) => _useJoystick = value;

        public void SetAutoPaint(bool isAutoPaint)
        {
            if (isAutoPaint)
            {
                DeselectAllColorData();
            }
            else
            {
                SetInteractableAllColorData();

                if (_currentColorData != null && _currentColorData.Count > 0)
                    _currentColorData.Select();
            }

            _isAutoPaint = isAutoPaint;
        }

        public void SetResetButton(SinglePressButton resetButton) => _resetButton = resetButton;

        private void Paint(IReadonlyTemplateCube cube)
        {
            if (_currentColorData == null || _currentColorData.Count <= 0)
                return;


            if (cube.Type != CubeType.In || cube.IsMarked)
                return;

            if (_isAutoPaint)
                AutoMatchCount++;

            _currentColorData.ReduceCount();
            ColorsCountChanged?.Invoke(_currentColorData.Color, _currentColorData.Count);
            cube.Mark();
            PaintApplied?.Invoke(cube, _currentColorData.Color, _isAutoPaint);

            if (_currentColorData.Count <= 0)
                OnColorDataZeroCount(_currentColorData);
        }

        private void SetPaintMaterials(IEnumerable<Color> colors)
        {
            foreach(var color in colors)
            {
                if(IsHasColor(color) == false)
                {
                    int count = colors.Count(c => c == color);
                    ColorData colorData = Instantiate(_colorDataPrefab, _contentRect);
                    colorData.Init(color, count);
                    _availableColors.Add(colorData);
                    colorData.Selected += OnColorDataSelected;
                }
            }

            _scrollContent.Rebuild();
        }

        private bool IsHasColor(Color color)
        {
            foreach(var colorUI in _availableColors)
            {
                if (colorUI.Color == color)
                    return true;
            }

            return false;
        }

        private void OnColorDataSelected(ColorData colorData)
        {
            if (_currentColorData != null && _currentColorData != colorData)
                _currentColorData.Deselect();

            _currentColorData = colorData;
            ColorsCountChanged?.Invoke(_currentColorData.Color, _currentColorData.Count);
            _positionHandler.SetPaintColor(_currentColorData.Color);
        }

        private void OnColorDataZeroCount(ColorData colorData)
        {
            if (colorData == null)
                return;

            _availableColors.Remove(colorData);
            colorData.Selected -= OnColorDataSelected;
            colorData.SwitchButtonInteraction(false);
            colorData.Deselect();
            _currentColorData = null;
            _positionHandler.SetPaintColor(Color.clear);
            ColorsCountChanged?.Invoke(Color.clear, 0);
        }

        private bool TrySetRandomColorData()
        {
            if(_availableColors.Count <= 0) 
                return false;

            int randomIndex = _availableColors.Count == 1 ? 0 : Random.Range(0, _availableColors.Count);

             Random.Range(0, _availableColors.Count);
            _currentColorData = _availableColors[randomIndex];

            if (_currentColorData.Count == 0)
            {
                OnColorDataZeroCount(_currentColorData);
                return false;
            }

            ColorsCountChanged?.Invoke(_currentColorData.Color, _currentColorData.Count);
            return true;
        }

        private void DeselectAllColorData()
        {
            foreach(ColorData colorData in _availableColors)
            {
                colorData.Deselect();
                colorData.SwitchButtonInteraction(false);
            }
        }

        private void SetInteractableAllColorData()
        {
            foreach (ColorData colorData in _availableColors)
                colorData.SwitchButtonInteraction(true);
        }
    }
}