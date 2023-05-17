using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTextTimed : MonoBehaviour
{
    [SerializeField] private float StartCountdown;
    [SerializeField] private float Countdown;
	[SerializeField] private GameObject Text;

	private void Awake()
	{
		Countdown = StartCountdown;
	}

	private void Update()
	{

		if (Countdown > 0)
		{
			Countdown -= Time.time / 100;
		}

		if (Countdown < 0)
		{
			Text.SetActive(false);
		}
	}

}
