using System;
using System.Collections;
using KinematicCharacterController;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeathAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private KinematicCharacterMotor motor;
    [SerializeField] private Rigidbody rbGun;
    [SerializeField] private float explosionForce = 10f;
    private ScopingSniper scopingSniper;
    private SniperShoot sniperShoot;
    [SerializeField] private Animator animatorRootSniper;

    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private AudioClip deathSound;
    private AudioSource audioSource;
    [SerializeField] private Player _player;
    
    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        scopingSniper = gameObject.GetComponent<ScopingSniper>();
        sniperShoot = gameObject.gameObject.GetComponent<SniperShoot>();
    }

    private float toSeconds = 42;
    private float fromSeconds = 40;
    public IEnumerator die()
    {
        motor.enabled = false;
        audioSource.time = fromSeconds;
        audioSource.PlayOneShot(deathSound);
        
        if (toSeconds > 0 && toSeconds > fromSeconds)
            audioSource.SetScheduledEndTime(AudioSettings.dspTime + (toSeconds - fromSeconds));
    
        playerCamera.AdjustableShake(5f, 0.1f, 1f);
        yield return new WaitForSeconds(5);
        playerCamera.enabled = false;
        rbGun.transform.parent = null;
        scopingSniper.enabled = false;
        sniperShoot.enabled = false;
        animatorRootSniper.enabled = false;
        Explode();
        yield return new WaitForSeconds(2);
        animator.SetTrigger("Death");
    }

    private void Explode()
    {
        boxCollider.enabled = true;
        rbGun.isKinematic = false;
        float angle = Random.Range(-20, 20);
        float rotation = Random.Range(0, 360);
        Vector3 direction =
            (Quaternion.AngleAxis(rotation, Vector3.up) * Quaternion.AngleAxis(angle, Vector3.forward)) * Vector3.up;

        // Debug.DrawLine(transform.position, transform.position + direction * 3);

        rbGun.AddForce(direction * explosionForce, ForceMode.Impulse);
    }
}