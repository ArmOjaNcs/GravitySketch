using UnityEngine;

namespace Assets.Sources.Save
{
    public static class SaveSystem
    {
        private const string SaveKey = "PlayerProgress";

        public static void Save(PlayerProgress progress)
        {
            string json = JsonUtility.ToJson(progress);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        public static PlayerProgress Load()
        {
            if (PlayerPrefs.HasKey(SaveKey))
            {
                string json = PlayerPrefs.GetString(SaveKey);

                return JsonUtility.FromJson<PlayerProgress>(json);
            }

            return new PlayerProgress();
        }
    }
}