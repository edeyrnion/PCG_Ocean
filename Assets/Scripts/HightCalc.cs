using UnityEngine;

public class HightCalc : MonoBehaviour
{
    [SerializeField] private PerlinNoiseLayer _perlinNoiseLayer;

    private void Update()
    {
        var x = transform.position.x * _perlinNoiseLayer.Scale + Time.timeSinceLevelLoad * _perlinNoiseLayer.Speed;
        var z = transform.position.z * _perlinNoiseLayer.Scale + Time.timeSinceLevelLoad * _perlinNoiseLayer.Speed;


        transform.position = new Vector3(transform.position.x, ((Mathf.PerlinNoise(x, z) - 0.5f) * 60f), transform.position.z);
    }
}
