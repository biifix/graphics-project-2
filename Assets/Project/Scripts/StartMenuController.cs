using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "Environment";

    private GUIStyle titleStyle;
    private GUIStyle buttonStyle;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void OnGUI()
    {
        Color previousColor = GUI.color;
        GUI.color = new Color(0.025f, 0.045f, 0.07f, 1f);
        GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = previousColor;

        CreateStyles();

        float titleWidth = Mathf.Min(Screen.width * 0.9f, 900f);
        Rect titleRect = new Rect(
            (Screen.width - titleWidth) * 0.5f,
            Screen.height * 0.28f,
            titleWidth,
            120f
        );
        GUI.Label(titleRect, "UNRESOLVED", titleStyle);

        float buttonWidth = Mathf.Min(Screen.width * 0.35f, 280f);
        Rect buttonRect = new Rect(
            (Screen.width - buttonWidth) * 0.5f,
            Screen.height * 0.58f,
            buttonWidth,
            64f
        );

        if (GUI.Button(buttonRect, "START", buttonStyle))
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }

    private void CreateStyles()
    {
        if (titleStyle == null)
        {
            titleStyle = new GUIStyle(GUI.skin.label);
            titleStyle.alignment = TextAnchor.MiddleCenter;
            titleStyle.fontSize = Mathf.Clamp(Screen.height / 10, 44, 86);
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.normal.textColor = new Color(0.55f, 0.92f, 1f);
        }

        if (buttonStyle == null)
        {
            buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.alignment = TextAnchor.MiddleCenter;
            buttonStyle.fontSize = Mathf.Clamp(Screen.height / 28, 20, 34);
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.hover.textColor = new Color(0.55f, 0.92f, 1f);
        }
    }
}
