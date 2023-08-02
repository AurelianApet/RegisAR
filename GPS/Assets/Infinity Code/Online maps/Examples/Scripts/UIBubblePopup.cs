/*     INFINITY CODE 2013-2019      */
/*   http://www.infinity-code.com   */

using System;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.Collections;

namespace InfinityCode.OnlineMapsDemos
{
    /// <summary>
    /// Example is how to use a combination of data from Google Places API on bubble popup.
    /// </summary>
    public class UIBubblePopup : MonoBehaviour
    {
        /// <summary>
        /// Root canvas
        /// </summary>
        public Canvas canvas;

        /// <summary>
        /// Bubble popup
        /// </summary>
        public GameObject bubble;

        /// <summary>
        /// Title text
        /// </summary>
        public Text title;

        /// <summary>
        /// Address text
        /// </summary>
        public Text address;

        /// <summary>
        /// Photo RawImage
        /// </summary>
        public RawImage photo;

        public CData[] datas;

        public GameObject markerPanel;

        public GameObject addConfirmWnd;
        public GameObject editConfirmWnd;
        public GameObject deleteConfirmWnd;
        public GameObject addoreditObject;

        public InputField mname;
        public InputField moffofday;
        public Dropdown mcategory;
        public Toggle mstatus;

        /// <summary>
        /// Reference to active marker
        /// </summary>
        private OnlineMapsMarker targetMarker;

        /// <summary>
        /// This method is called when downloading photo texture.
        /// </summary>
        /// <param name="texture2D">Photo texture</param>
        private void OnDownloadPhotoComplete(OnlineMapsWWW www)
        {
            Texture2D texture = new Texture2D(1, 1);
            www.LoadImageIntoTexture(texture);

            // Set place texture to bubble popup
            photo.texture = texture;
        }

        /// <summary>
        /// This method is called by clicking on the map
        /// </summary>
        private void OnMapClick()
        {
            if (Global.catclk)
                return;
            Debug.Log("map clicked!");
            // Remove active marker reference
            targetMarker = null;
            // Hide the popup
            bubble.SetActive(false);
            GameObject.Find("UI Root/control/edit").gameObject.SetActive(false);
            GameObject.Find("UI Root/control/del").gameObject.SetActive(false);
            GameObject.Find("UI Root/control/add").gameObject.SetActive(true);
        }

