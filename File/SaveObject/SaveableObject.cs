using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum ObjectType
{
    snowImage, fireImage, firefiles
}
public abstract class SaveableObject : MonoBehaviour
{
    protected string save;

    [SerializeField]
    private ObjectType objectType;

    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("스타트");
        
        SaveModelManager.Instance.SaveableObjects.Add(this);
        Debug.Log("로드한것" + SaveModelManager.Instance.SaveableObjects);
    }
    //string anchorid
    public virtual void  Save(int id)
    {
        
        PlayerPrefs.SetString(id.ToString(), objectType+"_"+transform.position.ToString());
    }

    public virtual void Load(string[] values)
    {
        transform.localPosition = SaveModelManager.Instance.StringToVector(values[1]);
    }

    public virtual void DestorySavable()
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
