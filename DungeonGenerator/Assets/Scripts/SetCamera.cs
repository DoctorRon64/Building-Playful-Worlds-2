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
        for (int i = 0; i < 20; i++)
        {
            GetPlayerCam();
        }
    }

	public void GetPlayerCam()
    {
        Player = FindObjectOfType<Player>().transform;
        VirtualCamera.Follow = FindObjectOfType<Player>().transform;
    }
}
