using UnityEngine;
using System.Collections;

// Scales floor to calibrated room size
public class ViveFloor : MonoBehaviour
{
    void Awake()
    {
        Valve.VR.HmdQuad_t roomSize = new Valve.VR.HmdQuad_t();
        SteamVR_PlayArea.GetBounds(SteamVR_PlayArea.Size.Calibrated, ref roomSize);
        Vector3 calibratedScale = transform.localScale;
        calibratedScale.x = Mathf.Abs(roomSize.vCorners0.v0 - roomSize.vCorners1.v0);
        calibratedScale.z = Mathf.Abs(roomSize.vCorners0.v2 - roomSize.vCorners3.v2);
        transform.localScale = calibratedScale;
    }
}
