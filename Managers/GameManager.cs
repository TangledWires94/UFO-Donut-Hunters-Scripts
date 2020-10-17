using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Manager<GameManager>
{
    public GameObject character;
    public List<GameObject> instantiatedManagers = new List<GameObject>();
    public int totalScore = 0;

    public List<GameObject> Managers = new List<GameObject>();

    void Start()
    {
        SceneManager.sceneLoaded += Reset;
        totalScore = 0;
        if(SceneManager.GetActiveScene().name != "MainMenu")
        {
            Reset(new Scene(), new LoadSceneMode());
        }
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= Reset;
    }

    void Reset(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(SceneManager.GetActiveScene().buildIndex > 0)
        {
            Transform playerSpawn = GameObject.Find("Player Spawn").GetComponent<Transform>();
            CharacterControl characterControl = Instantiate(character, playerSpawn.position, playerSpawn.rotation).GetComponent<CharacterControl>();
            Manager<UIManager>.Instance.CharacterEventSubscriptions(characterControl);
        } else
        {
            totalScore = 0;
            GameObject startButton = GameObject.Find("Start Game Button");
            startButton.GetComponent<Button>().onClick.AddListener(StartGame);
        }
    }

    public void StartGame()
    {
        foreach(GameObject manager in Managers)
        {
            instantiatedManagers.Add(Instantiate(manager));
        }
        SceneManager.LoadScene(1);
    }

    public void LoadMainMenu()
    {
        foreach(GameObject manager in instantiatedManagers)
        {
            Destroy(manager);
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadNewScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }
}
