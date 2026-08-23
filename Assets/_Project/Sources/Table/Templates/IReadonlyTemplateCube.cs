using UnityEngine;

namespace Table
{
    public interface IReadonlyTemplateCube
    {
        public bool IsMarked { get; }
        public bool IsColored { get; }
        public CubeType Type { get; }
        public int Index { get; }
        public Color Color { get; }
        public Vector3 Position { get; }
        public void SetColored(Color color);
        public void EnableRendering();
        public void DisableRendering();
        public void Highlight(Color color);
        public void StopHighlight();
        public void Mark();
    }
}