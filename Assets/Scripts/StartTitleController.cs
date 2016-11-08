using UnityEngine;
using System.Collections;

public class StartTitleController : MonoBehaviour {

    private SceneController _scene_controller;

    private int _loop_time;

    private int _default_loop_time = 3;

    private Animator _anim;

    private int _loop = 0;

    void Start ()
    {
        _scene_controller = GameObject.Find("Root").GetComponent<SceneController>();
        _anim = GetComponent<Animator>();
        StartCoroutine(WaitForIt());
    }

    IEnumerator WaitForIt ()
    {
        yield return new  WaitForSeconds(2f);
        _anim.SetTrigger("Next");
    }

	public void GoToNextScene ()
    {
        _scene_controller.GoToNextScene();
    }
}
