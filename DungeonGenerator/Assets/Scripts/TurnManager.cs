using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{
    public bool IsPlayerTurn;
    public bool IsOpponentTurn;
    public int AmountOfPlayerSteps;
    public int AmountOfOpponentSteps;
    public float OpponentTimeTurn;
	public Button EndTurnButton;
    public DungeonGenerator DungeonGenerator;
	private List<GameObject> Enemies = new List<GameObject>();

    public void Start()
    {
        for (int i = 0 ; i < DungeonGenerator.EnemyList.Count; i++)
		{
            Enemies.Add(DungeonGenerator.EnemyList[i]);
        }

        EndOpponentTurn();
    }

    public void PlayerTurn() 
    {
        FindObjectOfType<Player>().StepsAmount = AmountOfPlayerSteps;
    }

    [ContextMenu("Walk Enemies")]
    public IEnumerator EnemyTurn()
	{
        for (int i = 0; i < AmountOfOpponentSteps; i++)
        {
            MoveEnemy();
            yield return new WaitForSeconds(OpponentTimeTurn);
        }

        EndOpponentTurn();

        yield return new WaitForSeconds(0);
    }

    public void MoveEnemy()
	{
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].GetComponent<Enemy>().PatrolBehaviour();
        }
    }

    public void EndPlayerTurn()
    {
        EndTurnButton.interactable = false;
        IsPlayerTurn = false;
        IsOpponentTurn = true;
        StartCoroutine(EnemyTurn());
    }

    public void EndOpponentTurn()
    {
        EndTurnButton.interactable = true;
        IsOpponentTurn = false;
        IsPlayerTurn = true;
        PlayerTurn();
    }
}
