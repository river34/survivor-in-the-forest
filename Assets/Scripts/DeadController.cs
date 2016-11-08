using UnityEngine;
using System.Collections;

public class DeadController : MonoBehaviour {

    void Start()
    {
        StartCoroutine (WaitAndGoToNextScene (5f));
    }

    IEnumerator WaitAndGoToNextScene (float seconds)
    {
        yield return new WaitForSeconds (seconds);
        gameObject.GetComponent<SceneController>().GoToNextScene();
    }
}
