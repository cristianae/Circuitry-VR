using UnityEngine;

public class FlashingLight : MonoBehaviour
{
    public Light lampLight;  //lamp light
    public Renderer lampRenderer;    //glow
    public Color successColor = Color.green;
    public Color failColor = Color.red;
    public float flashSpeed = 2f;    //flashing speed
    public bool isFlashing = false;
    private Color originalColor;
    private Color originalEmission;
    private Material lampMaterial;
    public Color flashColor = Color.white;

    void Start()
    {
        if (lampLight == null)
            lampLight = GetComponentInChildren<Light>();
        if (lampRenderer == null)
            lampRenderer = GetComponent<Renderer>();
        if (lampRenderer != null)
        {
            lampMaterial = lampRenderer.material;
            originalColor = lampMaterial.color;
            if (lampMaterial.HasProperty("_EmissionColor"))
                originalEmission = lampMaterial.GetColor("_EmissionColor");
        }
    }

    void Update()
    {
        if (isFlashing)
        {
            float intensity = Mathf.Abs(Mathf.Sin(Time.time * flashSpeed));
            if (lampLight != null)
            {
                lampLight.color = flashColor;
                lampLight.intensity = Mathf.Lerp(0, 5, intensity);
            }
            if (lampMaterial != null)
            {
                Color emissive = flashColor * intensity * 2f;
                lampMaterial.SetColor("_EmissionColor", emissive);
                DynamicGI.SetEmissive(lampRenderer, emissive);
            }
        }
        else
        {
            ResetLamp();
        }
    }

    public void StartFlashing()
    {
        isFlashing = true;
    }

    public void StopFlashing()
    {
        isFlashing = false;
        ResetLamp();
    }

    private void ResetLamp()
    {
        if (lampLight != null)
        {
            lampLight.intensity = 1f;
            lampLight.color = Color.white;
        }

        if (lampMaterial != null)
        {
            lampMaterial.color = originalColor;
            lampMaterial.SetColor("_EmissionColor", originalEmission);
        }
    }
    public void SetStaticColor(Color c)
{
    isFlashing = false;
    if (lampLight != null) lampLight.color = c;
    if (lampMaterial != null)
    {
        lampMaterial.color = c;
        if (lampMaterial.HasProperty("_EmissionColor"))
            lampMaterial.SetColor("_EmissionColor", c);
    }
}

}
