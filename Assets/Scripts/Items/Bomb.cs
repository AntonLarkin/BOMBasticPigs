using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float timerToExplosion;
    [SerializeField] private ParticleSystem explosionPrefab;
    [SerializeField] private GameObject exposionWave;

    [SerializeField] private Vector2 verticalDirection;
    [SerializeField] private float horizontalStep;
    [SerializeField] private float verticalStep;

    [SerializeField] private LayerMask layerMask;

    private void OnEnable()
    {
        StartCoroutine(OnTimerOut());
    }

    private void Explode()
    {
        GetComponent<SpriteRenderer>().enabled = false;

        PlayExplosionEffect();
        StartCoroutine(CreateExplosions(verticalDirection,verticalStep));
        StartCoroutine(CreateExplosions(Vector2.right,horizontalStep));
        StartCoroutine(CreateExplosions(-verticalDirection, verticalStep));
        StartCoroutine(CreateExplosions(Vector2.left,horizontalStep));
        
        Destroy(gameObject, explosionPrefab.main.duration);
    }

    private void PlayExplosionEffect()
    {
        var exposion = Instantiate(exposionWave, transform.position, Quaternion.identity);
        Destroy(exposion, explosionPrefab.main.duration);
    }

    private IEnumerator OnTimerOut()
    {
        yield return new WaitForSeconds(timerToExplosion);

        Explode();
    }

    private IEnumerator CreateExplosions(Vector2 direction,float distance)
    {
        for (int i = 1; i < 3; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, layerMask);

            if (!hit.collider)
            {
                var exposion = Instantiate(exposionWave, (Vector2)transform.position + (i * direction), Quaternion.identity);
                Destroy(exposion, explosionPrefab.main.duration);
            }
            else
            { 
                break;
            }

            yield return new WaitForSeconds(.05f);
        }
    }
}
