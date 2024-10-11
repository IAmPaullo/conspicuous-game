using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Score UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private GameObject finalScorePanel;

    [Header("Animation Settings")]
    [SerializeField] private float scoreChangeDuration = 0.5f;
    [SerializeField] private Color gainColor = Color.green;
    [SerializeField] private Color lossColor = Color.red;
    [SerializeField] private Color defaultColor = Color.white;

    [SerializeField, ReadOnly] private int currentScore = 0;

    private void Start()
    {
        GameStateManager.Instance.ScoreChangeEvent += OnScoreChanged;
        GameStateManager.Instance.StateChangeEvent += OnStateChanged;

        UpdateScoreText();
        finalScorePanel.SetActive(false);
    }

    private void OnScoreChanged(int newScore)
    {
        if (newScore > currentScore)
        {
            ScoreAnimations.AnimateScoreChange(scoreText, currentScore, newScore, gainColor, defaultColor, scoreChangeDuration, UpdateScoreText);
        }
        else
        {
            ScoreAnimations.AnimateScoreChange(scoreText, currentScore, newScore, lossColor, defaultColor, scoreChangeDuration, UpdateScoreText);
        }

        currentScore = newScore;
    }

    private void OnStateChanged(GameStateManager.GameState newState)
    {
        if (newState == GameStateManager.GameState.GameOver)
        {
            EndGame();
        }
    }


    public void EndGame()
    {
        finalScoreText.text = "Final Score: " + currentScore;
        finalScorePanel.SetActive(true);
        ScoreAnimations.AnimateFinalScore(finalScorePanel, scoreChangeDuration);
    }


    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + currentScore.ToString();
    }

    private void OnDestroy()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.ScoreChangeEvent -= OnScoreChanged;
            GameStateManager.Instance.StateChangeEvent -= OnStateChanged;
        }
    }
}
