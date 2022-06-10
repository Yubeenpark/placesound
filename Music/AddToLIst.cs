using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System.IO;
using System;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using Microsoft.Azure.SpatialAnchors.Unity.Examples;

public enum Platf { Android }

[Serializable]
/*public static class StaticClass
{
    public static List<GameObject> CrossSceneInformation { get; set; }
}*/

public class AddToLIst : MonoBehaviour
{
    public static Dictionary<string, Item> anchorIDobject;
    static public List<GameObject> save_model;
    public static List<string> AnchorIds;
    Button makemodel;
    public GameObject send;
    public static GameObject _content;
    public PlaceObjectPlane _class;
    AndroidJavaObject obj;
    bool makeModelsign = false;
    public enum platform { Android }
    [Space(10)]
    public platform Platform;
    public float SpaceBetweenTheButtons;
    public Text ShowSongName;
    int NumberS = 0;
    [Header("Music folder")]
    [Tooltip("folder name or directory")]
    public string F_Name;

    [Header("Music Files")]
    public List<string> Names;

    string[] Directorys;
    [Header("Dropdown")] public Dropdown dropdown;


    [Header("Debug")]
    public string ErrorMessage;
    [Tooltip("show error message")]
    public Text Debug1;


    string PatchAndroid;
   
    public GameObject Content;
    private string prefabtype;
    private AudioSource aUdio;
    public int nameofmodel = 0;
    public AudioClip sound;
    private Button button { get { return GetComponent<Button>(); } }
    private static AudioSource soundPlay;
    private AudioSource musicSource;
   
    // Start is called before the first frame update