        /// <summary>
        /// This method is called by clicking on the marker
        /// </summary>
        /// <param name="marker">The marker on which clicked</param>
        private void OnMarkerClick(OnlineMapsMarkerBase marker)
        {
            if (Global.catclk)
                return;
            // Set active marker reference
            targetMarker = marker as OnlineMapsMarker;

            // Get a result item from instance of the marker
            CData data = marker["data"] as CData;
            if (data == null) return;
            // Show edit/delete button
            Global.curMarkerId = data.num;
            if(Global.curMarkerId != 1000)
            {
                GameObject.Find("UI Root/control/edit").gameObject.SetActive(true);
                GameObject.Find("UI Root/control/del").gameObject.SetActive(true);
                GameObject.Find("UI Root/control/add").gameObject.SetActive(false);
            }
            //modify markerpanel with existing values
            if (Global.curMarkerId != 1000)
            {
                markerPanel.transform.Find("Name/InputField").GetComponent<InputField>().text = Global.markers[data.num].name;
                markerPanel.transform.Find("offerofday/InputField").GetComponent<InputField>().text = Global.markers[data.num].offerofday;
                markerPanel.transform.Find("gps/InputField").GetComponent<InputField>().text = Global.markers[data.num].latitude.ToString() + "," + Global.markers[data.num].longitude.ToString();
                markerPanel.transform.Find("category/Dropdown").GetComponent<Dropdown>().value = Global.markers[data.num].category - 1;
                //markerPanel.transform.Find("status/Toggle").GetComponent<Toggle>().isOn = Global.markers[data.num].status;
                // Set title and address
                title.text = Global.markers[data.num].name;
                address.text = Global.info_string[data.num];
                Global.curMarkerLat = Global.markers[data.num].latitude;
                Global.curMarkerLng = Global.markers[data.num].longitude;
            }
            else
            {
                markerPanel.transform.Find("Name/InputField").GetComponent<InputField>().text = Global.curPos.name;
                markerPanel.transform.Find("offerofday/InputField").GetComponent<InputField>().text = Global.curPos.offerofday;
                markerPanel.transform.Find("gps/InputField").GetComponent<InputField>().text = Global.curPos.latitude.ToString() + "," + Global.curPos.longitude.ToString();
                markerPanel.transform.Find("category/Dropdown").GetComponent<Dropdown>().value = Global.curPos.category;
                //markerPanel.transform.Find("status/Toggle").GetComponent<Toggle>().isOn = Global.curPos.status;
                title.text = Global.curPos.name;
                address.text = Global.curInfo;
                Global.curMarkerLat = Global.curPos.latitude;
                Global.curMarkerLng = Global.curPos.longitude;
            }

            // Show the popup
            bubble.SetActive(true);

            // Destroy the previous photo
            if (photo.texture != null)
            {
                OnlineMapsUtils.Destroy(photo.texture);
                photo.texture = null;
            }

            /*
            OnlineMapsWWW www = new OnlineMapsWWW(data.photo_url);
            www.OnComplete += OnDownloadPhotoComplete;
            */
            // Initial update position
            UpdateBubblePosition();
        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before any of the Update methods are called the first time.
        /// </summary>
        private void Start()
        {
            // Subscribe to events of the map 
            OnlineMaps.instance.OnChangePosition += UpdateBubblePosition;
            OnlineMaps.instance.OnChangeZoom += UpdateBubblePosition;
            OnlineMapsControlBase.instance.OnMapClick += OnMapClick;
            OnlineMapsMarkerManager.RemoveAllItems();
            OnlineMapsMarkerManager.instance.defaultTexture = Resources.Load<Texture2D>("DefaultMarker");
            if (OnlineMapsControlBaseDynamicMesh.instance != null)
            {
                OnlineMapsControlBaseDynamicMesh.instance.OnMeshUpdated += UpdateBubblePosition;
            }

            if (OnlineMapsCameraOrbit.instance != null)
            {
                OnlineMapsCameraOrbit.instance.OnCameraControl += UpdateBubblePosition;
            }


            // Initial hide popup
            bubble.SetActive(false);
        }

        private void ChangeMarkerColor(OnlineMapsMarker mk, int cate)
        {
            Texture2D tex2 = null;
            switch (cate)
            {
                case 1:
                    tex2 = Resources.Load<Texture2D>("1");
                    break;
                case 2:
                    tex2 = Resources.Load<Texture2D>("2");
                    break;
                case 3:
                    tex2 = Resources.Load<Texture2D>("3");
                    break;
                case 4:
                    tex2 = Resources.Load<Texture2D>("4");
                    break;
                case 5:
                    tex2 = Resources.Load<Texture2D>("5");
                    break;
                case 6:
                    tex2 = Resources.Load<Texture2D>("6");
                    break;
                case 7:
                    tex2 = Resources.Load<Texture2D>("7");
                    break;
                case 8:
                    tex2 = Resources.Load<Texture2D>("8");
                    break;
                case 9:
                    tex2 = Resources.Load<Texture2D>("9");
                    break;
                case 10:
                    tex2 = Resources.Load<Texture2D>("10");
                    break;
                case 11:
                    tex2 = Resources.Load<Texture2D>("11");
                    break;
                default:
                    tex2 = Resources.Load<Texture2D>("DefaultMarker");
                    break;

            }
            mk.texture = tex2;
            mk.scale = 0.1f;
            cleanPanel();
        }

        private void Update()
        {
            if (Global.marker_set && Global.gps_set && !Global.markerSetted)
            {
                OnlineMapsMarkerManager.RemoveAllItems();
                OnlineMapsMarker curMarker = OnlineMapsMarkerManager.CreateItem(Global.curPos.longitude, Global.curPos.latitude, Resources.Load<Texture2D>("carmarker"), null);
                curMarker["data"] = new CData(Global.curPos.name, Global.curInfo, "", Global.curPos.longitude, Global.curPos.latitude, 1000);
                curMarker.OnClick += OnMarkerClick;
                curMarker.scale = 0.1f;
                curMarker.SetPosition(Global.curPos.longitude, Global.curPos.latitude);
                for (int i = 0; i < Global.markers.Count; i ++)
                {
                    Texture2D tex2 = null;
                    switch (Global.markers[i].category)
                    {
                        case 1:
                            tex2 = Resources.Load<Texture2D>("1");
                            break;
                        case 2:
                            tex2 = Resources.Load<Texture2D>("2");
                            break;
                        case 3:
                            tex2 = Resources.Load<Texture2D>("3");
                            break;
                        case 4:
                            tex2 = Resources.Load<Texture2D>("4");
                            break;
                        case 5:
                            tex2 = Resources.Load<Texture2D>("5");
                            break;
                        case 6:
                            tex2 = Resources.Load<Texture2D>("6");
                            break;
                        case 7:
                            tex2 = Resources.Load<Texture2D>("7");
                            break;
                        case 8:
                            tex2 = Resources.Load<Texture2D>("8");
                            break;
                        case 9:
                            tex2 = Resources.Load<Texture2D>("9");
                            break;
                        case 10:
                            tex2 = Resources.Load<Texture2D>("10");
                            break;
                        case 11:
                            tex2 = Resources.Load<Texture2D>("11");
                            break;
                        default:
                            tex2 = Resources.Load<Texture2D>("DefaultMarker");
                            break;

                    }
                    OnlineMapsMarker marker = OnlineMapsMarkerManager.CreateItem(Global.markers[i].longitude, Global.markers[i].latitude, tex2, null);
                    marker["data"] = new CData(Global.markers[i].name, Global.info_string[i], "", Global.markers[i].longitude, Global.markers[i].latitude, i);
                    marker.scale = 0.1f;
                    if (!Global.markers[i].status)
                    {
                        marker.enabled = false;
                    }
                    marker.OnClick += OnMarkerClick;
                }
                Global.markerSetted = true;
                OnlineMaps.instance.Redraw();
            }
        }
        /// <summary>
        /// Updates the popup position
        /// </summary>
        private void UpdateBubblePosition()
        {
            // If no marker is selected then exit.
            if (targetMarker == null) return;

            // Hide the popup if the marker is outside the map view
            if (!targetMarker.inMapView)
            {
                if (bubble.activeSelf) bubble.SetActive(false);
            }
            else if (!bubble.activeSelf) bubble.SetActive(true);

            // Convert the coordinates of the marker to the screen position.
            Vector2 screenPosition = OnlineMapsControlBase.instance.GetScreenPosition(targetMarker.position);

            // Add marker height
            screenPosition.y += targetMarker.height;

            // Get a local position inside the canvas.
            Vector2 point;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPosition, null, out point);

            // Set local position of the popup
            (bubble.transform as RectTransform).localPosition = point;
        }

