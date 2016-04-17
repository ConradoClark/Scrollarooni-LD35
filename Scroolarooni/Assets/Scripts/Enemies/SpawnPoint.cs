using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    public Camera cam;
    public float minDistanceFromCenterToTrigger;
    private bool triggered;
    public EnemySpawnData[] spawnData;

    void Start()
    {

    }

    void Update()
    {
        if (!triggered)
        {
            var camera2DPos = new Vector2(cam.transform.position.x, cam.transform.position.y);
            if (Vector2.Distance(this.transform.position, camera2DPos) < minDistanceFromCenterToTrigger)
            {
                this.SpawnEnemies();
                triggered = true;
            }
        }
    }

    void SpawnEnemies()
    {
        foreach(var data in spawnData)
        {
            GameObject enemy = GameObject.Instantiate(data.enemyType);
            enemy.transform.position = cam.transform.position + new Vector3(data.spawnLocation.x, data.spawnLocation.y);
        }
    }
}
