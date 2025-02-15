using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class Road : MonoBehaviour
{
    public GameObject carPrefab;
    public Transform[] spawns;
    public ScoreSystem scoreSystem;
    public int maxCars = 15;

    public float minSpawnDelay;
    public float maxSpawnDelay;

    public float minSpeed;
    public float maxSpeed;

    [Header("Collision Avoidance")]
    public bool enableCollisionAvoidance;
    public List<float> checkedTimes;
    public float deceleration;

    private List<Car> cars;
    private List<Car>[] carsPerLane = new List<Car>[4];

    private void Start()
    {
        cars = new List<Car>();
        carsPerLane[0] = new List<Car>();
        carsPerLane[1] = new List<Car>();
        carsPerLane[2] = new List<Car>();
        carsPerLane[3] = new List<Car>();
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true) {
            if (cars.Count() < maxCars) {
                int lane = UnityEngine.Random.Range(0, 3);

                Transform spawn = spawns[lane];

                Car spawnedCar = Instantiate(carPrefab, spawn.position, spawn.rotation).GetComponent<Car>();
                spawnedCar.direction = spawn.forward;
                spawnedCar.velocity = UnityEngine.Random.Range(minSpeed, maxSpeed);
                spawnedCar.acceleration = 0;
                spawnedCar.scoreSystem = scoreSystem;

                cars.Add(spawnedCar);
            }


            yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }

    private void FixedUpdate()
    {
        if (enableCollisionAvoidance) {
            CollisionAvoidance();
        }
    }

    private void CollisionAvoidance() {
        foreach (List<Car> cars in carsPerLane)
        {
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
                    Car front = cars[i];
                    bool collided = false;

                    for (int j = 0; j < cars.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        if (Vector3.Dot(front.transform.position - cars[j].transform.position, cars[i].direction) < 0)
                        {
                            front = cars[j];
                        }

                        bool overlapped = Physics.ComputePenetration(
                            cars[i].boxCollider, futurePositions[i], cars[i].transform.rotation,
                            cars[j].boxCollider, futurePositions[j], cars[j].transform.rotation,
                            out _, out _
                        );

                        collided |= overlapped;
                    }

                    if (cars[i] != front && collided)
                    {
                        StartCoroutine(DecelerationCoroutine(cars[i], front.velocity));
                    }
                }
            }
        }
    }

    public IEnumerator DecelerationCoroutine(Car car, float newVelocity) {
        car.velocity = 0;
        yield return new WaitForSeconds(0.5f);
        car.velocity = newVelocity;
    }

    public void CarDespawned(Car car) {
        cars.Remove(car);
        carsPerLane[car.lane].Remove(car);
    }
}
