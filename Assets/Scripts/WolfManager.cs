using UnityEngine;
using System.Collections;

public class WolfManager : MonoBehaviour
{
    public Transform[] _spawnPoints;

    private float _min_spawn_time = 15f;

    private float _max_spawn_time = 20f;

    private int _total_num;

    private int _current_num;

    private int _default_total_num = 6;

    private int _default_current_num = 1;

    private MainController _main_controller;

    private bool _reminder_played;

    void Start ()
    {
        // Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
        InvokeRepeating ("Spawn", Random.Range (_min_spawn_time, _max_spawn_time), Random.Range (_min_spawn_time, _max_spawn_time));

        _total_num = 0;
        _current_num = 0;

        _main_controller = GameObject.Find("Root").GetComponent<MainController>();
    }

    void Spawn ()
    {
        // Find a random index between zero and one less than the number of spawn points.
        int spawnPointIndex = Random.Range(0, _spawnPoints.Length);

        if (_total_num < _default_total_num && _current_num < _default_current_num)
        {
            // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
            GameObject _wolf = (GameObject) Instantiate(Resources.Load("Prefabs/Wolf_final"), _spawnPoints[spawnPointIndex].position, _spawnPoints[spawnPointIndex].rotation, transform);

            _total_num++;
            _wolf.GetComponent<WolfController>().index = _total_num;
            _current_num++;
        }
    }

    void Update ()
    {
        if (_main_controller.GameIsEnd())
        {
            return;
        }

        if (_total_num >= _default_total_num)
        {
            // trigger the "rescue team and gunshot" event
            GameObject.Find("Root").GetComponent<MainController>().Survive();
        }
    }

    public void DecreaseCurrentNum ()
    {
        _current_num--;
    }

    public int GetCurrentNum ()
    {
        return _current_num;
    }

    public void PlayReminder ()
    {
        if (!_reminder_played)
        {
            _main_controller.HearWolf();
            _reminder_played = true;
        }
    }
}
