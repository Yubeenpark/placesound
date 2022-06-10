// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.SceneManagement;
using Microsoft.Azure.SpatialAnchors.Unity.Examples;


namespace Microsoft.Azure.SpatialAnchors.Unity.Examples
{
    public class Item
    {
        public string sceneName;
        public GameObject model;

        public Item(string _sceneName, GameObject _model)
        {
            this.sceneName = _sceneName;
            this.model = _model;
        }
        public void Show()
        {
            Debug.Log(this.sceneName);
            Debug.Log(this.model);
        }
     
    }
    
    public class AzureSpatialAnchorsNearbyDemoScript : DemoScriptBase
    {
        internal enum AppState
        {
            Placing = 0,
            Saving,
            ReadyToSearch,
            Searching,
            ReadyToNeighborQuery,
            Neighboring,
            //Deleting,
            Done,
            AddAnother,
            ModeCount,
            SearchingEverything
        }

        private readonly Color[] colors =
        {
            Color.white,
            Color.magenta,
            Color.magenta,
            Color.cyan,
            Color.magenta,
            Color.green,
            Color.grey,
            Color.grey
        };

        private readonly Vector3[] scaleMods =
        {
            new Vector3(0,0,0),
            new Vector3(0,0,0),
            new Vector3(0,0,0),
            new Vector3(0,0,.1f),
            new Vector3(0,0,0),
            new Vector3(0,.1f,0),
            new Vector3(0,0,0),
            new Vector3(0,0,0)
        };
        private readonly int numToMake = 1;
        //AddToLIst addtoList;
        private AppState _currentAppState = AppState.Placing;

        DemoScriptBase _class;
        AppState currentAppState
        {
            get
            {
                return _currentAppState;
            }
            set
            {
                if (_currentAppState != value)
                {
                    Debug.LogFormat("State from {0} to {1}", _currentAppState, value);
                    _currentAppState = value;
                    
                }
            }
        }
        /// <summary>
        /// 여기 고쳐야함 anchorid 이름 붙이기중.
        /// </summary>
        /// 
        protected static String SceneName = "";
        public static int anchorNum = 0;
       // protected static GameObject AnchorModel;
       // public static AnchorIdFromScene anchorIDs;
        public static List<string> AnchorIds;
        public Text text;
        //public static List<string> anchorIds = new List<string>();
        readonly Dictionary<AppState, Dictionary<string, GameObject>> spawnedObjectsPerAppState = new Dictionary<AppState, Dictionary<string, GameObject>>();
        InputInteractionBase inputInteractionBase;

        Dictionary<string, GameObject> spawnedObjectsInCurrentAppState
        {
            get
            {
                if (spawnedObjectsPerAppState.ContainsKey(_currentAppState) == false)
                {
                    spawnedObjectsPerAppState.Add(_currentAppState, new Dictionary<string, GameObject>());
                }

                return spawnedObjectsPerAppState[_currentAppState];
            }
        }
        public static List<GameObject> modelObjects;
        //public static Dictionary<string , Item> anchorIDobject;
        public static List<string> SceneList;
        //key = anchorId, GameObject에 scene name넣어야 함. 
        bool isAdding = false;
        /// <summary>
        /// Start is called on the frame when a script is enabled just before any
        /// of the Update methods are called the first time.
        /// </summary>
        public override void Start()
        {
        
            if(StaticClass.CrossSceneInformation!=null && (StaticClass.CrossAnchor!=null))
            {
                modelObjects = StaticClass.CrossSceneInformation;
                AnchorIds = StaticClass.CrossAnchor;
                SceneList = StaticClass.SceneList;
                foreach (GameObject a in modelObjects)
                {
                    Debug.Log("리스트에 저장해놓은 오브젝트를 꺼내봅시다  " + a);
                }
            }
            else
            {
                modelObjects = new List<GameObject>();
                AnchorIds = new List<string>();
                SceneList = new List<string>();
            }

        //AnchorModel = new GameObject();
        /* anchorIDs.setName*/
       
            Debug.Log(">>Azure Spatial Anchors Demo Script Start");
            //anchorIDobject = new Dictionary< string, Item>();
            base.Start();

            if (!SanityCheckAccessConfiguration())
            {
                return;
            }
            if(AddToLIst._content)
                Debug.Log("  스판오브젝트_content   :  " + AddToLIst._content);
            else
            {
                Debug.Log("  스판오브젝트_content   :  업승ㅁ");
            }
            feedbackBox.text = "Find nearby demo.  First, we need to place a few anchors. Tap somewhere to place the first one";

            Debug.Log("Azure Spatial Anchors Demo script started");
        }

        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        public override void Update()
        {
            base.Update();

            HandleCurrentAppState();
        }
  
