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
    [SerializeField] private Camera camera;
    [SerializeField] private Transform sniperTransform;
    [SerializeField] private Transform scopeTransform;
    [SerializeField] private GunSway gunSway;
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

        if (Input.GetMouseButtonDown(0) && isScoped) // 0 is the left mouse button
        {
            RaycastHit hit;
            if (Physics.Raycast(scopeTransform.position, scopeTransform.forward, out hit))
            {
                if (hit.transform.tag == "Car")
                {
                    Car car = hit.transform.GetComponent<Car>();
                    if (car != null)
                    {
                        car.Explode();
                        Debug.Log("Car Exploded");
                    }
                    
                }
            }
        }
    }
}