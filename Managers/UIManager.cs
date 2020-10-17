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
        scoreText = GameObject.Find("Score Text").GetComponent<Text>();
        totalScoreText = GameObject.Find("Total Score").GetComponent<Text>();
        levelEndAnimator = GameObject.Find("Level End Popup").GetComponent<Animator>();
        score = 0;
        scoreText.text = "Score = 0";
        levelFinished = false;
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
            //Add popup to show when player dies
        }
    }
}