        public static void  getModelLIst(List<GameObject> a)
        {
            modelObjects = a;
        }
        public static void getSceneName(string name)
        {
            Debug.Log("씬 이름 현재:  " + SceneName);

            Debug.Log("씬 이름 들어온값:  " + name);


            SceneName = name;
            SceneList = StaticClass.SceneList;
            SceneList.Add(SceneName);
            //anchorIDs = new AnchorIdFromScene(SceneName);
            //SpecificObject ac = AddToLIst._content.GetComponent<SpecificObject>();
            //ac.sceneName = SceneName;
            Debug.Log("앵커 이름 설정함" + SceneName);

        }
            
        private void HandleCurrentAppState()
        {
            int timeLeft = (int)(dueDate - DateTime.Now).TotalSeconds;
            switch (currentAppState)
            {
                case AppState.ReadyToSearch:
                    feedbackBox.text = "Next: Tap to start looking for just the first anchor we placed.";
                    break;
                case AppState.Searching:
                    feedbackBox.text = $"Looking for the first anchor you made. Give up in {timeLeft}";
                    if (timeLeft < 0)
                    {
                        Debug.Log("Out of time");
                        // Restart the demo..
                        feedbackBox.text = "Failed to find the first anchor. Try again.";
                        currentAppState = AppState.Done;
                    }
                    break;
                case AppState.SearchingEverything:
                    feedbackBox.text = "find everything ";
                    break;
                case AppState.ReadyToNeighborQuery:
                    feedbackBox.text = "Next: Tap to start looking for anchors nearby the first anchor we placed.";
                    break;
                case AppState.AddAnother:
                    feedbackBox.text = "Next: Add another model.";
                    break;
                case AppState.Neighboring:
                    // We should find all anchors except for the anchor we are using as the source anchor.
                    feedbackBox.text = $"Looking for anchors nearby the first anchor. {locatedCount}/{numToMake - 1} {timeLeft}";
                    if (timeLeft < 0)
                    {
                        feedbackBox.text = "Failed to find all the neighbors. Tap to delete anchors.";
                        //currentAppState = AppState.Deleting;
                    }
                    if (locatedCount == numToMake - 1)
                    {
                        feedbackBox.text = "Found them all! ";
                        //currentAppState = AppState.Deleting;
                        //SceneManager.LoadScene("AzureSpatialAnchorsDemoLauncher"); 
                        currentAppState = AppState.Done;
                        //SceneManager.LoadScene("AzureSpatialAnchorsDemoLauncher");


                    }
                    break;
                case AppState.Done:
                    currentAppState = AppState.AddAnother;
                    break;

            }
        }
        public void setAppastate()
        {
            currentAppState = AppState.SearchingEverything;
        }
        protected override bool IsPlacingObject()
        {
            return currentAppState == AppState.Placing;
        }

        protected override Color GetStepColor()
        {
            return colors[(int)currentAppState];
        }

        private int locatedCount = 0;

        protected override void OnCloudAnchorLocated(AnchorLocatedEventArgs args)
        {
            base.OnCloudAnchorLocated(args);

            if (args.Status == LocateAnchorStatus.Located)
            {
                if (currentAppState == AppState.SearchingEverything)
                {
                    Debug.Log("          SearchingEverything이다.");
                    foreach (GameObject a in modelObjects)
                    {
                        Debug.Log("      각각 " + a + "이다.");
                        UnityDispatcher.InvokeOnAppThread(() =>
                        {
                            locatedCount++;
                            currentCloudAnchor = args.Anchor;

                            Pose anchorPose = currentCloudAnchor.GetPose();
                            SpawnOrMoveCurrentAnchoredObject(anchorPose.position, anchorPose.rotation);

                            spawnedObject.transform.localScale += scaleMods[(int)currentAppState];
                            anchoredObjectPrefab = a;
                            spawnedObject = null;



                        });
                    }

                }
                else
                {
                    UnityDispatcher.InvokeOnAppThread(() =>
                    {
                    locatedCount++;
                    currentCloudAnchor = args.Anchor;

                    Pose anchorPose = currentCloudAnchor.GetPose();
                    SpawnOrMoveCurrentAnchoredObject(anchorPose.position, anchorPose.rotation);

                    spawnedObject.transform.localScale += scaleMods[(int)currentAppState];
                    spawnedObject = null;

                        if (currentAppState == AppState.Searching)
                        {
                            currentAppState = AppState.ReadyToNeighborQuery;
                        }
                    });
                 }
            }

        }
          

        private DateTime dueDate = DateTime.Now;
        private readonly List<GameObject> allSpawnedObjects = new List<GameObject>();
        private readonly List<Material> allSpawnedMaterials = new List<Material>();

