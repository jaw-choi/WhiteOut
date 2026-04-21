using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WhiteOut.Systems;

namespace WhiteOut.UI
{
    [DisallowMultipleComponent]
    public sealed class GameOverPanelController : MonoBehaviour
    {
        [SerializeField] private GameFlowController gameFlowController = null;
        [SerializeField] private GameObject gameOverPanel = null;
        [SerializeField] private Text gameOverText = null;
        [SerializeField] private TMP_Text gameOverTmpText = null;
        [SerializeField] private Button restartButton = null;

        private void Awake()
        {
            if (gameFlowController == null)
            {
                gameFlowController = FindFirstObjectByType<GameFlowController>();
            }
        }

        private void OnEnable()
        {
            if (gameFlowController != null)
            {
                gameFlowController.GameOverStateChanged += HandleGameOverStateChanged;
            }

            if (restartButton != null)
            {
                restartButton.onClick.AddListener(RestartGame);
            }

            SetPanelVisible(gameFlowController != null && gameFlowController.IsGameOver);
        }

        private void OnDisable()
        {
            if (gameFlowController != null)
            {
                gameFlowController.GameOverStateChanged -= HandleGameOverStateChanged;
            }

            if (restartButton != null)
            {
                restartButton.onClick.RemoveListener(RestartGame);
            }
        }

        private void HandleGameOverStateChanged(bool isGameOver)
        {
            SetPanelVisible(isGameOver);
        }

        private void SetPanelVisible(bool isVisible)
        {
            if (gameOverPanel != null)
            {
                gameOverPanel.SetActive(isVisible);
            }

            if (!isVisible || gameFlowController == null)
            {
                return;
            }

            SetGameOverText(UIFormat.GameOver(gameFlowController.SurvivalTime, gameFlowController.BestSurvivalTime));
        }

        private void SetGameOverText(string value)
        {
            if (gameOverTmpText != null)
            {
                gameOverTmpText.text = value;
            }

            if (gameOverText != null)
            {
                gameOverText.text = value;
            }
        }

        private void RestartGame()
        {
            if (gameFlowController != null)
            {
                gameFlowController.RestartGame();
            }
        }
    }
}
