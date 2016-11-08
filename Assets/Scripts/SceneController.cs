using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

	public void GoToNextScene ()
    {
        if (SceneManager.GetActiveScene().name == "Start")
        {
            StartCoroutine(ChangeScene("Main"));
        }
        else if (SceneManager.GetActiveScene().name == "Dead")
        {
            StartCoroutine(ChangeScene("Main"));
        }
        else if (SceneManager.GetActiveScene().name == "Success")
        {
            StartCoroutine(ChangeScene("Credits"));
        }
        else if (SceneManager.GetActiveScene().name == "Credits")
        {
            StartCoroutine(ChangeScene("Start"));
        }
    }

    public void GoToScene (string name)
    {
        StartCoroutine(ChangeScene(name));
    }

    /*
    void Start ()
    {
        StartCoroutine (WaitAndGo (10));
    }

    IEnumerator WaitAndGo (int seconds)
    {
        yield return new WaitForSeconds(seconds);
        GoToNextScene();
    }
    */

    IEnumerator ChangeScene (string scene)
    {
        float fadeTime = 0f;

        if (SceneManager.GetActiveScene().name == "Main" && scene == "Dead")
        {
            gameObject.GetComponent<MainEnding>().enabled = true;
            fadeTime = 3f;
        }
        else
        {
            fadeTime = gameObject.GetComponent<Fading>().BeginFade(1);
        }

        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(scene);
    }
}
