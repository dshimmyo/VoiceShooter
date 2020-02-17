using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    private Transform shooterOriginXform;

    private float MicLoudness = 0;
    private string _device;
    private AudioClip _clipRecord = null;//new AudioClip();
    private int _sampleWindow = 128;

    private void OnEnable()
    {
        InitMic();
    }

    void InitMic()
    {
        if (_device == null)
        {
            _device = Microphone.devices[0];
            _clipRecord = Microphone.Start(_device, true, 999, 44100);
            Debug.Log(_clipRecord);
        }
    }

    float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1);
        if (micPosition < 0) return 0;
        _clipRecord.GetData(waveData, micPosition);
        for (int i = 0; i < _sampleWindow; ++i)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }

    void Start()
    {
        shooterOriginXform = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space) || LevelMax() > 0.1)
        {
            GameObject newProjectile = GameObject.Instantiate(projectile, shooterOriginXform.position, shooterOriginXform.rotation);
            Rigidbody rb = newProjectile.GetComponent<Rigidbody>();
            rb.velocity = shooterOriginXform.forward * 10f;
        }
    }
}
