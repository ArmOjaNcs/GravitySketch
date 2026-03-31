using Assets.Sources.Dissolvable;
using Assets.Sources.Pause;
using Assets.Sources.Utils;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Sources.Table
{
    public class Template : MonoBehaviour
    {
        [SerializeField] private TemplateFrameReference _frameReference;
        [SerializeField] private List<TemplateCube> _templateCubes = new List<TemplateCube>();

        private TemplateCube[] _inCubes;
        private int _inCubesCount;

        public IReadOnlyList<IReadonlyTemplateCube> TemplateCubes => _templateCubes;

        public void Init()
        {
            _templateCubes = GetComponentsInChildren<TemplateCube>().ToList();
            _frameReference.InitTemplateCubes(_templateCubes);
            _inCubes = _templateCubes.Where(c => c.Type == CubeType.In).ToArray();
            _inCubesCount = _inCubes.Length;
            Debug.Log("Total IN cubes: " + _inCubesCount);
        }

        public IReadonlyTemplateCube GetCube(int index)
        {
            foreach (IReadonlyTemplateCube cube in _templateCubes)

                if (cube.Index == index)
                {
                    _inCubesCount--;
                    Debug.Log("Total IN cubes: " + _inCubesCount);
                    return cube;
                }

            return null;
        }

        public void DropDown(PauseHandler pauseHandler)
        {
            foreach (TemplateCube templateCube in _templateCubes)
            {
                if(templateCube.Type == CubeType.Border || templateCube.IsMarked)
                {
                    templateCube.AddComponent<DissolvableObstacle>();
                    DissolvableObstacle dissolvableObstacle = templateCube.GetComponent<DissolvableObstacle>();
                    dissolvableObstacle.Init(pauseHandler);
                    dissolvableObstacle.SetDissolveAnimationTime(UserUtils.Unit + UserUtils.HalfOfUnit);
                    dissolvableObstacle.DropDown();
                }
            }
        }
    }
}