using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene(1.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator LoadScene(float sec)
    {
        yield return new WaitForSeconds(sec);
        Application.LoadLevel("UI Bubble Popup");
    }
}
