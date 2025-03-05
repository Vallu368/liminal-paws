using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFixedCamera : MonoBehaviour
{
    public GameObject mainCameraObject;
    public GameObject staticCameraObject;
    public bool mainActive = true;

    void Start()
    {
        staticCameraObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        {
            if (other.gameObject.tag == "Player")
            {
                Debug.Log("touched dog :)");
                if (mainActive)
                {
                    mainCameraObject.SetActive(false);
                    staticCameraObject.SetActive(true);
                    mainActive = false;
                }
                else
                {
                    mainCameraObject.SetActive(true);
                    staticCameraObject.SetActive(false);
                    mainActive = true;
                }
            }
        }
    }
}
