using UnityEngine;
using System.Collections;

public class MotionMainController : MonoBehaviour {

    private bool _holded_left;

    private bool _holded_right;

    private GameObject _hold_point;

    private Vector3 _position;

    private Transform _left_transform;

    private Transform _right_transform;

    void Start ()
    {
        _holded_left = false;
        _holded_right = false;
        _position = Vector3.zero;
        // _left_controller = GameObject.Find("Controller (left)");
        // _right_controller = GameObject.Find("Controller (right)");
    }

    public void SetLeftController(Transform left_transform)
    {
        _left_transform = left_transform;
    }

    public void UnsetLeftController()
    {
        _left_transform = null;
    }

    public void SetRightController (Transform right_transform)
    {
        _right_transform = right_transform;
    }

    public void UnsetRightController()
    {
        _right_transform = null;
    }

    public void SetHoldedLeft ()
    {
        _holded_left = true;
    }

    public void UnsetHoldedLeft()
    {
        _holded_left = false;
    }

    public bool GetHoldedLeft()
    {
        return _holded_left;
    }

    public void SetHoldedRight()
    {
        _holded_right = true;
    }

    public void UnsetHoldedRight()
    {
        _holded_right = false;
    }

    public bool GetHoldedRight()
    {
        return _holded_right;
    }

    public GameObject CreateHoldPoint ()
    {
        _hold_point = new GameObject();
        return _hold_point;
    }

    public Transform GetHoldPoint ()
    {
        return _hold_point.transform;
    }

    public void ResetHoldPointPos ()
    {
        if (_hold_point)
        {
            if (_left_transform && _right_transform)
            {
                _position = (_left_transform.position + _right_transform.position) / 2.0f;
                _hold_point.transform.position = _position;
            }
        }
    }

    public void DestroyHoldPoin ()
    {
        _hold_point = null;
    }

    void Update ()
    {
        ResetHoldPointPos ();
    }
}
