using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Forest : MonoBehaviour
{
    public TerrainGenerator terrainGenerator;
    public List<GameObject> spawnedObjects = new List<GameObject>();
    public int spawnCount = 1;
    public Transform spawnPoint;

    private List<GameObject> instantiated = new List<GameObject>();

    public void GenerateForest()
    {
        foreach (GameObject gameObject in instantiated)
        {
            Destroy(gameObject);
        }

        terrainGenerator.seed = Random.Range(-10000, 10000);
        terrainGenerator.GenerateNoisemap();
        terrainGenerator.GenerateColormap();
        terrainGenerator.GenerateMesh();

        Debug.Log("found forest");

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 rayOrigin = spawnPoint.position + spawnPoint.rotation * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            Ray ray = new Ray(rayOrigin, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                GameObject toSpawn = spawnedObjects[Random.Range(0, spawnedObjects.Count)];
                instantiated.Add(Instantiate(toSpawn, hitInfo.point, Quaternion.identity, spawnPoint));
            }
        }
    }


}
