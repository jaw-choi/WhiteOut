using UnityEngine;

namespace WhiteOut.Systems
{
    public static class HighScoreService
    {
        public const string BestSurvivalTimeKey = "WhiteOut.BestSurvivalTime";

        public static float LoadBestSurvivalTime()
        {
            return PlayerPrefs.GetFloat(BestSurvivalTimeKey, 0f);
        }

        public static bool TrySaveBestSurvivalTime(float survivalTime)
        {
            var previousBest = LoadBestSurvivalTime();
            if (survivalTime <= previousBest)
            {
                return false;
            }

            PlayerPrefs.SetFloat(BestSurvivalTimeKey, survivalTime);
            PlayerPrefs.Save();
            return true;
        }

        public static void ClearBestSurvivalTime()
        {
            PlayerPrefs.DeleteKey(BestSurvivalTimeKey);
            PlayerPrefs.Save();
        }
    }
}
