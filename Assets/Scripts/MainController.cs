using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{

    private float _lifetime = 3600f;

    private bool _guest_is_dead = false;

    private FireplaceController _fireplace_controller;

    private GameObject _wolf_manager;

    public AudioSource _audio_source;

    public AudioSource _audio_source_2;

    public AudioClip[] _clips;
    public bool[] stopped;
    private AudioLine[] _audio_lines;

    private int _current;

    private bool _voice_stopped;

    private bool _is_end;

    private LightController _sun_controller;

    private SkyboxController _skybox_controller;

    private bool _PickUpTorch_played = false;

    void Start()
    {
        _fireplace_controller = GameObject.Find("Fireplace").GetComponent<FireplaceController>();

        _sun_controller = GameObject.Find("Sun Light").GetComponent<LightController>();

        _skybox_controller = GetComponent<SkyboxController>();

        _audio_source = gameObject.AddComponent<AudioSource>();
        _audio_source.loop = false;
        _audio_source.playOnAwake = false;
        _audio_source.priority = 256;   // top priority

        _audio_source_2 = gameObject.AddComponent<AudioSource>();
        _audio_source_2.loop = false;
        _audio_source_2.playOnAwake = false;
        _audio_source_2.priority = 256;   // top priority

        _voice_stopped = false;

        if (_clips.Length > 0)
        {
            _audio_lines = new AudioLine[_clips.Length];
            for (int i = 0; i < _clips.Length; i++)
            {
                // _audio_lines[i] = new AudioLine(_clips[i]);
                _audio_lines[i] = new AudioLine
                {
                    _clip = _clips[i],
                    _seconds = 2.5f,
                    _played = false,
                };

                if (i == 0)
                {
                    _audio_lines[i]._seconds = 5f;
                }
            }
        }

        _current = 0;

        _is_end = false;

        _sun_controller.StartSunset();

        // StartCoroutine(WaitAndSurvive(10));
    }

    void Update()
    {
        if (GameIsEnd())
        {
            return;
        }

        // lifetime of the game
        _lifetime -= Time.deltaTime;

        if (_lifetime <= 0)
        {
            EndGame();
        }
        else
        {
            // voice over
            if (_audio_lines != null && _current < _audio_lines.Length && !_audio_lines[_current]._played && !_voice_stopped)
            {
                _audio_lines[_current]._seconds -= Time.deltaTime;

                if (_audio_lines[_current]._seconds <= 0f)
                {
                    _audio_lines[_current]._played = true;

                    float seconds = PlayVoiceOverNow(_audio_lines[_current]._clip);

                    if (_current < stopped.Length)
                    {
                        _voice_stopped = stopped[_current];
                    }

                    _current += 1;

                    if (_current < _audio_lines.Length)
                    {
                        _audio_lines[_current]._seconds += seconds;
                    }
                }
            }

            // wolves
            // if camp fire is on, enable wolf manager
            if (_fireplace_controller.GetFireStatus() && !_wolf_manager)
            {
                _wolf_manager = Instantiate(Resources.Load("Prefabs/Wolf Manager"), transform, false) as GameObject;
            }
        }
    }

    public void EndGame()
    {
        if (_guest_is_dead)
        {
            gameObject.GetComponent<SceneController>().GoToScene("Dead");
        }

        if (!_guest_is_dead)
        {
            gameObject.GetComponent<SceneController>().GoToScene("Success");
        }
    }

    public void SetGuestDeathStatus(bool status)
    {
        _guest_is_dead = status;
    }

    public float PlayVoiceOverNow(AudioClip audioClip)
    {
        _audio_source.Stop();
        print("Voice : " + audioClip.name);
        _audio_source.clip = audioClip;
        _audio_source.Play();
        return audioClip.length;
    }

    public float PlayVoiceOver(float seconds, AudioClip audioClip)
    {
        StartCoroutine(WaitAndPlay(seconds, audioClip));
        return audioClip.length;
    }

    IEnumerator WaitAndPlay(float seconds, AudioClip audioClip)
    {
        yield return new WaitForSeconds(seconds);
        _audio_source.Stop();
        print("Voice : " + audioClip.name);
        _audio_source.clip = audioClip;
        _audio_source.Play();
    }

    IEnumerator WaitAndDestroy(float seconds, GameObject gameObject)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    IEnumerator WaitAndSurvive(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Survive();
    }

    public void Die_Of_Cold()
    {
        _is_end = true;

        SetGuestDeathStatus(true);
        EndGame();
    }

    public void Die_Of_Wolf()
    {
        _is_end = true;

        SetGuestDeathStatus(true);
        EndGame();
    }

    public void Survive()
    {
        _is_end = true;

        // trigger gun shot and search team event
        SetGuestDeathStatus(false);
        StartCoroutine(WaitAndSurvive());
        _sun_controller.StartSunrise();
        _skybox_controller.ChangeToDay();
    }

    IEnumerator WaitAndSurvive()
    {
        float time_1 = SavedByGunShot();
        float time_2 = SavedBySearchParty() + 2f + 2f;
        yield return new WaitForSeconds((time_1 > time_2) ? time_1 : time_2);
        EndGame();
    }

    public void HearWolf ()
    {
        PlayVoiceOverNow(Resources.Load("Sounds/Dialogues/Hurry Up and Find Weapon Cue, Farther off Than Sounds") as AudioClip);
    }

    public void PickUpTorch ()
    {
        if (_PickUpTorch_played)
        {
            PlayVoiceOverNow(Resources.Load("Sounds/Dialogues/You Picked Up Torch, Pos Feedback") as AudioClip);
            _PickUpTorch_played = true;
        }
    }

    public void FireIsDying ()
    {
        PlayVoiceOverNow(Resources.Load("Sounds/Dialogues/Don't Forget to Keep Fire Going Cue") as AudioClip);
    }

    public void FirstWolfGone ()
    {
        PlayVoiceOverNow(Resources.Load("Sounds/Dialogues/Victory Over Wolf") as AudioClip);
    }

    public void SecondWolfGone()
    {
        PlayVoiceOverNow(Resources.Load("Sounds/Dialogues/Victory Over Wolf 1") as AudioClip);
    }

    public void ThirdWolfGone()
    {
        PlayVoiceOverNow(Resources.Load("Sounds/Dialogues/Too Long to Die") as AudioClip);
    }

    public void ForthWolfGone()
    {
        PlayVoiceOverNow(Resources.Load("Sounds/Dialogues/Victory Over Wolf 2") as AudioClip);
    }

    public void FifthWolfGone()
    {
        PlayVoiceOverNow(Resources.Load("Sounds/Dialogues/If I'm Going Out, I'm Taking You All Out Too") as AudioClip);
    }

    public void WolfGone(int index)
    {
        if (index == 1)
        {
            FirstWolfGone();
        }
        else if (index == 2)
        {
            SecondWolfGone();
        }
        else if (index == 3)
        {
            ThirdWolfGone();
        }
        else if (index == 4)
        {
            ForthWolfGone();
        }
        else if (index == 5)
        {
            FifthWolfGone();
        }
    }

    public void ForgotFire()
    {
        PlayVoiceOverNow(Resources.Load("Sounds/Dialogues/You Forgot Fire, Better Restart It") as AudioClip);
    }

    public float SavedByGunShot ()
    {
        AudioClip _clip = Resources.Load("Sounds/Dialogues/End Ambience (underneath voiced over script)") as AudioClip;
        print("Voice : " + _clip.name);
        _audio_source_2.clip = _clip;
        _audio_source_2.Play();
        _audio_source_2.volume = 0.5f;
        return _clip.length;
    }

    public float SavedBySearchParty ()
    {
        return PlayVoiceOver(2f, Resources.Load("Sounds/Dialogues/End of Game (Search Party Lines)") as AudioClip);
    }

    public float CloseToForest ()
    {
        return PlayVoiceOverNow(Resources.Load("Sounds/Dialogues/Don't Burn Everything") as AudioClip);
    }

    public float Kindling ()
    {
        return PlayVoiceOverNow(Resources.Load("Sounds/Dialogues/Kindling Encouragement Cue") as AudioClip);
    }

    public bool GameIsEnd ()
    {
        return _is_end;
    }
}
