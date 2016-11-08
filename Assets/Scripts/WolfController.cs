using UnityEngine;
using System.Collections;

public class WolfController : MonoBehaviour {

    private Transform _guest;

    private NavMeshAgent _nav;

    private Vector3 _original_position;

    private float _default_approaching_speed = 0.8f;

    private float _default_retrieving_speed = 1.2f;

    private WolfManager _wolf_manager;

    private float _distance;

    private float _default_distance = 5f;

    private bool _holwed;

    private bool _is_approaching;

    private bool _is_retrieving;

    private bool _is_stopped;

    private float _default_destroy_time = 3f;

    private float _default_stop_time = 3f;

    private MainController _main_controller;

    private AudioClip[] _wolfSounds;

    private AudioSource _audio;

    private bool _reminder_played;

    private bool _success_played;

    public int index;

    void Start ()
    {
        _nav = GetComponent<NavMeshAgent>();

        _guest = GameObject.Find("Player").transform;

        _wolf_manager = transform.parent.gameObject.GetComponent<WolfManager>();

        _main_controller = GameObject.Find("Root").GetComponent<MainController>();

        _original_position = transform.position;

        _holwed = false;

        _is_approaching = false;
        _is_retrieving = false;
        _is_stopped = false;

        SetApproaching ();

        _audio = GetComponent<AudioSource>();
        _audio.loop = false;
        _audio.priority = 255;

        _wolfSounds = new AudioClip[3];
        _wolfSounds[0] = Resources.Load("Sounds/Wolf Growl 1") as AudioClip;
        _wolfSounds[1] = Resources.Load("Sounds/Wolf Growl 2") as AudioClip;
        _wolfSounds[2] = Resources.Load("Sounds/Wolf Growl 3") as AudioClip;

        _reminder_played = false;
        _success_played = false;
    }

    void Update ()
    {
        if (_main_controller.GameIsEnd())
        {
            SetStopped();
            return;
        }

        // wolf howls at certain distance and stop for 2 seconds.
        _distance = Vector3.Distance (transform.position, _guest.position);
        if (_distance <= _default_distance && _is_approaching && !_holwed)
        {
            _holwed = true;
            _audio.clip = _wolfSounds[Random.Range(0, _wolfSounds.Length-1)];
            _audio.Play();
            StartCoroutine(WaitAndMove ());

            if (!_reminder_played)
            {
                _reminder_played = true;
                _wolf_manager.PlayReminder();
            }
        }
    }

    void OnTriggerEnter (Collider other)
    {
        // guest dies of wolf
        if (CompareTag(Tags.WOLF) && other.CompareTag(Tags.PLAYER))
        {
            SetStopped();
            _main_controller.Die_Of_Wolf ();
        }
    }

    public void SetApproaching ()
    {
        _nav.speed = _default_approaching_speed;
        _nav.SetDestination(_guest.position);
        _is_approaching = true;
    }

    public void SetStopped ()
    {
        if (!_is_stopped)
        {
            _nav.Stop();
            _is_stopped = true;
        }
    }

    public void SetResume ()
    {
        if (_is_stopped)
        {
            _nav.speed += 0.4f;
            _nav.Resume();
            _is_stopped = false;
        }
    }

    public void SetRetrieving ()
    {
        if (!_is_retrieving)
        {
            _nav.speed = _default_retrieving_speed;
            _nav.angularSpeed = 0f;
            _nav.SetDestination(_original_position);
            StartCoroutine(WaitAndDestroy());
            _is_retrieving = true;

            if (!_success_played)
            {
                _main_controller.WolfGone(index);
                _success_played = true;
            }
        }
    }

    public bool GetRetrieving ()
    {
        return _is_retrieving;
    }

    IEnumerator WaitAndDestroy ()
    {
        _wolf_manager.DecreaseCurrentNum();
        yield return new WaitForSeconds(_default_destroy_time);
        Destroy(gameObject);
    }

    IEnumerator WaitAndMove ()
    {
        SetStopped ();
        yield return new WaitForSeconds(_default_stop_time);
        SetResume();
    }
}
