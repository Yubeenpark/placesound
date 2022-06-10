using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System.IO;
using System;

public enum Plat{Android}

[Serializable]



public class NativeMusic : MonoBehaviour
{

    AndroidJavaObject obj;
    public enum platform { Android }
    [Space(10)]
    public platform Platform;
    public float SpaceBetweenTheButtons;
    public Text ShowSongName;

    [Header("Music folder")]
    [Tooltip("folder name or directory")]
    public string F_Name;

    [Header("Music Files")]
    public List<string> Names;
    string[] Directorys;

    [Header("Game Objects")]
    [Tooltip("Hierarchy Game Object")]
    public GameObject SelectionIndicator;
    public GameObject Play, Pause;

    [Header("Debug")]
    public string ErrorMessage;
    [Tooltip("show error message")]
    public Text Debug1;
    [Tooltip("show folder location")]
    public Text debug2;
    // 
    string PatchAndroid;
    Vector3 POS = new Vector3(0, 0, 0);
    int NumberS = 0;

    [Header("Prefabs")]
    public Toggle _Button;
    //public GameObject template;
    public int modelsName = 0;

    [Header("parents")]
    [Tooltip("Hierarchy ParentButton")]
    public RectTransform Content;
    public float AddHeightToContent;

    [Header("Bools")]
    public bool Random_b;
    public bool Sequence_b = true;
    public bool Repeat_b;
    public bool first_time = true;
    public bool pause;

    public GameObject Musicmd;
    public GameObject premodel;

    ///////////////////////////do not change//////////////

    void Update() {
        if (Names.Count != 0) {
            if (Random_b == true && first_time == false && obj.Call<Boolean>("IsPlaying", false) == false && pause == false)
            {
                _Random();
            }
            else if (Sequence_b == true && first_time == false && obj.Call<Boolean>("IsPlaying", false) == false && pause == false)
            {
                Next();
            }
            else if (Repeat_b == true && first_time == false && obj.Call<Boolean>("IsPlaying", false) == false && pause == false)
            {
                Set(NumberS);
            }
        }


    }
    void Start()
    {
        Musicmd = new GameObject("name");
        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
        {
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
        }
        _Button.transform.position = POS;
        obj = new AndroidJavaObject("com.example.mediaplayerl.Test2");
        obj.Call("start", F_Name, this.gameObject.name, "set");
        Debug.Log("object call시작. " + F_Name + "게임오브젝트" + this.gameObject.name);
        Debug.Log("object call시작. ");
    }

    public void Change(String i)
    {
        obj.Call("start", i, this.gameObject.name, "set");
    }
    ///////Send message from some button to load Music/////

    public void START()
    {
        try
        {
            Debug1.text = null;
            Directorys = Directory.GetFiles(PatchAndroid, "*.mp3");
            Debug.Log("mp3 file 찾기 " + Directorys);
            StartCoroutine("load");
            Debug.Log("load 시작");
        }
        catch(Exception e)
        {
            Debug.Log(e);
            Debug1.text = "start에서 에러";
        }

    }



    ///////////////////////////do not change//////////////

    void _Random()
    {
        NumberS = UnityEngine.Random.Range(0, Names.Count - 1);
        SelectionIndicator.transform.SetParent(GameObject.Find("Button" + NumberS + "(Clone)").transform);
        SelectionIndicator.transform.localPosition = new Vector3Int(0, 0, 0);
        Set(NumberS);
    }


    public void Set(int i)
    {
        try
        {

            pause = false;
            Play.SetActive(false);
            Pause.SetActive(true);
            first_time = false;
            NumberS = i;
            Debug.Log("곡이름 " + Names);
            
            Debug1.text = _Button.name;
            ShowSongName.text = Names[NumberS];
            obj.Call("Set", Names[NumberS]);
            


        }
        catch (Exception e)
        {
            Debug.Log(e);
            Debug1.text = "Set 에러 발생";
        }


    }

    /////////////Send message from some button///////////   

