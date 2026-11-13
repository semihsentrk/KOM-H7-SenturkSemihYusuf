using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameMode { Timing, Movement, Collision, Combined }

public class GameSession : MonoBehaviour
{
    public static GameSession Current;
    public GameMode mode;
    public int targetScore = 6;
    public int lives = 3;
    public float timeLimit = 90f;

    public int Score { get; private set; }
    public float Elapsed { get; private set; }
    public bool Finished { get; private set; }
    public bool Won { get; private set; }
    public string Message { get; private set; }
    public float MessageUntil { get; private set; }
    public float Difficulty => 1f + Elapsed / 45f;

    void Awake()
    {
        Current = this;
        Physics2D.IgnoreLayerCollision(8, 9, true);
        Message = string.Empty;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) ||
            (Finished && mode == GameMode.Timing && Input.GetKeyDown(KeyCode.Space)))
            Restart();

        if (Finished) return;
        Elapsed = Mathf.Min(timeLimit, Elapsed + Time.deltaTime);
        if (Elapsed >= timeLimit) End(false, "Süre doldu.");
    }

    public void AddScore()
    {
        if (Finished) return;
        Score++;
        if (mode == GameMode.Timing && Score >= targetScore)
            End(true, "Tüm atışlar hedefe ulaştı!");
    }

    public void LoseLife(string reason)
    {
        if (Finished) return;
        lives--;
        ShowMessage(reason);
        if (lives <= 0) End(false, reason);
    }

    public void ShowMessage(string text)
    {
        Message = text;
        MessageUntil = Time.time + 3f;
    }

    public void End(bool success, string reason)
    {
        if (Finished) return;
        Finished = true;
        Won = success;
        Message = reason;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
