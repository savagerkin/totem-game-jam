using System.Collections;
using UnityEditor.Rendering.Analytics;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Car : MonoBehaviour
{
    public Rigidbody rb;

    public Vector3 movementDir;
    public float speed;
    public float acceleration;
    public float lifetime;

    public float explosionForce;
    public float deathDelay;

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }

        speed += acceleration * Time.deltaTime;
        Vector3 newPosition = transform.position + movementDir * speed * Time.deltaTime;

        transform.position = newPosition;

        if (Input.GetKeyDown(KeyCode.K)) {
            Explode();
        }
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    public void Explode() {
        rb.isKinematic = false;

        float angle = Random.Range(-20, 20);
        float rotation = Random.Range(0, 360);
        Vector3 direction = (Quaternion.AngleAxis(rotation, Vector3.up) * Quaternion.AngleAxis(angle, Vector3.forward)) * Vector3.up;

        // Debug.DrawLine(transform.position, transform.position + direction * 3);

        rb.AddForce(direction * explosionForce, ForceMode.Impulse);
        StartCoroutine(Death());
    }
}
