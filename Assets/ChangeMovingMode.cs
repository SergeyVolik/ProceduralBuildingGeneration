using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ChangeMovingMode : MonoBehaviour
{
    [SerializeField]
    TMP_Text butonText;
    [SerializeField]
    GameObject FPSController;

    [SerializeField]
    GameObject FreeCamera;
    [SerializeField]
    GameObject ToPanelCamera;

    [SerializeField]
    GameObject canvasWithChageMode;

    [SerializeField]
    GameObject canvasWithSettings;

    private enum CameraStates { Free, FPS, Panel }
    CameraStates state = CameraStates.Free;
    private void Awake()
    {
        butonText.text = "ToFreeCamera";
    }
    public void ChangeMode()
    {

        switch (state)
        {
            case CameraStates.FPS:
                butonText.text = "ToFPSCamera";

                FPSController.SetActive(false);
                FreeCamera.SetActive(true);
                ToPanelCamera.SetActive(false);

                FreeCamera.transform.rotation = FPSController.transform.rotation;
                FreeCamera.transform.position = FPSController.transform.position;

                canvasWithSettings.SetActive(false);
                canvasWithChageMode.SetActive(true);
                state = CameraStates.Free;
                break;
            case CameraStates.Panel:
                canvasWithSettings.SetActive(true);
                canvasWithChageMode.SetActive(false);
                butonText.text = "ToFreeCamera";
                ToPanelCamera.SetActive(true);
                FPSController.SetActive(false);
                FreeCamera.SetActive(false);
                state = CameraStates.FPS;
                break;
            case CameraStates.Free:
                canvasWithSettings.SetActive(false);
                canvasWithChageMode.SetActive(true);
                butonText.text = "ToPanelCamera";
                ToPanelCamera.SetActive(false);
                FPSController.SetActive(true);
                FreeCamera.SetActive(false);
               
                state = CameraStates.Panel;
                break;
        }

       
    }
    
}
