using UnityEngine;

public class CarDespawner : MonoBehaviour
{
    public ScoreSystem scoreSystem;
    public Road road;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Car car = other.GetComponent<Car>();
            scoreSystem.updateScore(car, ScoreAction.NoShot);
            road.CarDespawned(car);
            Destroy(other.gameObject);
        }
    }
}
