using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformManager : MonoBehaviour
{
    public GameObject platformPrefab;
    public int minPlatforms = 2;
    public float platformLength = 9f;
    public float minStayTime = 5f;
    public Text scoreText;

    private List<GameObject> platforms = new List<GameObject>();
    private Vector3 lastPosition;
    private int score = 0;
    private GameObject initialPlatform;
    private bool initialPlatformRemoved = false;

    void Start()
    {
        lastPosition = Vector3.zero;
        initialPlatform = CreatePlatform(lastPosition);
        
        StartCoroutine(ManagePlatforms());
        UpdateScoreDisplay();
    }

    IEnumerator ManagePlatforms()
    {
        while (true)
        {
            yield return new WaitForSeconds(minStayTime / 2);

            Vector3 nextPosition = GetNextPosition();
            CreatePlatform(nextPosition);

            if (platforms.Count > minPlatforms)
            {
                GameObject oldPlatform = platforms[0];
                platforms.RemoveAt(0);

                if (oldPlatform != null)
                {
                    PlatformScript oldPlatformScript = oldPlatform.GetComponent<PlatformScript>();
                    if (oldPlatformScript != null)
                    {
                        StartCoroutine(oldPlatformScript.ScaleOutAndDestroy());
                    }
                    else
                    {
                        Destroy(oldPlatform);
                    }
                }
            }

            score++;
            UpdateScoreDisplay();

            if (!initialPlatformRemoved && platforms.Count >= minPlatforms && PlayerHasLeftInitialPlatform())
            {
                Destroy(initialPlatform);
                initialPlatformRemoved = true;
                initialPlatform = null;
                Destroy(gameObject);
            }
        }
    }

    void DestroyAllPlatformsWithName(string platformName)
    {
        GameObject[] platformsToDestroy = GameObject.FindGameObjectsWithTag("Platform");

        foreach (GameObject platform in platformsToDestroy)
        {
            if (platform.name == platformName || platform.CompareTag("Platform"))
            {
                Destroy(platform);
            }
        }
    }

    bool PlayerHasLeftInitialPlatform()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            return Vector3.Distance(player.transform.position, initialPlatform.transform.position) > platformLength;
        }
        return false;
    }

    GameObject CreatePlatform(Vector3 position)
    {
        if (platforms.Count == 0 || Vector3.Distance(lastPosition, position) >= platformLength)
        {
            GameObject newPlatform = Instantiate(platformPrefab, position, Quaternion.identity);
            newPlatform.tag = "Platform";

            Text platformTimerText = newPlatform.GetComponentInChildren<Text>();
            if (platformTimerText == null)
            {
                Debug.LogError("TimerText component not found in the platform prefab.");
                return null;
            }

            PlatformScript platformScript = newPlatform.AddComponent<PlatformScript>();
            platformScript.Setup(minStayTime, platformTimerText);

            lastPosition = position;
            platforms.Add(newPlatform);
            Debug.Log("Created new platform at position: " + position);

            return newPlatform;
        }
        return null;
    }

    Vector3 GetNextPosition()
    {
        Vector3[] directions = new Vector3[]
        {
            new Vector3(platformLength, 0f, 0f),
            new Vector3(-platformLength, 0f, 0f),
            new Vector3(0f, 0f, platformLength),
            new Vector3(0f, 0f, -platformLength)
        };

        Vector3 randomDirection;
        Vector3 nextPosition;
        int attempt = 0;
        const int maxAttempts = 10;

        do
        {
            randomDirection = directions[Random.Range(0, directions.Length)];
            nextPosition = lastPosition + randomDirection;
            attempt++;
        }
        while (IsPositionOccupied(nextPosition) && attempt < maxAttempts);

        Debug.Log("Generated Next Position: " + nextPosition);

        return nextPosition;
    }

    bool IsPositionOccupied(Vector3 position)
    {
        foreach (GameObject platform in platforms)
        {
            if (platform != null)
            {
                if (Vector3.Distance(platform.transform.position, position) < platformLength)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
