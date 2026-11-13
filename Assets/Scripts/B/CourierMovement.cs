using UnityEngine;

public class CourierMovement : MonoBehaviour
{
    public Rigidbody2D body;
    public Transform feet;
    public LayerMask groundLayers;
    public MagnetLauncher launcher;
    public bool manualProfile;
    public float lightSpeed = 7.5f;
    public float heavySpeed = 4.5f;
    public float lightAcceleration = 42f;
    public float heavyAcceleration = 20f;
    public float lightBrake = 55f;
    public float heavyBrake = 22f;
    public float lightJump = 12f;
    public float heavyJump = 9.5f;
    public float coyoteTime = 0.12f;
    public float jumpBuffer = 0.14f;

    public int Facing { get; private set; } = 1;
    public bool Loaded => manualProfile ? heavyProfile : launcher != null && launcher.ball.Held;
    public bool Grounded { get; private set; }
    public Vector3 SpawnPoint { get; set; }

    float horizontal;
    float coyoteCounter;
    float bufferCounter;
    bool heavyProfile;
    bool cutJump;

    void Start()
    {
        SpawnPoint = transform.position;
    }

    void Update()
    {
        if (GameSession.Current.Finished) return;
        horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal != 0) Facing = horizontal > 0 ? 1 : -1;
        if (manualProfile && Input.GetKeyDown(KeyCode.Tab)) heavyProfile = !heavyProfile;
        bufferCounter -= Time.deltaTime;
        if (Input.GetButtonDown("Jump")) bufferCounter = jumpBuffer;
        if (Input.GetButtonUp("Jump")) cutJump = true;
        if (transform.position.y < -8f) Respawn();
    }

    void FixedUpdate()
    {
        if (GameSession.Current.Finished)
        {
            body.linearVelocity = Vector2.zero;
            return;
        }

        Grounded = body.linearVelocity.y <= 0.1f &&
            Physics2D.OverlapBox(feet.position, new Vector2(0.65f, 0.12f), 0f, groundLayers);
        coyoteCounter = Grounded ? coyoteTime : coyoteCounter - Time.fixedDeltaTime;

        float speed = Loaded ? heavySpeed : lightSpeed;
        float acceleration = Loaded ? heavyAcceleration : lightAcceleration;
        float brake = Loaded ? heavyBrake : lightBrake;
        float rate = horizontal == 0f ? brake : acceleration;
        Vector2 velocity = body.linearVelocity;
        velocity.x = Mathf.MoveTowards(velocity.x, horizontal * speed, rate * Time.fixedDeltaTime);

        if (bufferCounter > 0f && coyoteCounter > 0f)
        {
            velocity.y = Loaded ? heavyJump : lightJump;
            bufferCounter = 0f;
            coyoteCounter = 0f;
        }

        if (cutJump && velocity.y > 0f) velocity.y *= 0.5f;
        cutJump = false;
        body.linearVelocity = velocity;
    }

    public void Respawn()
    {
        GameSession.Current.LoseLife("Boşluğa düştün. Kontrol noktasına döndün.");
        transform.position = SpawnPoint;
        body.linearVelocity = Vector2.zero;
        horizontal = 0f;
        coyoteCounter = 0f;
        bufferCounter = 0f;
        if (launcher != null) launcher.ball.ReturnToHand();
    }
}
