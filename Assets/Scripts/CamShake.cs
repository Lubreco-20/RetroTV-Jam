using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    CinemachineCamera virtualCam;
    CinemachineBasicMultiChannelPerlin multiChannel;

    private void Awake()
    {
        virtualCam = GetComponent<CinemachineCamera>();
        //multiChannel = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        multiChannel.AmplitudeGain = intensity;
        Invoke("StopShakeCamera", time);
    }

    public void StopShakeCamera()
    {
        multiChannel.AmplitudeGain = 0f;
    }
}
