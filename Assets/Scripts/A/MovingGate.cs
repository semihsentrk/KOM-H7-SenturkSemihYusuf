using UnityEngine;

public class MovingGate : MonoBehaviour
{
    public float amplitude = 1.6f;
    public float speed = 1.4f;
    public float speedIncrease = 0.025f;
    public float maximumSpeed = 3.5f;

    Vector3 origin;
    float phase;

    void Start()
    {
        origin = transform.position;
    }

    void FixedUpdate()
    {
        if (GameSession.Current.Finished) return;
        float currentSpeed = Mathf.Min(maximumSpeed, speed + GameSession.Current.Elapsed * speedIncrease);
        phase += currentSpeed * Time.fixedDeltaTime;
        transform.position = origin + Vector3.up * Mathf.Sin(phase) * amplitude;
    }
}
