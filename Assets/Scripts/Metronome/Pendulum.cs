using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public Transform endPoint;
    public LineRenderer lineRenderer;
    
    public float pendulumLength = 2f;
    public float tempo = 120f;
    public float amplitude = 30f;

    private float time;

    void Start()
    {
        time = 0f;
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        time += Time.deltaTime;
        float period = 60f / tempo;
        float angle = amplitude * Mathf.Sin((time / period) * Mathf.PI * 2);
        transform.rotation = Quaternion.Euler(0, 0, angle);
        Vector3 endPosition = transform.position + (transform.up * pendulumLength);
        endPoint.position = endPosition;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }
}