        public void addMarker()
        {
            //add marker process
            addConfirmWnd.SetActive(false);
            StartCoroutine(addmarker_thread());
        }

        IEnumerator addmarker_thread()
        {
            Debug.Log("Starting add marker");
            WWWForm form = new WWWForm();
            form.AddField("name", mname.text);
            form.AddField("boffer", moffofday.text);
            form.AddField("category", mcategory.value + 1);
            form.AddField("lat", Global.curMarkerLat.ToString());
            form.AddField("lng", Global.curMarkerLng.ToString());
            // int ss = 1;
            // if (!Global.curMarkerStatus)
            // {
            //     ss = 0;
            // }
            //form.AddField("status", ss.ToString());
            string requestURL = Global.api_domain + Global.addMarkerApi;
            WWW www = new WWW(requestURL, form);
            yield return addmarker_process(www);
        }

        IEnumerator addmarker_process(WWW www)
        {
            yield return www;
            if (www.error == null)
            {                
                JSONNode jsonNode = SimpleJSON.JSON.Parse(www.text);
                if (jsonNode["success"].AsInt > 0)
                {
                    Debug.Log("success in adding marker" + Global.markers.Count);
                    Debug.Log("category value = " + (mcategory.value + 1));
                    Debug.Log("name = " + mname.text);
                    int id = jsonNode["success"].AsInt;
                    bool cactive = jsonNode["active"].AsBool;
                    //remove draggable feature.
                    OnlineMapsMarker marker = OnlineMaps.instance.markers[OnlineMaps.instance.markers.Length - 1];
                    marker["data"] = new CData(mname.text, moffofday.text, "", marker.position.x, marker.position.y, Global.markers.Count - 1);
                    marker.OnClick += OnMarkerClick;
                    marker.RemoveDraggable();
                    Debug.Log("cate - " + cactive);
                    if (cactive)
                    {
                        marker.enabled = true;
                    }
                    else
                    {
                        marker.enabled = false;
                    }
                    Global.info_string[Global.markers.Count - 1] = moffofday.text;
                    Global.markers[Global.markers.Count - 1].id = id;
                    Global.markers[Global.markers.Count - 1].name = mname.text;
                    Global.markers[Global.markers.Count - 1].offerofday = moffofday.text;
                    Global.markers[Global.markers.Count - 1].category = mcategory.value + 1;
                    //Global.markers[Global.markers.Count - 1].status = mstatus.isOn;
                    ChangeMarkerColor(marker, mcategory.value + 1);
                }
                else
                {
                    Debug.Log("fail in adding marker");
                    Global.markers.RemoveAt(Global.markers.Count - 1);
                    Global.info_string.RemoveAt(Global.info_string.Count - 1);
                    OnlineMapsMarkerManager.RemoveItemAt(OnlineMapsMarkerManager.CountItems - 1);
                    cleanPanel();
                }
            }
            markerPanel.SetActive(false);
        }

