using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    public CinemachineVirtualCamera activeCam;
    public int numCam;


    private void Awake()
    {
        numCam= 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            activeCam.Priority = 1;
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            activeCam.Priority = 0;
        }
    }

}
