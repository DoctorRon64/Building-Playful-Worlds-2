using System.Collections.Generic;
using UnityEngine;
using static DungeonGenerator;

public class Player : MonoBehaviour
{
	public Transform MovePoint;
	public Animator AnimatorController;
	public float MoveSpeed = 5f;
	public LayerMask WallLayer;
	private TurnManager TurnManager;
    private DungeonGenerator DungeonGenerator;

	private void Awake()
	{
        DungeonGenerator = FindObjectOfType<DungeonGenerator>();
        MovePoint.parent = null;
		AnimatorController = GetComponent<Animator>();
        TurnManager = FindObjectOfType<TurnManager>();
	}

	private void Update()
	{
		PlayerMovement();
	}

	private void PlayerMovement()
	{
        //ga naar movepoint
        transform.position = Vector3.MoveTowards(transform.position, MovePoint.position, MoveSpeed * Time.deltaTime);

        if (TurnManager.IsPlayerTurn == true)
		{
            //als ik niet op movepoint zit
            if (Vector3.Distance(transform.position, MovePoint.position) <= .05f)
            {
                //als ik naar links of rechts beweeg
                if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
                {
                    if (!Physics2D.OverlapCircle(MovePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), .2f, WallLayer))
                    {
                        //zet movepoint positie
                        MovePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    }
                }

                else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
                {
                    if (!Physics2D.OverlapCircle(MovePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), .2f, WallLayer))
                    {
                        MovePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
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