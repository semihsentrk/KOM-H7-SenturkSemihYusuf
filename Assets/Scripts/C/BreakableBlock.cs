using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    public int hitsRequired = 1;
    public float minimumImpact = 8f;
    public float difficultyIncrease = 0.01f;
    public float maximumImpact = 17f;
    public SpriteRenderer visual;
    public Color damagedColor = new Color(1f, 0.75f, 0.25f);
    public bool Broken { get; private set; }
    public float RequiredImpact => GameSession.Current.mode == GameMode.Combined
        ? Mathf.Min(maximumImpact, minimumImpact + GameSession.Current.Elapsed * difficultyIncrease)
        : minimumImpact;

    public void Hit(float speed)
    {
        if (Broken) return;
        if (speed < RequiredImpact)
        {
            GameSession.Current.ShowMessage("Atış zayıf. Daha uzun basılı tut.");
            return;
        }

        hitsRequired--;
        if (hitsRequired > 0)
        {
            visual.color = damagedColor;
            GameSession.Current.ShowMessage("Zırh çatladı. Bir güçlü darbe daha gerekiyor.");
            return;
        }

        Broken = true;
        GameSession.Current.AddScore();
        gameObject.SetActive(false);
    }
}
