using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BoardButton : MonoBehaviour
{
    public static event Action OnClick;

    [SerializeField] GameManager gameManager;
    public TextMeshProUGUI buttonText;
    public int indexer = 0;

    private void Start()
    {
        GameManager.OnRestart += RestartLogic;
    }

    public void ClickButton()
    {
        if (gameManager.currentTurn == GameManager.CurrentTurn.P1)
        {
            buttonText.text = "X";
        }

        else if (gameManager.currentTurn == GameManager.CurrentTurn.P2)
        {
            buttonText.text = "O";
        }

        OnClick.Invoke();
    }

    public void RestartLogic()
    {
        buttonText.text = "";
    }
}
