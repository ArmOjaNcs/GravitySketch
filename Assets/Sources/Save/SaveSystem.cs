using UnityEngine;

namespace Assets.Sources.Save
{
    public static class SaveSystem
    {
        private const string PlayerProgressKey = "PlayerProgress";
        private const string AudioSettingsKey = "AudioSettings";

        public static void SavePlayerProgress(PlayerProgress progress)
        {
            string json = JsonUtility.ToJson(progress);
            PlayerPrefs.SetString(PlayerProgressKey, json);
            PlayerPrefs.Save();
        }

        public static PlayerProgress LoadPlayerProgress()
        {
            if (PlayerPrefs.HasKey(PlayerProgressKey))
            {
                string json = PlayerPrefs.GetString(PlayerProgressKey);

                return JsonUtility.FromJson<PlayerProgress>(json);
            }

            return new PlayerProgress();
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