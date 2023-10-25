using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public Animator animator;
    public ParticleSystem dustParticles;
    public AudioSource moveAudio;

    private Vector3 targetPosition;
    private bool isLerping;
    private string lastInput = "";
    private string currentInput = "";
    private bool hasMovedOnce = false;

    private readonly Dictionary<string, Vector3> directions = new Dictionary<string, Vector3>
    {
        { "w", Vector3.up },
        { "a", Vector3.left },
        { "s", Vector3.down },
        { "d", Vector3.right }
    };

    private readonly Dictionary<string, string> animationTriggers = new Dictionary<string, string>
    {
        { "w", "MoveUp" },
        { "a", "MoveLeft" },
        { "s", "MoveDown" },
        { "d", "MoveRight" }
    };

    void Start()
    {
        Setup();
    }

    void Update()
    {
        HandleInputAndMovement();
    }

    private void Setup()
    {
        moveAudio.Stop();
        targetPosition = transform.position;
    }

    private void HandleInputAndMovement()
    {
        GetInput();

        if (hasMovedOnce)
        {
            if (!isLerping)
            {
                HandleStaticStateMovement();
            }
            else
            {
                ContinueLerping();
            }
        }
    }

    private void HandleStaticStateMovement()
    {
        TryMove(lastInput);

        if (!CanMoveTo(targetPosition))
        {
            TryMove(currentInput);
        }

        if (CanMoveTo(targetPosition))
        {
            BeginLerpProcess();
        }
    }

    private void GetInput()
    {
        if (Input.anyKeyDown)
        {
            lastInput = Input.inputString;

            if (directions.ContainsKey(lastInput))
            {
                TryMove(lastInput);

                if (CanMoveTo(targetPosition))
                {
                    hasMovedOnce = true;
                    BeginLerpProcess();
                }
            }
        }
    }

    private void TryMove(string directionKey)
    {
        if (directions.TryGetValue(directionKey, out Vector3 dir))
        {
            targetPosition = transform.position + dir;
            currentInput = directionKey;
        }
    }

    private bool CanMoveTo(Vector3 targetPos)
    {
        return true;
    }

    private void BeginLerpProcess()
    {
        isLerping = true;
        ActivateAnimationAndEffects(currentInput);
    }

    private void ContinueLerping()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position.Equals(targetPosition))
        {
            EndLerpProcess();
        }
    }

    private void ActivateAnimationAndEffects(string directionKey)
    {
        SetAnimation(directionKey);
        dustParticles.Play();
        if (!moveAudio.isPlaying) moveAudio.Play();
    }

    private void SetAnimation(string directionKey)
    {
        StopAllAnimations();
        if (animationTriggers.TryGetValue(directionKey, out string animTrigger))
        {
            animator.SetTrigger(animTrigger);
        }
    }

    private void StopAllAnimations()
    {
        foreach (var trigger in animationTriggers.Values)
        {
            animator.ResetTrigger(trigger);
        }

        dustParticles.Stop();
        moveAudio.Stop();
    }

    private void EndLerpProcess()
    {
        isLerping = false;
        StopAllAnimations();
    }
}
