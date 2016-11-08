using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class CreditsOpening : MonoBehaviour
{

    private Texture2D fadeOutTexture;
    private float fadeSpeed = 0.8f;
    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private float fadeDir = -1;     // direction to fade: in = -1, out = 1
    private bool stage_1 = false;
    private bool stage_2 = false;
    private bool stage_3 = true;
    private bool stage_4 = false;
    private bool stage_5 = false;
    private bool stage_6 = false;
    public GameObject camera;
    private Transform camera_transform;

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
        float fadeTime = BeginFade(-1);
        StartCoroutine(WaitForIt(fadeTime, 1));
        camera.GetComponent<Blur>().enabled = true;
        camera.GetComponent<Blur>().iterations = 3;
        camera.GetComponent<ColorCorrectionCurves>().enabled = true;

        // RenderSettings.fogColor = new Color(0, 0, 0, 100);
    }

    void Update()
    {
        if (stage_3)
        {
            StartCoroutine(WaitForIt(0, 4));
            stage_3 = false;
        }
        if (stage_4)
        {
            StartCoroutine(WaitForIt(2, 5));
            stage_4 = false;
        }
    }

    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return fadeSpeed;
    }

    IEnumerator WaitForIt (float fadeTime, int stage, bool rotation = false)
    {
        if (rotation)
        {
            float duration = fadeTime;
            float elapsed = 0f;
            float spinSpeed = 22.5f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                camera.transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            yield return new WaitForSeconds(fadeTime);
        }

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
        else if (stage == 5)
        {
            stage_5 = true;
        }
        else if (stage == 6)
        {
            stage_6 = true;
        }
    }
}