    void Start()
    {
        if (StaticClass.CrossSceneInformation != null)
        {
            save_model = StaticClass.CrossSceneInformation;
            foreach (GameObject ar in save_model)
            {
                Debug.Log(save_model.Count + "개있음 :    인포메이션으로 가져온 " + ar);
            }
        }
        aUdio = new AudioSource();
        musicSource = new AudioSource();
        //Content.GetComponent<AudioSource>().clip = new AudioSource().clip;
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }
        //_Button.transform.position = POS;
        Debug.Log("스타트. ");
        obj = new AndroidJavaObject("com.example.mediaplayerl.Test2");
        Debug.Log("오브젝트 콜 시작. ");
        obj.Call("start", F_Name, this.gameObject.name, "set");
        Debug.Log("object call시작. " + F_Name + "게임오브젝트" + this.gameObject.name);
        Debug.Log("object call시작. ");
        Content = new GameObject();
        
  
    }

    public static void saveItem(Dictionary<string, Item> _anchorIDobject,GameObject model)
    {
        anchorIDobject = _anchorIDobject;
        save_model.Add(model);


    }

    public void goScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    public void modelprefab(GameObject list)
    {
        
        prefabtype = list.name;
        Debug.Log("컨텐츠 네임" + prefabtype);
    }
    public void STARTFILE()
    {
        try
        {
            Debug1.text = null;
            Directorys = Directory.GetFiles(PatchAndroid, "*.mp3");
            Debug.Log("mp3 file 찾기 " + Directorys);
            StartCoroutine("load");
            Debug.Log("load 시작");
            //SetdropdownOption();
            Debug.Log("SetdropdownOption 끝");
        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug.Log("STARTFILE 에러");
        }

    }
    public void Change(String i)
    {
        obj.Call("start", i, this.gameObject.name, "set");
    }
    // Update is called once per frame
    //private void SetdropdownOption()
    IEnumerator load()
    {
      
            dropdown.options.Clear();
            for (int i = 0; i < Directorys.Length; i++)
            {
                Dropdown.OptionData option = new Dropdown.OptionData();
                string musicPath = Path.GetFullPath(Directorys[i]);
            Debug.Log("디렉토리 안에 있는 것 "+ Directorys[i]);
            Names.Add(Path.GetFileNameWithoutExtension(Directorys[i]));
                option.text = Names[i];
                dropdown.options.Add(option);
                yield return null;
            }


    }
    public void set(string patch)
    {

        PatchAndroid = patch;
        Debug.Log("patchAndroid" + PatchAndroid);
        Debug1.text = patch;

    }

    public void Set(int i)
    {
        try
        {

            NumberS = i;
            Debug.Log("곡이름 " + Names);

            ShowSongName.text = Names[NumberS];
            obj.Call("Set", Names[NumberS]);



        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug1.text = "Set 에러 발생";
        }


    }
    //IEnumerator
    
     IEnumerator GetAudioClip(string filepath)
    {
        
        string  patb = "file://" + filepath;
        Debug.Log("실제 경로" + patb);
        Debug.Log(@"file://" + @filepath);
            using (var uwr = UnityWebRequestMultimedia.GetAudioClip(patb, AudioType.MPEG))
        {
            
                Debug.Log("오디오 클립 가져오기");
            ((DownloadHandlerAudioClip)uwr.downloadHandler).streamAudio = true;

                yield return uwr.SendWebRequest();

                if (uwr.result==UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log(", 주소 에러"+uwr.error);
                    yield break;
                }
                else if(uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("커넥션 에러 에러" + uwr.error);
                yield break;
            }
            else if (uwr.result == UnityWebRequest.Result.DataProcessingError)
            {
                Debug.Log("프로세싱 에러 에러" + uwr.error);
                yield break;
            }
            
            DownloadHandlerAudioClip dlHandler = (DownloadHandlerAudioClip)uwr.downloadHandler;
            try { 
                if (dlHandler.isDone)
                {
                    if (dlHandler.audioClip)
                        Debug.Log("오디오 있다구구우우");
                //musicSource.clip = dlHandler.audioClip;
                    

                    if (dlHandler.audioClip != null)
                    {
                        if (DownloadHandlerAudioClip.GetContent(uwr))
                            Debug.Log("오디오 있다구구우우222");
                        Debug.Log("null이 아님");
                        //musicSource.clip = DownloadHandlerAudioClip.GetContent(uwr);
                        //musicSource.pitch = 1;
                        //musicSource.Play();
                        //audio.Play();
                        //Content.GetComponent<AudioSource>().clip = DownloadHandlerAudioClip.GetContent(uwr);
                        Debug.Log("music 설정 ");
                    //musicSource = gameObject.AddComponent<AudioSource>();
                    Debug.Log("musicsorce넣기  ");
                        _content.AddComponent<AudioSource>();
                        AudioClip test = DownloadHandlerAudioClip.GetContent(uwr);
                        Debug.Log("test넣기완료  ");
                        //Content.clip = DownloadHandlerAudioClip.GetContent(uwr);
                        // _content.clip = DownloadHandlerAudioClip.GetContent(uwr);
                        _content.GetComponent<AudioSource>().clip = test;
                        Debug.Log("musicsorce넣기완료  ");
                        _content.GetComponent<AudioSource>().pitch = 1;
                        //_content.GetComponent<AudioSource>().Play();
                        //musicSource = gameObject.AddComponent<AudioSource>();
                        //musicSource.clip= DownloadHandlerAudioClip.GetContent(uwr);
                        //musicSource.spatialBlend = 1;
                        //musicSource.minDistance = 1;
                        //musicSource.maxDistance = 4;
                        //musicSource.rolloffMode = AudioRolloffMode.Linear;
                        //Content.GetComponent<AudioSource>().Play();
                        try
                    {
                            //musicSource.clip = DownloadHandlerAudioClip.GetContent(uwr);
                            //musicSource.clip = Content.GetComponent<AudioSource>().clip;
                            // musicSource.Play();

                            //Content.GetComponent<AudioSource>().clip = DownloadHandlerAudioClip.GetContent(uwr);

                            
                     
              
                             _content.GetComponent<AudioSource>().minDistance = 1;
                            _content.GetComponent<AudioSource>().maxDistance = 3;
                            _content.GetComponent<AudioSource>().loop = true;
                            _content.GetComponent<AudioSource>().spatialBlend = 1;
                            _content.GetComponent<AudioSource>().rolloffMode = AudioRolloffMode.Linear;
                             Debug.Log("스패셜 블렌드:" + _content.GetComponent<AudioSource>().spatialBlend);
                            if (_content.GetComponent<AudioSource>().clip)
                                Debug.Log("클립있음");
                            //_content.GetComponent<AudioSource>().Play();
                            DontDestroyOnLoad(_content);
                            
                            if(anchorIDobject!=null)
                            {
                                Debug.Log("저장해놓은 걸 꺼내봅시다  "+anchorIDobject);
                                //anchorIDobject = AzureSpatialAnchorsNearbyDemoScript.anchorIDobject;
                                
                            }
                            if(save_model!=null)
                            {
                                foreach(GameObject a in save_model)
                                {
                                    Debug.Log("리스트에 저장해놓은 오브젝트를 꺼내봅시다  " + a);
                                }
                            }
                           
                            Debug.Log("컨텐트는 뭔가 " + _content);

                            
                            // Debug.Log("이거는 뭔가 " + Content);
                            
                            
                           // AzureSpatialAnchorsNearbyDemoScript.getObject(_content);
                            //Content.GetComponent<AudioSource>().Play();
                            ///넣어야함. 


                        }
                    catch (NullReferenceException e)
                    {
                        Debug.Log("오류 발생 " + e);
                    }
                   
                }
                    else
                    {
                        Debug.Log("오디오 클립을 찾지 못함. .");
                    }
                }
                else
                {
                    Debug.Log("The download process is not completely finished.");
                }
        }
                    catch (NullReferenceException e)
        {
            Debug.Log("오류 발생 " + e);
        }
    }
        }

    
    static List<ARRaycastHit> s_hits = new List<ARRaycastHit>();
    public void Music(Dropdown dropdown)
    {
        try
        {
            int index = dropdown.value;

            string audioPath = Path.GetFullPath(Directorys[index]);
            Debug.Log("경로" + audioPath);
            if (File.Exists(audioPath))
                Debug.Log("디렉토리 안에 있음 " + audioPath);



            Debug.Log("getmusic 끝");


            _content = Resources.Load<GameObject>(prefabtype);

            if (_content)
            {
                Debug.Log("컨텐트 잘 가져옴");
            }
            else
            {
                Debug.Log("컨텐트 못 불러옴. ");
            }
            Debug.Log("content설정완료 ");
            //byte[] byteTexture = System.IO.File.re()

            StartCoroutine(GetAudioClip(audioPath));


            //_class.MakeModel(_content);
        }
        catch (Exception e)
        {
            Debug.Log("에러 발생:" + e);
        }

      
    }
    /*
    IEnumerator load()
    {
        try
        {
            Debug.Log("directory load: " + Directorys);
            Debug.Log("directory number: " + Directorys.Length);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        for (int i = 0; i < Directorys.Length; i++)
        {

            try
            {
                // _Button = Instantiate(template);

                Names.Add(Path.GetFileNameWithoutExtension(Directorys[i]));
                _Button.name = "Button" + i;
                Debug.Log("이름:" + Names[i]);
                _Button.GetComponent<MusicButton>().Indicator = SelectionIndicator;
                _Button.GetComponentInChildren<Text>().text = Names[i];
                _Button.GetComponent<MusicButton>().Class = GetComponent<NativeMusic>();
                _Button.GetComponent<MusicButton>().Song_Number = i;
                Instantiate<Toggle>(_Button, Content, false);
                Debug.Log("버튼이란뭔가" + _Button);
                _Button.transform.localPosition = _Button.transform.localPosition -= new Vector3(0, SpaceBetweenTheButtons, 0);
                //Content.sizeDelta = new Vector2(Content.sizeDelta.x, Content.sizeDelta.y + AddHeightToContent);
            }
            catch (NullReferenceException e)
            {
                Debug.Log(e);
            }



            yield return null;
        }

    }

    void Update()
    {
        
    }
    */
}
