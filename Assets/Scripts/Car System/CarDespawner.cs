using UnityEngine;

public class CarDespawner : MonoBehaviour
{
    public Road road;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            road.CarDespawned(other.GetComponent<Car>());
            Destroy(other.gameObject);
        }
    }
}
