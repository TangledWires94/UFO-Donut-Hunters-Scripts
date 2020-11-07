using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : Manager<InputManager>
{
    public delegate void InputManagerEvent();
    public event InputManagerEvent OnMoveLeft;
    public event InputManagerEvent OnMoveRight;
    public event InputManagerEvent OnJump;
    public event InputManagerEvent OnLeftClick;
    public event InputManagerEvent OnLeftMouseRelease;
    public event InputManagerEvent OnCharacterRelease;

    public delegate void BooleanInputManagerEvent(bool active);
    public event BooleanInputManagerEvent PausePressed;


    public enum GameState { LineDraw, MovePlayer, Paused, LevelEnd};
    public GameState state;
    public GameState lastState = GameState.LineDraw;

    //public Button startButton, resetButton, menuButton, retryMenuButton, quitButton, retryButton;
    //public GameObject pauseMenu;

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
                    if (PausePressed != null)
                    {
                        PausePressed.Invoke(true);
                    } else
                    {
                        Debug.Log("Not subsrcibed to pause event");
                    }
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
                    if(PausePressed != null)
                    {
                        PausePressed.Invoke(true);
                    }
                    Time.timeScale = 0;
                }
                break;

            case GameState.Paused:
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    state = lastState;
                    Time.timeScale = 1;
                    if (PausePressed != null)
                    {
                        PausePressed.Invoke(false);
                    }
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
                    if (PausePressed != null)
                    {
                        PausePressed.Invoke(true);
                    }
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
        state = GameState.LineDraw;
    }

    public void SetGameState(int stateIndex)
    {
        state = (GameState)stateIndex;
    }
}
