using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    private CinemachineCamera vCam;
    private CinemachineBasicMultiChannelPerlin noise;

    static CamShake instance;
    public static CamShake Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null) instance = this;

        vCam = GetComponent<CinemachineCamera>();
        noise = vCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        if (noise == null) return;
        noise.AmplitudeGain = intensity;
        Invoke("StopShakeCamera", time);
    }

    private void StopShakeCamera()
    {
        noise.AmplitudeGain = 0f;
    }
}
