using UnityEngine;
using System.Collections;

public class Lighter : MonoBehaviour {

    /* 
        /$$$$$$$  /$$$$$$$$  /$$$$$$  /$$$$$$$        /$$$$$$$$ /$$   /$$ /$$$$$$  /$$$$$$        /$$$$$$$  /$$       /$$$$$$$$  /$$$$$$   /$$$$$$  /$$$$$$$$
        | $$__  $$| $$_____/ /$$__  $$| $$__  $$      |__  $$__/| $$  | $$|_  $$_/ /$$__  $$      | $$__  $$| $$      | $$_____/ /$$__  $$ /$$__  $$| $$_____/
        | $$  \ $$| $$      | $$  \ $$| $$  \ $$         | $$   | $$  | $$  | $$  | $$  \__/      | $$  \ $$| $$      | $$      | $$  \ $$| $$  \__/| $$      
        | $$$$$$$/| $$$$$   | $$$$$$$$| $$  | $$         | $$   | $$$$$$$$  | $$  |  $$$$$$       | $$$$$$$/| $$      | $$$$$   | $$$$$$$$|  $$$$$$ | $$$$$   
        | $$__  $$| $$__/   | $$__  $$| $$  | $$         | $$   | $$__  $$  | $$   \____  $$      | $$____/ | $$      | $$__/   | $$__  $$ \____  $$| $$__/   
        | $$  \ $$| $$      | $$  | $$| $$  | $$         | $$   | $$  | $$  | $$   /$$  \ $$      | $$      | $$      | $$      | $$  | $$ /$$  \ $$| $$      
        | $$  | $$| $$$$$$$$| $$  | $$| $$$$$$$/         | $$   | $$  | $$ /$$$$$$|  $$$$$$/      | $$      | $$$$$$$$| $$$$$$$$| $$  | $$|  $$$$$$/| $$$$$$$$
        |__/  |__/|________/|__/  |__/|_______/          |__/   |__/  |__/|______/ \______/       |__/      |________/|________/|__/  |__/ \______/ |________/
      
        THIS SCRIPT IS VERY SIMILAR TO THE FLASHLIGHT ONE. USES EVENTS TO TRACK BUTTON PRESSES SO THERE ARE INDIVIDUAL BOOLEAN VARIABLES TO CHECK IF BUTTONS ARE PRESSED AND THEY ARE ACTIVATED WHEN THE EVENT IS CALLED
    */

    SteamVR_Controller.Device device;
    private SteamVR_Controller.Device controller { get { return SteamVR_Controller.Input((int)trackedObj.index); } }
    private SteamVR_TrackedObject trackedObj;

    //Fire Source (Particle source under the lighter)
    [SerializeField]
    private GameObject _lighterFlame;

    //Is fire running
    private bool _isLighterEnabled;

    //Flag to check if he is holding item or not
    private bool _isLeftHolding = false;
    private bool _isRightHolding = false;

    //Flag to check if the buttons are being pressed
    private bool _isLeftPadPressed = false;
    private bool _isLeftTriggerPressed = false;
    private bool _isRightPadPressed = false;
    private bool _isRightTriggerPressed = false;

    //Controller objects to track buttonpresses
    public SteamVR_TrackedController _leftController;
    public SteamVR_TrackedController _rightController;


    //Play for a fixed amount of seconds
    [SerializeField]
    private float _lighterMaxTime = 30f;
    private float _lighterLifeTime;

    //Initialize audio sources
    private AudioSource _audioSource;
    private AudioClip[] _lighterSuccessSound;
    private AudioClip[] _lighterFailSound;
    private AudioClip _lighterFailTriggerSound;
    private AudioClip _lighterFirstTimePickupSound;
    private int _lighterSuccessAudioCheck;
    private int _lighterFailAudioCheck;
    private int _lighterFirstTimeCheck;

    // collider
    private GameObject _collider;

    // Use this for initialization
    void Start()
    {

        _lighterLifeTime = 0.0f;

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
        _lighterSuccessSound = new AudioClip [2];
        _lighterSuccessSound[0] = Resources.Load("Sounds/Dialogues/Lighter Success 1") as AudioClip;
        _lighterSuccessSound[1] = Resources.Load("Sounds/Dialogues/Lighter Success 2") as AudioClip;

        _lighterFailSound = new AudioClip[4];
        _lighterFailSound[0] = Resources.Load("Sounds/Dialogues/Lighter Fail 1") as AudioClip;
        _lighterFailSound[1] = Resources.Load("Sounds/Dialogues/Lighter Fail 2") as AudioClip;
        _lighterFailSound[2] = Resources.Load("Sounds/Dialogues/Lighter Fail 3") as AudioClip;
        _lighterFailSound[3] = Resources.Load("Sounds/Dialogues/Lighter Fail 4") as AudioClip;

        _lighterFailTriggerSound = Resources.Load("Sounds/Dialogues/Lighter Fail, Triggered") as AudioClip;

        _lighterFirstTimePickupSound = Resources.Load("Sounds/Dialogues/Hope This Lighter Works") as AudioClip;

        _lighterSuccessAudioCheck = 1;
        _lighterFailAudioCheck = 1;
        _lighterFirstTimeCheck = 1;
    }


