using UnityEngine;

public class CameraGameOverEffect : MonoBehaviour
{
    private Material cameraMaterial;
    public float grayscale;
    public float appliedTime = 1.5f;

    void Start()
    {
        cameraMaterial = new Material(Shader.Find("Custom/Grayscale"));
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        cameraMaterial.SetFloat("_Grayscale", grayscale);
        Graphics.Blit(source, destination, cameraMaterial);
    }
}
