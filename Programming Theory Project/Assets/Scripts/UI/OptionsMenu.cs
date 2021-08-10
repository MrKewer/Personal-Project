using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Button infoButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private ScrollRect scrollView;
    [SerializeField] private GameObject infoContent;
    [SerializeField] private RectTransform infoRectTrans;
    [SerializeField] private GameObject controlsContent;
    [SerializeField] private RectTransform controlsRectTrans;
    private Vector3 resetPosition = new Vector3(0, 0, 0);

    [Space]
    [Header("HyperLinks")] //Shown in the Information section
    [Space]
    [SerializeField] private Button artstationLink;
    [SerializeField] private Button linkedinLink;
    [SerializeField] private Button playStoreLink;
    [SerializeField] private Button unityLearnLink;

    private void OnEnable()
    {
        //Sets the info section active
        DeactivateAll();
        infoContent.gameObject.SetActive(true);
        scrollView.content = infoRectTrans;
    }
    void Start()
    {
        //Add listeners to the buttons
        infoButton.onClick.AddListener(InfoButtonClick);
        controlsButton.onClick.AddListener(ControlsButtonClick);

        //HyperLinks
        artstationLink.onClick.AddListener(delegate { GoToWebsite("https://www.artstation.com/mrkewer"); });
        linkedinLink.onClick.AddListener(delegate { GoToWebsite("https://www.linkedin.com/in/danie-grobbelaar-0435099b/"); });
        playStoreLink.onClick.AddListener(delegate { GoToWebsite("https://play.google.com/store/apps/developer?id=MrKewer"); });
        unityLearnLink.onClick.AddListener(delegate { GoToWebsite("https://learn.unity.com/u/5a3d6b6203b0020018f46b55?tab=profile"); });
    }
    private void InfoButtonClick() //Sets the Info section active
    {
        DeactivateAll();
        infoContent.gameObject.SetActive(true);
        scrollView.content = infoRectTrans;
        infoRectTrans.transform.position = resetPosition;
    }
    private void ControlsButtonClick() //Sets the Controls section active
    {
        DeactivateAll();
        controlsContent.gameObject.SetActive(true);
        scrollView.content = controlsRectTrans;
        controlsRectTrans.transform.position = resetPosition;
    }

    private void GoToWebsite(string webLink) //Open the web address
    {
        Application.OpenURL(webLink);
    }

    private void DeactivateAll() //Clear all screens
    {
        controlsContent.gameObject.SetActive(false);
        infoContent.gameObject.SetActive(false);
    }

}
