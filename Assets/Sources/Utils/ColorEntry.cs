using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Utils
{
    [Serializable]
    public class ColorEntry
    {
        public Renderer Renderer;
        public Color Color;
        public List<Material> Materials;
    }
}