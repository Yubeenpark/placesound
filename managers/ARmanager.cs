using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class ARmanager : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        
        var xrManagerSettings = UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager;
        xrManagerSettings.DeinitializeLoader();

        SceneManager.LoadScene("AzureSpatialAnchorsBasicDemo", LoadSceneMode.Single);
        xrManagerSettings.InitializeLoaderSync();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
