using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Tactics.Core{
public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance {get; private set;}
    private CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake() {
        if (Instance != null)
        {
            Debug.LogError("There is more than one ScreenShake! " + transform + "-" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void Shake(float intensity = 1f)
    {
        cinemachineImpulseSource.GenerateImpulse(intensity);
    }
}
}
