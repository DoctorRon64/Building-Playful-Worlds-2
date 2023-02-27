using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private void Start()
    {
        GetComponent<CinemachineVirtualCamera>().Follow = FindObjectOfType<Player>().transform;
    }
}
