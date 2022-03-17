using UnityEngine;

public class Seeds : MonoBehaviour
{
    [SerializeField] private float value;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<Player>();

        if (player != null)
        {
            player.CollectSeeds(value);
        }

        Destroy(gameObject);
    }
}
