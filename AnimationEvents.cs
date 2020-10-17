using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void SetInputManagerState(int stateIndex)
    {
        Manager<InputManager>.Instance.SetGameState(stateIndex);
    }
}
