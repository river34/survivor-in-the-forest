using UnityEngine;
using System.Collections;

public class RightHandAnimator : MonoBehaviour {

    SteamVR_Controller.Device device;
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    //Controller objects to track buttonpresses
    [SerializeField]
    private SteamVR_TrackedController _rightController;

    private Animator _rightHandAnimator;
    //Flag to check if the buttons are being pressed
    private bool _isRightPadPressed = false;
    private bool _isRightTriggerPressed = false;

    // Use this for initialization
    void Start()
    {
        _rightHandAnimator = GetComponent<Animator>();

        _rightController.PadClicked += C_LeftPadClicked;
        _rightController.PadUnclicked += C_LeftPadUnClicked;
        _rightController.TriggerClicked += C_LeftTriggerClicked;
        _rightController.TriggerUnclicked += C_LeftTriggerUnClicked;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        //  _rightHandAnimator.SetTrigger("flipLighter");
        //  _rightHandAnimator.SetBool("GrabFlashight",true);
        //  _rightHandAnimator.SetBool("GrabTorch",true);
        //  _rightHandAnimator.SetBool("GrabLighter",true);
        //  _rightHandAnimator.SetBool("GrabItem",true);


        Debug.Log("Collides");
        if (other.CompareTag(Tags.LIGHTER) && _isRightTriggerPressed)
        {
            _rightHandAnimator.SetBool("GrabLighter", true);
        }
        else if (other.CompareTag(Tags.LIGHTER) && _isRightTriggerPressed && _isRightPadPressed)
        {
            _rightHandAnimator.SetTrigger("flipLighter");
        }
        else if (other.CompareTag(Tags.TORCH) && _isRightTriggerPressed)
        {
            _rightHandAnimator.SetBool("GrabTorch", true);
        }
        else if (other.CompareTag(Tags.FLASHLIGHT) && _isRightTriggerPressed)
        {
            _rightHandAnimator.SetBool("GrabFlashight", true);
        }
        else if (_isRightTriggerPressed)
        {
            _rightHandAnimator.SetBool("GrabItem", true);
        }
    }

    //When the item is in contact with the controller. EVERY FRAME
    void OnTriggerStay(Collider other)
    {
        Debug.Log("Collides");
        if (other.CompareTag(Tags.LIGHTER) && _isRightTriggerPressed)
        {
            _rightHandAnimator.SetBool("GrabLighter", true);
        }
        else if (other.CompareTag(Tags.LIGHTER) && _isRightTriggerPressed && _isRightPadPressed)
        {
            _rightHandAnimator.SetTrigger("flipLighter");
        }
        else if (other.CompareTag(Tags.TORCH) && _isRightTriggerPressed)
        {
            _rightHandAnimator.SetBool("GrabTorch", true);
        }
        else if (other.CompareTag(Tags.FLASHLIGHT) && _isRightTriggerPressed)
        {
            _rightHandAnimator.SetBool("GrabFlashight", true);
        }
        else if (_isRightTriggerPressed)
        {
            _rightHandAnimator.SetBool("GrabItem", true);
        }
    }

    //On exiting the trigger
    void OnTriggerExit(Collider other)
    {
        Debug.Log("Colliderlost");
    }

    //LEFT CONTROLLER EVENTS
    private void C_LeftPadClicked(object sender, ClickedEventArgs e)
    {
        _isRightPadPressed = true;
    }
    private void C_LeftPadUnClicked(object sender, ClickedEventArgs e)
    {
        _isRightPadPressed = false;
    }
    private void C_LeftTriggerClicked(object sender, ClickedEventArgs e)
    {
        _isRightTriggerPressed = true;
    }
    private void C_LeftTriggerUnClicked(object sender, ClickedEventArgs e)
    {
        _isRightTriggerPressed = false;
    }
}
