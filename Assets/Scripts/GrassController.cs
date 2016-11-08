using UnityEngine;
using System.Collections;

public class GrassController : MonoBehaviour {

    private bool _start_count_down;

    private float _lifetime;

    private GameObject _smoke;

    private bool _is_in_fireplace;

    private float _default_destroy_time = 0.5f;

    void Start ()
    {
        _start_count_down = false;
        _lifetime = 0f;
        _is_in_fireplace = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (CompareTag(Tags.GRASS) && other.tag == Tags.TORCH_COLLIDER && !_is_in_fireplace)
        {
            if (other.GetComponent<ColliderController>().GetFireStatus())
            {
                Smoke();
            }
        }
        else if (CompareTag(Tags.GRASS) && other.CompareTag(Tags.LIGHTER_COLLIDER) && !_is_in_fireplace)
        {
            if (other.GetComponent<ColliderController>().GetFireStatus())
            {
                Smoke();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (CompareTag(Tags.GRASS) && other.tag == Tags.TORCH_COLLIDER)
        {
            if (other.GetComponent<ColliderController>().GetFireStatus())
            {
                StartCoroutine(WaitAndDestroy(_default_destroy_time));
            }
        }
        else if (CompareTag(Tags.GRASS) && other.CompareTag(Tags.LIGHTER_COLLIDER))
        {
            if (other.GetComponent<ColliderController>().GetFireStatus())
            {
                StartCoroutine(WaitAndDestroy(_default_destroy_time));
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

    IEnumerator WaitAndDestroy(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    public void SetISInFireplace(bool is_in_fireplace)
    {
        _is_in_fireplace = is_in_fireplace;
    }
}
