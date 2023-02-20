using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public static int Player1Score { get; protected set; } = 0;
    public static int Player2Score { get; protected set; } = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }

        else if (Instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        GameManager.OnPlayerWon += IncreaseScore;
    }

    public static void IncreaseScore(string playerNum)
    {
        if (playerNum == "P1")
        {
            Player1Score++;
        }
        else if (playerNum == "P2")
        {
            Player2Score++;
        }
    }
}
