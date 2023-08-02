using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormManage : MonoBehaviour
{
    public GameObject notiPopup;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(notiPopup.activeSelf){
            string str = "";
            for(int i = 0; i < Global.info_string.Count; i++)
            {
                str += Global.info_string[i] + "\n";
            }
            notiPopup.transform.Find("Label").transform.GetComponent<UILabel>().text = str;
        }
    }

    public void closeInfo()
    {
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void openNoti()
    {
        if (notiPopup.activeSelf)
        {
            notiPopup.SetActive(false);
            this.GetComponent<UILabel>().text = "Test GPS On";
        }
        else
        {
            notiPopup.SetActive(true);
            this.GetComponent<UILabel>().text = "Test GPS Off";
        }
    }
}
