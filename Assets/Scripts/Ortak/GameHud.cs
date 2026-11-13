using UnityEngine;

public class GameHud : MonoBehaviour
{
    public MagnetLauncher launcher;
    public CourierMovement movement;
    public float finishX = 60f;

    GUIStyle title;
    GUIStyle text;
    GUIStyle button;
    GUIStyle right;
    GUIStyle centered;
    readonly Color ink = new Color(0.035f, 0.065f, 0.10f, 0.96f);
    readonly Color cyan = new Color(0.25f, 0.91f, 0.92f);
    readonly Color orange = new Color(1f, 0.69f, 0.27f);

    void PrepareStyles()
    {
        title = new GUIStyle(GUI.skin.label) { fontSize = 28, fontStyle = FontStyle.Bold };
        text = new GUIStyle(GUI.skin.label) { fontSize = 20, wordWrap = true };
        right = new GUIStyle(text) { alignment = TextAnchor.MiddleRight };
        centered = new GUIStyle(text) { alignment = TextAnchor.MiddleCenter };
        button = new GUIStyle(GUI.skin.button) { fontSize = 18 };
        title.normal.textColor = Color.white;
        text.normal.textColor = Color.white;
        right.normal.textColor = Color.white;
        centered.normal.textColor = Color.white;
    }

    void Panel(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, Texture2D.whiteTexture);
        GUI.color = Color.white;
    }

    void OnGUI()
    {
        if (title == null) PrepareStyles();
        Matrix4x4 previous = GUI.matrix;
        GUI.matrix = Matrix4x4.Scale(new Vector3(Screen.width / 1280f, Screen.height / 720f, 1f));
        GameSession session = GameSession.Current;

        GUI.Label(new Rect(24, 20, 350, 34), "Hak: " + session.lives + "     Süre: " + Mathf.CeilToInt(session.timeLimit - session.Elapsed), text);

        string progress = "Hedef: " + session.Score + " / " + session.targetScore;
        if (session.mode == GameMode.Movement)
            progress = "Yol: %" + Mathf.RoundToInt(Mathf.Clamp01(movement.transform.position.x / finishX) * 100f);
        if (session.mode == GameMode.Collision)
            progress = "Kırılan blok: " + Mathf.Min(session.Score, session.targetScore - 1) + " / " + (session.targetScore - 1);
        if (session.mode == GameMode.Combined)
            progress = "Açılan kapı: " + session.Score + " / " + session.targetScore;
        GUI.Label(new Rect(930, 20, 326, 34), progress, right);

        if (launcher != null)
        {
            if (launcher.adjustableAim)
                GUI.Label(new Rect(24, 612, 220, 32), "Açı: " + Mathf.RoundToInt(launcher.angle) + "°", text);
            if (launcher.Charging)
            {
                GUI.Label(new Rect(24, 649, 220, 30), "Güç: %" + Mathf.RoundToInt(launcher.Charge * 100), text);
                Panel(new Rect(24, 689, 180, 6), new Color(0.14f, 0.23f, 0.29f));
                Panel(new Rect(24, 689, 180 * launcher.Charge, 6), orange);
            }
        }

        if (movement != null)
            GUI.Label(new Rect(1036, 656, 220, 34), movement.Loaded ? "Yüklü" : "Hafif", right);
        if (!session.Finished && Time.time < session.MessageUntil)
            GUI.Label(new Rect(320, 650, 640, 48), session.Message, centered);

        if (session.Finished)
        {
            Panel(new Rect(0, 0, 1280, 720), new Color(0.01f, 0.025f, 0.04f, 0.65f));
            Panel(new Rect(380, 229, 520, 280), ink);
            Panel(new Rect(380, 229, 520, 5), session.Won ? cyan : orange);
            GUI.Label(new Rect(415, 260, 460, 45), session.Won ? "Görev tamamlandı" : "Oyun bitti", title);
            GUI.Label(new Rect(415, 323, 450, 65), session.Message, text);
            if (GUI.Button(new Rect(415, 421, 450, 50), "Yeniden başlat", button)) session.Restart();
        }
        GUI.matrix = previous;
    }
}
