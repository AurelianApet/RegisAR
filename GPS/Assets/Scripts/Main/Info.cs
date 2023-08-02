using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    private GameObject target;
    public GameObject info_popup;
    public int Info_num = -1;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            target = GetClickedObject();
            if (target != null && target.Equals(gameObject))
            {
                Debug.Log("clicked right!");
                if (info_popup != null && Info_num > -1)
                {
                    info_popup.SetActive(true);
                    info_popup.transform.Find("Label").GetComponent<UILabel>().text = Global.info_string[Info_num];
                }
            }
        }
    }

    private GameObject GetClickedObject()
    {
        RaycastHit hit;
        GameObject target1 = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(true == (Physics.Raycast(ray.origin, ray.direction * 10, out hit)))
        {
            target1 = hit.collider.gameObject;
        }
        return target1;
    }
}
