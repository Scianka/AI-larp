using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsInfo : MonoBehaviour
{
    public GameObject group;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (group.activeSelf) group.SetActive(false);
            else group.SetActive(true);
        }
    }
}