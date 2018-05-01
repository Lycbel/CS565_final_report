using UnityEngine;
using UnityEngine.UI;

public class ButtonCreater : MonoBehaviour
{
    public GameObject buttonPrefab;
    public GameObject panelToAttachButtonsTo;
    void Start()//Creates a button and sets it up
    {
        GameObject button = (GameObject)Instantiate(buttonPrefab);
        button.transform.SetParent(panelToAttachButtonsTo.transform);//Setting button parent
        button.GetComponent<Button>().onClick.AddListener(OnClick);//Setting what button does when clicked
                                                                   //Next line assumes button has child with text as first gameobject like button created from GameObject->UI->Button
        button.transform.GetChild(0).GetComponent<Text>().text = "This is button text";//Changing text
    }
    void OnClick()
    {
        Debug.Log("clicked!");
    }
}