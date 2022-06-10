
using UnityEngine;
using UnityEngine.UI;
public class SliderB : MonoBehaviour
{
    
    public AddToLIst _class;
    public InputField Inpf;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => CH());

    }

    void CH()
    {
        _class.Change(Inpf.text);


    }
   
}
