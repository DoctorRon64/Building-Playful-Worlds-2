using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBoss : Enemy
{
    public override void CheckIfEnemyDies()
    {
        if (Health <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
