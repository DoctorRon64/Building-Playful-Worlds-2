using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOver : MonoBehaviour
{
	public void GameWon()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	public void GameLose()
	{
		LoadScene("lose");
	}

	public void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}
