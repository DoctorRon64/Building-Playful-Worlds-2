using System.Collections.Generic;
using UnityEngine;
using static DungeonGenerator;

public class Player : MonoBehaviour
{
	public Transform MovePoint;
	public Animator AnimatorController;
	public float MoveSpeed = 5f;
    public int StepsAmount;
	public LayerMask WallLayer;
	private TurnManager TurnManager;
    private DungeonGenerator DungeonGenerator;
    private Vector2Int[] WhichSideToMove = new Vector2Int[4];

    private void Awake()
	{
        WhichSideToMove[0] = new Vector2Int(0, 1); //up
        WhichSideToMove[1] = new Vector2Int(0, -1); //down
        WhichSideToMove[2] = new Vector2Int(1, 0); //right
        WhichSideToMove[3] = new Vector2Int(-1, 0); //left

        DungeonGenerator = FindObjectOfType<DungeonGenerator>();
        MovePoint.parent = DungeonGenerator.transform;
		AnimatorController = GetComponent<Animator>();
        TurnManager = FindObjectOfType<TurnManager>();
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
        transform.position = Vector3.MoveTowards(transform.position, MovePoint.position, MoveSpeed * Time.deltaTime);

        if (StepsAmount > 0 && TurnManager.IsPlayerTurn == true)
		{
            //als ik niet op movepoint zit
            if (Vector3.Distance(transform.position, MovePoint.position) <= .01f)
            {
                //als ik naar links of rechts beweeg
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    if (isFloorTile(MovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f)) == true) 
                    {
                        //zet movepoint positie
                        MovePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                        StepsAmount--;
                    }
                }

                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    if (isFloorTile(MovePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f)) == true)
					{
                        MovePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                        StepsAmount--;
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