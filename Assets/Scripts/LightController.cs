using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour {

    private float _sunset_init_rotation_x = 50.5f;

    private float _sunset_final_rotation_x = 20.5f;

    private float _sunset_time = 15f;

    private float _sunset_speed;

    private float _sunset_init_intensity = 1.5f;

    private float _sunset_final_intensity = 0f;

    private float _sunset_intensity_speed;

    private bool _is_sunset;

    private float _sunrise_init_rotation_x = 20.5f;

    private float _sunrise_final_rotation_x = 50.5f;

    private float _sunrise_time = 15f;

    private float _sunrise_speed;

    private float _sunrise_init_intensity = 0f;

    private float _sunrise_final_intensity = 2f;

    private float _sunrise_intensity_speed;

    private bool _is_sunrise;

    void Start ()
    {
        _sunset_speed = (_sunset_final_rotation_x - _sunset_init_rotation_x) / _sunset_time;

        _sunset_intensity_speed = (_sunset_final_intensity - _sunset_init_intensity) / _sunset_time;

        _is_sunset = false;

        _sunrise_speed = (_sunrise_final_rotation_x - _sunrise_init_rotation_x) / _sunrise_time;

        _sunrise_intensity_speed = (_sunrise_final_intensity - _sunrise_init_intensity) / _sunrise_time;

        _is_sunrise = false;

        // StartSunset ();

        // StartSunrise();
    }

	void Update ()
    {
        //  print("_is_sunset : " + _is_sunset);
        if (_is_sunset)
        {
             print(transform.eulerAngles.x + " , " + _sunset_final_rotation_x);

            if (transform.eulerAngles.x >= _sunset_final_rotation_x)
            {
                transform.Rotate (_sunset_speed * Time.deltaTime, 0, 0);
            }

            if (gameObject.GetComponent<Light>().intensity >= _sunset_final_intensity)
            {
                gameObject.GetComponent<Light>().intensity += _sunset_intensity_speed * Time.deltaTime;
            }
        }
        else if (_is_sunrise)
        {            
            if (transform.eulerAngles.x <= _sunrise_final_rotation_x)
            {
                transform.Rotate(_sunrise_speed * Time.deltaTime, 0, 0);
            }

            if (gameObject.GetComponent<Light>().intensity <= _sunrise_final_intensity)
            {
                gameObject.GetComponent<Light>().intensity += _sunrise_intensity_speed * Time.deltaTime;
            }
        }
    }

    public void SetSunset (bool is_sunset)
    {
        _is_sunset = is_sunset;
    }

    public void StartSunset ()
    {
        SetSunset (true);

        SetSunrise (false);

        transform.eulerAngles = new Vector3 (_sunset_init_rotation_x, 0, 0);

        gameObject.GetComponent<Light>().intensity = _sunset_init_intensity;
    }

    public void SetSunrise (bool is_sunrise)
    {
        _is_sunrise = is_sunrise;
    }

    public void StartSunrise()
    {
        SetSunrise (true);

        SetSunset (false);

        transform.eulerAngles = new Vector3(_sunrise_init_rotation_x, 0, 0);

        gameObject.GetComponent<Light>().intensity = _sunrise_init_intensity;
    }
}
