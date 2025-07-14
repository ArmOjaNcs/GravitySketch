using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Sources.Utils;
using Assets.Sources.Level;
using Assets.Sources.Table;
using Assets.Sources.Pause;

namespace Assets.Sources.ColorizerScripts
{
    public class Colorizer : PauseableObject
    {
        [SerializeField] private ColoringPositionHandler _positionHandler;
        [SerializeField] private float _autoRate;

        private PaintStage _stage;
        private Queue<Color> _availableColors = new();
        private bool _isAutoPaint;
        private float _currentTime;

        public event Action<IEnumerable<Color>> QueueChanged;
        public event Action<IReadonlyTemplateCube, Color, bool> PaintApplied;
        public event Action Reseted;

        private bool IsReseting => Input.GetMouseButtonDown(1);

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

        private void Start()
        {
            QueueChanged?.Invoke(Colors);
        }

        private void Update()
        {
            if (_stage == null || IsPaused)
                return;

            if (_isAutoPaint == false)
            {
                if (IsReseting)
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

        public void Init(PaintStage paintStage, IReadOnlyList<Color> colors)
        {
            _stage = paintStage;
            SetPaintMaterials(colors);
        }

        public void SetAutoPaint(bool isAutoPaint) => _isAutoPaint = isAutoPaint;

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