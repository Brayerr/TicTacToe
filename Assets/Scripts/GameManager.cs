using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnRestart;
    public static event Action<string> OnPlayerWon;
    public static event Action<int> OnUndoCommand;
    public static event Action<int, string> OnRedoCommand;


    public static GameManager Instance;

    [SerializeField] BoardButton[] buttons = new BoardButton[9];
    [SerializeField] string[] board = new string[9];
    int indexHolder;
    string valueHolder;


    public int turnCounter = 9;
    public enum CurrentTurn
    {
        P1,
        P2,
    }
    public CurrentTurn currentTurn = CurrentTurn.P1;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        BoardButton.OnClick += ChangeCurrentTurnToNext;
        OnRestart += SetP1Turn;
        CommandsManager.OnUndo += ChangeCurrentTurnUndo;
        CommandsManager.OnRedo += ChangeCurrentTurnRedo;
    }



    public void ChangeCurrentTurnToNext()
    {
        turnCounter--;
        UpdateStrings();
        OnUndoCommand.Invoke(indexHolder);
        OnRedoCommand.Invoke(indexHolder, valueHolder);
        WinCondition();
        if (currentTurn == CurrentTurn.P1) currentTurn = CurrentTurn.P2;
        else if (currentTurn == CurrentTurn.P2) currentTurn = CurrentTurn.P1;
    }

    public void ChangeCurrentTurnUndo(int index)
    {
        turnCounter++;
        UndoLogic(index);
        OnRedoCommand.Invoke(index, valueHolder);
        if (currentTurn == CurrentTurn.P1) currentTurn = CurrentTurn.P2;
        else if (currentTurn == CurrentTurn.P2) currentTurn = CurrentTurn.P1;
    }

    public void ChangeCurrentTurnRedo(int index, string value)
    {
        turnCounter--;
        RedoLogic(index, value);
        OnUndoCommand.Invoke(index);
        if (currentTurn == CurrentTurn.P1) currentTurn = CurrentTurn.P2;
        else if (currentTurn == CurrentTurn.P2) currentTurn = CurrentTurn.P1;
    }

    public void RestartButton() => OnRestart.Invoke();

    public void SetP1Turn()
    {
        turnCounter = 9;
        currentTurn = CurrentTurn.P1;
    } 

    public bool WinCondition()
    {
        if (board[0] == "X" && board[1] == "X" && board[2] == "X"
            || board[3] == "X" && board[4] == "X" && board[5] == "X"
            || board[6] == "X" && board[7] == "X" && board[8] == "X"
            || board[0] == "X" && board[4] == "X" && board[8] == "X"
            || board[2] == "X" && board[4] == "X" && board[6] == "X"
            || board[1] == "X" && board[4] == "X" && board[7] == "X"
            || board[0] == "X" && board[3] == "X" && board[6] == "X"
            || board[2] == "X" && board[5] == "X" && board[8] == "X")
        {
            OnPlayerWon.Invoke("P1");
            return true;
        }

        else if (board[0] == "O" && board[1] == "O" && board[2] == "O"
            || board[3] == "O" && board[4] == "O" && board[5] == "O"
            || board[6] == "O" && board[7] == "O" && board[8] == "O"
            || board[0] == "O" && board[4] == "O" && board[8] == "O"
            || board[2] == "O" && board[4] == "O" && board[6] == "O"
            || board[1] == "O" && board[4] == "O" && board[7] == "O"
            || board[0] == "O" && board[3] == "O" && board[6] == "O"
            || board[2] == "O" && board[5] == "O" && board[8] == "O")
        {
            OnPlayerWon.Invoke("P2");
            return true;
        }

        else
        {
            TieCondition();
            return false;
        }


    }

    public void TieCondition()
    {
        if (turnCounter == 0)
        {
            OnPlayerWon.Invoke("Tie");
        }
    }

    public void UpdateStrings()
    {
        for (int i = 0; i < board.Length; i++)
        {
            if(board[i] != buttons[i].buttonText.text)
            {
                indexHolder = i;
                valueHolder = buttons[i].buttonText.text;
                board[i] = buttons[i].buttonText.text;
            }
        }
    }

    public void UndoLogic(int index)
    {
        if (board[index] != "")
        {
            valueHolder = board[index];
            board[index] = "";
            buttons[index].buttonText.text = "";
        }
    }

    public void RedoLogic(int index, string value)
    {
        board[index] = value;
        buttons[index].buttonText.text = value;
    }
}
