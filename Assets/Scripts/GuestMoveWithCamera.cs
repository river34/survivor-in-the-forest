using UnityEngine;
using System.Collections;

public class GuestMoveWithCamera : MonoBehaviour {

    private GameObject camera;
	
    void Start ()
    {
        camera = GameObject.Find("Camera (eye)");
    }

	void Update () {
        transform.position = camera.transform.position;
        transform.rotation = camera.transform.rotation;
    }
}
