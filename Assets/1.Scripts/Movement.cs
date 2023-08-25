using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody2D rgPlayer;
    Transform trPlayer;
    public float flipSpeed = 10f;
    public float rescueSpeed = 10f;
    Scoring scoring;
    public bool isSwapHolding = false;
    public bool isSwapOnce = false;
    public Animator animator;
    private AudioSource audioSource;
    public AudioClip woosh1;
    public AudioClip woosh2;
  
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        scoring = FindObjectOfType<Scoring>();
        trPlayer = transform.GetChild(0).GetComponent<Transform>();
        rgPlayer = GetComponent<Rigidbody2D>();
        StartCoroutine(GravitySwap());
        StartCoroutine(GravityHold());
        StartCoroutine(FlipSprite());
        StartCoroutine(BackToOrigin());
    }

    IEnumerator BackToOrigin()
    {
       
        while (true)
        {
            if (rgPlayer.position.x > -5f)
                rgPlayer.position = Vector2.MoveTowards(rgPlayer.position, new Vector2(-5, rgPlayer.position.y), rescueSpeed * Time.deltaTime);
            else
            if (rgPlayer.position.x < -11f)
            {
              
                animator.SetTrigger("dead");
                scoring.gameOver();
                Destroy(gameObject);
                break;
            }
            
            if ((isSwapHolding && rgPlayer.position.x >= -11f && rgPlayer.position.x < -5f) || (Input.GetKeyDown(KeyCode.Space) && rgPlayer.position.x >= -11f && rgPlayer.position.x < -5f))
            {

                while (rgPlayer.position.x < -5)
                {
                    if (rgPlayer.position.x < -11f)
                    {
                      
                        animator.SetTrigger("dead");
                        scoring.gameOver();
                        Destroy(gameObject);
                    }

                    rgPlayer.position = Vector2.MoveTowards(rgPlayer.position, new Vector2(-5, rgPlayer.position.y), rescueSpeed * Time.deltaTime);
                    yield return null;
                }
                rgPlayer.position = new Vector2(-5, rgPlayer.position.y);
            }
            yield return null;
        }
    }

    IEnumerator GravityHold()
    {
        while (true)
        {
            float posY = rgPlayer.position.y;

            while (Input.GetKey(KeyCode.LeftShift))
            {
                rgPlayer.velocity = Vector3.zero;
                rgPlayer.position = new Vector2(rgPlayer.position.x, posY);
                yield return null;
            }

            yield return null;
        }
    }

    IEnumerator GravitySwap()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Space) || isSwapOnce)
            {
               
                isSwapOnce = false;
                rgPlayer.velocity = Vector3.zero;
                audioSource.PlayOneShot(rgPlayer.gravityScale > 0 ? woosh1 : woosh2, 1f);
                rgPlayer.gravityScale = -rgPlayer.gravityScale;
            }
            yield return null;
        }
    }

    IEnumerator FlipSprite()
    {
        while (true)
        {
            if (rgPlayer.gravityScale < 0)
                trPlayer.rotation = Quaternion.Lerp(trPlayer.rotation, Quaternion.Euler(180, 0, 0), flipSpeed * Time.deltaTime);
            else
                trPlayer.rotation = Quaternion.Lerp(trPlayer.rotation, Quaternion.Euler(0, 0, 0), flipSpeed * Time.deltaTime); 
            yield return null;
        }
    }
}
