using UnityEngine;
using System.Collections;

public class SkyboxController : MonoBehaviour {

    void Awake ()
    {
        RenderSettings.skybox.SetFloat("_Blend", 0);
    }

    public void ChangeToDay ()
    {
        StartCoroutine(Change());
    }

    IEnumerator Change()
    {
        for (float f = 0; f < 1; f += 0.0002f)
        {
            RenderSettings.skybox.SetFloat("_Blend", f);
            yield return null;
        }
    }
}
