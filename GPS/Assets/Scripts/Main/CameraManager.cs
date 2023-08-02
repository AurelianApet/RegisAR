using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System;
using System.IO;
using System.Collections;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour {
    WebCamTexture mycam;
    void Start()
    {
        mycam = new WebCamTexture(640, 480);
        GetComponent<Renderer>().material.mainTexture = mycam;
        mycam.Play();
    }
}
