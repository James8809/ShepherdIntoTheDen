using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CombatEffectManager : MonoBehaviour
{
    private bool _inTimeStop = false;
    private bool _inScreenShake = false;
    private const float frameTime = 1.0f / 60.0f;
    private CinemachineBasicMultiChannelPerlin _cinemachinePerlin;
    [SerializeField] private PlayerReferenceManager _referenceManager;

    private int frameCountShake;

    private void OnEnable()
    {
        _referenceManager.EffectManager = this;
    }
    
    private void OnDisable()
    {
        _referenceManager.EffectManager = null;
    }

    private void Awake()
    {
        _cinemachinePerlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void StopTime(int frames)
    {
        StartCoroutine(StopTimeRoutine(frames));
    }

    public void ShakeCamera(int frames, float magnitude)
    {
        StartCoroutine(ShakeCameraRoutine(frames, magnitude));
    }

    private IEnumerator StopTimeRoutine(int frames)
    {
        if (_inTimeStop)
        {
            yield break;
        }

        _inTimeStop = true;
        Time.timeScale = 0.0f;
        yield return new WaitForSecondsRealtime(frames * frameTime);
        Time.timeScale = 1.0f;
        _inTimeStop = false;
    }

    private IEnumerator ShakeCameraRoutine(int frames, float magnitude)
    {
        frameCountShake = 0;
        if (_inScreenShake)
        {
            yield break;// currently only one camera shake event. TODO: change this so that it resets time.
        }

        _inScreenShake = true;
        _cinemachinePerlin.m_AmplitudeGain = magnitude;
        float framePct = 1.0f / frames;
        
        for (; frameCountShake < frames; frameCountShake++)
        {
            yield return null;
            _cinemachinePerlin.m_AmplitudeGain = Mathf.Lerp(magnitude, 0, frameCountShake * framePct);
        }

        _cinemachinePerlin.m_AmplitudeGain = 0; // reset to 0 to be sure.
        _inScreenShake = false;
    }
}
