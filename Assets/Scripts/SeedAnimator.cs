using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedAnimator : MonoBehaviour
{
    [SerializeField]
    float minAngle = -30f;

    [SerializeField]
    float maxAngle = 100f;

    [SerializeField]
    float rotateTimeFactor = 1.0f;

    [SerializeField]
    Vector3 minScale = new Vector3(0.85f, 0.85f, 0.85f);

    [SerializeField]
    Vector3 maxScale = new Vector3(1.1f, 1.1f, 1.1f);

    [SerializeField]
    Vector3 scaleTimeFactor = Vector3.one;

    [SerializeField]
    Color color1 = Color.cyan;

    [SerializeField]
    Color color2 = Color.red;

    [SerializeField]
    Color color3 = Color.green;

    [SerializeField]
    Renderer seedRenderer_;

    [SerializeField]
    Color particleColor1 = Color.yellow;

    [SerializeField]
    Color particleColor2 = Color.green;

    [SerializeField]
    ParticleSystem particleSystem_;

    // Start is called before the first frame update
    void Start()
    {
        ResetColor();
    }

    public void ResetColor()
    {
        seedRenderer_.material.SetColor("_Color1", color1);
        seedRenderer_.material.SetColor("_Color2", color2);
        seedRenderer_.material.SetColor("_Color3", color3);

        var main = particleSystem_.main;
        main.startColor =
            new ParticleSystem.MinMaxGradient(particleColor1, particleColor2);
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Lerp(minAngle, maxAngle, Mathf.Abs(Mathf.Sin(Time.time * rotateTimeFactor)));
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = rotation;

        transform.localScale = new Vector3(
                Mathf.Lerp(minScale.x, maxScale.x, Mathf.Abs(Mathf.Sin(Time.time * scaleTimeFactor.x))),
                Mathf.Lerp(minScale.y, maxScale.y, Mathf.Abs(Mathf.Sin(Time.time * scaleTimeFactor.y))),
                Mathf.Lerp(minScale.z, maxScale.z, Mathf.Abs(Mathf.Sin(Time.time * scaleTimeFactor.z)))
         );
    }
}
