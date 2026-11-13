using UnityEngine;

public class MagnetLauncher : MonoBehaviour
{
    public EnergyBall ball;
    public Transform holdPoint;
    public Transform aimGuide;
    public CourierMovement movement;
    public KeyCode fireKey = KeyCode.Space;
    public bool adjustableAim;
    public bool automaticReturn = true;
    public float chargeDuration = 1f;
    public float minimumSpeed = 8f;
    public float maximumSpeed = 19f;
    public float angle = 12f;

    public float Charge { get; private set; }
    public bool Charging { get; private set; }
    public Vector2 Direction { get; private set; }

    void Start()
    {
        ball.launcher = this;
        ball.Hold(holdPoint);
    }

    void Update()
    {
        if (GameSession.Current.Finished) return;
        if (adjustableAim)
        {
            float aim = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) ? 1f : 0f)
                - (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) ? 1f : 0f);
            angle = Mathf.Clamp(angle + aim * 65f * Time.deltaTime, -25f, 60f);
        }

        int facing = movement != null ? movement.Facing : 1;
        holdPoint.localPosition = new Vector3(0.85f * facing, 0.25f, 0f);
        float radians = angle * Mathf.Deg2Rad;
        Direction = new Vector2(Mathf.Cos(radians) * facing, Mathf.Sin(radians));
        aimGuide.position = holdPoint.position;
        aimGuide.right = Direction;

        if (!automaticReturn && Input.GetKeyDown(KeyCode.F) && !ball.Held)
        {
            if (Vector2.Distance(ball.transform.position, transform.position) > 2f)
                GameSession.Current.LoseLife("Uzaktan geri çağırma: bir hak harcandı.");
            ball.ReturnToHand();
        }

        if (!ball.Held) return;
        if (Input.GetKeyDown(fireKey))
        {
            Charging = true;
            Charge = 0f;
        }
        if (Charging) Charge = Mathf.Clamp01(Charge + Time.deltaTime / chargeDuration);
        if (Charging && Input.GetKeyUp(fireKey)) Fire();
    }

    public void Fire()
    {
        Charging = false;
        ball.transform.position = holdPoint.position;
        if (movement != null)
        {
            Vector2 origin = (Vector2)transform.position + Vector2.up * 0.25f;
            Vector2 forward = Vector2.right * movement.Facing;
            RaycastHit2D wall = Physics2D.CircleCast(origin, 0.32f, forward, 0.85f, movement.groundLayers);
            if (wall) ball.transform.position = wall.centroid - forward * 0.03f;
        }
        ball.Launch(Direction * Mathf.Lerp(minimumSpeed, maximumSpeed, Charge));
        Charge = 0f;
    }
}
