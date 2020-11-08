using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : Manager<UIManager>
{
    public Text scoreText, totalScoreText;
    public int score = 0;
    public Animator levelEndAnimator;
    public bool levelFinished;
    public GameObject retryMenu, pauseMenu;
    public Button startButton, resetButton, menuButton, retryMenuButton, quitButton, retryButton;
    public Transform linePointsBar;
    public float linePointsBarMax;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += Reset;
        Reset(SceneManager.GetActiveScene(), new LoadSceneMode());
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= Reset;
    }

    //Parameters are only here to allow function to be assigned as delegate to scene loading event, may use these later to ignore main menu etc.
    void Reset(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.name != "Main Menu")
        {
            Manager<InputManager>.Instance.PausePressed += TogglePause;
            linePointsBar = GameObject.Find("Points Bar").GetComponent<Transform>();
            linePointsBarMax = linePointsBar.transform.localScale.y;
            Manager<LineDrawingManager>.Instance.LineDrawingPointsChanged += UpdateLinePoints;
            scoreText = GameObject.Find("Score Text").GetComponent<Text>();
            totalScoreText = GameObject.Find("Total Score").GetComponent<Text>();
            levelEndAnimator = GameObject.Find("Level End Popup").GetComponent<Animator>();
            score = 0;
            scoreText.text = "Score = 0";
            levelFinished = false;

            if (GameObject.Find("StartButton") != null)
            {
                startButton = GameObject.Find("StartButton").GetComponent<Button>();
                startButton.onClick.AddListener(Manager<InputManager>.Instance.ReleasePlayer);
            }

            if (GameObject.Find("ResetButton") != null)
            {
                resetButton = GameObject.Find("ResetButton").GetComponent<Button>();
                resetButton.onClick.AddListener(Manager<InputManager>.Instance.Reset);
            }

            if (GameObject.Find("MainMenuButton") != null)
            {
                menuButton = GameObject.Find("MainMenuButton").GetComponent<Button>();
                menuButton.onClick.AddListener(Manager<InputManager>.Instance.GoToMainMenu);
            }
            else
            {
                Debug.Log("No main menu");
            }

            if (GameObject.Find("RetryMainMenuButton") != null)
            {
                retryMenuButton = GameObject.Find("RetryMainMenuButton").GetComponent<Button>();
                retryMenuButton.onClick.AddListener(Manager<InputManager>.Instance.GoToMainMenu);
            } else
            {
                Debug.Log("Can't find retry main menu button");
            }

            if (GameObject.Find("QuitButton") != null)
            {
                quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
                quitButton.onClick.AddListener(Manager<InputManager>.Instance.QuitGame);
            }

            if (GameObject.Find("Pause Menu") != null)
            {
                pauseMenu = GameObject.Find("Pause Menu");
                pauseMenu.SetActive(false);

            }

            if (GameObject.Find("RetryButton") != null)
            {
                retryButton = GameObject.Find("RetryButton").GetComponent<Button>();
                retryButton.onClick.AddListener(Manager<InputManager>.Instance.Reset);
            }
            else
            {
                Debug.Log("Can't find retry button");
            }

            retryMenu = GameObject.Find("Retry Menu");
            retryMenu.SetActive(false);
        }
    }

    public void CharacterEventSubscriptions(CharacterControl character)
    {
        character.OnScoreChange += UpdateScore;
        character.OnLevelEnd += PlayEndLevel;
        character.OnPlayerDestroyed += PlayPlayerDied;
    }

    public void UpdateScore(int scoreChange)
    {
        score += scoreChange;
        scoreText.text = "Score = " + score.ToString();
    }

    public void PlayEndLevel()
    {
        levelFinished = true;
        int totalScore = score + Manager<GameManager>.Instance.totalScore;
        totalScoreText.text = "Total Score: " + totalScore.ToString();
        levelEndAnimator.SetTrigger("LevelEnd");
    }

    public void PlayPlayerDied()
    {
        if (!levelFinished)
        {
            Manager<SoundManager>.Instance.PlaySoundEffect(SoundManager.SoundEffect.PlayerDestroyed);
            retryMenu.SetActive(true);
        }
    }

    public void TogglePause(bool active)
    {
        pauseMenu.SetActive(active);
    }

    void UpdateLinePoints(float percentage)
    {
        linePointsBar.localScale = new Vector2(linePointsBar.localScale.x, linePointsBarMax * percentage);
    }
}
