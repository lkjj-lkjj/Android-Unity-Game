using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ObstacleSpawner : MonoBehaviour
{
    public DrivingSurfaceManager DrivingSurfaceManager;
    public GameObject ObstaclePrefab;
    public int ObstacleCount = 5;

    public static Vector3 RandomInTriangle(Vector3 v1, Vector3 v2)
    {
        float u = Random.Range(0.0f, 1.0f);
        float v = Random.Range(0.0f, 1.0f);
        if (v + u > 1)
        {
            v = 1 - v;
            u = 1 - u;
        }
        return (v1 * u) + (v2 * v);
    }

    public static Vector3 FindRandomLocation(ARPlane plane)
    {
        var mesh = plane.GetComponent<ARPlaneMeshVisualizer>().mesh;
        var triangles = mesh.triangles;
        var triangleIndex = (int)Random.Range(0, triangles.Length / 3) * 3;
        var vertices = mesh.vertices;
        var randomInTriangle = RandomInTriangle(vertices[triangles[triangleIndex]], vertices[triangles[triangleIndex + 1]]);
        var randomPoint = plane.transform.TransformPoint(randomInTriangle);
        return randomPoint;
    }

    public void SpawnObstacles(ARPlane plane)
    {
        for (int i = 0; i < ObstacleCount; i++)
        {
            var obstacleClone = Instantiate(ObstaclePrefab);
            obstacleClone.transform.position = FindRandomLocation(plane);
        }
    }

    private bool spawned = false;

    private void Update()
    {
        var lockedPlane = DrivingSurfaceManager.LockedPlane;
        if (lockedPlane != null && !spawned)
        {
            SpawnObstacles(lockedPlane);
            spawned = true;
        }
    }
}