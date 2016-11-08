using UnityEngine;
using System.Collections;

public class FireplaceController : MonoBehaviour
{
    private ArrayList _branches;
    private ArrayList _grasses;
    private ArrayList _burned_branches;
    private ArrayList _burned_grasses;
    private GameObject _fire;
    private GameObject _fire_collider;
    private GameObject _wolf_manager;
    private bool _lighter;
    private bool _ready;
    private bool _fire_is_on;
    private bool _start_count_down;
    private float _default_lifetime = 40f;      // camp fire extend time
    private float _lifetime;
    private float _default_overall_lifetime = 90f;      // survive time before camp fire
    private float _overall_lifetime;
    private MainController _main_controller;
    private bool _cue_played;
    private bool _fire_die_cue_played;
    private bool _forget_cue_played;

    void Start()
    {
        _branches = new ArrayList();
        _grasses = new ArrayList();
        _burned_branches = new ArrayList();
        _burned_grasses = new ArrayList();

        _fire = Instantiate(Resources.Load("Prefabs/Camp Fire"), transform, false) as GameObject;
        _fire.SetActive(false);

        _fire_collider = Instantiate(Resources.Load("Prefabs/Camp Fire Collider"), transform, false) as GameObject;
        _fire_collider.SetActive(false);

        _lighter = false;
        _ready = false;
        _fire_is_on = false;
        _start_count_down = false;
        _overall_lifetime = _default_overall_lifetime;

        _main_controller = GameObject.Find("Root").GetComponent<MainController>();
        _cue_played = false;
        _fire_die_cue_played = false;
        _forget_cue_played = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // print(other.tag);

        if (CompareTag(Tags.FIREPLACE) && other.tag == Tags.BRANCH)
        {
            if (other.gameObject.transform.parent != transform)
            {
                other.gameObject.transform.parent = transform;
            }

            if (!_branches.Contains(other.gameObject) && !_burned_branches.Contains(other.gameObject))
            {
                _branches.Add(other.gameObject);
            }

            other.GetComponent<BranchController>().SetISInFireplace(true);
            // other.GetComponent<MeshCollider>().enabled = false;
            // other.GetComponent<BoxCollider>().enabled = true;

            if (!_cue_played)
            {
             //   _main_controller.Kindling();
                _cue_played = true;
            }
        }
        /*
        if (CompareTag(Tags.FIREPLACE) && other.tag == Tags.GRASS)
        {
            if (other.gameObject.transform.parent != transform)
            {
                other.gameObject.transform.parent = transform;
            }

            if (!_grasses.Contains(other.gameObject) && !_burned_grasses.Contains(other.gameObject))
            {
                _grasses.Add(other.gameObject);
            }

            other.GetComponent<GrassController>().SetISInFireplace(true);
            other.GetComponent<MeshCollider>().enabled = false;
            other.GetComponent<BoxCollider>().enabled = true;

            if (!_fireplace_cue_played)
            {
                _audio_source.Stop();
                _audio_source.clip = Resources.Load("Sounds/Dialogues/Kindling Encouragement Cue") as AudioClip;
                _audio_source.Play();
                _fireplace_cue_played = true;
            }
        }
        */

        if (CompareTag(Tags.FIREPLACE) && other.tag == Tags.LIGHTER_COLLIDER)
        {
            if (_ready)
            {
                if (_lighter != other.GetComponent<ColliderController>().GetFireStatus())
                {
                    _lighter = other.GetComponent<ColliderController>().GetFireStatus();
                    _start_count_down = true;
                }
            }
        }

        if (CompareTag(Tags.FIREPLACE) && other.tag == Tags.TORCH_COLLIDER)
        {
            if (_ready)
            {
                if (_lighter != other.GetComponent<ColliderController>().GetFireStatus())
                {
                    _lighter = other.GetComponent<ColliderController>().GetFireStatus();
                    _start_count_down = true;
                }
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (CompareTag(Tags.FIREPLACE) && other.tag == Tags.LIGHTER_COLLIDER)
        {
            if (_ready)
            {
                if (_lighter != other.GetComponent<ColliderController>().GetFireStatus())
                {
                    _lighter = other.GetComponent<ColliderController>().GetFireStatus();
                    _start_count_down = true;
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (CompareTag(Tags.FIREPLACE) && other.tag == Tags.BRANCH)
        {
            other.gameObject.transform.parent = GameObject.Find("Firewoods").transform;

            if (_branches.Contains(other.gameObject))
            {
                _branches.Remove(other.gameObject);
            }

            other.GetComponent<BranchController>().SetISInFireplace(false);
            // other.GetComponent<MeshCollider>().enabled = true;
            // other.GetComponent<BoxCollider>().enabled = false;
        }

        /*
        if (CompareTag(Tags.FIREPLACE) && other.tag == Tags.GRASS)
        {
            other.gameObject.transform.parent = GameObject.Find("Branches").transform;

            if (_branches.Contains(other.gameObject))
            {
                _branches.Remove(other.gameObject);
            }

            other.GetComponent<GrassController>().SetISInFireplace(false);
            other.GetComponent<MeshCollider>().enabled = true;
            other.GetComponent<BoxCollider>().enabled = false;
        }
        */

        if (CompareTag(Tags.FIREPLACE) && other.tag == Tags.LIGHTER_COLLIDER)
        {
            _lighter = false;
        }

        if (CompareTag(Tags.FIREPLACE) && other.tag == Tags.TORCH_COLLIDER)
        {
            _lighter = false;
        }
    }

    void Update()
    {
        if (_main_controller.GameIsEnd())
        {
            return;
        }

        _overall_lifetime -= Time.deltaTime;

        if (_branches != null && _branches.Count > 0 /* && _grasses.Count > 0 */)
        {
            _ready = true;
        }

        if (_start_count_down)
        {
            _lifetime -= Time.deltaTime;

            if ((_ready && _lighter) || (_ready && _fire_is_on))
            {
                if (_branches.Count > 0 /* && _grasses.Count > 0 */)
                {
                    _fire_is_on = true;

                    GameObject _branch = (GameObject)_branches[0];
                    _branches.Remove(_branch);
                    _burned_branches.Add(_branch);
                    StartCoroutine(WaitAndDestroy(_default_lifetime, _branch, _burned_branches));

                    /*
                    GameObject _grass = (GameObject)_grasses[0];
                    _grasses.Remove(_grass);
                    _burned_grasses.Add(_grass);
                    StartCoroutine(WaitAndDestroy(_default_lifetime, _grass, _burned_grasses));
                    */

                    _lifetime += _default_lifetime;

                    _ready = false;
                    _lighter = false;

                    _overall_lifetime = _overall_lifetime + _lifetime + _default_overall_lifetime;
                }
            }

            if (_lifetime <= 5)
            {
                if (!_fire_die_cue_played)
                {
                    _main_controller.FireIsDying();
                    _fire_die_cue_played = true;
                }
            }

            if (_lifetime <= 0 && !_forget_cue_played)
            {
                _lifetime = 0;
                _fire_is_on = false;
                _main_controller.ForgotFire();
                _forget_cue_played = true;
            }

            if (_fire_is_on)
            {
                // Set everything in the fire to be ingrabbable
                foreach (Transform child in transform)
                {
                    child.gameObject.layer = LayerMask.NameToLayer("Default");
                }

                if (!_fire.activeSelf)
                {
                    _fire.SetActive(true);
                }

                if (!_fire_collider.activeSelf)
                {
                    _fire_collider.SetActive(true);
                }

                // enable wolf_manager if it is not enabled
                /*
                if (!_wolf_manager)
                {
                    _wolf_manager = Instantiate(Resources.Load("Prefabs/Wolf Manager"), Vector3.zero, Quaternion.identity) as GameObject;
                    _wolf_manager.SetActive(true);
                }
                */
            }
            else
            {
                // Set everything in the fire to be grabbable
                foreach (Transform child in transform)
                {
                    if (child.gameObject.layer != LayerMask.NameToLayer("Grabbable"))
                    {
                        child.gameObject.layer = LayerMask.NameToLayer("Grabbable");
                    }
                }

                if (_fire.activeSelf)
                {
                    _fire.SetActive(false);
                }

                if (_fire_collider.activeSelf)
                {
                    _fire_collider.SetActive(false);
                }
            }
        }

        // die of cold
        if (_overall_lifetime <= 0)
        {
            _main_controller.Die_Of_Cold();
        }

        // print(" _ready : " + _ready + " _lighter : " + _lighter + "_ lifetime : " + _lifetime + " _overall_lifetime : " + _overall_lifetime);
    }

    IEnumerator WaitAndDestroy(float seconds, GameObject game_object, ArrayList list)
    {
        yield return new WaitForSeconds(seconds);

        if (list.Contains(game_object))
        {
            list.Remove(game_object);
        }

        Destroy(game_object);
    }

    public bool GetFireStatus()
    {
        return _fire_is_on;
    }
}
