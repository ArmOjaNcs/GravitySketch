using UnityEngine;

namespace Assets.Sources.ColorizerScripts
{
    public interface IReadonlyColorData
    {
        public Color Color {  get; }
        public int Count { get; }
    }
}