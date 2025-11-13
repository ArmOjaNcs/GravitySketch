using UnityEngine;

namespace Assets.Sources.Save
{
    public static class SaveSystem
    {
        private const string PlayerProgressKey = "PlayerProgress";
        private const string InputBindingsKey = "InputBindings";
        private const string AudioSettingsKey = "AudioSettings";

        public static void SavePlayerProgress(PlayerProgress progress)
        {
            string json = JsonUtility.ToJson(progress);
            PlayerPrefs.SetString(PlayerProgressKey, json);
            PlayerPrefs.Save();
        }

        public static PlayerProgress LoadPlayerProgress()
        {
            //PlayerPrefs.DeleteAll();

            if (PlayerPrefs.HasKey(PlayerProgressKey))
            {
                string json = PlayerPrefs.GetString(PlayerProgressKey);

                return JsonUtility.FromJson<PlayerProgress>(json);
            }

            return new PlayerProgress();
        }

        public static void SaveInputBindings(InputBindings bindings)
        {
            string json = JsonUtility.ToJson(bindings);
            PlayerPrefs.SetString(InputBindingsKey, json);
            PlayerPrefs.Save();
        }

        public static InputBindings LoadInputBindings()
        {
            if (PlayerPrefs.HasKey(InputBindingsKey))
            {
                string json = PlayerPrefs.GetString(InputBindingsKey);
                return JsonUtility.FromJson<InputBindings>(json);
            }

            return new InputBindings(); 
        }

        public static void SaveAudioSettings(AudioSettings audioSettings)
        {
            string json = JsonUtility.ToJson(audioSettings);
            PlayerPrefs.SetString(AudioSettingsKey, json);
            PlayerPrefs.Save();
        }

        public static AudioSettings LoadAudioSettings()
        {
            if (PlayerPrefs.HasKey(AudioSettingsKey))
            {
                string json = PlayerPrefs.GetString(AudioSettingsKey);

                return JsonUtility.FromJson<AudioSettings>(json);
            }

            return new AudioSettings();
        }
    }
}