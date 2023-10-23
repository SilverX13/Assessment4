using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Vector2[] localPathPoints = {
        new Vector2(1.92f, -0.16f),
        new Vector2(1.92f, -0.8f),
        new Vector2(1.44f, -0.8f),
        new Vector2(1.44f, -1.28f),
        new Vector2(1.92f, -1.28f),
        new Vector2(1.92f, -1.76f),
        new Vector2(1.44f, -1.76f),
        new Vector2(1.44f, -2.24f),
        new Vector2(0.96f, -2.24f),
        new Vector2(0.96f, -1.28f),
        new Vector2(0.16f, -1.28f),
        new Vector2(0.16f, -0.16f),
     };


    private int currentPointIndex = 0;
    public float speed = 1f;
    private Animator animator;
    public GameObject manualLevelLayout;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        MovePacStudent();
    }

    void MovePacStudent()
    {
        Vector2 globalTargetPoint = localPathPoints[currentPointIndex] + (Vector2)manualLevelLayout.transform.position;

        if (Vector2.Distance(transform.position, globalTargetPoint) < 0.01f)
        {
            currentPointIndex = (currentPointIndex + 1) % localPathPoints.Length;
        }

        Vector2 direction = (globalTargetPoint - (Vector2)transform.position);
        direction.y = -direction.y;
        direction = direction.normalized;
        transform.position = Vector2.MoveTowards(transform.position, globalTargetPoint, speed * Time.deltaTime);

        UpdateAnimation(direction);
    }

    void UpdateAnimation(Vector2 direction)
    {
        Vector2 globalTargetPoint = localPathPoints[currentPointIndex] + (Vector2)manualLevelLayout.transform.position;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Horizontal movement
            if (globalTargetPoint.x > transform.position.x)
            {
                animator.Play("Pac right");
            }
            else
            {
                animator.Play("Pac left");
            }
        }
        else
        {
            // Vertical movement
            if (globalTargetPoint.y < transform.position.y)
            {
                animator.Play("Pac down");
            }
            else
            {
                animator.Play("Pac up");
            }
        }
    }



}
