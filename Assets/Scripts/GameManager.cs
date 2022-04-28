using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Sizes { Small, Medium, Large }

public class GameManager : MonoBehaviour
{
    [Header("Level")]
    public List<Level> levels;
    public int currLevel = 0;
    public Transform levelPanel;
    public Text levelNumText;
    public float showLevelTextTime;

    [Header("Cat")]
    public List<Transform> catSpawnLocations;
    public float catSpawnTime;

    [Header("Box")]
    public Transform[] boxSpawnLocations;
    public GameObject[] boxPrefabs;

    [Header("Box Placement Bounds")]
    public List<Renderer> gameBounds;

    public static GameManager Instance { get; private set; }
    public static bool IsPaused { get; private set; }

    private float catSpawnCooldown = 0f;
    private int catDir = -1; // -1 or 1 based on where it spawns

    private List<CatController> catsOnBoard = new List<CatController>();
    private List<BoxController> boxesOnBoard = new List<BoxController>();

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        StartCoroutine(ShowLevelNumText());

        foreach (Level level in levels)
        {
            level.GenerateLevel();
        }
    }

    public void Update()
    {
        if (!IsPaused && LevelsRemain())
        {
            if (levels[currLevel].remainingCats.Count == 0 && !CatsStillMoving())
            {
                Win();
            }

            catSpawnCooldown += Time.deltaTime;
            if (catSpawnCooldown >= catSpawnTime && levels[currLevel].remainingCats.Count > 0)
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

    private bool LevelsRemain()
    {
        return levels.Count > currLevel;
    }

    private IEnumerator ShowLevelNumText()
    {
        Pause();
        levelNumText.text = "Level " + currLevel;
        levelPanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(showLevelTextTime);

        levelPanel.gameObject.SetActive(false);
        Unpause();
    }

    private bool CatsStillMoving()
    {
        foreach(CatController cat in catsOnBoard)
        {
            if (!cat.inBox)
            {
                return true;
            }
        }
        return false;
    }

    private void SpawnCats()
    {
        Transform catSpawnLocation;
        if (catDir == 1)
        {
            catSpawnLocation = catSpawnLocations[0];
        }
        else if (catDir == -1)
        {
            catSpawnLocation = catSpawnLocations[1];
        }
        else
        {
            Debug.LogError("Invalid cat direction");
            catSpawnLocation = catSpawnLocations[0];
        }

        GameObject cat = Instantiate(Resources.Load("Prefabs/Cat"), catSpawnLocation) as GameObject;
        cat.transform.localPosition = Vector3.zero;

        int randCat = UnityEngine.Random.Range(0, levels[currLevel].remainingCats.Count);
        Sizes size = levels[currLevel].remainingCats[randCat];

        levels[currLevel].remainingCats.RemoveAt(randCat);
        cat.GetComponent<CatController>().CreateCat(size, new Vector3(catDir, 0, 0), levels[currLevel].catSpeed);

        catsOnBoard.Add(cat.GetComponent<CatController>());

        catDir *= -1; // Switch the side of the map that the next cat will spawn from
    }

    private void SpawnBox(Transform position)
    {
        GameObject box = Instantiate(boxPrefabs[UnityEngine.Random.Range(0, boxPrefabs.Length)], position);
        box.transform.localPosition = -box.GetComponent<BoxController>().GetOffset(); // Offset for the sprite not being centered

        boxesOnBoard.Add(box.GetComponent<BoxController>());
    }

    public void Win()
    {
        Debug.Log("You win! Level " + currLevel + " complete.");
        currLevel++;

        // Clear board
        foreach(CatController cat in catsOnBoard)
        {
            if (cat.gameObject != null)
            {
                Destroy(cat.gameObject);
            }
        }
        catsOnBoard.Clear();

        foreach(BoxController box in boxesOnBoard)
        {
            if (box.gameObject != null)
            {
                box.Remove();
                Destroy(box.gameObject);
            }
        }
        boxesOnBoard.Clear();

        if (LevelsRemain())
        {
            StartCoroutine(ShowLevelNumText());
        }
    }

    public void GameOver()
    {
        Pause();
        Debug.Log("Game Over");
    }

    public static void Pause()
    {
        //Time.timeScale = 0;
        IsPaused = true;
    }

    public static void Unpause()
    {
        //Time.timeScale = 1;
        IsPaused = false;
    }
}
