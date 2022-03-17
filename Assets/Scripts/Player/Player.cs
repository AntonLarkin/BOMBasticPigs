using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [Header("Sprites")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite turnRightSprite;
    [SerializeField] private Sprite turnLeftSprite;
    [SerializeField] private Sprite turnUpSprite;
    [SerializeField] private Sprite turnDownSprite;

    [Header("Movement")]
    [SerializeField] private Joystick joystick;
    [SerializeField] private float speed;

    private Rigidbody2D rb;

    private void OnEnable()
    {
        gameManager.OnBombDrop += DropBomb;
    }

    private void OnDisable()
    {
        gameManager.OnBombDrop -= DropBomb;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (joystick.Horizontal > 0.5f || joystick.Horizontal < -0.5f)
        {
            MoveHorizontal();
        }
        else if (joystick.Vertical > 0.5f || joystick.Vertical < -0.5f)
        {
            MoveVertical();
        }
    }

    public void CollectSeeds(float value)
    {
        gameManager.AddSeeds(value);
    }

    public void LoseLife()
    {
        if (gameManager.CurrentLifes > 0)
        {
            gameManager.ReduceLife();
            GetComponent<CircleCollider2D>().enabled = false;
            StartCoroutine(OnLoseLifeBeInvisible());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void MoveHorizontal()
    {
        if (joystick.Horizontal < 0)
        {
            spriteRenderer.sprite = turnLeftSprite;
        }
        else
        {
            spriteRenderer.sprite = turnRightSprite;
        }

        Vector2 dir = transform.right * joystick.Horizontal;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.Translate (dir * speed * Time.deltaTime, Space.World);
    }

    private void MoveVertical()
    {
        if (joystick.Vertical < 0)
        {
            spriteRenderer.sprite = turnDownSprite;
        }
        else
        {
            spriteRenderer.sprite = turnUpSprite;
        }

        Vector2 dir = transform.up * joystick.Vertical;
        transform.rotation = Quaternion.Euler(0, 0, -4);
        transform.Translate(dir * speed * Time.deltaTime,Space.Self);
    }

    private void DropBomb()
    {
        gameManager.CreateBomb(transform.position);
    }
   
    private IEnumerator OnLoseLifeBeInvisible()
    {
        yield return new WaitForSeconds(1f);

        GetComponent<CircleCollider2D>().enabled = true;
    }
}