    public void Next()
    {
        if (Names.Count != 0) {
            if (Play.activeInHierarchy == true)
            {
                Play.SetActive(false);
                Pause.SetActive(true);
                play(1);
            }
            if (SelectionIndicator.activeInHierarchy == false)
            {
                SelectionIndicator.SetActive(true);
            }
            if (NumberS < Names.Count - 1)
            {
                NumberS += 1;
                SelectionIndicator.transform.SetParent(GameObject.Find("Button" + NumberS + "(Clone)").transform);
                SelectionIndicator.transform.localPosition = new Vector3Int(0, 0, 0);
                Set(NumberS);
            }
            else
            {
                NumberS = 0;
                SelectionIndicator.transform.SetParent(GameObject.Find("Button" + NumberS + "(Clone)").transform);
                SelectionIndicator.transform.localPosition = new Vector3Int(0, 0, 0);
                Set(NumberS);
            }
        }
        else
        {
            Debug1.text = ErrorMessage;

        }
    }
    public void Prev()
    {
        if (Names.Count != 0)
        {
            if (Play.activeInHierarchy == true)
            {
                Play.SetActive(false);
                Pause.SetActive(true);
                play(1);
            }
            if (SelectionIndicator.activeInHierarchy == false)
            {
                SelectionIndicator.SetActive(true);
            }

            if (NumberS > 0)
            {
                NumberS -= 1;
                SelectionIndicator.transform.SetParent(GameObject.Find("Button" + NumberS + "(Clone)").transform);
                SelectionIndicator.transform.localPosition = new Vector3Int(0, 0, 0);
                Set(NumberS);
            }
            else
            {
                NumberS = Names.Count - 1;
                SelectionIndicator.transform.SetParent(GameObject.Find("Button" + NumberS + "(Clone)").transform);
                SelectionIndicator.transform.localPosition = new Vector3Int(0, 0, 0);
                Set(NumberS);
            }
        }
        else
        {
            Debug1.text = ErrorMessage;

        }
    }

    ///////////////////////////do not change//////////////

    public void play(int i)
    {
        if (i == 0)
        {
            if (Names.Count != 0)
            {
                if (Play.activeInHierarchy == true)
                {
                    pause = false;
                    Play.SetActive(false);
                    Pause.SetActive(true);
                }
                else
                {
                    pause = true;
                    Play.SetActive(true);
                    Pause.SetActive(false);
                }
            }
            else
            {
                Debug1.text = ErrorMessage;

            }
        }
        obj.Call("play");
    }

    public void set(string patch)
    {

        PatchAndroid = patch;
        Debug.Log("patchAndroid" + PatchAndroid);
        debug2.text = patch;

    }

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

  public void MakeModel()
    {
       
    try
    {
       if(_Button.isOn)
            { 

                Debug1.text = "click 받음 ";
            Debug.Log("시작");
            premodel = Resources.Load("snow") as GameObject;
            Debug.Log("로드완료");  
            Musicmd = Instantiate(premodel,POS, Quaternion.identity);
            Debug.Log("Musicmd insitate _model");
            Musicmd.name= "model" + modelsName;
            AudioClip audioSource = _Button.GetComponent<AudioSource>().clip;
            Musicmd.GetComponent<AudioSource>().clip = audioSource;
            Musicmd.GetComponent<AudioSource>().spatialBlend = 1;
            Musicmd.SendMessage("MakeModel", Musicmd);
                modelsName++;

            }
       
    }
    catch (ArgumentException e)
    {
        Debug.Log(e);
        Debug1.text = "modeling애러 ";
    }
       
    }

    ////////////////////////////Controls/////////////////
    public void Random()
    {
        Random_b = true;
        Sequence_b = false;
        Repeat_b = false;

    }
    public void Repeat()
    {
        Random_b = false;
        Sequence_b = false;
        Repeat_b = true;

    }
    public void Sequence()
    {
        Random_b = false;
        Sequence_b = true;
        Repeat_b = false;

    }

}
