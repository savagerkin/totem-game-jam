using System.Collections;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SniperShoot : MonoBehaviour
{
    private Volume volume;
    private DepthOfField depthOfField;
    private bool isScoped = false;
    private bool canShoot = true;
    [SerializeField] private Camera camera;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private Transform sniperTransform;
    [SerializeField] private Transform scopeTransform;
    [SerializeField] private GunSway gunSway;
    [SerializeField] private TMP_Text velocityText;
    [SerializeField] private AudioClip shootSound;
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        isScoped = false;
        volume = camera.GetComponent<Volume>();
        if (volume.profile.TryGet(out DepthOfField dof))
        {
            depthOfField = dof;
        }
    }

    [SerializeField] private Animator _animator;
    private float velocity = 0.0f;
    [SerializeField] private LayerMask layerMask;
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!isScoped)
            {
                gunSway.enabled = false;
                sniperTransform.localPosition = new Vector3(0, 0, 0);
                sniperTransform.localRotation = Quaternion.identity;
                _animator.SetTrigger("PutUp");
                depthOfField.active = true;
                isScoped = true;
            }
            else
            {
                gunSway.enabled = true;
                depthOfField.active = false;
                _animator.SetTrigger("PutDown");
                isScoped = false;
            }
        }

        velocityText.text = Mathf.RoundToInt(velocity) + " km/h";

        RaycastHit hit;
        if (Physics.Raycast(scopeTransform.position, scopeTransform.forward, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.transform.tag == "Car")
            {
                Car car = hit.transform.GetComponent<Car>();

                if (car != null)
                {
                    velocity = car.velocity;
                    if (Input.GetMouseButtonDown(0) && isScoped && canShoot) // 0 is the left mouse button
                    {
                        audioSource.PlayOneShot(shootSound);
                        StartCoroutine(ShootWithCooldown());
                        playerCamera.Shake();
                        car.Explode();
                        Debug.Log("Car Exploded");
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && isScoped && canShoot) // 0 is the left mouse button
                {
                    audioSource.PlayOneShot(shootSound);
                    StartCoroutine(ShootWithCooldown());
                    playerCamera.Shake();
                }

                velocity = 0;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && isScoped && canShoot) // 0 is the left mouse button
            {
                audioSource.PlayOneShot(shootSound);
                StartCoroutine(ShootWithCooldown());
                playerCamera.Shake();
            }
        }
    }

    private IEnumerator ShootWithCooldown()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }
}