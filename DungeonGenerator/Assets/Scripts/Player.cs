using System.Collections.Generic;
using UnityEngine;
using static DungeonGenerator;

public class Player : MonoBehaviour
{
	public Vector2 MovePoint;
	public Animator AnimatorController;
	public float MoveSpeed = 5f;
    public int StepsAmount;
	public LayerMask WallLayer;

	private TurnManager TurnManager;
    private DungeonGenerator DungeonGenerator;
    private Inventory Inventory;

    private Vector2Int[] WhichSideToMove = new Vector2Int[4];

    private void Awake()
	{
        WhichSideToMove[0] = new Vector2Int(0, 1); //up
        WhichSideToMove[1] = new Vector2Int(0, -1); //down
        WhichSideToMove[2] = new Vector2Int(1, 0); //right
        WhichSideToMove[3] = new Vector2Int(-1, 0); //left

        MovePoint = transform.position;
        Inventory = GetComponent<Inventory>();
        DungeonGenerator = FindObjectOfType<DungeonGenerator>();
        TurnManager = FindObjectOfType<TurnManager>();
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
        return GetTileTypeWithKey(vector2) == TileType.Floor || GetTileTypeWithKey(vector2) == TileType.StartFloor;
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
                        Inventory.IfPlayerOnItem();

                        StepsAmount--;
                        TurnManager.GetIfPlayerWalked();
                        
                    }
                }

                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    if (isFloorTile(MovePoint + new Vector2(0f, Input.GetAxisRaw("Vertical"))) == true)
					{
                        MovePoint += new Vector2(0f, Input.GetAxisRaw("Vertical"));
                        Inventory.IfPlayerOnItem();

                        StepsAmount--;
                        TurnManager.GetIfPlayerWalked();
                    }
                }
                AnimatorController.SetBool("Moving", false);
            }
            else
            {
                AnimatorController.SetBool("Moving", true);
            }
        }
        
	}
}