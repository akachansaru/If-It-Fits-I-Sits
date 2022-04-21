using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sizes { Small, Medium, Large }

public class GameManager : MonoBehaviour
{
    [Header("Cat")]
    public Transform catSpawnLocation;
    public float catSpawnTime;

    [Header("Box")]
    public Transform[] boxSpawnLocations;
    public GameObject[] boxPrefabs;

    public static GameManager Instance { get; private set; }
    public static bool IsPaused { get; private set; }

    private float catSpawnCooldown = 0f;

    private void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        if (!IsPaused)
        {
            catSpawnCooldown += Time.deltaTime;
            if (catSpawnCooldown >= catSpawnTime)
            {
                SpawnCats();
                catSpawnCooldown = 0f;
            }

            for (int i = 0; i < boxSpawnLocations.Length; i++)
            {
                if (boxSpawnLocations[i].transform.childCount == 0)
                {
                    SpawnBox(boxSpawnLocations[i]);
                }
            }
        }
    }

    private void SpawnCats()
    {
        GameObject cat = Instantiate(Resources.Load("Prefabs/Cat"), catSpawnLocation) as GameObject;
        cat.transform.localPosition = Vector3.zero;
        cat.GetComponent<CatController>().CreateRandomCat();
    }

    private void SpawnBox(Transform position)
    {
        GameObject box = Instantiate(boxPrefabs[UnityEngine.Random.Range(0, boxPrefabs.Length)], position);
        box.transform.localPosition = -box.GetComponent<BoxController>().sprite.transform.localPosition; // Offset for the sprite not being centered
    }

    public void GameOver()
    {
        Pause();
        Debug.Log("Game Over");
    }

    public static void Pause()
    {
        Time.timeScale = 0;
        IsPaused = true;
    }

    public static void Unpause()
    {
        Time.timeScale = 1;
        IsPaused = false;
    }
}
