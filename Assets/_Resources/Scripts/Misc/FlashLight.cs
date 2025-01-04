using UnityEngine;

public class FlashLight : MonoBehaviour
{
    public Light linkedLight;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            linkedLight.enabled = !linkedLight.enabled;
        }
    }

    void LightOff()
    {
        linkedLight.enabled = false;
    }
}