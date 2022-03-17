using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float dirtyTimeOut;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite turnRightSprite;
    [SerializeField] private Sprite turnLeftSprite;
    [SerializeField] private Sprite turnUpSprite;
    [SerializeField] private Sprite turnDownSprite;
    [SerializeField] private Sprite turnRightSpriteAngry;
    [SerializeField] private Sprite turnLeftSpriteAngry;
    [SerializeField] private Sprite turnUpSpriteAngry;
    [SerializeField] private Sprite turnDownSpriteAngry;
    [SerializeField] private Sprite dirtyRightSprite;
    [SerializeField] private Sprite dirtyLeftSprite;
    [SerializeField] private Sprite dirtyUpSprite;
    [SerializeField] private Sprite dirtyDownSprite;

    [Header("Movement")]
    [SerializeField] private Vector2 verticalDirection;
    [SerializeField] private float horizontalStep;
    [SerializeField] private float verticalStep;
    [SerializeField] private int minNumberOfSteps;
    [SerializeField] private int maxNumberOfSteps;
    private Vector2[] possibleDirections = new Vector2[4];

    [Header("Follow Player")]
    [SerializeField] private LayerMask layerMaskFollow;
    [SerializeField] private float vision;

    [SerializeField] private LayerMask layerMask;

    bool isReadyToMove;
    bool isFollowingPlayer;
    bool isDirty;

    private Vector2 lastSeenPosition;
    private Vector2 currentPosition;
    private int currentDirectionIndex;
    private float currentDistance;
    private int numberOfSteps;

    private void Start()
    {
        DefineDirections();
        ChangeDirection();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().LoseLife();
        }
    }

    private void Update()
    {
        if (!isDirty)
        {
            if (isFollowingPlayer)
            {
                FollowPlayer(lastSeenPosition);
                return;
            }

            if (isReadyToMove)
            {
                Move();
            }

            CheckPlayer();
        }
    }

    public void BecomeDirty()
    {
        isDirty = true;
        switch (currentDirectionIndex)
        {
            case 0:
                currentDistance = horizontalStep * numberOfSteps;
                spriteRenderer.sprite = dirtyLeftSprite;
                break;
            case 1:
                currentDistance = horizontalStep * numberOfSteps;
                spriteRenderer.sprite = dirtyRightSprite;
                break;
            case 2:
                currentDistance = verticalStep * numberOfSteps;
                spriteRenderer.sprite = dirtyUpSprite;
                break;
            case 3:
                currentDistance = verticalStep * numberOfSteps;
                spriteRenderer.sprite = dirtyDownSprite;
                break;
        }

        StartCoroutine(OnBeingDirty());
    }

    private void DefineDirections()
    {
        possibleDirections[0] = Vector2.left;
        possibleDirections[1] = Vector2.right;
        possibleDirections[2] = verticalDirection;
        possibleDirections[3] = -verticalDirection;
    }

    private void Move()
    {
        Vector2 endPosition = possibleDirections[currentDirectionIndex] * currentDistance;
        transform.position = Vector2.MoveTowards(transform.position, currentPosition + endPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, currentPosition + endPosition) <= 0.01f)
        {
            isReadyToMove = false;
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        bool isDirectionChanged = false;

        while (!isDirectionChanged)
        {
            currentDirectionIndex = Random.Range(0, possibleDirections.Length);
            numberOfSteps = Random.Range(minNumberOfSteps, maxNumberOfSteps);

            switch (currentDirectionIndex)
            {
                case 0:
                    currentDistance = horizontalStep * numberOfSteps;
                    spriteRenderer.sprite = turnLeftSprite;
                    break;
                case 1:
                    currentDistance = horizontalStep * numberOfSteps;
                    spriteRenderer.sprite = turnRightSprite;
                    break;
                case 2:
                    currentDistance = verticalStep * numberOfSteps;
                    spriteRenderer.sprite = turnUpSprite;
                    break;
                case 3:
                    currentDistance = verticalStep * numberOfSteps;
                    spriteRenderer.sprite = turnDownSprite;
                    break;
            }

            currentPosition = transform.position;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, possibleDirections[currentDirectionIndex], currentDistance, layerMask);

            if (!hit.collider)
            {
                isReadyToMove = true;
                isDirectionChanged = true;
            }
            else
            {
                isReadyToMove = false;
            }
        }
    }

    private void CheckPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, possibleDirections[currentDirectionIndex], vision, layerMaskFollow);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Pumpkin"))
            {
                return;
            }

            if (hit.collider)
            {
                Debug.DrawRay(transform.position, possibleDirections[currentDirectionIndex] * vision, Color.black, 2f);
                switch (currentDirectionIndex)
                {
                    case 0:
                        currentDistance = horizontalStep * vision;
                        spriteRenderer.sprite = turnLeftSpriteAngry;
                        break;
                    case 1:
                        currentDistance = horizontalStep * vision;
                        spriteRenderer.sprite = turnRightSpriteAngry;
                        break;
                    case 2:
                        currentDistance = verticalStep * vision;
                        spriteRenderer.sprite = turnUpSpriteAngry;
                        break;
                    case 3:
                        currentDistance = verticalStep * vision;
                        spriteRenderer.sprite = turnDownSpriteAngry;
                        break;
                }

                lastSeenPosition = gameManager.AttachItemToAGrid(hit.transform.position);

                isFollowingPlayer = true;
            }
        }
    }

    private void FollowPlayer(Vector2 playersPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, playersPosition, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, playersPosition) <= 0.01f)
        {
            isFollowingPlayer = false;
            ChangeDirection();
        }
    }

    private IEnumerator OnBeingDirty()
    {
        yield return new WaitForSeconds(dirtyTimeOut);

        isDirty = false;
    }
}
