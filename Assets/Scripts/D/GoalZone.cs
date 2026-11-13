using UnityEngine;

public class GoalZone : MonoBehaviour
{
    public bool acceptsBall;
    public bool checkpoint;
    public bool requireHeldBall;
    public int requiredScore;
    public Transform respawnPoint;

    bool activated;
    Collider2D waitingPlayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!acceptsBall && other.GetComponent<CourierMovement>() != null) waitingPlayer = other;
        CheckGoal(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other == waitingPlayer) waitingPlayer = null;
    }

    void Update()
    {
        if (waitingPlayer != null) CheckGoal(waitingPlayer);
    }

    void CheckGoal(Collider2D other)
    {
        GameSession session = GameSession.Current;
        if (session.Finished || activated) return;
        EnergyBall ball = other.GetComponent<EnergyBall>();
        CourierMovement player = other.GetComponent<CourierMovement>();
        if (acceptsBall ? ball == null : player == null) return;
        if (session.Score < requiredScore)
        {
            session.ShowMessage("Önce işaretli engelleri kırmalısın.");
            return;
        }
        if (requireHeldBall && !player.launcher.ball.Held)
        {
            session.ShowMessage("Çıkışa küreyi yanında taşıyarak gel.");
            return;
        }
        if (checkpoint)
        {
            activated = true;
            player.SpawnPoint = respawnPoint.position;
            if (session.mode == GameMode.Movement) session.AddScore();
            GetComponent<SpriteRenderer>().color = new Color(0.28f, 0.9f, 0.66f, 0.4f);
            session.ShowMessage("Kontrol noktası kaydedildi.");
            return;
        }
        if (acceptsBall)
        {
            session.AddScore();
            ball.ReturnToHand();
            if (session.mode == GameMode.Collision) session.End(true, "Küre testleri tamamlandı!");
        }
        else
        {
            if (session.mode == GameMode.Movement) session.AddScore();
            session.End(true, "Teslimat tamamlandı!");
        }
    }
}
