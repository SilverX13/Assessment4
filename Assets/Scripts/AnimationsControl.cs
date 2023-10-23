using UnityEngine;
using UnityEngine.UI;

public class TriggerAnimations : MonoBehaviour
{
    public Animator pacStudent1Animator;
    public Animator powerPelletAnimator;
    public Animator pacDeadAnimator;
    public Animator ghostScaredAnimator;
    public Animator ghostDeadAnimator;
    public Animator ghost1Animator;
    public Animator ghost2Animator;
    public Animator ghost3Animator;
    public Animator ghost4Animator;
    public Button playButton;

    void Start()
    {
        DisableAllAnimators();

        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
    }

    void DisableAllAnimators()
    {
        if (pacStudent1Animator != null) pacStudent1Animator.enabled = false;
        if (powerPelletAnimator != null) powerPelletAnimator.enabled = false;
        if (pacDeadAnimator != null) pacDeadAnimator.enabled = false;
        if (ghostScaredAnimator != null) ghostScaredAnimator.enabled = false;
        if (ghostDeadAnimator != null) ghostDeadAnimator.enabled = false;
        if (ghost1Animator != null) ghost1Animator.enabled = false;
        if (ghost2Animator != null) ghost2Animator.enabled = false;
        if (ghost3Animator != null) ghost3Animator.enabled = false;
        if (ghost4Animator != null) ghost4Animator.enabled = false;
    }

    public void OnPlayButtonClicked()
    {
        PlayAnimations();
    }

    void PlayAnimations()
    {
        if (pacStudent1Animator != null)
        {
            pacStudent1Animator.enabled = true;
            pacStudent1Animator.Play("Pac down");
        }
        if (powerPelletAnimator != null)
        {
            powerPelletAnimator.enabled = true;
            powerPelletAnimator.Play("Flashing power pellets");
        }
        if (pacDeadAnimator != null)
        {
            pacDeadAnimator.enabled = true;
            pacDeadAnimator.Play("Pac dead");
        }
        if (ghostScaredAnimator != null)
        {
            ghostScaredAnimator.enabled = true;
            ghostScaredAnimator.Play("Ghost scared");
        }
        if (ghostDeadAnimator != null)
        {
            ghostDeadAnimator.enabled = true;
            ghostDeadAnimator.Play("Ghost dead");
        }
        if (ghost1Animator != null)
        {
            ghost1Animator.enabled = true;
            ghost1Animator.Play("Ghost1 left");
        }
        if (ghost2Animator != null)
        {
            ghost2Animator.enabled = true;
            ghost2Animator.Play("Ghost2 left");
        }
        if (ghost3Animator != null)
        {
            ghost3Animator.enabled = true;
            ghost3Animator.Play("Ghost3 left");
        }
        if (ghost4Animator != null)
        {
            ghost4Animator.enabled = true;
            ghost4Animator.Play("Ghost4 left");
        }
    }
}