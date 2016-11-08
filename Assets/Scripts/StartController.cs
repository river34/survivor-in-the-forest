using UnityEngine;
using System.Collections;

public class StartController : MonoBehaviour {

    void Start()
    {
        // StartCoroutine(WaitAndGoToNextScene(3f));
    }

    IEnumerator WaitAndGoToNextScene(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.GetComponent<SceneController>().GoToNextScene();
    }
}
