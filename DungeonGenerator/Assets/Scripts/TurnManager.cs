using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public bool IsPlayerTurn;
    public bool IsOpponentTurn;
    public Button EndTurnButton;

    public void Start()
    {
        EndTurnButton.interactable = true;
        IsPlayerTurn = true;
        IsOpponentTurn = false;
    }

    public void EndPlayerTurn()
    {
        if (IsPlayerTurn)
        {
            EndTurnButton.interactable = false;
            IsPlayerTurn = false;
            IsOpponentTurn = true;
        }
    }

    public void EndEnemyTurn()
    {
        if (IsOpponentTurn)
        {
            EndTurnButton.interactable = true;
            IsOpponentTurn = false;
            IsPlayerTurn = true;
        }
    }
}
