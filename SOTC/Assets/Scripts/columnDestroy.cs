using Unity.VisualScripting;
using UnityEngine;

public class ColumnDestroy : MonoBehaviour
{
    public int forceStrength = 10;
    public float randomnessThreshold = 0.2f;

    void Start()
    {
        foreach (Transform child in transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("boss"))
        {
            foreach (Transform child in transform)
            {
                Rigidbody rb = child.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    Vector3 collisionDirection = (child.position - other.transform.position).normalized;
                    float randomness = Random.Range(-randomnessThreshold, randomnessThreshold);
                    Vector3 randomOffset = new Vector3(
                        randomness,
                        randomness,
                        randomness
                    );
                    Vector3 finalForce = (collisionDirection + randomOffset) * forceStrength;
                    rb.AddForce(finalForce, ForceMode.Impulse);
                }
            }

            Destroy(GetComponent<Collider>());
            Destroy(other.gameObject.GetComponent<MoveObject>());
        }
    }
}