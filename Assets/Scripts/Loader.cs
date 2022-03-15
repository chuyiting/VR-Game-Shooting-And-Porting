using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour {

    public HandScanner handScanner;
    private RectTransform rectComponent;
    private Image imageComp;
    public float speed = 0.0f;
    public float completness = 0.0f;

    private bool isLoading = false;
   

    // Use this for initialization
    void Awake () {
        rectComponent = GetComponent<RectTransform>();
        imageComp = rectComponent.GetComponent<Image>();
        imageComp.fillAmount = completness;
    }

    void Update()
    {
        if (!isLoading)
        {
            return;
        }

        if (imageComp.fillAmount < 1f)
        {
            imageComp.fillAmount = imageComp.fillAmount + Time.deltaTime * speed;
            completness = imageComp.fillAmount;
        } else {
            SendFinishNotification();
            isLoading = false;
        }

    }

    public void StartLoading()
    {
        imageComp.fillAmount = 0.0f;
        completness = 0.0f;
        isLoading = true;
    }

    public void StopLoading()
    {
        imageComp.fillAmount = 0.0f;
        completness = 0.0f;
        isLoading = false;
    }

    private void SendFinishNotification()
    {
        handScanner.OnLoadingComplete();
    }
}
