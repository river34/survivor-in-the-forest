using UnityEngine;
using System.Collections;

public class FlashLight : MonoBehaviour
{

    private Valve.VR.EVRButtonId triggerButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    private Valve.VR.EVRButtonId touchpadButton = Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad;
    SteamVR_Controller.Device device;

    //Flashlight Source (Spotlight Object under left hand model)
	[SerializeField]
	public GameObject _lightSourceLeft;
	public GameObject _lightSourceRight;
    public GameObject _lightSourceIn;

    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;
    
	//Flag to check if he is holding item or not
	private bool _isLeftHolding = false;
	private bool _isRightHolding = false;

    //Flag to check if the buttons are being pressed
    private bool _isLeftPadPressed = false;
    private bool _isLeftTriggerPressed = false;
    private bool _isRightPadPressed = false;
    private bool _isRightTriggerPressed = false;

    //Flag to check if the game is on
    private bool _isGameStart = true;


    //Controller objects to track buttonpresses
    public SteamVR_TrackedController _leftController;
    public SteamVR_TrackedController _rightController;

    //Initialize audio sources
    private AudioSource _audioSource;
    private AudioClip _flashlightFailSound;
    private int _flashlightFailAudioCheck;

    //Play for a fixed amount of seconds
    [SerializeField]
    private float _flashlightMaxTime = 30f;
    private float _flashlightLifeTime;


    // Use this for initialization
    void Start()
	{
		//Left event controller. Updates values based on triggers and button presses using events.
		_leftController.PadClicked += C_LeftPadClicked;
		_leftController.PadUnclicked += C_LeftPadUnClicked;
		_leftController.TriggerClicked += C_LeftTriggerClicked;
		_leftController.TriggerUnclicked += C_LeftTriggerUnClicked;

		//Right event controller. Updates values based on triggers and button presses using events.
		_rightController.PadClicked += C_RightPadClicked;
		_rightController.PadUnclicked += C_RightPadUnClicked;
		_rightController.TriggerClicked += C_RightTriggerClicked;
		_rightController.TriggerUnclicked += C_RightTriggerUnClicked;

        //Loading resources to sound model
        _audioSource = gameObject.AddComponent<AudioSource>();
        //Dialogue of flashlight fail
        _flashlightFailSound = Resources.Load("Sounds/Dialogues/Flashlight Off") as AudioClip;

        _flashlightFailAudioCheck = 1;

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        _flashlightLifeTime = _flashlightLifeTime + Time.deltaTime;

        //Checking if the trigger button is pressed and also checks which hand is holding the flashlight
        if (_isGameStart)
        {
            _lightSourceLeft.SetActive(true);
            _lightSourceIn.SetActive(true);
        }
        else if (_flashlightLifeTime >= _flashlightMaxTime)
        {
            _lightSourceLeft.SetActive(false);
            _lightSourceRight.SetActive(false);
            _lightSourceIn.SetActive(false);

            //Play Sound/Dialogue of no battery
            if (!_audioSource.isPlaying && _flashlightFailAudioCheck == 1)
            {
                _audioSource.clip = _flashlightFailSound;
                _audioSource.Play();
            }
            _flashlightFailAudioCheck++;

        }
        else if (_isLeftHolding/* && _isLeftPadPressed */)
        {
            Debug.Log("Left hold + Pad Pressed");
            _lightSourceLeft.SetActive(true);
            _lightSourceIn.SetActive(true);

        }
        else if(_isRightHolding/* && _isRightPadPressed */)
        {
            Debug.Log("Right hold + Pad Pressed");
            _lightSourceRight.SetActive(true);
            _lightSourceIn.SetActive(true);
        }
        else
        {
		//If nothing is pressed, disable both sources
			_lightSourceLeft.SetActive(false);
			_lightSourceRight.SetActive(false);
            _lightSourceIn.SetActive(false);
            _flashlightFailAudioCheck = 1;
        }
    }

	//When the item collides with the controllers. ON ENTER
    void OnTriggerEnter(Collider other)
    {
		if (other.CompareTag(Tags.LEFT_CONTROLLER) && _isLeftTriggerPressed)
        {
				Debug.Log("LEFT COLLIDER");
				GameObject _branch = other.transform.parent.gameObject;
				_branch.tag = Tags.SMALL_GRABBABLE;
				_isLeftHolding = true;
                _isGameStart = false;

        }
        else if (other.CompareTag(Tags.RIGHT_CONTROLLER) && _isRightTriggerPressed)
        {
            Debug.Log("RIGHT COLLIDER");
            GameObject _branch = other.transform.parent.gameObject;
            _branch.tag = Tags.SMALL_GRABBABLE;
            _isRightHolding = true;
            _isGameStart = false;
        }
    }

    /*
	//When the item is in contact with the controller. EVERY FRAME
	void OnTriggerStay(Collider other)
	{      

		if (other.CompareTag(Tags.LEFT_CONTROLLER) && _isLeftTriggerPressed)
		{
			Debug.Log("LEFT COLLIDER");
			GameObject _branch = other.transform.parent.gameObject;
			_branch.tag = Tags.SMALL_GRABBABLE;
			_isLeftHolding = true;
            _isGameStart = false;
        }
		else if (other.CompareTag(Tags.RIGHT_CONTROLLER) && _isRightTriggerPressed)
		{
			Debug.Log("RIGHT COLLIDER");
			GameObject _branch = other.transform.parent.gameObject;
			_branch.tag = Tags.SMALL_GRABBABLE;
			_isRightHolding = true;
            _isGameStart = false;
        }
	}
    */

	//On exiting the trigger
    void OnTriggerExit(Collider other)
    {
		Debug.Log("Colliderlost");
		_isLeftHolding = false;
		_isRightHolding = false;
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

    //RIGHT CONTROLLER EVENTS
    private void C_RightPadClicked(object sender, ClickedEventArgs e)
    {
        _isRightPadPressed = true;
    }
    private void C_RightPadUnClicked(object sender, ClickedEventArgs e)
    {
        _isRightPadPressed = false;
    }
    private void C_RightTriggerClicked(object sender, ClickedEventArgs e)
    {
        _isRightTriggerPressed = true;
    }
    private void C_RightTriggerUnClicked(object sender, ClickedEventArgs e)
    {
        _isRightTriggerPressed = false;
    }

}