        public void cleanPanel()
        {
            moffofday.text = "";
            mname.text = "";
            moffofday.text = "";
            mcategory.value = 0;
        }

        public void editMarker()
        {
            markerPanel.SetActive(false);
            //edit marker process
            StartCoroutine(editmarker_thread());
            //marker panel modify
            editConfirmWnd.SetActive(false);
        }

        IEnumerator editmarker_thread()
        {
            Debug.Log("start editing marker");
            WWWForm form = new WWWForm();
            form.AddField("bid", Global.markers[Global.curMarkerId].id);
            form.AddField("name", mname.text);
            form.AddField("boffer", moffofday.text);
            form.AddField("lat", Global.curMarkerLat.ToString());
            form.AddField("lng", Global.curMarkerLng.ToString());
            form.AddField("category", mcategory.value + 1);
            // int ss = 1;
            // if (!Global.markers[Global.curMarkerId].status)
            // {
            //     ss = 0;
            // }
            //form.AddField("status", ss.ToString());
            string requestURL = Global.api_domain + Global.editMarkerApi;
            WWW www = new WWW(requestURL, form);
            yield return editmarker_process(www);
        }

        IEnumerator editmarker_process(WWW www)
        {
            yield return www;
            if (www.error == null)
            {
                Debug.Log("success in editing marker");
                JSONNode jsonNode = SimpleJSON.JSON.Parse(www.text);
                if (jsonNode["success"].ToString() == "1")
                {
                    Global.info_string[Global.curMarkerId] = moffofday.text;
                    Global.markers[Global.curMarkerId].name = mname.text;
                    Global.markers[Global.curMarkerId].offerofday = moffofday.text;
                    Global.markers[Global.curMarkerId].category = mcategory.value + 1;
                    //Global.markers[Global.curMarkerId].status = mstatus.isOn;
                    Global.markers[Global.curMarkerId].latitude = Global.curMarkerLat;
                    Global.markers[Global.curMarkerId].longitude = Global.curMarkerLng;
                    OnlineMapsMarker marker = OnlineMaps.instance.markers[Global.curMarkerId + 1];
                    marker.RemoveDraggable();
                    bool cactive = jsonNode["active"].AsBool;
                    Debug.Log("cate - " + cactive);
                    if (cactive)
                    {
                        marker.enabled = true;
                    }
                    else
                    {
                        marker.enabled = false;
                    }
                    ChangeMarkerColor(marker, Global.markers[Global.curMarkerId].category);
                }
                else
                {
                    cleanPanel();
                }
            }
        }

        public void deleteMarker()
        {
            //delete marker process
            bubble.SetActive(false);
            StartCoroutine(delmarker_thread());
            deleteConfirmWnd.SetActive(false);
        }

        IEnumerator delmarker_thread()
        {
            Debug.Log("start deleting marker - "  + Global.markers[Global.curMarkerId].id.ToString());
            WWWForm form = new WWWForm();
            form.AddField("bid", Global.markers[Global.curMarkerId].id);
            string requestURL = Global.api_domain + Global.delMarkerApi;
            WWW www = new WWW(requestURL, form);
            yield return delmarker_process(www);
        }

