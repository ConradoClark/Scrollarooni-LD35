using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour
{
    public Camera cam;
    public float minDistanceFromCenterToTrigger;
    private bool triggered;
    public ObjectSpawnData[] spawnData;

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
                SpawnObjects();
                triggered = true;
            }
        }
    }

    private void SpawnObjects()
    {
        foreach (var data in spawnData)
        {
            var spawnPosition = this.transform.position + new Vector3(data.transform.localPosition.x, data.transform.localPosition.y);

            if (data.spawnPoof == null)
            {
                this.StartCoroutine(SpawnEnemy(0f, data.objectType, spawnPosition));
                return;
            }

            GameObject poof = GameObject.Instantiate(data.spawnPoof);
            poof.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);

            float delay = 0f;
            Lifetime lifetime = poof.GetComponent<Lifetime>();
            if (lifetime != null)
            {
                delay = lifetime.Duration / 2f;
            }

            this.StartCoroutine(SpawnEnemy(delay, data.objectType, spawnPosition));
        }
    }

    IEnumerator SpawnEnemy(float delay, GameObject enemyType, Vector3 spawnPosition)
    {
        yield return new WaitForSeconds(delay);
        GameObject enemy = GameObject.Instantiate(enemyType);
        enemy.transform.position = new Vector3(spawnPosition.x, spawnPosition.y,0);
    }
}
