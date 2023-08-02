using UnityEngine;
using System.Collections;
using System.IO;
using System;
using LitJson;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;

public class ToolbarManager : MonoBehaviour {
    public GameObject gyroPopuwnd;
    public GameObject exitPopupWnd;
    public GameObject internetMessageBox;
    public GameObject on_push;
    public GameObject off_push;

    public Toggle allCat;
    public Toggle resturant;
    public Toggle drinks;
    public Toggle realEs;
    public Toggle health;
    public Toggle home;
    public Toggle family;
    public Toggle pets;
    public Toggle activities;
    public Toggle automobile;
    public Toggle shopping;
    public Toggle free;
    public Toggle gps;

    public GameObject cateogries;
    public GameObject cate_panel;
    public GameObject addMerchant;
    public GameObject testGPS;
    public GameObject delMerchant;
    public GameObject editMerchant;
    public GameObject showMap;
    public GameObject border;
    // Use this for initialization
    void Start() {
        getMarkers();
        allCat.onValueChanged.AddListener((x) => Invoke("SelectAll_Category", 0.1f));
        StartCoroutine(load_categories());
    }

    IEnumerator load_categories()
    {
        string requestURL = Global.api_domain + Global.getCategoryApi;
        WWW www = new WWW(requestURL);
        yield return getcategories_process(www);
    }

    IEnumerator getcategories_process(WWW www)
    {
        string[] cname = new string[] { };
        int[] cid = new int[] { };
        bool[] cactive = new bool[] { };
        yield return www;
        if (www.error == null)
        {
            JSONNode jsonNode = SimpleJSON.JSON.Parse(www.text);
            cname = new string[jsonNode.Count];
            cid = new int[jsonNode.Count];
            cactive = new bool[jsonNode.Count];
            for (int i = 0; i < jsonNode.Count; i++)
            {
                cname[i] = jsonNode[i]["name"].ToString().Replace("\"", "");
                cid[i] = jsonNode[i]["id"].AsInt;
                cactive[i] = true;
                if (jsonNode[i]["active"].AsInt == 0)
                {
                    cactive[i] = false;
                }
            }
        }
        try
        {
            if (cactive[0])
            {
                resturant.isOn = true;
            }
            if (cactive[1])
            {
                drinks.isOn = true;
            }
            if (cactive[2])
            {
                health.isOn = true;
            }
            if (cactive[3])
            {
                home.isOn = true;
            }
            if (cactive[4])
            {
                family.isOn = true;
            }
            if (cactive[5])
            {
                pets.isOn = true;
            }
            if (cactive[6])
            {
                activities.isOn = true;
            }
            if (cactive[7])
            {
                automobile.isOn = true;
            }
            if (cactive[8])
            {
                shopping.isOn = true;
            }
            if (cactive[9])
            {
                free.isOn = true;
            }
            if (cactive[10])
            {
                realEs.isOn = true;
            }
        }
        catch(Exception ex)
        {

        }
    }

    public void ExitOkWnd()
    {
        Application.Quit();
    }

    public void ExitCancelWnd()
    {
        exitPopupWnd.SetActive(false);
    }

    public void onBtnClose() {
        exitPopupWnd.SetActive(true);
    }

    public void OnGyroCancelButton()
    {
        gyroPopuwnd.SetActive(false);
    }

    public void onSetPush()
    {
        if (Global.isset_push)
        {
            on_push.SetActive(false);
            off_push.SetActive(true);
            Global.isset_push = false;
        }
        else
        {
            on_push.SetActive(true);
            off_push.SetActive(false);
            Global.isset_push = true;
        }
    }

    public void getMarkers()
    {
        StartCoroutine(getmarkers_thread());
    }

    IEnumerator getmarkers_thread()
    {
        string requestURL = Global.api_domain + Global.getMarkersApi;
        WWW www = new WWW(requestURL);
        yield return getmarkers_process(www);
    }

