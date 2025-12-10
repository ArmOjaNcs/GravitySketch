using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Assets.Sources.Utils
{
    public static class Translator
    {
        [SerializeField] private static string _currentLang = "en";

        private static readonly Dictionary<string, Dictionary<string, string>> _dictionary =
            new Dictionary<string, Dictionary<string, string>>()
        {
            { "Start", new Dictionary<string, string>()
                {
                    { "ru", "Начать" },
                    { "tr", "Başla" },
                    { "en", "Start" }
                }
            },
            { "Continue", new Dictionary<string, string>()
                {
                    { "ru", "Продолжить" },
                    { "tr", "Devam Et" },
                    { "en", "Continue" }
                }
            },
            { "Destroyer", new Dictionary<string, string>()
                {
                    { "ru", "Разрушитель" },
                    { "tr", "Yok edici" },
                    { "en", "Destroyer" }
                }
            },
            { "Stormtrooper", new Dictionary<string, string>()
                {
                    { "ru", "Штурмовик" },
                    { "tr", "Stormtrooper'ın" },
                    { "en", "Stormtrooper" }
                }
            },
            { "Sniper", new Dictionary<string, string>()
                {
                    { "ru", "Снайпер" },
                    { "tr", "Keskin nişancı" },
                    { "en", "Sniper" }
                }
            },
            { "Bomber", new Dictionary<string, string>()
                {
                    { "ru", "Бомбардировщик" },
                    { "tr", "Bombardıman uçağı" },
                    { "en", "Bomber" }
                }
            },
             { "Boss", new Dictionary<string, string>()
                {
                    { "ru", "Босс" },
                    { "tr", "Patron" },
                    { "en", "Boss" }
                }
            },
            { "Level", new Dictionary<string, string>()
                {
                    { "ru", "Уровень" },
                    { "tr", "Seviye" },
                    { "en", "Level" }
                }
            },
            { "Total score", new Dictionary<string, string>()
                {
                    { "ru", "Общий счет" },
                    { "tr", "Toplam puan" },
                    { "en", "Total score" }
                }
            },
            { "Loading...", new Dictionary<string, string>()
                {
                    { "ru", "Загрузка..." },
                    { "tr", "Yükleniyor..." },
                    { "en", "Loading..." }
                }
            },
             { "Great!!!", new Dictionary<string, string>()
                {
                    { "ru", "Отлично!!!" },
                    { "tr", "Harika!!!" },
                    { "en", "Great!!!" }
                }
            },
             { "Game over", new Dictionary<string, string>()
                {
                    { "ru", "Конец игры" },
                    { "tr", "Oyun bitti" },
                    { "en", "Game over" }
                }
            }
        };

        public static string Get(string key)
        {
            if (_dictionary.TryGetValue(key, out var langs))
            {
                if (langs.ContainsKey(_currentLang))
                    return langs[_currentLang];
                else if (langs.ContainsKey("en"))
                    return langs["en"];
            }

            return key;
        }

        public static void UpdateLang()
        {
            if (YandexGame.EnvironmentData == null || string.IsNullOrEmpty(YandexGame.lang))
                _currentLang = "en";
            else
                _currentLang = YandexGame.lang;
        }
    }
}