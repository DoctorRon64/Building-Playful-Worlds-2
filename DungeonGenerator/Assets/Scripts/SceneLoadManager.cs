using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoadManager : MonoBehaviour
{
	public void ToNextScene()
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

	public void Restart()
	{
		LoadScene("Dungeongenerator");
	}

	public void MainMenu()
	{
		LoadScene("MainMenu");
	}
}
