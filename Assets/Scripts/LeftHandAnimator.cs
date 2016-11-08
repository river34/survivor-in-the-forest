using UnityEngine;
using System.Collections;

public class LeftHandAnimator : MonoBehaviour {

    SteamVR_Controller.Device device;
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    //Controller objects to track buttonpresses
    [SerializeField]
    private SteamVR_TrackedController _leftController;

    private Animator _leftHandAnimator;
    //Flag to check if the buttons are being pressed
    private bool _isLeftPadPressed = false;
    private bool _isLeftTriggerPressed = false;

    // Use this for initialization
    void Start ()
    {
        _leftHandAnimator = GetComponent<Animator>();

        _leftController.PadClicked += C_LeftPadClicked;
        _leftController.PadUnclicked += C_LeftPadUnClicked;
        _leftController.TriggerClicked += C_LeftTriggerClicked;
        _leftController.TriggerUnclicked += C_LeftTriggerUnClicked;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider other)
    {


        //  _leftHandAnimator.SetTrigger("flipLighter");
        //  _leftHandAnimator.SetBool("GrabFlashight",true);
        //  _leftHandAnimator.SetBool("GrabTorch",true);
        //  _leftHandAnimator.SetBool("GrabLighter",true);
        //  _leftHandAnimator.SetBool("GrabItem",true);
        

        Debug.Log("Collides");
        if(other.CompareTag(Tags.LIGHTER) && _isLeftTriggerPressed)
        {
            _leftHandAnimator.SetBool("GrabLighter", true);
        }
        else if (other.CompareTag(Tags.LIGHTER) && _isLeftTriggerPressed && _isLeftPadPressed)
        {
            _leftHandAnimator.SetTrigger("flipLighter");
        }
        else if (other.CompareTag(Tags.TORCH) && _isLeftTriggerPressed)
        {
            _leftHandAnimator.SetBool("GrabTorch", true);
        }
        else if (other.CompareTag(Tags.FLASHLIGHT) && _isLeftTriggerPressed)
        {
            _leftHandAnimator.SetBool("GrabFlashight", true);
        }
        else if (_isLeftTriggerPressed)
        {
            _leftHandAnimator.SetBool("GrabItem", true);
        }
    }

    //When the item is in contact with the controller. EVERY FRAME
    void OnTriggerStay(Collider other)
    {
        Debug.Log("Collides");
        if (other.CompareTag(Tags.LIGHTER) && _isLeftTriggerPressed)
        {
            _leftHandAnimator.SetBool("GrabLighter", true);
        }
        else if (other.CompareTag(Tags.LIGHTER) && _isLeftTriggerPressed && _isLeftPadPressed)
        {
            _leftHandAnimator.SetTrigger("flipLighter");
        }
        else if (other.CompareTag(Tags.TORCH) && _isLeftTriggerPressed)
        {
            _leftHandAnimator.SetBool("GrabTorch", true);
        }
        else if (other.CompareTag(Tags.FLASHLIGHT) && _isLeftTriggerPressed)
        {
            _leftHandAnimator.SetBool("GrabFlashight", true);
        }
        else if (_isLeftTriggerPressed)
        {
            _leftHandAnimator.SetBool("GrabItem", true);
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
        _isLeftPadPressed = true;
    }
    private void C_LeftPadUnClicked(object sender, ClickedEventArgs e)
    {
        _isLeftPadPressed = false;
    }
    private void C_LeftTriggerClicked(object sender, ClickedEventArgs e)
    {
        _isLeftTriggerPressed = true;
    }
    private void C_LeftTriggerUnClicked(object sender, ClickedEventArgs e)
    {
        _isLeftTriggerPressed = false;
    }
}
