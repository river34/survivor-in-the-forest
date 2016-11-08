using UnityEngine;
using System.Collections;

public class CreditsPanelController : MonoBehaviour {

    private SceneController _scene_controller;

    private float speed = 20f;

    void Start ()
    {
        _scene_controller = GameObject.Find("Root").GetComponent<SceneController>();
        StartCoroutine(WaitForIt());
    }

    IEnumerator WaitForIt()
    {
        yield return new WaitForSeconds(10f);
        GoToNextScene();
    }

    void Update () {
        transform.position += new Vector3 (0, speed * Time.deltaTime, 0);
    }

    public void GoToNextScene()
    {
        _scene_controller.GoToNextScene();
    }
}
