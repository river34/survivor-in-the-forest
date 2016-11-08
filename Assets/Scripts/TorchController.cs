using UnityEngine;
using System.Collections;

public class TorchController : MonoBehaviour
{

    // if torch fire is started
    private bool _start_count_down;

    private bool _fire_is_on;

    // torch fire lasts for 60 seconds
    private float _default_lifetime = 30f;

    // torch fire's remaining time, decreased with Time 
    private float _lifetime;

    // child object of fire (particle system)
    private GameObject _fire;

    // child object of smoke (particle system)
    private GameObject _smoke;

    // child object of spark (particle system)
    private GameObject _spark;

    // collider
    private GameObject _collider;

    private MainController _main_controller;

    private bool _played = false;
   

    void Start ()
    {
        _start_count_down = false;
        _fire_is_on = false;

        _lifetime = _default_lifetime;

        _main_controller = GameObject.Find("Root").GetComponent<MainController>();
    }

    void Update()
    {
        if (_start_count_down)
        {
            _lifetime -= Time.deltaTime;

            if (_lifetime > 0)
            {
                //
            }
            else
            {
                _lifetime = 0f;
                FireOff();
            }
        }
    }

    //When the item collides with the controllers. ON ENTER
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.LEFT_CONTROLLER) || other.CompareTag(Tags.RIGHT_CONTROLLER))
        {
            if (!_played)
            {
                _main_controller.PickUpTorch();
                _played = true;
            }
        }
    }

    public float GetLifeTime()
    {
        return _lifetime;
    }

    public void SetLifeTime(float lifetime)
    {
        _lifetime = lifetime;
    }

    public void ExtendLifeTime()
    {
        _lifetime += _default_lifetime;
    }

    public void SetStart(bool start_count_down)
    {
        _start_count_down = start_count_down;
    }

    public void Smoke()
    {
        if (!_smoke)
        {
            _smoke = Instantiate(Resources.Load("Prefabs/Branch Smoke"), transform, false) as GameObject;
        }

        if (_smoke && !_smoke.activeSelf)
        {
            _smoke.SetActive(true);
        }
    }

    public void SmokeOff()
    {
        StartCoroutine(WaitAndSmokeOff(0.5f));
    }

    IEnumerator WaitAndSmokeOff(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (_smoke && _smoke.activeSelf)
        {
            _smoke.SetActive(false);
        }
    }

    public void Spark()
    {
        if (!_spark)
        {
            _spark = Instantiate(Resources.Load("Prefabs/Torch Spark"), transform, false) as GameObject;
        }

        if (_spark && !_spark.activeSelf)
        {
            _spark.SetActive(true);
        }

        SparkOff();
    }

    public void SparkOff()
    {
        StartCoroutine(WaitAndSmokeOff(0.5f));
    }

    IEnumerator WaitAndSparkOff(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (_spark && _spark.activeSelf)
        {
            _spark.SetActive(false);
        }
    }

    public void FireOn()
	{
		_fire_is_on = true;
		_start_count_down = true;

        if (!_fire)
        {
            _fire = Instantiate(Resources.Load("Prefabs/Torch Fire"), transform, false) as GameObject;
        }

		if (!_fire.activeSelf)
		{
			_fire.SetActive (true);
		}

        if (!_smoke)
        {
            _smoke = Instantiate(Resources.Load("Prefabs/Branch Smoke"), transform, false) as GameObject;
        }

		if (!_smoke.activeSelf)
		{
			_smoke.SetActive (true);
		}

        if (!_collider)
        {
            _collider = Instantiate(Resources.Load("Prefabs/Torch Collider"), transform, false) as GameObject;
        }

		if (!_collider.activeSelf)
		{
			_collider.SetActive (true);
		}

        ExtendLifeTime();
    }

    public void FireOff()
	{
		_fire_is_on = false;
		_start_count_down = false;

        if (_fire && _fire.activeSelf)
        {
            _fire.SetActive(false);
        }

        if (_smoke && _smoke.activeSelf)
        {
            _smoke.SetActive(false);
        }

        if (_collider)
        {
            Destroy (_collider);
        }
    }

    public bool GetFireStatus()
    {
        return _fire_is_on;
    }
}