    // Update is called once per frame
    void FixedUpdate()
    {

        //Checking if the trigger button is pressed and also checks which hand is holding the flashlight
        if(_lighterLifeTime >= _lighterMaxTime)
        {
            _lighterFlame.SetActive(false);
            _isLighterEnabled = false;

            //Play Sound
            if (!_audioSource.isPlaying && _lighterFailAudioCheck == 1)
            {
                if(Random.Range(0,3) == 3)
                {
                    _audioSource.clip = _lighterFailTriggerSound;
                    _audioSource.Play();
                }
                else
                {
                    _audioSource.clip = _lighterFailSound[Random.Range(0, 4)];
                    _audioSource.Play();
                }
            }
            _lighterFailAudioCheck++;

        }
        else if((_lighterFirstTimeCheck == 1 && _isLeftHolding) || (_lighterFirstTimeCheck == 1 && _isRightHolding))
        {
            if (_lighterFirstTimeCheck == 1)
            {
                _audioSource.clip = _lighterFirstTimePickupSound;
                _audioSource.Play();
            }
            _lighterFirstTimeCheck++;

        }
        else if (_isLeftHolding/* && _isLeftPadPressed*/)
        {
            Debug.Log("Flip Lighter");
            _lighterFlame.SetActive(true);
            _isLighterEnabled = true;

            //Play Sound/Dialogue of no fuel
            if (!_audioSource.isPlaying && _lighterSuccessAudioCheck == 1)
            {
                _audioSource.clip = _lighterSuccessSound[Random.Range(0, 1)];
                _audioSource.Play();
            }

            _lighterSuccessAudioCheck++;

            //Add Lighter time to check if the lighter is working or not
            _lighterLifeTime = _lighterLifeTime + Time.deltaTime;

        }
        else if (_isRightHolding/* && _isRightPadPressed */)
        {
            Debug.Log("Flip Lighter");
            _lighterFlame.SetActive(true);
            _isLighterEnabled = true;

            //Play Sound
            if (!_audioSource.isPlaying && _lighterSuccessAudioCheck == 1)
            {
                _audioSource.clip = _lighterSuccessSound[Random.Range(0, 1)];
                _audioSource.Play();
            }

            _lighterSuccessAudioCheck++;

            //Add Lighter time to check if the lighter is working or not
            _lighterLifeTime = _lighterLifeTime + Time.deltaTime;
        }
        else
        {

            //If nothing is pressed, disable both sources
            _lighterFlame.SetActive(false);
            _isLighterEnabled = false;

            //If nothing is pressed reset lighter dialogue/sound to play the next time they start it
            _lighterSuccessAudioCheck = 1;
            _lighterFailAudioCheck = 1;
        }

        if (_isLighterEnabled)
        {
            if (!_collider)
            {
                _collider = Instantiate(Resources.Load("Prefabs/Lighter Collider"), transform, false) as GameObject;
                _collider.SetActive(true);
            }
        }
        else
        {
            if (!_collider)
            {
                Destroy(_collider);
            }
        }
    }

    //Function to return status of lighter
    public bool isLighterOn()
    {
        return _isLighterEnabled;
    }

    //When the item collides with the controllers. ON ENTER
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collides");
        if (other.CompareTag(Tags.LEFT_CONTROLLER) && _isLeftTriggerPressed)
        {
            Debug.Log("LEFT COLLIDER");
            _isLeftHolding = true;
        }
        else if (other.CompareTag(Tags.RIGHT_CONTROLLER) && _isRightTriggerPressed)
        {
            Debug.Log("RIGHT COLLIDER");
            _isRightHolding = true;
        }
    }

    //When the item is in contact with the controller. EVERY FRAME
    void OnTriggerStay(Collider other)
    {

        if (other.CompareTag(Tags.LEFT_CONTROLLER) && _isLeftTriggerPressed)
        {
            Debug.Log("LEFT COLLIDER");
            _isLeftHolding = true;
        }
        else if (other.CompareTag(Tags.RIGHT_CONTROLLER) && _isRightTriggerPressed)
        {
            Debug.Log("RIGHT COLLIDER");
            _isRightHolding = true;
        }
    }

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
