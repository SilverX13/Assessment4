using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


public class PacStudentMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5.0f;
    private Vector2 moveDirection = Vector2.zero;
    private Rigidbody2D rb;
    private bool isTeleporting = false;

    [Header("Gameplay Elements")]
    public Transform leftTeleporterPosition;
    public Transform rightTeleporterPosition;
    public Transform respawnPosition;
    public GameObject[] lifeIndicators;

    [Header("Audio")]
    public AudioSource moveAudio;
    public AudioClip wallCollisionSound;
    public AudioSource pacDeathAudio;
    public AudioClip diamondSound;
    public AudioSource scaredAudioSource;

    [Header("Visual Effects")]
    public Animator animator;
    public ParticleSystem dustParticles;
    public ParticleSystem wallCollisionParticles;

    [Header("UI Elements")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI gameTimerText;
    

    [Header("Game Status")]
    public string startSceneName = "StartScene";
    public int totalPellets;
    public int ghostLives = 3;
    private int score = 0;
    private int eatenPellets = 0;
    private float elapsedTime = 0f;
    private bool isGameStarted = false;
    private bool isGameOver = false;
    private bool isScared = false;
    private bool isRecovering = false;
    private bool hasGivenScore = false;
    public float scaredTime = 10f;

    private Dictionary<KeyCode, Vector2> movementMappings;
    private Dictionary<KeyCode, string> animationMappings;

    private Dictionary<KeyCode, Vector2> directions = new Dictionary<KeyCode, Vector2>
    {
        { KeyCode.W, Vector2.up },
        { KeyCode.A, Vector2.left },
        { KeyCode.S, Vector2.down },
        { KeyCode.D, Vector2.right }
    };

    private Dictionary<KeyCode, string> animationTriggers = new Dictionary<KeyCode, string>
    {
        { KeyCode.W, "MoveUp" },
        { KeyCode.A, "MoveLeft" },
        { KeyCode.S, "MoveDown" },
        { KeyCode.D, "MoveRight" }
    };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAudio.Stop();
        gameOverText.gameObject.SetActive(false);
        gameTimerText.text = "00:00:00";

    }

    void Update()
    {
        if (isGameStarted && !isGameOver)
        {
            elapsedTime += Time.deltaTime;
            int minutes = (int)elapsedTime / 60;
            int seconds = (int)elapsedTime % 60;
            int milliseconds = (int)(elapsedTime * 100) % 100;

            gameTimerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
        if (!isGameOver)
        {
            GetInput();
            PlayAnimationBasedOnDirection();
            CheckGameOver();
        }
    }
    public void StartGame()
    {
        isGameStarted = true;
    }
    void CheckGameOver()
    {
        if (eatenPellets >= totalPellets)
        {
            StartCoroutine(ShowGameOver());
        }
    }
    IEnumerator ShowGameOver()
    {
        isGameOver = true;
        isGameStarted = false;

        moveDirection = Vector2.zero;
        rb.velocity = Vector2.zero;

        moveAudio.Stop();
        scaredAudioSource.Stop();
        countdownText.gameObject.SetActive(false);

        gameOverText.gameObject.SetActive(true);

        int currentScore = score;

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        float bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);

        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("HighScore", currentScore);
            PlayerPrefs.SetFloat("BestTime", elapsedTime);
        }
        else if (currentScore == highScore && elapsedTime < bestTime)
        {
            PlayerPrefs.SetFloat("BestTime", elapsedTime);
        }



        yield return new WaitForSeconds(3);
        UnityEngine.SceneManagement.SceneManager.LoadScene(startSceneName);
    }

    void GetInput()
    {
        foreach (var keyDirectionPair in directions)
        {
            if (Input.GetKeyDown(keyDirectionPair.Key))
            {
                moveDirection = keyDirectionPair.Value;
                break;
            }
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;

        if (moveDirection != Vector2.zero && !moveAudio.isPlaying)
        {
            moveAudio.Play();
        }

        else if (moveDirection == Vector2.zero)
        {
            moveAudio.Stop();
        }
    }

    void PlayAnimationBasedOnDirection()
    {
        foreach (var pair in animationTriggers)
        {
            if (directions[pair.Key] == moveDirection)
            {
                animator.SetTrigger(pair.Value);
            }
            else
            {
                animator.ResetTrigger(pair.Value);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            AudioSource.PlayClipAtPoint(wallCollisionSound, transform.position);
            wallCollisionParticles.Play();
            moveDirection = Vector2.zero;
        }
        else if (collision.gameObject.CompareTag("LeftTele") && !isTeleporting)
        {
            StartCoroutine(TeleportCooldown(rightTeleporterPosition));
        }
        else if (collision.gameObject.CompareTag("RightTele") && !isTeleporting)
        {
            StartCoroutine(TeleportCooldown(leftTeleporterPosition));
        }


    }

    IEnumerator TeleportCooldown(Transform teleportToPosition)
    {
        isTeleporting = true;
        Vector3 offset = moveDirection * 0.2f;
        transform.position = teleportToPosition.position + offset;
        yield return new WaitForSeconds(0.1f);
        isTeleporting = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Diot"))
        {
            eatenPellets++;
            score += 10;
            UpdateScoreUI();

            moveAudio.Stop();

            moveAudio.PlayOneShot(diamondSound);

            StartCoroutine(PlayMoveAudioAfterDelay(diamondSound.length));

            Destroy(other.gameObject);
        }
        if (other.CompareTag("BSC"))
        {
            score += 100;
            UpdateScoreUI();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("powerpellet"))
        {
            isScared = true;
            score += 100;
            UpdateScoreUI();

            AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource audio in allAudioSources)
            {
                audio.Stop();
            }

            scaredAudioSource.Play();

            StartCoroutine(ScaredCountdown());

            GameObject[] ghosts = GameObject.FindGameObjectsWithTag("GhostTag");
            foreach (GameObject ghost in ghosts)
            {
                Animator ghostAnimator = ghost.GetComponent<Animator>();
                if (ghostAnimator != null)
                {
                    ghostAnimator.SetTrigger("scared");
                }
            }

            Destroy(other.gameObject);
        }

        if (other.CompareTag("GhostTag") && isScared)
        {
            if (!hasGivenScore)
            {
                score += 300;
                UpdateScoreUI();
                hasGivenScore = true;
            }
            Animator ghostAnimator = other.gameObject.GetComponent<Animator>();
            if (ghostAnimator != null)
            {
                ghostAnimator.SetTrigger("death");
                StartCoroutine(ReviveGhostAfterDeath(ghostAnimator));
            }
        }

        if (other.CompareTag("GhostTag") && !isScared && ghostLives > 0)
        {
            DieAndRespawn();
        }


    }

    void DieAndRespawn()
    {
        ghostLives--;
        pacDeathAudio.Play();
        if (ghostLives == 0)
        {
            StartCoroutine(ShowGameOver());
            return;
        }


        UpdateLifeIndicator();

        animator.SetTrigger("death");

        transform.position = respawnPosition.position;

        score = 0;
        UpdateScoreUI();
        elapsedTime = 0f;

    }

    void UpdateLifeIndicator()
    {
        for (int i = 0; i < lifeIndicators.Length; i++)
        {
            if (i < ghostLives)
            {
                lifeIndicators[i].SetActive(true);
            }
            else
            {
                lifeIndicators[i].SetActive(false);
            }
        }
    }
    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    IEnumerator ReviveGhostAfterDeath(Animator ghostAnimator)
    {
        yield return new WaitForSeconds(5f);
        ghostAnimator.ResetTrigger("death");
        ghostAnimator.ResetTrigger("scared");
        ghostAnimator.SetTrigger("back");
        hasGivenScore = false;
    }


    IEnumerator ScaredCountdown()
    {
        float timer = scaredTime;

        countdownText.gameObject.SetActive(true);
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("GhostTag");
        while (timer > 0)
        {
            countdownText.text = Mathf.Ceil(timer).ToString();

            if (timer <= 3 && !isRecovering)
            {
                isRecovering = true;

                foreach (GameObject ghost in ghosts)
                {
                    Animator ghostAnimator = ghost.GetComponent<Animator>();
                    if (ghostAnimator != null)
                    {
                        ghostAnimator.SetTrigger("recover");
                    }
                }
            }
            else
            {
                foreach (GameObject ghost in ghosts)
                {
                    Animator ghostAnimator = ghost.GetComponent<Animator>();
                    if (ghostAnimator != null)
                    {
                        ghostAnimator.ResetTrigger("recover");
                    }
                }
            }
            yield return new WaitForSeconds(1);
            timer -= 1;
        }

        countdownText.text = "";
        countdownText.gameObject.SetActive(false);
        scaredAudioSource.Stop();

        GameObject[] allGhosts = GameObject.FindGameObjectsWithTag("GhostTag");
        foreach (GameObject ghost in allGhosts)
        {
            Animator ghostAnimator = ghost.GetComponent<Animator>();
            if (ghostAnimator != null)
            {
                ghostAnimator.SetTrigger("back");
            }
        }
        isScared = false;
        isRecovering = false;
    }


    IEnumerator PlayMoveAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay + 1f);
        if (moveDirection != Vector2.zero)
        {
            moveAudio.Play();
        }
    }


}