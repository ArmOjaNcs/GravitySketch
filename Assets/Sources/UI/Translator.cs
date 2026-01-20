using System;
using System.Collections.Generic;
using UnityEngine;
using YG;

namespace Assets.Sources.Utils
{
    public static class Translator
    {
        [SerializeField] private static string _currentLang = "en";

        public static event Action<string> LangChanged;

        public static string CurrentLang => _currentLang;

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

        private static readonly Dictionary<string, Dictionary<string, string>> _keyDictionary =
        new Dictionary<string, Dictionary<string, string>>()
        {
            // ===== Letters =====
            { "A", new() { { "en","A" },{ "ru","Ф" },{ "tr","A" } } },
            { "B", new() { { "en","B" },{ "ru","И" },{ "tr","B" } } },
            { "C", new() { { "en","C" },{ "ru","С" },{ "tr","C" } } },
            { "D", new() { { "en","D" },{ "ru","В" },{ "tr","D" } } },
            { "E", new() { { "en","E" },{ "ru","У" },{ "tr","E" } } },
            { "F", new() { { "en","F" },{ "ru","А" },{ "tr","F" } } },
            { "G", new() { { "en","G" },{ "ru","П" },{ "tr","G" } } },
            { "H", new() { { "en","H" },{ "ru","Р" },{ "tr","H" } } },
            { "I", new() { { "en","I" },{ "ru","Ш" },{ "tr","İ" } } },
            { "J", new() { { "en","J" },{ "ru","О" },{ "tr","J" } } },
            { "K", new() { { "en","K" },{ "ru","Л" },{ "tr","K" } } },
            { "L", new() { { "en","L" },{ "ru","Д" },{ "tr","L" } } },
            { "M", new() { { "en","M" },{ "ru","Ь" },{ "tr","M" } } },
            { "N", new() { { "en","N" },{ "ru","Т" },{ "tr","N" } } },
            { "O", new() { { "en","O" },{ "ru","Щ" },{ "tr","O" } } },
            { "P", new() { { "en","P" },{ "ru","З" },{ "tr","P" } } },
            { "Q", new() { { "en","Q" },{ "ru","Й" },{ "tr","Q" } } },
            { "R", new() { { "en","R" },{ "ru","К" },{ "tr","R" } } },
            { "S", new() { { "en","S" },{ "ru","Ы" },{ "tr","S" } } },
            { "T", new() { { "en","T" },{ "ru","Е" },{ "tr","T" } } },
            { "U", new() { { "en","U" },{ "ru","Г" },{ "tr","U" } } },
            { "V", new() { { "en","V" },{ "ru","М" },{ "tr","V" } } },
            { "W", new() { { "en","W" },{ "ru","Ц" },{ "tr","W" } } },
            { "X", new() { { "en","X" },{ "ru","Ч" },{ "tr","X" } } },
            { "Y", new() { { "en","Y" },{ "ru","Н" },{ "tr","Y" } } },
            { "Z", new() { { "en","Z" },{ "ru","Я" },{ "tr","Z" } } },

            // ===== Digits =====
            { "Alpha0", new() { { "en","0" },{ "ru","0" },{ "tr","0" } } },
            { "Alpha1", new() { { "en","1" },{ "ru","1" },{ "tr","1" } } },
            { "Alpha2", new() { { "en","2" },{ "ru","2" },{ "tr","2" } } },
            { "Alpha3", new() { { "en","3" },{ "ru","3" },{ "tr","3" } } },
            { "Alpha4", new() { { "en","4" },{ "ru","4" },{ "tr","4" } } },
            { "Alpha5", new() { { "en","5" },{ "ru","5" },{ "tr","5" } } },
            { "Alpha6", new() { { "en","6" },{ "ru","6" },{ "tr","6" } } },
            { "Alpha7", new() { { "en","7" },{ "ru","7" },{ "tr","7" } } },
            { "Alpha8", new() { { "en","8" },{ "ru","8" },{ "tr","8" } } },
            { "Alpha9", new() { { "en","9" },{ "ru","9" },{ "tr","9" } } },

            // ===== Function keys =====
            { "F1", new(){{"en","F1"},{"ru","F1"},{"tr","F1"}} },
            { "F2", new(){{"en","F2"},{"ru","F2"},{"tr","F2"}} },
            { "F3", new(){{"en","F3"},{"ru","F3"},{"tr","F3"}} },
            { "F4", new(){{"en","F4"},{"ru","F4"},{"tr","F4"}} },
            { "F5", new(){{"en","F5"},{"ru","F5"},{"tr","F5"}} },
            { "F6", new(){{"en","F6"},{"ru","F6"},{"tr","F6"}} },
            { "F7", new(){{"en","F7"},{"ru","F7"},{"tr","F7"}} },
            { "F8", new(){{"en","F8"},{"ru","F8"},{"tr","F8"}} },
            { "F9", new(){{"en","F9"},{"ru","F9"},{"tr","F9"}} },
            { "F10", new(){{"en","F10"},{"ru","F10"},{"tr","F10"}} },
            { "F11", new(){{"en","F11"},{"ru","F11"},{"tr","F11"}} },
            { "F12", new(){{"en","F12"},{"ru","F12"},{"tr","F12"}} },

            // ===== Navigation =====
            { "Space", new(){{"en","Space"},{"ru","Пробел"},{"tr","Boşluk"}} },
            { "Tab", new(){{"en","Tab"},{"ru","Tab"},{"tr","Tab"}} },
            { "Return", new(){{"en","Enter"},{"ru","Enter"},{"tr","Enter"}} },
            { "Escape", new(){{"en","Esc"},{"ru","Esc"},{"tr","Esc"}} },

            // ===== Arrows =====
            { "UpArrow", new(){{"en","↑"},{"ru","↑"},{"tr","↑"}} },
            { "DownArrow", new(){{"en","↓"},{"ru","↓"},{"tr","↓"}} },
            { "LeftArrow", new(){{"en","←"},{"ru","←"},{"tr","←"}} },
            { "RightArrow", new(){{"en","→"},{"ru","→"},{"tr","→"}} },

            // ===== Modifiers =====
            { "LeftShift", new(){{"en","L Shift"},{"ru","Левый Shift"},{"tr","Sol Shift"}} },
            { "RightShift", new(){{"en","R Shift"},{"ru","Правый Shift"},{"tr","Sağ Shift"}} },
            { "LeftControl", new(){{"en","L Ctrl"},{"ru","Левый Ctrl"},{"tr","Sol Ctrl"}} },
            { "RightControl", new(){{"en","R Ctrl"},{"ru","Правый Ctrl"},{"tr","Sağ Ctrl"}} },
            { "LeftAlt", new(){{"en","L Alt"},{"ru","Левый Alt"},{"tr","Sol Alt"}} },
            { "RightAlt", new(){{"en","R Alt"},{"ru","Правый Alt"},{"tr","Sağ Alt"}} },

            // ===== Mouse =====
            { "Mouse0", new(){{"en","LMB"},{"ru","ЛКМ"},{"tr","Sol Tık"}} },
            { "Mouse1", new(){{"en","RMB"},{"ru","ПКМ"},{"tr","Sağ Tık"}} },
            { "Mouse2", new(){{"en","MMB"},{"ru","СКМ"},{"tr","Orta Tık"}} },
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

        public static string GetKey(KeyCode key)
        {
            string keyName = key.ToString();

            if (_keyDictionary.TryGetValue(keyName, out var langs))
            {
                if (langs.TryGetValue(_currentLang, out var value))
                    return value;

                if (langs.TryGetValue("en", out var fallback))
                    return fallback;
            }

            return keyName;
        }

        public static void UpdateLang()
        {
            if (YandexGame.EnvironmentData == null || string.IsNullOrEmpty(YandexGame.lang))
                _currentLang = "en";
            else
                _currentLang = YandexGame.lang;

            LangChanged?.Invoke(_currentLang);
        }
    }
}