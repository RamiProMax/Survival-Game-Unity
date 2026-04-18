using UnityEngine;

public class FireFlicker : MonoBehaviour
{
    public Light fireLight;
    public float minIntensity = 1.5f;
    public float maxIntensity = 3.5f;

    void Update()
    {
        fireLight.intensity = Random.Range(minIntensity, maxIntensity);
    }
}