using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicSettingsConnector : MonoBehaviour
{
    [SerializeField] DLSSSettings DLSS;

    private void Start()
    {
        DLSS.InitializeSetting();
    }

}
