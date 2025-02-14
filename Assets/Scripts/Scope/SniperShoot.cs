
using UnityEngine;

public class SniperShoot : MonoBehaviour
{
    [SerializeField] private Transform scopeTransform;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            RaycastHit hit;
            if (Physics.Raycast(scopeTransform.position, scopeTransform.forward, out hit))
            {
                if (hit.transform.tag == "Car")
                {
                    /*Car car = hit.transform.GetComponent<Car>();
                    if (car != null)
                    {
                        car.Explode();
                    }
                    */
                }
            }
        }
    }
}