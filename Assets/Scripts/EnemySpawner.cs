using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public DrivingSurfaceManager DrivingSurfaceManager;
    public GameObject EnemyPrefab;
    public int EnemyCount = 3;

    private bool spawned = false;
    private List<Vector3> spawnedPositions = new List<Vector3>();

    void Update()
    {
        var lockedPlane = DrivingSurfaceManager.LockedPlane;
        if (lockedPlane != null && !spawned)
        {
            for (int i = 0; i < EnemyCount; i++)
            {
                Vector3 spawnPos;
                int tryCount = 0;
                do
                {
                    spawnPos = ObstacleSpawner.FindRandomLocation(lockedPlane);
                    tryCount++;
                }
                while (IsTooCloseToOthers(spawnPos, spawnedPositions) && tryCount < 10);

                var enemy = Instantiate(EnemyPrefab);
                enemy.transform.position = spawnPos;
                spawnedPositions.Add(spawnPos);
            }
            spawned = true;
        }
    }

    private bool IsTooCloseToOthers(Vector3 pos, List<Vector3> others, float minDist = 0.5f)
    {
        foreach (var other in others)
        {
            if (Vector3.Distance(pos, other) < minDist)
                return true;
        }
        return false;
    }
}