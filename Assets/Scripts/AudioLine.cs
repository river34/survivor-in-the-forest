using UnityEngine;

public class AudioLine
{
    // The default constructor has no parameters. The default constructor 
    // is invoked in the processing of object initializers.
    public AudioLine ()
    {

    }

    // The following constructor has parameters for two of the three 
    // properties. 
    public AudioLine (AudioClip clip, bool played = false, float seconds = 2, bool stop_after = true)
    {
        _clip = clip;
        _played = played;
        _seconds = seconds;
        _stop_after = stop_after;
    }

    // Properties.
    public AudioClip _clip { get; set; }
    public bool _played { get; set; }
    public float _seconds { get; set; }
    public bool _stop_after { get; set; }
}