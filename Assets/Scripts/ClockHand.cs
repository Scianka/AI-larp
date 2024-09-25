using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockHand : MonoBehaviour
{
    // if this element is to be only decorative, it could simulate a working pressure device
    void Update() => transform.Rotate(0f, 0f, -6f * Time.deltaTime);
}
