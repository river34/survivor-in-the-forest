using UnityEngine;
using System.Collections;

public class Fading : MonoBehaviour {

    private Texture2D fadeOutTexture;
    private float fadeSpeed = 0.6f;
    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private float fadeDir = -1;     // direction to fade: in = -1, out = 1

    void OnGUI ()
    {
        fadeOutTexture = Resources.Load("Textures/black") as Texture2D;

        alpha += fadeDir * fadeSpeed * Time.deltaTime;

        // force the number between 0 and 1
        alpha = Mathf.Clamp01(alpha);

        // set colot of GUI
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    void OnLevelWasLoaded()
    {
        BeginFade(-1);
    }

    public float BeginFade (int direction)
    {
        fadeDir = direction;
        return fadeSpeed;
    }
}
