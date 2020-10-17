using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterControl : MonoBehaviour
{
    Rigidbody2D rb;
    public float movementForce, jumpForce;
    public bool jumping = true;

    public delegate void IntEvent(int integer);
    public event IntEvent OnScoreChange;
    public delegate void CharacterEvent();
    public CharacterEvent OnLevelEnd, OnPlayerDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.Sleep();

        Manager<InputManager>.Instance.OnCharacterRelease += ReleaseCharacter;
        Manager<InputManager>.Instance.OnMoveLeft += MoveLeft;
        Manager<InputManager>.Instance.OnMoveRight += MoveRight;
        Manager<InputManager>.Instance.OnJump += Jump;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "PointObject":
                if (OnScoreChange != null)
                {
                    OnScoreChange.Invoke(1); //When create a point object script add variable for point value
                    Manager<SoundManager>.Instance.PlaySoundEffect(SoundManager.SoundEffect.Pickup);
                }
                Destroy(other.gameObject); //When create a point object script create destroy object function to call
                break;

            case "EndLevel":
                if (OnLevelEnd != null)
                {
                    OnLevelEnd.Invoke();
                    Manager<SoundManager>.Instance.PlaySoundEffect(SoundManager.SoundEffect.EndLevel);
                }
                break;

            case "DestroyPlayer":
                if (OnPlayerDestroyed != null)
                {
                    OnPlayerDestroyed.Invoke();
                    Manager<InputManager>.Instance.OnCharacterRelease -= ReleaseCharacter;
                    Manager<InputManager>.Instance.OnMoveLeft -= MoveLeft;
                    Manager<InputManager>.Instance.OnMoveRight -= MoveRight;
                    Manager<InputManager>.Instance.OnJump -= Jump;
                    Destroy(this.gameObject);
                }
                break;

            default:
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Floor")
        {
            jumping = false;
        }
    }
    void ReleaseCharacter()
    {
        rb.WakeUp();
        rb.gravityScale = 1;
        Manager<SoundManager>.Instance.PlaySoundEffect(SoundManager.SoundEffect.Release);
    }

    void MoveLeft()
    {
        rb.AddForce(Vector2.left * movementForce, ForceMode2D.Impulse);
    }

    void MoveRight()
    {
        rb.AddForce(Vector2.right * movementForce, ForceMode2D.Impulse);
    }

    void Jump()
    {
        if(jumping == false)
        {
            jumping = true;
            Manager<SoundManager>.Instance.PlaySoundEffect(SoundManager.SoundEffect.Jump);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
