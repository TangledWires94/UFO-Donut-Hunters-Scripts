using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : Manager<SoundManager>
{
    public AudioSource audioSource;
    public enum BackgroundMusic { MainMenu, Level };
    public List<AudioClip> backgroundMusic = new List<AudioClip>();
    public enum SoundEffect { Pickup, Release, EndLevel, PlayerDestroyed, Jump};
    public List<AudioClip> soundEffects = new List<AudioClip>();
    public string lastScene;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        ChangeBackgroundMusic(BackgroundMusic.MainMenu);
        SceneManager.sceneLoaded += LevelChanged;
        lastScene = SceneManager.GetActiveScene().name;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= LevelChanged;
    }

    public void PlaySoundEffect(SoundEffect soundEffect)
    {
        audioSource.PlayOneShot(soundEffects[(int)soundEffect]);
    }

    public void ChangeBackgroundMusic(BackgroundMusic music)
    {
        audioSource.Stop();
        audioSource.clip = backgroundMusic[(int)music];
        audioSource.loop = true;
        audioSource.Play();
    }

    void LevelChanged(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.name == "MainMenu")
        {
            if(lastScene != "MainMenu")
            {
                ChangeBackgroundMusic(BackgroundMusic.MainMenu);
            }
        }
        else
        {
            if(lastScene == "MainMenu")
            {
                ChangeBackgroundMusic(BackgroundMusic.Level);
            }
        }

        lastScene = scene.name;
    }
}
