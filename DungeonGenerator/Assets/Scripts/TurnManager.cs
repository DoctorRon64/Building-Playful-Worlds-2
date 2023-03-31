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

    [SerializeField] private Image PlayerImage;
    [SerializeField] private Image EnemyImage;
    [SerializeField] private Slider PlayerHealthSlider;

    public float OpponentTimeTurn;
    public DungeonData DungeonData;
    private Player player;

    public void Start()
    {
        PlayerImage.enabled = true;
        player = FindObjectOfType<Player>();
        EndOpponentTurn();
    }

	private void Update()
	{
        PlayerHealthSlider.value = player.Health;
	}

	public void PlayerTurn() 
    {
        PlayerImage.enabled = true;
        player.StepsAmount = AmountOfPlayerSteps;
    }

    public void GetIfPlayerWalked()
    {
        if (player.StepsAmount <= 0)
        {
            EndPlayerTurn();
        }
    }

    [ContextMenu("Walk Enemies")]
    public IEnumerator EnemyTurn()
	{
        EnemyImage.enabled = true;
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
        for (int i = 0; i < DungeonData.EnemyList.Count; i++)
        {
            DungeonData.EnemyList[i].GetComponent<Enemy>().PatrolBehaviour();
        }
    }

    public void EndPlayerTurn()
    {
        IsPlayerTurn = false;
        IsOpponentTurn = true;
        PlayerImage.enabled = false;
        StartCoroutine(EnemyTurn());
    }

    public void EndOpponentTurn()
    {
        IsOpponentTurn = false;
        IsPlayerTurn = true;
        EnemyImage.enabled = false;

        for (int i = 0; i < DungeonData.EnemyList.Count; i++)
		{
            DungeonData.EnemyList[i].CheckIfCanHurtPlayer();
		}

        PlayerTurn();
    }
}
