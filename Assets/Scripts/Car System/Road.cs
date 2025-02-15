using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;



public class Road : MonoBehaviour
{
    public GameObject carPrefab;
    public Transform spawnPosition;

    public Vector3 laneDirection;
    public float spawnDelay;

    [Header("Collision Avoidance")]
    public bool enableCollisionAvoidance;
    public List<float> checkedTimes;
    public float deceleration;

    private List<Car> cars;

    private void Start()
    {
        cars = new List<Car>();
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true) {
            laneDirection = transform.rotation * Vector3.forward;

            Car spawnedCar = Instantiate(carPrefab, spawnPosition.position, spawnPosition.rotation).GetComponent<Car>();
            spawnedCar.direction = laneDirection;
            spawnedCar.velocity = Random.Range(1, 10);
            spawnedCar.acceleration = Random.Range(1, 10);

            cars.Add(spawnedCar);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void FixedUpdate()
    {
        if (enableCollisionAvoidance) {
            CollisionAvoidance();
        }
    }

    private void CollisionAvoidance() {
        foreach (float checkedTime in checkedTimes)
        {
            List<Vector3> futurePositions = new List<Vector3>();

            foreach (Car car in cars)
            {
                Vector3 futurePosition = car.transform.position + car.direction * (car.velocity * checkedTime + (car.acceleration * Mathf.Pow(checkedTime, 2) / 2));
                futurePositions.Add(futurePosition);
            }

            for (int i = 0; i < cars.Count; i++)
            {
                bool front = true;
                bool collided = false;

                for (int j = 0; j < cars.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    if (Vector3.Dot(cars[i].transform.position - cars[j].transform.position, laneDirection) < 0)
                    {
                        front = false;
                    }

                    bool overlapped = Physics.ComputePenetration(
                        cars[i].boxCollider, futurePositions[i], cars[i].transform.rotation,
                        cars[j].boxCollider, futurePositions[j], cars[j].transform.rotation,
                        out _, out _
                    );

                    collided |= overlapped;
                }

                if (!front && collided)
                {
                    StartCoroutine(DecelerationCoroutine(cars[i]));
                }
            }
        }
    }

    public IEnumerator DecelerationCoroutine(Car car) {
        car.acceleration = deceleration;
        yield return new WaitForSeconds(0.5f);
        car.acceleration = Random.Range(3, 6);
    }

    public void CarDespawned(Car car) {
        cars.Remove(car);
    }
}