        protected override void SpawnOrMoveCurrentAnchoredObject(Vector3 worldPos, Quaternion worldRot)
        {
            if (currentCloudAnchor != null && spawnedObjectsInCurrentAppState.ContainsKey(currentCloudAnchor.Identifier))
            {
                spawnedObject = spawnedObjectsInCurrentAppState[currentCloudAnchor.Identifier];
            }

            bool spawnedNewObject = spawnedObject == null;

            base.SpawnOrMoveCurrentAnchoredObject(worldPos, worldRot);

            if (spawnedNewObject)
            {
                allSpawnedObjects.Add(spawnedObject);
                allSpawnedMaterials.Add(spawnedObjectMat);

                if (currentCloudAnchor != null && spawnedObjectsInCurrentAppState.ContainsKey(currentCloudAnchor.Identifier) == false)
                {
                    spawnedObjectsInCurrentAppState.Add(currentCloudAnchor.Identifier, spawnedObject);
                }
            }

            #if WINDOWS_UWP || UNITY_WSA
            if (currentCloudAnchor != null
                    && spawnedObjectsInCurrentAppState.ContainsKey(currentCloudAnchor.Identifier) == false)
            {
                spawnedObjectsInCurrentAppState.Add(currentCloudAnchor.Identifier, spawnedObject);
            }
            #endif
        }
        public void Reset()
        {
           
        }

        public async override Task AdvanceDemoAsync()
        {
            switch (currentAppState)
            {
                case AppState.Placing:
                    isAdding = false;
                    if (spawnedObject != null)
                    {
                        currentAppState = AppState.Saving;
                        if (!CloudManager.IsSessionStarted)
                        {
                            await CloudManager.StartSessionAsync();
                        }
                        await SaveCurrentObjectAnchorToCloudAsync();
                    }
                    break;
                case AppState.ReadyToSearch:
                    await DoSearchingPassAsync();
                    break;
                case AppState.ReadyToNeighborQuery:
                    DoNeighboringPassAsync();
                    break;
                case AppState.SearchingEverything:
                    isAdding = true;
                    await DoSearchingEverything();
                    //int nums = AnchorIds.Count;
                   /* for (int i = 0; i < nums; i++)
                    {
                        anchoredObjectPrefab = modelObjects[i];
                        i++;
                    }*/
                    break;
                case AppState.AddAnother:
                    //LoaderUtility.GetActiveLoader();
                    //ScriptableObjects(anchorIds);
                    
                    Debug.Log("LoaderUtility는 함");
                    DontDestroyOnLoad(modelObjects[anchorNum]);
                    DontDestroyOnLoad(AddToLIst._content);
                    StaticClass.CrossSceneInformation= modelObjects;
                    StaticClass.CrossAnchor = AnchorIds;
                    StaticClass.SceneList = SceneList;
                    
                    //AddToLIst.saveItem(anchorIDobject, AddToLIst._content);
                    Debug.Log("죽기 전에 보내기");
                    goToAddMore();


                    break;
                /*case AppState.Deleting:
                    foreach (var anchorIdentifier in anchorIds)
                    {
                        Debug.Log("앵커아이디: " + anchorIds);
                        CloudSpatialAnchor anchorToBeDeleted = await CloudManager.Session.GetAnchorPropertiesAsync(anchorIdentifier);
                        if (anchorToBeDeleted == null)
                        {
                            Debug.LogError("Failed to get properties for anchor: " + anchorIdentifier);
                            continue;
                        }
                        await CloudManager.DeleteAnchorAsync(anchorToBeDeleted);
                    }
                    
                    CleanupObjectsBetweenPasses();
                    currentAppState = AppState.Done;
                    feedbackBox.text = $"Finished deleting anchors. Tap to restart.";
                    break;*/
                case AppState.Done:
                    await CloudManager.ResetSessionAsync();
                    currentAppState = AppState.Placing;
                    feedbackBox.text = $"Place an object. {allSpawnedObjects.Count}/{numToMake} ";
                    break;
            }
        }
       //저장하기

