using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{

    public static GameStateManager Instance;
    public enum GameState
    {
        Waiting,
        Playing,
        GameOver
    }

    private GameState currentState;
    private int score = 0;
    private int matchPoints = 10;
    private int mismatchPenalty = 5;

    [SerializeField] private CardController cardController;

    public delegate void OnStateChange(GameState newState);
    public event OnStateChange StateChangeEvent;

    public delegate void OnScoreChange(int newScore);
    public event OnScoreChange ScoreChangeEvent;


    private void Awake()
    {
        if (Instance != null)
        {
            throw new Exception("Only one instance allowed!");
        }
        Instance = this;
    }
    private void Start()
    {

        cardController.MatchFoundEvent += OnMatchFound;
        cardController.MatchFailEvent += OnMatchFail;
    }

    public GameState GetCurrentState()
    {
        return currentState;
    }

    public void SetGameState(GameState newState)
    {
        currentState = newState;
        StateChangeEvent?.Invoke(newState);
    }

    public bool CanSelectCard()
    {
        return currentState == GameState.Playing;
    }

    private void OnMatchFound(Card card1, Card card2)
    {
        AddScore(matchPoints);
        StartCoroutine(WaitAndContinue(1f));
    }

    private void OnMatchFail(Card card1, Card card2)
    {
        SubtractScore(mismatchPenalty);
        StartCoroutine(WaitAndContinue(1f));
    }

    private IEnumerator WaitAndContinue(float waitTime)
    {
        SetGameState(GameState.Waiting);
        yield return new WaitForSeconds(waitTime);
        SetGameState(GameState.Playing);
    }

    public void AddScore(int points)
    {
        score += points;
        ScoreChangeEvent?.Invoke(score);
    }

    public void SubtractScore(int penalty)
    {
        score -= penalty;
        if (score < 0) score = 0;
        ScoreChangeEvent?.Invoke(score);
    }

    public int GetScore()
    {
        return score;
    }

    public void EndGame()
    {
        SetGameState(GameState.GameOver);
        Debug.Log("Game Over! Score: " + score);
    }

    public void ResetGame()
    {
        score = 0;
        cardController.ResetGame();
        SetGameState(GameState.Playing);
        ScoreChangeEvent?.Invoke(score);
    }
}
