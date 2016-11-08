using UnityEngine;
using System.Collections;

// Allows a controller to pick up Grabbable objects.
// Place this script on an "attach" point of the controller.
//
// Warning:
// This script assumes only one object will ever be in range.
// For many games, you'll need to track multiple objects in range but only grab the closest one.
[RequireComponent(typeof(Collider))]
public class MotionController : MonoBehaviour
{
    // TrackedController ancestor (contains button events)
    private SteamVR_TrackedController _trackedController;

    // Object (if any) in grab range and not currently grabbed
    private GameObject _hovered;

    // Object currently grabbed

    private GameObject _grabbed;

    // private MotionMainController _motionMainController;

    private GameObject _branches;
    private GameObject _grasses;
    private GameObject _tools;

    void Start()
    {
        // TrackedController is on great-grandparent of attach point
        //HAND MODEL HEIRARCHY IS NEW
        //TrackedController is now on grandparent of attach point
        _trackedController = transform.parent.parent.parent.GetComponent<SteamVR_TrackedController>();

        // Register trigger click and unclick callbacks
        _trackedController.TriggerClicked += new ClickedEventHandler(OnTriggerClick);
        _trackedController.TriggerUnclicked += new ClickedEventHandler(OnTriggerUnclick);

        // _motionMainController = transform.parent.parent.parent.parent.GetComponent<MotionMainController>();

        _branches = GameObject.Find("Branches");
        _grasses = GameObject.Find("Grasses");
        _tools = GameObject.Find("Tools");
    }

    void OnTriggerEnter(Collider other)
    {
        _hovered = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _hovered)
        {
            _hovered = null;
        }
    }

    public void OnTriggerClick(object sender, ClickedEventArgs e)
    {
        if (_hovered && _hovered.transform.parent != transform)
        {
            _hovered.transform.parent = transform;
            _grabbed = _hovered;
            _hovered = null;

            Rigidbody body = _grabbed.GetComponent<Rigidbody>();
            Debug.Assert(body != null, "Grabbable lacks a RigidBody");
            body.isKinematic = true;
        }
    }

    public void OnTriggerUnclick (object sender, ClickedEventArgs e)
    {
        if (_grabbed)
        {
            _hovered = _grabbed;
            _grabbed = null;

            if (_hovered.CompareTag("Branch"))
            {
                _hovered.transform.parent = _branches.transform;
            }
            else if (_hovered.CompareTag("Grass"))
            {
                _hovered.transform.parent = _grasses.transform;
            }
            else
            {
                _hovered.transform.parent = _tools.transform;
            }

            Rigidbody body = _hovered.GetComponent<Rigidbody>();
            Debug.Assert(body != null, "Grabbable lacks a RigidBody");
            body.isKinematic = false;
            body.velocity = SteamVR_Controller.Input((int)_trackedController.controllerIndex).velocity;
            body.angularVelocity = SteamVR_Controller.Input((int)_trackedController.controllerIndex).angularVelocity;
        }
    }
}