        public async void delete()
        {
            foreach (var anchorIdentifier in AnchorIds)
            {
                Debug.Log("앵커아이디: " + AnchorIds);
                CloudSpatialAnchor anchorToBeDeleted = await CloudManager.Session.GetAnchorPropertiesAsync(anchorIdentifier);
                if (anchorToBeDeleted == null)
                {
                    Debug.LogError("Failed to get properties for anchor: " + anchorIdentifier);
                    continue;
                }
                await CloudManager.DeleteAnchorAsync(anchorToBeDeleted);
            }

            CleanupObjectsBetweenPasses();
            currentAppState = AppState.Done;
            feedbackBox.text = $"Finished deleting anchors. Tap to restart.";
        }
        protected override async Task OnSaveCloudAnchorSuccessfulAsync()
        {
            await base.OnSaveCloudAnchorSuccessfulAsync();
            /*modelObjects = StaticClass.CrossSceneInformation;
            foreach(GameObject ar in modelObjects)
            {
                Debug.Log(modelObjects.Count+ "개있음 :    인포메이션으로 가져온 " + ar);
            }*/
           
            modelObjects.Add(AddToLIst._content);
            foreach (GameObject ar in modelObjects)
            {
                Debug.Log("추가한 것에 "+modelObjects.Count + "개있음 :    인포메이션으로 가져온 " + ar);
            }
            string texting = string.Empty;
            for (int i =0; i< AnchorIds.Count;i++)
            {
                texting += "ancrhorID: " + AnchorIds[i];
            }
            text.text = texting;
            Debug.Log("모델 오브젝트 넣었음");
            Debug.Log("앵커에 추가하기 ");
            // ReadFile.WriteToFile("test", anchorIDobject);
            Debug.Log("   지금 저장하고 있는 컨텐트:  "+AddToLIst._content);
           
            
            AnchorIds.Add(currentCloudAnchor.Identifier);
           
            //anchorIDs.Add(currentCloudAnchor.Identifier);
            Debug.Log("            앵커아이디 : " + AnchorIds[0]);

            //spawnedObject = null;
            //currentCloudAnchor = null;
            if (allSpawnedObjects.Count < numToMake)
            {
                feedbackBox.text = $"Saved...Make another {allSpawnedObjects.Count}/{numToMake} ";
                currentAppState = AppState.Placing;
            }
            else
            {
                feedbackBox.text = "Saved... ready to start finding them.";
                currentAppState = AppState.ReadyToSearch;
            }
        }

        protected override void OnSaveCloudAnchorFailed(Exception exception)
        {
            base.OnSaveCloudAnchorFailed(exception);
        }

        private async Task DoSearchingPassAsync()
        {
            await CloudManager.ResetSessionAsync();
            Debug.Log("AzureDemoscript에서 Do searching pass Async중 ");
            SetGraphEnabled(true); // set LocateStrategy to VisualInformation 원래 false
            anchorNum = AnchorIds.Count - 1;
            IEnumerable<string> anchorsToFind = new[] { AnchorIds[anchorNum] };
           
            Debug.Log("   앵커아이디가 무엇이냐 "+ AnchorIds + "anchorIds: "+ AnchorIds[anchorNum]);
            SetAnchorIdsToLocate(anchorsToFind);
            locatedCount = anchorNum;
            dueDate = DateTime.Now.AddSeconds(30);
            currentWatcher = CreateWatcher();
      
            currentAppState = AppState.Searching;

        }
        //다 찾기 로드하기
  
        private async Task DoSearchingEverything()
        {
            LocateAnchorStatus a = LocateAnchorStatus.Located;
            Debug.Log("존재하는것"+a);
               //savemanager로 로드 
               await CloudManager.ResetSessionAsync();
            SetGraphEnabled(false);
            //GameObject ob;
            int nums = AnchorIds.Count;
           
            Debug.Log("     안에 몇개 들어 있는가: " + nums);
            for (int i = 0; i < nums; i++)
            {
                foreach (GameObject ab in modelObjects)
                    Debug.Log("게임오브젝트 안에 있는 것" + ab);
                
                //anchoredObjectPrefab = modelObjects[i];
                IEnumerable<string> AnchorToFind = new[] { AnchorIds[i] };
                Debug.Log("    앵커아이디: " + AnchorIds[i]);
                Debug.Log("    프리팹은 무엇인가 " + anchoredObjectPrefab);
                SetAnchorIdsToLocate(AnchorToFind);
                locatedCount = nums;
                dueDate = DateTime.Now.AddSeconds(30);

                currentWatcher = CreateWatcher();
            }
            //currentWatcher = CreateWatcher();
            Debug.Log("   워쳐나와라");
            
            currentAppState = AppState.SearchingEverything;

        }


        private void DoNeighboringPassAsync()
        {
            Debug.Log("      DoNeighboringPassAsync]에서 DoNeighboringPassAsync중 ");

            SetGraphEnabled(true, true); // set LocateStrategy to Relationship
            ResetAnchorIdsToLocate();
            SetNearbyAnchor(currentCloudAnchor, 10, numToMake);
            locatedCount = 0; 
            dueDate = DateTime.Now.AddSeconds(30);
            currentWatcher = CreateWatcher();
            currentAppState = AppState.Neighboring;
        }

        private void CleanupObjectsBetweenPasses()
        {
            foreach (GameObject go in allSpawnedObjects)
            {
                Destroy(go);
            }
            allSpawnedObjects.Clear();

            foreach (Material m in allSpawnedMaterials)
            {
                Destroy(m);
            }
            allSpawnedMaterials.Clear();

            currentCloudAnchor = null;
            spawnedObject = null;
            spawnedObjectMat = null;
            spawnedObjectsPerAppState.Clear();
            AnchorIds.Clear();
        }
       
    }


}
