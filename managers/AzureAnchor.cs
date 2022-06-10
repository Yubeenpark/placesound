using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.SpatialAnchors;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;
using System.Linq;
using System.Threading.Tasks;
using System;

public class AzureAnchor : MonoBehaviour
{
    internal enum AppState
    {
        Placing = 0,
        Saving,
        ReadyToSearch,
        Searching,
        ReadyToNeighborQuery,
        Neighboring,
        Deleting,
        Done,
        ModeCount
    }

    private ARPlaneManager m_ARPlanceManager;
    private ARSession aRSession;
    public ARRaycastManager arRaycastManager;

    public Text Debug1;
    private readonly int numToMake = 2;

    private int locatedCount = 0;
    private System.DateTime dueDate = DateTime.Now;



     // Create a local anchor, perhaps by hit-testing and spawning an object within the scene
     Vector3 hitPosition = new Vector3();
    Vector2 screenCenter = new Vector2(0.5f, 0.5f);
    List<ARRaycastHit> aRRaycastHits = new List<ARRaycastHit>();
    


}