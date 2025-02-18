using UnityEngine;
using TMPro;
using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.NVIDIA;

[Serializable]
public class DLSSSettings : DropDownSetting<string, int>
{
    [SerializeField] Camera camera = Camera.main;

    public DLSSSettings()
    {
        name = GetType().Name; ;
    }
    protected override void GetSettingData(out int data)
    {
        if (PlayerPrefs.HasKey(name))
        {
            data = PlayerPrefs.GetInt(name);
        }
        else
        {
            data = 0;
        }
    }

    protected override void SetSettingData(int data)
    {
        if (camera != null)
        {
            HDAdditionalCameraData hdCam = camera.GetComponent<HDAdditionalCameraData>();
            hdCam.deepLearningSuperSamplingQuality = (uint)data;
            PlayerPrefs.SetInt(name, data);
        }
    }
}
