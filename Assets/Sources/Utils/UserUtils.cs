using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Sources.Utils
{
    public static class UserUtils
    {
        public const int ImageResolution = 32;
        public const int ColorizerBarCount = 5;
        public const int MinRotateSpeed = 10;
        public const int MaxRotateSpeed = 50;
        public const int MaxRotation = 360;
        public const int DefaultLayer = 0;
        public const int NormalLayer = 7;
        public const int FallingLayer = 8;
        public const int ObstacleLayer = 12;
        public const int PhysicalMissileLayer = 23;
        public const int RewardBySize = 100;
        public const int MatchScore = 500;
        public const int ShowScore = 5000;

        public const string TotalScore = "Total score: ";
        public const string CollectScore = "Collect score: ";
        public const string PaintScore = "Paint score: ";
        public const string Horizontal = nameof(Horizontal);
        public const string Vertical = nameof(Vertical);
        public const string DissolvableObject = nameof(DissolvableObject);
        public const string DissolvableObstacle = nameof(DissolvableObstacle);
        public const string Obstacle = nameof(Obstacle);
        public const string Dissolved = nameof(Dissolved);
        public const string Untagged = nameof(Untagged);
        public const string Player = nameof(Player);
        public const string MoveSpeed = "Move speed ";
        public const string BoostSpeed = "Boost speed ";
        public const string DefenceTime = "Defence time ";
        public const string Damage = "Damage ";
        public const string Upgraded = "Upgraded!!!";
        public const string Loading = "Loading...";
        public const string ShowCounts = "Show counts: ";
        public const string MainMenu = nameof(MainMenu);
        public const string Radar = nameof(Radar);
        public const string RadarPaint = nameof(RadarPaint);
        public const string MixerGroupSound = nameof(MixerGroupSound);
        public const string MixerGroupInterface = nameof(MixerGroupInterface);

        public const float ShowTime = 0.1f;
        public const float CalculateTime = 2f;
        public const float PlayerStartHealth = 100;
        public const float PlayerHealthByGrow = 5;
        public const float One = 1;
        public const float Two = 2;
        public const float Three = 3;
        public const float HalfUnit = 0.5f;
        public const float ThirdOfUnit = 0.33f;
        public const float TimeForShow = 5f;
        public const float MinPercentToComplete = 0.7f;
        public const float ExitTime = 3;
        public const float PlayerDamageMultiplier = 1.5f;
        public const float LoadTime = 5f;

        public const char PlusSign = '+';
        public const char DefaultChar = '\0';

        private const float MinAlfa = 0.1f;

        public static readonly int ColorID = Shader.PropertyToID("_Color");

        public static bool IsBlack(Color color)
        {
            return Mathf.Approximately(color.r, 0)
                && Mathf.Approximately(color.g, 0)
                && Mathf.Approximately(color.b, 0);
        }

        public static bool IsTransparent(Color color) => color.a < MinAlfa;

        public static Vector3 GetRandomRotateDirection()
        {
            float xRotation = Random.Range(0, MaxRotation);
            float yRotation = Random.Range(0, MaxRotation);
            float zRotation = Random.Range(0, MaxRotation);

            return new Vector3(xRotation, yRotation, zRotation);
        }

        public static Color GetRandomColor()
        {
            Color color = new Color(Random.Range(0f, 1000f) / 1000, Random.Range(0f, 1000f)
                / 1000, Random.Range(0f, 1000f) / 1000);
            return color;
        }

        public static Color GetColorByPercentage(float percent)
        {
            Color color;
            float normalizedPosition = 0;

            if (percent < 0.25f)
            {
                normalizedPosition = percent / 0.25f;
                color = Color.Lerp(Color.red, new Color(1f, 0.5f, 0f), normalizedPosition);
            }
            else if (percent < 0.5f)
            {
                normalizedPosition = (percent - 0.25f) / 0.25f;
                color = Color.Lerp(new Color(1f, 0.5f, 0f), Color.yellow, normalizedPosition);
            }
            else if (percent < 0.75f)
            {
                normalizedPosition = (percent - 0.50f) / 0.25f;
                color = Color.Lerp(Color.yellow, Color.green, normalizedPosition);
            }
            else
            {
                normalizedPosition = (percent - 0.75f) / 0.25f;
                color = Color.Lerp(Color.green, new Color(0.7f, 1f, 0.7f), normalizedPosition);
            }

            return color;
        }

        public static void SetActiveElements(bool isActive, List<GameObject> gameObjects = null)
        {
            if (gameObjects == null)
                return;

            foreach (GameObject gameObj in gameObjects)
                gameObj.SetActive(isActive);
        }

        public static string GetSceneName(int index)
        {
            switch (index)
            {
                case 0:
                    return Radar;

                case 1:
                    return RadarPaint;

                default:
                    return string.Empty;
            }
        }
    }
}