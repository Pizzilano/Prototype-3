using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;
    public float floatForce;
    private float gravityModifier = 0.5f;
    public bool isLowEnough;
    private Rigidbody playerRb;
    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;
    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;
    public AudioClip hitGroundSound;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        // modify the gravity in game
        Physics.gravity *= gravityModifier;

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);

    }

    // Update is called once per frame
    void Update()
    {
        // check if player is low enough to apply an upward force
        if (transform.position.y <= 10.5f)
        {
            isLowEnough = true;
        }
        // check if player is already highly placed, then stop applying upward force
        else if (transform.position.y >= 15f)
        {
            isLowEnough = false;
            playerRb.velocity = Vector3.zero;
        }
        // While space is pressed and player is low enough, float up
        if (Input.GetKey(KeyCode.Space) && !gameOver && isLowEnough)
        {
            playerRb.AddForce(Vector3.up * floatForce, ForceMode.Impulse);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }
        // if player collides with ground, make sound and apply force to player
        else if (other.gameObject.CompareTag("Ground"))
        {
            playerRb.AddForce(Vector3.up * 3, ForceMode.Impulse);
            playerAudio.PlayOneShot(hitGroundSound, 1.5f);
        }

    }

}
