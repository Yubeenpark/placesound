
using UnityEngine;
using UnityEngine.UI;
public class MusicButton : MonoBehaviour
{

    Button _Button;
    public NativeMusic Class;
    public int Song_Number;
    public GameObject Indicator;

    private void Start()
    {
        
        _Button = GetComponent<Button>();
        _Button.onClick.AddListener(() => Play());
    }




    void Play()
    {
        Indicator.SetActive(true);
        Indicator.transform.SetParent(GetComponent<Transform>());
        Indicator.transform.localPosition = new Vector3(0,0,0);
        Class.Set(Song_Number);

    }



    
}