        IEnumerator delmarker_process(WWW www)
        {
            yield return www;
            if (www.error == null)
            {
                Debug.Log("success in deleting marker");
                JSONNode jsonNode = SimpleJSON.JSON.Parse(www.text);
                if (jsonNode["success"].ToString() == "1")
                {
                    if (Global.delItem)
                    {
                        OnlineMapsMarkerManager.RemoveItemAt(Global.curMarkerId);
                    }
                    else
                    {
                        OnlineMapsMarkerManager.RemoveItemAt(Global.curMarkerId + 1);
                    }
                    Global.delItem = true;
                    address.text = "";
                    title.text = "";
                }
                else
                {
                    Debug.Log("fail in deleting marker");
                }
            }
            bubble.SetActive(false);
            cleanPanel();
        }

        public void cancelAddConfirmWnd()
        {
            addConfirmWnd.SetActive(false);
            markerPanel.SetActive(true);
        }

        public void cancelEditConfirmWnd()
        {
            editConfirmWnd.SetActive(false);
            markerPanel.SetActive(true);
        }

        public void cancelDeleteConfirmWnd()
        {
            deleteConfirmWnd.SetActive(false);
        }

        public void clkAddMarker()
        {
            OnlineMapsMarkerBase.markerPanel = markerPanel;
            Global.selAddorEdit = 1;
            addoreditObject.GetComponent<Text>().text = "Add";
            //add marker on the map
            double lng, lat;
            OnlineMapsControlBase.instance.GetCoords(out lng, out lat, new Vector2(Screen.width / 2,  Screen.height / 2));
            OnlineMapsMarker marker = OnlineMapsMarkerManager.CreateItem(lng, lat, "");
            Global.curMarkerLat = lat;
            Global.curMarkerLng = lng;
            //OnlineMapsMarker marker = OnlineMapsMarkerManager.CreateItem(OnlineMaps.instance.position, "");
            marker["data"] = new CData("", "", "", lng, lat, Global.markers.Count);
            Global.markers.Add(new Global.GPS_pos(-1, "", "", lat, lng, 0));
            Global.info_string.Add("Located at x meters - ");
            marker.scale = 1;
            marker.SetDraggable();
            //marker.OnClick += OnMarkerClick;
            markerPanel.transform.Find("gps/InputField").GetComponent<InputField>().text = lat.ToString() + "," + lng.ToString();
            mname.text = "";
            moffofday.text = "";
            mcategory.value = 0;

            markerPanel.SetActive(true);
        }

        public void clkEditMarker()
        {
            markerPanel.SetActive(true);
            Global.selAddorEdit = 0;
            Global.curMarkerLat = Global.markers[Global.curMarkerId].latitude;
            Global.curMarkerLng = Global.markers[Global.curMarkerId].longitude;
            addoreditObject.GetComponent<Text>().text = "Update";
            OnlineMapsMarkerBase.markerPanel = markerPanel;
            OnlineMapsMarker marker = OnlineMaps.instance.markers[Global.curMarkerId + 1];
            marker.SetDraggable();
        }

        public void clkDeleteMarker()
        {
            deleteConfirmWnd.SetActive(true);
            if (markerPanel.activeSelf)
            {
                markerPanel.SetActive(false);
            }
        }

        public void cancelPanel()
        {
            markerPanel.SetActive(false);
            if(Global.selAddorEdit == 1)
            {
                //if add, remove item
                Global.markers.RemoveAt(Global.markers.Count - 1);
                Global.info_string.RemoveAt(Global.info_string.Count - 1);
                OnlineMapsMarkerManager.RemoveItemAt(OnlineMapsMarkerManager.CountItems - 1);
            }
            else
            {
                //if edit, remove draggable
                OnlineMapsMarker marker = OnlineMaps.instance.markers[Global.curMarkerId + 1];
                marker.RemoveDraggable();
            }
        }

        public void updatePanel()
        {
            if (Global.selAddorEdit == 0)
            {
                editConfirmWnd.SetActive(true);
                markerPanel.SetActive(false);
            }
            else
            {
                addConfirmWnd.SetActive(true);
                markerPanel.SetActive(false);
            }
        }

        [Serializable]
        public class CData
        {
            public string title;
            public string address;
            public string photo_url;
            public double longitude;
            public double latitude;
            public int num;

            public CData(string t, string a, string p, double lng, double lat, int n)
            {
                title = t;
                address = a;
                photo_url = p;
                longitude = lng;
                latitude = lat;
                num = n;
            }
        }
    }
}