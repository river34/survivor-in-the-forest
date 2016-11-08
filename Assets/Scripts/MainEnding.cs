using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class MainEnding : MonoBehaviour
{

    private Texture2D fadeOutTexture;
    private float fadeSpeed = 0.8f;
    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private float fadeDir = -1;     // direction to fade: in = -1, out = 1
    private bool stage_1 = false;
    private bool stage_2 = false;
    private bool stage_3 = false;
    private bool stage_4 = false;
    private GameObject camera;

    void OnGUI()
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
        //
    }

    void Start()
    {
        camera = GameObject.Find("Camera (eye)");

        float fadeTime = BeginFade(1);
        StartCoroutine(WaitForIt(fadeTime, 1));
        fadeTime = BeginFade(-1);
        StartCoroutine(WaitForIt(fadeTime, 5));
        camera.GetComponent<Blur>().enabled = false;
    }

    void Update ()
    {
        // Close eyes
        if (stage_1)
        {
            float fadeTime = BeginFade(-1);
            StartCoroutine(WaitForIt(fadeTime, 2));
            stage_1 = false;
        }
        // Open eyes
        if (stage_2)
        {
            float fadeTime = BeginFade(1);
            StartCoroutine(WaitForIt(fadeTime, 3));
            camera.GetComponent<Blur>().iterations = 1;
            camera.GetComponent<ColorCorrectionCurves>().enabled = false;
            stage_2 = false;
        }
        // Close eyes
        if (stage_3)
        {
            float fadeTime = BeginFade(-1);
            StartCoroutine(WaitForIt(fadeTime, 4));
            stage_3 = false;
        }
        // Open eyes
        if (stage_4)
        {
            float fadeTime = BeginFade(1);
            StartCoroutine(WaitForIt(fadeTime, 5));

            camera.GetComponent<Blur>().enabled = true;
            camera.GetComponent<Blur>().iterations = 3;
            camera.GetComponent<ColorCorrectionCurves>().enabled = true;
            stage_4 = false;
        }
    }

    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return fadeSpeed;
    }

    IEnumerator WaitForIt(float fadeTime, int stage)
    {
        yield return new WaitForSeconds(fadeTime);
        if (stage == 1)
        {
            stage_1 = true;
        }
        else if (stage == 2)
        {
            stage_2 = true;
        }
        else if (stage == 3)
        {
            stage_3 = true;
        }
        else if (stage == 4)
        {
            stage_4 = true;
        }
    }
}
