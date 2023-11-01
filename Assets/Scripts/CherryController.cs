using System.Collections;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherryPrefab;
    public Transform centerPoint;
    public float spawnRate = 10f;

    private Camera mainCamera;
    private Vector2 spawnPosition;
    private Vector2 endPosition;

    private void Start()
    {
        SetupCamera();
        StartSpawningCherries();
    }

    private void SetupCamera()
    {
        mainCamera = Camera.main;
    }

    private void StartSpawningCherries()
    {
        StartCoroutine(InitialDelay());
    }

    private IEnumerator InitialDelay()
    {
        yield return new WaitForSeconds(spawnRate);
        yield return StartCoroutine(SpawnAndMoveCherry());
    }

    private IEnumerator SpawnAndMoveCherry()
    {
        while (true)
        {
            SpawnNewCherry();
            yield return StartCoroutine(MoveCherryToDestination());
            yield return new WaitForSeconds(spawnRate);
        }
    }

    private void SpawnNewCherry()
    {
        spawnPosition = DetermineSpawnPosition();
        InstantiateCherry();
        DetermineEndPosition();
    }

    private Vector2 DetermineSpawnPosition()
    {
        return RandomPositionOutsideCamera();
    }

    private void InstantiateCherry()
    {
        Instantiate(cherryPrefab, spawnPosition, Quaternion.identity);
    }

    private void DetermineEndPosition()
    {
        endPosition = (2 * new Vector2(centerPoint.position.x, centerPoint.position.y)) - spawnPosition;
    }

    private IEnumerator MoveCherryToDestination()
    {
        GameObject cherry = GameObject.FindWithTag("BSC"); // Assuming the prefab has a tag "Cherry"
        float t = 0;
        Vector2 startPosition = cherry.transform.position;

        while (t < 1)
        {
            t += Time.deltaTime / spawnRate;
            cherry.transform.position = Vector2.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        Destroy(cherry);
    }

    private Vector2 RandomPositionOutsideCamera()
    {
        Vector2 viewPos = Vector2.zero;

        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0:
                viewPos = new Vector2(-0.1f, Random.value);
                break;
            case 1:
                viewPos = new Vector2(1.1f, Random.value);
                break;
            case 2:
                viewPos = new Vector2(Random.value, 1.1f);
                break;
            default:
                viewPos = new Vector2(Random.value, -0.1f);
                break;
        }

        return mainCamera.ViewportToWorldPoint(viewPos);
    }
}