    IEnumerator getmarkers_process(WWW www)
    {
        yield return www;
        Global.markers.Clear();
        if (www.error == null)
        {
            JSONNode jsonNode = SimpleJSON.JSON.Parse(www.text);
            for(int i = 0; i < jsonNode.Count; i++)
            {
                string mname = jsonNode[i]["name"].ToString().Replace("\"", "");
                int mid = jsonNode[i]["id"].AsInt;
                string mboffer = jsonNode[i]["boffer"].ToString().Replace("\"", "");
                double mlat = jsonNode[i]["lat"].AsDouble;
                double mlng = jsonNode[i]["lng"].AsDouble;
                int mcategory = jsonNode[i]["category"].AsInt;
                bool mstatus = true;
                if(jsonNode[i]["active"].AsInt == 0)
                {
                    mstatus = false;
                }
                Global.markers.Add(new Global.GPS_pos(mid, mname, mboffer, mlat, mlng, mcategory, mstatus));
                Global.isArr.Add(false);
                Global.info_string.Add("Located at x meters - " + mboffer);
            }
            if (Input.location.status == LocationServiceStatus.Running)
            {
                Global.curPos.latitude = Input.location.lastData.latitude;
                Global.curPos.longitude = Input.location.lastData.longitude;
            }
            Global.curPos.id = 1000;
            //Global.markers.Add(Global.curPos);
            //Global.info_string.Add("Posicion actual");
            Global.curInfo = "Posicion actual";
            Global.marker_set = true;
        }
    }

    public void SelectAll_Category()
    {
        if (allCat.isOn)
        {
            resturant.isOn = true;
            drinks.isOn = true;
            realEs.isOn = true;
            health.isOn = true;
            home.isOn = true;
            family.isOn = true;
            pets.isOn = true;
            activities.isOn = true;
            automobile.isOn = true;
            shopping.isOn = true;
            free.isOn = true;
        }
        else
        {
            resturant.isOn = false;
            drinks.isOn = false;
            realEs.isOn = false;
            health.isOn = false;
            home.isOn = false;
            family.isOn = false;
            pets.isOn = false;
            activities.isOn = false;
            automobile.isOn = false;
            shopping.isOn = false;
            free.isOn = false;
        }
    }

    public void categoryClk()
    {
        Global.catclk = true;
        addMerchant.SetActive(false);
        testGPS.SetActive(false);
        border.SetActive(false);
        delMerchant.SetActive(false);
        editMerchant.SetActive(false);
        cateogries.SetActive(true);
        cate_panel.SetActive(true);
        showMap.SetActive(true);
    }

    public void showInMap()
    {
        Global.catclk = false;
        addMerchant.SetActive(true);
        testGPS.SetActive(true);
        border.SetActive(true);
        delMerchant.SetActive(false);
        editMerchant.SetActive(false);
        cateogries.SetActive(false);
        cate_panel.SetActive(false);
        showMap.SetActive(false);
        string activies = "";
        if (resturant.isOn)
        {
            activies = activies + ", 1";
        }
        if (drinks.isOn)
        {
            activies = activies + ", 2";
        }
        if (health.isOn)
        {
            activies = activies + ", 3";
        }
        if (home.isOn)
        {
            activies = activies + ", 4";
        }
        if (family.isOn)
        {
            activies = activies + ", 5";
        }
        if (pets.isOn)
        {
            activies = activies + ", 6";
        }
        if (activities.isOn)
        {
            activies = activies + ", 7";
        }
        if (automobile.isOn)
        {
            activies = activies + ", 8";
        }
        if (shopping.isOn)
        {
            activies = activies + ", 9";
        }
        if (free.isOn)
        {
            activies = activies + ", 10";
        }
        if (realEs.isOn)
        {
            activies = activies + ", 11";
        }
        StartCoroutine(updateCategory(activies));
    }

    IEnumerator updateCategory(string actLists)
    {
        string requestURL = Global.api_domain + Global.setCategoryApi + actLists;
        WWW www = new WWW(requestURL);
        yield return update_cat_process(www);
    }

    IEnumerator update_cat_process(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            StartCoroutine(update_markers());
        }
    }

    IEnumerator update_markers()
    {
        string requestURL = Global.api_domain + Global.getMarkersApi;
        WWW www = new WWW(requestURL);
        yield return updatemarkers_process(www);
    }

    IEnumerator updatemarkers_process(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            JSONNode jsonNode = SimpleJSON.JSON.Parse(www.text);
            for (int i = 0; i < jsonNode.Count; i++)
            {
                bool mstatus = true;
                if (jsonNode[i]["active"].AsInt == 0)
                {
                    mstatus = false;
                }
                Global.markers[i].status = mstatus;
            }
            Global.markerSetted = false;
        }
    }
}
