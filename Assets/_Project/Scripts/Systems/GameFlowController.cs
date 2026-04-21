using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WhiteOut.Systems
{
    [DisallowMultipleComponent]
    public sealed class GameFlowController : MonoBehaviour
    {
        [SerializeField] private SurvivalSystem survivalSystem;

        public float SurvivalTime { get; private set; }
        public float BestSurvivalTime { get; private set; }
        public bool IsGameOver { get; private set; }

        public event Action<float> SurvivalTimeChanged;
        public event Action<float> BestSurvivalTimeChanged;
        public event Action<bool> GameOverStateChanged;
        public event Action GameOver;

        private void Awake()
        {
            if (survivalSystem == null)
            {
                survivalSystem = FindFirstObjectByType<SurvivalSystem>();
            }

            BestSurvivalTime = HighScoreService.LoadBestSurvivalTime();
        }

        private void OnEnable()
        {
            if (survivalSystem != null)
            {
                survivalSystem.Died += HandleSurvivalDied;
            }
        }

        private void OnDisable()
        {
            if (survivalSystem != null)
            {
                survivalSystem.Died -= HandleSurvivalDied;
            }
        }

        private void Start()
        {
            SurvivalTimeChanged?.Invoke(SurvivalTime);
            BestSurvivalTimeChanged?.Invoke(BestSurvivalTime);
            GameOverStateChanged?.Invoke(IsGameOver);
        }

        private void Update()
        {
            if (IsGameOver)
            {
                return;
            }

            SurvivalTime += Time.deltaTime;
            SurvivalTimeChanged?.Invoke(SurvivalTime);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void SetGameOver(bool isGameOver)
        {
            if (IsGameOver == isGameOver)
            {
                return;
            }

            IsGameOver = isGameOver;

            if (IsGameOver)
            {
                SaveBestTimeIfNeeded();
            }

            GameOverStateChanged?.Invoke(IsGameOver);

            if (IsGameOver)
            {
                GameOver?.Invoke();
            }
        }

        private void HandleSurvivalDied()
        {
            SetGameOver(true);
        }

        private void SaveBestTimeIfNeeded()
        {
            if (!HighScoreService.TrySaveBestSurvivalTime(SurvivalTime))
            {
                return;
            }

            BestSurvivalTime = SurvivalTime;
            BestSurvivalTimeChanged?.Invoke(BestSurvivalTime);
        }
    }
}
