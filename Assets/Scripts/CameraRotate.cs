using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{


   

    public void RotateCamera()
    {
       transform.rotation *= Quaternion.Euler(0, 180, 0);
    }

}


