using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class UIManager : MonoBehaviour
{
    public static event Action OnUndo;
    public static event Action OnRedo;

    public static UIManager Instance;
    [SerializeField] Canvas endGameCanvas;
    [SerializeField] TextMeshProUGUI endGameCanvasTitle;
    [SerializeField] TextMeshProUGUI player1Score;
    [SerializeField] TextMeshProUGUI player2Score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        
        else if(Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        GameManager.OnPlayerWon += EndGameCanvasSetActive;
        GameManager.OnRestart += EndGameCanvasSetFalse;
        GameManager.OnPlayerWon += ScoreUpdater;
        endGameCanvas.gameObject.SetActive(false);
    }

    public void EndGameCanvasSetActive(string playerNum)
    {
        if (playerNum == "P1") endGameCanvasTitle.text = "Player 1 Wins!";
        else if (playerNum == "P2") endGameCanvasTitle.text = "Player 2 Wins!";
        else if (playerNum == "Tie") endGameCanvasTitle.text = "Draw";

        endGameCanvas.gameObject.SetActive(true);
    }

    public void EndGameCanvasSetFalse()
    {
        endGameCanvas.gameObject.SetActive(false);
    }

    public void ScoreUpdater(string playerNum)
    {
        if (playerNum == "P1") player1Score.text = "Player 1 (X) score : " + ScoreManager.Player1Score;
        if (playerNum == "P2") player2Score.text = "Player 2 (O) score : " + ScoreManager.Player2Score;
    }

    public void UndoButton() => OnUndo.Invoke();

    public void RedoButton() => OnRedo.Invoke();
}
