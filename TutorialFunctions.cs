using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialFunctions : MonoBehaviour
{
    //Functions to handle events that only happen in tutorial level

    public GameObject tutorialUI;
    public Text tutorialText;
    public string drawingText, movingText;
    bool tutorialActive = true;

    void Start()
    {
        Manager<InputManager>.Instance.OnCharacterRelease += PlayerReleased;
        tutorialActive = true;
    }

    public void ChangeTutorialText(bool UFOReleased)
    {
        if (tutorialActive)
        {
            if (UFOReleased)
            {
                tutorialText.text = ConvertString(movingText);
            }
            else
            {
                tutorialText.text = ConvertString(drawingText);
            }
        }
    }

    void PlayerReleased()
    {
        ChangeTutorialText(true);
    }

    void LevelEnd()
    {
        Destroy(tutorialUI);
        Manager<InputManager>.Instance.OnCharacterRelease -= PlayerReleased;
    }

    public void SubToPlayerEvents(CharacterControl characterControl)
    {
        characterControl.OnLevelEnd += LevelEnd;
    }

    string ConvertString(string uneditedString)
    {
        string editedText = uneditedString.Replace("*", "\n");
        return editedText;
    }

}
