using UnityEngine;

public class PingPongMover : MonoBehaviour
{
    public Vector3 puntoA;
    public Vector3 puntoB;
    public float velocidad = 3f;
    public float distanciaCambio = 0.2f;

    Rigidbody rb;
    Vector3 objetivo;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        objetivo = puntoB;
    }

    void FixedUpdate()
    {
        Vector3 dir = (objetivo - transform.position);
        if (dir.magnitude < distanciaCambio)
            objetivo = (objetivo == puntoA) ? puntoB : puntoA;

        Vector3 paso = dir.normalized * velocidad * Time.fixedDeltaTime;
        rb.MovePosition(transform.position + paso);

        if (paso.sqrMagnitude > 0.0001f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.1f);
    }
}
