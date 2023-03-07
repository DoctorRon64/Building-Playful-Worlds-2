using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCamera : MonoBehaviour
{
    public CinemachineVirtualCamera VirtualCamera;
    public Transform Player;

    public void Awake()
    {
        VirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void GetPlayerCam()
    {
        Player = FindObjectOfType<Player>().transform;
        VirtualCamera.Follow = FindObjectOfType<Player>().transform;
    }
}
