using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputManager : Manager<InputManager>
{
    public delegate void InputManagerEvent();
    public event InputManagerEvent OnMoveLeft;
    public event InputManagerEvent OnMoveRight;
    public event InputManagerEvent OnJump;
    public event InputManagerEvent OnLeftClick;
    public event InputManagerEvent OnLeftMouseRelease;
    public event InputManagerEvent OnCharacterRelease;

    public enum GameState { LineDraw, MovePlayer, Paused, LevelEnd};
    public GameState state;
    public GameState lastState = GameState.LineDraw;

    public Button startButton, resetButton, menuButton, quitButton;
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        state = GameState.LineDraw;
        lastState = GameState.LineDraw;
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
        }

        switch (state)
        {
            case GameState.LineDraw:
                if (Input.GetMouseButton(0))
                {
                    if(OnLeftClick != null)
                    {
                        OnLeftClick.Invoke();
                    }
                }
                if (Input.GetMouseButtonUp(0))
                {
                    if (OnLeftMouseRelease != null)
                    {
                        OnLeftMouseRelease.Invoke();
                    }
                }
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    ReleasePlayer();
                }
                if (Input.GetButtonDown("Reset"))
                {
                    Reset();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    state = GameState.Paused;
                    lastState = GameState.LineDraw;
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0;
                }
                break;
            case GameState.MovePlayer:
                if (Input.GetButton("Left"))
                {
                    if (OnMoveLeft != null)
                    {
                        OnMoveLeft.Invoke();
                    }
                }
                if (Input.GetButton("Right"))
                {
                    if (OnMoveRight != null)
                    {
                        OnMoveRight.Invoke();
                    }
                }
                if (Input.GetButtonDown("Jump"))
                {
                    if (OnJump != null)
                    {
                        OnJump.Invoke();
                    }
                }
                if (Input.GetButtonDown("Reset"))
                {
                    Reset();
                }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    state = GameState.Paused;
                    lastState = GameState.MovePlayer;
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0;
                }
                break;

            case GameState.Paused:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    state = lastState;
                    Time.timeScale = 1;
                    pauseMenu.SetActive(false);
                }
                break;

            case GameState.LevelEnd:
                if (Input.GetButtonDown("Reset"))
                {
                    Reset();
                } else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    state = GameState.Paused;
                    lastState = GameState.LevelEnd;
                    Time.timeScale = 0;
                    pauseMenu.SetActive(true);
                } else if (Input.anyKeyDown)
                {
                    Manager<GameManager>.Instance.totalScore += Manager<UIManager>.Instance.score;
                    int levelIndex = SceneManager.GetActiveScene().buildIndex;
                    if(levelIndex >= SceneManager.sceneCountInBuildSettings - 1)
                    {
                        Manager<GameManager>.Instance.LoadMainMenu();
                    } else
                    {
                        Manager<GameManager>.Instance.LoadNewScene(levelIndex + 1);
                    }
                }
                break;

            default:
                break;
        }
    }

    public void Reset()
    {
        OnMoveLeft = null;
        OnMoveRight = null;
        OnJump = null;
        OnLeftClick = null;
        OnCharacterRelease = null;
        OnLeftMouseRelease = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        state = GameState.LineDraw;
        lastState = GameState.LineDraw;
    }

    public void ReleasePlayer()
    {
        OnCharacterRelease.Invoke();
        state = GameState.MovePlayer;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        Manager<GameManager>.Instance.LoadMainMenu();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(GameObject.Find("StartButton") != null)
        {
            startButton = GameObject.Find("StartButton").GetComponent<Button>();
            startButton.onClick.AddListener(ReleasePlayer);
        }

        if (GameObject.Find("ResetButton") != null)
        {
            resetButton = GameObject.Find("ResetButton").GetComponent<Button>();
            resetButton.onClick.AddListener(Reset);
        }

        if (GameObject.Find("MainMenuButton") != null)
        {
            menuButton = GameObject.Find("MainMenuButton").GetComponent<Button>();
            menuButton.onClick.AddListener(GoToMainMenu);
        }
        else
        {
            Debug.Log("No main menu");
        }

        if (GameObject.Find("QuitButton") != null)
        {
            quitButton = GameObject.Find("QuitButton").GetComponent<Button>();
            quitButton.onClick.AddListener(QuitGame);
        }

        if (GameObject.Find("Pause Menu") != null)
        {
            pauseMenu = GameObject.Find("Pause Menu");
            pauseMenu.SetActive(false);

        }

        state = GameState.LineDraw;
    }

    public void SetGameState(int stateIndex)
    {
        state = (GameState)stateIndex;
    }
}
