using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarSlider : MonoBehaviour
{
    void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }
}
