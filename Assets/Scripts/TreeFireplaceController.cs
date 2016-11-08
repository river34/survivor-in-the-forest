using UnityEngine;
using System.Collections;

public class TreeFireplaceController : MonoBehaviour {

	private bool _start_count_down;

	private bool _fire_is_on;

	private bool _force_stop_fire;

	private float _lifetime;

	private GameObject _smoke;

	private GameObject _fire;

	private ArrayList _torches;

	// collider
	private GameObject _collider;

    private MainController _main_controller;

    private bool _played;

	void Start()
	{
		_start_count_down = false;
		_fire_is_on = false;
		_force_stop_fire = false;
		_lifetime = 0f;
		_torches = new ArrayList ();
        _main_controller = GameObject.Find("Root").GetComponent<MainController>();
        _played = false;
    }

	void OnTriggerEnter(Collider other)
	{
		// print (tag + " , " + other.tag);

		if (CompareTag(Tags.TREE_FIREPLACE) && other.tag == Tags.TORCH_COLLIDER)
		{
			if (other.GetComponent<ColliderController>().GetFireStatus())
			{
				_torches.Add (other.transform.parent.gameObject);
				Smoke ();
                if (!_played)
                {
                    _main_controller.CloseToForest();
                    _played = true;
                }
                // StartCoroutine (WaitAndFireOn (Random.Range (1, 3)));
            }
		}
		else if (CompareTag(Tags.TREE_FIREPLACE) && other.tag == Tags.TREE_FIREPLACE_COLLIDER)
		{
			if (other.GetComponent<ColliderController>().GetFireStatus())
			{
				StartCoroutine (WaitAndFireOn (Random.Range (1, 8)));
			}
			// print (name + " , " + other.name + " , " + other.GetComponent<ColliderController>().GetFireStatus());
		}
	}

	void OnTriggerExit (Collider other)
	{
		// print (tag + " , " + other.tag);

		if (CompareTag (Tags.TREE_FIREPLACE) && other.tag == Tags.TORCH_COLLIDER)
		{
			if (_torches.Contains (other.transform.parent.gameObject)) {
				_torches.Remove (other.transform.parent.gameObject);
			}

			if (_torches.Count <= 0)
			{
				StartCoroutine(WaitAndSmokeOff(2));

				FireOff ();
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

	IEnumerator WaitAndDestroy(int seconds)
	{
		yield return new WaitForSeconds(seconds);
		Destroy(gameObject);
	}

	IEnumerator WaitAndFireOn(int seconds)
	{
		yield return new WaitForSeconds(seconds);
		FireOn();
	}

	public void FireOn()
	{
		if (!_force_stop_fire)
		{
			_fire_is_on = true;

			if (!_fire) {
				_fire = Instantiate (Resources.Load ("Prefabs/Tree Fireplace Fire"), transform, false) as GameObject;
			}

			if (!_fire.activeSelf)
			{
				_fire.SetActive (true);
			}

			if (!_smoke) {
				_smoke = Instantiate (Resources.Load ("Prefabs/Tree Fireplace Smoke"), transform, false) as GameObject;
				_smoke.SetActive (true);
			}

			if (!_smoke.activeSelf)
			{
				_smoke.SetActive (true);
			}

			if (!_collider) {
				_collider = Instantiate (Resources.Load ("Prefabs/Tree Fireplace Collider"), transform, false) as GameObject;
				_collider.SetActive (true);
			}

			if (!_collider.activeSelf)
			{
				_collider.SetActive (true);
			}

			gameObject.GetComponent<Collider> ().enabled = false;

			_force_stop_fire = false;
		}		
	}

	public void FireOff()
	{
		_fire_is_on = false;
		_force_stop_fire = true;
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

	public void Smoke()
	{
		if (!_smoke)
		{
			_smoke = Instantiate(Resources.Load("Prefabs/Tree Fireplace Smoke"), transform, false) as GameObject;
			_smoke.SetActive(true);
		}
	}

	IEnumerator WaitAndSmokeOff(int seconds)
	{
		yield return new WaitForSeconds(seconds);
		if (_smoke && _smoke.activeSelf)
		{
			_smoke.SetActive(false);
		}
	}
}
