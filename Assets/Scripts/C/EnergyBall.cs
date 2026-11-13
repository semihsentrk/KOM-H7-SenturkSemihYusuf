using UnityEngine;

public class EnergyBall : MonoBehaviour
{
    public Rigidbody2D body;
    public MagnetLauncher launcher;
    public TrailRenderer trail;
    public float bounce = 0.85f;
    public float returnDelay = 3.5f;
    public bool Held { get; private set; }
    public float LastImpact { get; private set; }

    Transform hand;
    Vector2 incomingVelocity;
    float flightTime;

    void LateUpdate()
    {
        if (Held && hand != null) transform.position = hand.position;
    }

    void FixedUpdate()
    {
        incomingVelocity = body.linearVelocity;
    }

    void Update()
    {
        if (Held || GameSession.Current.Finished) return;
        flightTime += Time.deltaTime;
        bool outside = transform.position.y < -8f || Mathf.Abs(transform.position.x) > 120f;
        if (outside || (launcher.automaticReturn && flightTime >= returnDelay))
        {
            if (GameSession.Current.mode == GameMode.Timing || outside)
                GameSession.Current.LoseLife("Küre hedefe ulaşmadı.");
            ReturnToHand();
        }
    }

    public void Hold(Transform target)
    {
        hand = target;
        Held = true;
        body.linearVelocity = Vector2.zero;
        body.angularVelocity = 0f;
        body.simulated = false;
        transform.position = hand.position;
        trail.emitting = false;
        trail.Clear();
    }

    public void ReturnToHand()
    {
        Hold(launcher.holdPoint);
    }

    public void Launch(Vector2 velocity)
    {
        Held = false;
        flightTime = 0f;
        body.position = transform.position;
        body.simulated = true;
        body.linearVelocity = velocity;
        incomingVelocity = velocity;
        trail.Clear();
        trail.emitting = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (Held || GameSession.Current.Finished || collision.contactCount == 0) return;
        LastImpact = incomingVelocity.magnitude;
        BreakableBlock block = collision.collider.GetComponent<BreakableBlock>();
        if (block != null) block.Hit(LastImpact);
        Vector2 normal = collision.GetContact(0).normal;
        body.linearVelocity = Vector2.Reflect(incomingVelocity, normal) * bounce;
    }
}
