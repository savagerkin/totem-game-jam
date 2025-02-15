using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Car : MonoBehaviour
{
    public Rigidbody rb;
    public BoxCollider boxCollider;
    public ScoreSystem scoreSystem;

    public Vector3 direction;
    public float velocity;
    public float acceleration;
    public float lifetime;

    public float noShotReward;

    public bool moving = true;

    public float explosionForce;
    public float deathDelay;

    public int lane;

    private bool speeding;
    public float maxVelocity;

    [SerializeField] private AudioClip carSound;

    private void Start()
    {
        speeding = false;
        noShotReward = 0;

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }

        if (moving)
        {
            velocity += acceleration * Time.deltaTime;

            if (maxVelocity < velocity) { 
                velocity = maxVelocity;
            }

            if (speeding)
            {
                noShotReward += (maxVelocity - velocity) * Time.deltaTime;
            }

            if (scoreSystem.speedLimits[lane] < velocity)
            {
                speeding = true;
            }
            else {
                speeding = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            if (velocity <= 0)
            {
                acceleration = Random.Range(3, 6);
            }

            Vector3 newPosition = transform.position + direction * velocity * Time.deltaTime;
            rb.MovePosition(newPosition);
        }
    }

    [SerializeField] private GameObject explosionEffect;
    private AudioSource audioSource;
    [SerializeField] private AudioClip boomSound;


    private IEnumerator Death()
    {
        audioSource.loop = false;
        audioSource.Stop();
        GameObject explosionInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(boomSound);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
        Destroy(explosionInstance, 2f);
    }

    public void Explode()
    {
        rb.isKinematic = false;

        float angle = Random.Range(-20, 20);
        float rotation = Random.Range(0, 360);
        Vector3 direction =
            (Quaternion.AngleAxis(rotation, Vector3.up) * Quaternion.AngleAxis(angle, Vector3.forward)) * Vector3.up;

        // Debug.DrawLine(transform.position, transform.position + direction * 3);

        rb.freezeRotation = false;
        rb.AddForce(direction * explosionForce, ForceMode.Impulse);
        StartCoroutine(Death());
    }
}