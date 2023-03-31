using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DungeonGenerator;

public class Player : MonoBehaviour
{
	public Vector2 MovePoint;
	public Animator AnimatorController;
	public float MoveSpeed = 5f;
    public int StepsAmount;

    public int Health = 20;
    public GameObject EnemieHitHud;
    private TurnManager TurnManager;
    private DungeonGenerator DungeonGenerator;
    private InventoryManager Inventory;
    public DungeonData dungeonData;

    private Vector2Int[] WhichSideToMove = new Vector2Int[4];

    private void Awake()
	{
        WhichSideToMove[0] = new Vector2Int(0, 1); //up
        WhichSideToMove[1] = new Vector2Int(0, -1); //down
        WhichSideToMove[2] = new Vector2Int(1, 0); //right
        WhichSideToMove[3] = new Vector2Int(-1, 0); //left

        MovePoint = transform.position;
        EnemieHitHud.SetActive(false);
        DungeonGenerator = FindObjectOfType<DungeonGenerator>();
        TurnManager = FindObjectOfType<TurnManager>();
        Inventory = FindObjectOfType<InventoryManager>();
        AnimatorController = GetComponent<Animator>();
    }

	private void Update()
	{
		PlayerMovement();
	}

    private TileType GetTileTypeWithKey(Vector2Int _Vector2)
    {
        DungeonGenerator.Dungeon.TryGetValue(_Vector2, out TileType tiletip);
        return tiletip;
    }

    private bool isFloorTile(Vector3 _vector3)
    {
        Vector2Int vector2 = new Vector2Int((int)_vector3.x, (int)_vector3.y);
        return GetTileTypeWithKey(vector2) == TileType.Floor || GetTileTypeWithKey(vector2) == TileType.StartFloor || GetTileTypeWithKey(vector2) == TileType.BossFloor;
    }

    private void PlayerMovement()
	{
        //ga naar movepoint
        transform.position = Vector3.MoveTowards(transform.position, MovePoint, MoveSpeed * Time.deltaTime);

        if (StepsAmount > 0 && TurnManager.IsPlayerTurn == true)
		{
            //als ik niet op movepoint zit
            if (Vector3.Distance(transform.position, MovePoint) <= .01f)
            {
                //als ik naar links of rechts beweeg
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    if (isFloorTile(MovePoint + new Vector2(Input.GetAxisRaw("Horizontal"), 0f)) == true) 
                    {
                        //zet movepoint positie
                        MovePoint += new Vector2(Input.GetAxisRaw("Horizontal"), 0f);

                        StepsAmount--;
                        TurnManager.GetIfPlayerWalked();
                    }

                    for (int i = 0; i < dungeonData.EnemyList.Count; i++)
					{
                        if (dungeonData.EnemyList[i].transform.position == (Vector3)MovePoint)
						{
                            CheckIfPlayerSteppedOnEnemy();
                        }
                    }
                    
                }

                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    if (isFloorTile(MovePoint + new Vector2(0f, Input.GetAxisRaw("Vertical"))) == true)
					{
                        MovePoint += new Vector2(0f, Input.GetAxisRaw("Vertical"));

                        StepsAmount--;
                        TurnManager.GetIfPlayerWalked();
                    }

                    for (int i = 0; i < dungeonData.EnemyList.Count; i++)
                    {
                        if (dungeonData.EnemyList[i].transform.position == (Vector3)MovePoint)
                        {
                            CheckIfPlayerSteppedOnEnemy();
                        }
                    }
                }

                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 0f && Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 0f)
                {
                    GetItemWhenWalkedOn();
                }
                AnimatorController.SetBool("Moving", false);
            }
            else
            {
                AnimatorController.SetBool("Moving", true);
            }
        }
        
	}

    public void GetItemWhenWalkedOn()
    {
        for (int i = 0; i < dungeonData.ItemList.Count; i++)
        {
            if (gameObject.transform.position == dungeonData.ItemList[i].transform.position)
            {
                Inventory.PickupItem(dungeonData.ItemList[i]);
            }
        }
    }

    public void CheckIfPlayerSteppedOnEnemy()
	{
        for (int i = 0; i < dungeonData.EnemyList.Count; i++)
		{
            if (gameObject.transform.position == dungeonData.EnemyList[i].transform.position || (Vector3)MovePoint == dungeonData.EnemyList[i].transform.position)
			{
                TakeDamage(dungeonData.EnemyList[i].AttackDamage);
			}
		}
	}

    public void TakeDamage(int _Damage)
    {
        Health -= _Damage;
    }

    public void ApplyHealth(int _Health)
    {
        Health += _Health;
    }

    public void DoDamage(int _Damage)
	{
        for (int i = 0; i < dungeonData.EnemyList.Count; i++)
		{
            for (int j = 0; j < WhichSideToMove.Length; j++)
            {
                if (dungeonData.EnemyList[i].transform.position == transform.position + new Vector3(WhichSideToMove[j].x, WhichSideToMove[j].y, 0f))
                {
                    dungeonData.EnemyList[i].TakeDamage(_Damage);
                }
            }
        }
	}

    public void CheckIfEnemyDies()
    {
        if (Health <= 0)
        {
            SceneManager.LoadScene("lose");
        }
    }
}