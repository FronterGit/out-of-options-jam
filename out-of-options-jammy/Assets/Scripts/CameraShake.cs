using FirstGearGames.SmoothCameraShaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraShake : MonoBehaviour
{

    public ShakeData cameraShakeData;

    // Update is called once per frame
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CameraShakerHandler.Shake(cameraShakeData);
        }
        
    }
}
