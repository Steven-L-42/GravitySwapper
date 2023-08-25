using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

public class P_Collide : MonoBehaviour
{
    private Scoring scoring;
    public ShakeData myShake;
    public Animator animator;
    private AudioSource audioSource;
    public AudioClip[] meows;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); 
        scoring = FindObjectOfType<Scoring>();
    }

    private bool hasCollided = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasCollided && collision.gameObject.CompareTag("Spikes"))
        {
            Debug.Log("SPIKE");
            hasCollided = true; // Mark as collided to prevent further calls
            Destroy(gameObject);
            scoring.gameOver();
        }
    }
    public float moewSound = 0.5f;
    int seed;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            //seed = (int)System.DateTime.Now.Ticks;
            //Random.InitState(seed);
            //audioSource.Stop();
            //audioSource.PlayOneShot(meows[Random.Range(0,3)], moewSound);
            CameraShakerHandler.Shake(myShake);

        }
    }

}
