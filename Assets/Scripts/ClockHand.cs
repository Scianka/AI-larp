using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClockHand : MonoBehaviour
{
    void Update() => transform.Rotate(0f, 0f, -6f * Time.deltaTime);
}
