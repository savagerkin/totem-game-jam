using UnityEngine;

public class CarDespawner : MonoBehaviour
{
    public ScoreSystem scoreSystem;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            Car car = other.GetComponent<Car>();
            scoreSystem.updateScore(car, ScoreAction.NoShot);
            Destroy(other.gameObject);
        }
    }
}
