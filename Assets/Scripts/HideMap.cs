using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideMap : MonoBehaviour
{
    public GameObject manualMap;
    public GameObject fullMap;
    public Button playButton;
    void Start()
    {
        SetObjectsActive(true, false, true);
        playButton.onClick.AddListener(ShowfullMap);
    }
    void ShowfullMap()
    {
        fullMap.SetActive(true);
        manualMap.SetActive(false);
    }


    void SetObjectsActive(bool manualMapActive, bool fullMapActive, bool playButtonActive)
    {
        if (manualMap != null) manualMap.SetActive(manualMapActive);
        if (fullMap != null) fullMap.SetActive(fullMapActive);
        if (playButton != null) playButton.gameObject.SetActive(playButtonActive);

    }
}
