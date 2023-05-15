using UnityEngine;

public class EndBoss : Enemy
{
    public override void CheckIfEnemyDies()
    {
        if (Health <= 0)
        {
            SceneLoadManager.ToNextScene();
        }
    }
}
