using UnityEngine;
using System.Collections;

public class BranchController : MonoBehaviour {

    private bool _start_count_down;

    private float _lifetime;

    private GameObject _smoke;

    private bool _is_in_fireplace;

    private float _default_smoke_off_time = 2f;

    void Start ()
    {
        _start_count_down = false;
		_is_in_fireplace = false;
        _lifetime = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (CompareTag(Tags.BRANCH) && other.tag == Tags.TORCH_COLLIDER && !_is_in_fireplace)
        {
            if (other.GetComponent<ColliderController>().GetFireStatus())
            {
                Smoke();
            }
        }
        else if (CompareTag(Tags.BRANCH) && other.CompareTag(Tags.LIGHTER_COLLIDER) && !_is_in_fireplace)
        {
            if (other.GetComponent<ColliderController>().GetFireStatus())
            {
                Smoke();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (CompareTag(Tags.BRANCH) && other.tag == Tags.TORCH_COLLIDER && !_is_in_fireplace)
        {
            if (other.GetComponent<ColliderController>().GetFireStatus())
            {
                StartCoroutine(WaitAndSmokeOff(_default_smoke_off_time));
            }
        }
        else if (CompareTag(Tags.BRANCH) && other.CompareTag(Tags.LIGHTER_COLLIDER) && !_is_in_fireplace)
        {
            if (other.GetComponent<ColliderController>().GetFireStatus())
            {
                StartCoroutine(WaitAndSmokeOff(_default_smoke_off_time));
            }
        }
    }

    void Update()
    {
        if (_start_count_down && _lifetime > 0)
        {
            _lifetime -= Time.deltaTime;

            if (_lifetime <= 0f)
            {
                _lifetime = 0f;

                if (gameObject.activeSelf)
                {
                    gameObject.SetActive(false);
                }
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

		if (!_smoke.activeSelf)
		{
			_smoke.SetActive (true);
		}
    }

    IEnumerator WaitAndSmokeOff(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (_smoke && _smoke.activeSelf)
        {
            _smoke.SetActive(false);
        }
    }

    public void SetISInFireplace(bool is_in_fireplace)
    {
        _is_in_fireplace = is_in_fireplace;
    }
}
