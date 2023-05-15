using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    [SerializeField] private DungeonData dungeonData;
    [SerializeField] GameObject target;
    [SerializeField] GameObject targetObject;
    public float rotationSpeed;

    private void Start()
    {
        target = FindAnyObjectByType<Player>().gameObject;
        targetObject = dungeonData.EndBoss.gameObject;
    }

    private void Update()
    {
        Vector2 direction = targetObject.transform.position - target.transform.position;

        float rotationAngle = Mathf.Atan2(direction.y, direction.x);

        float rotationAngleDegrees = rotationAngle * Mathf.Rad2Deg + 90f;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, rotationAngleDegrees);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
