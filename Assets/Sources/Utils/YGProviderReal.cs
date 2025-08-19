using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Assets.Sources.Utils
{
    public class YGProviderReal : IYGProvider
    {
        public void Init()
        {
            //YG.Instance.Init();
        }

        public string GetLanguage()
        {
            return string.Empty;//YG.Instance.GetLanguage();
        }
    }
}