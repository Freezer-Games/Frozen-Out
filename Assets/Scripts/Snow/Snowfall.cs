using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowfall : MonoBehaviour
{
    public Shader snowfallShader;
    [Range(0.0001f, 0.1f)]
    public float flakeAmount = 0.01f;
    [Range(0, 1)]
    public float flakeOpacity = 0.08f;

    private Material _snowfallMaterial;
    private MeshRenderer _meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _snowfallMaterial = new Material(snowfallShader);
    }

    // Update is called once per frame
    void Update()
    {
        _snowfallMaterial.SetFloat("_FlakeAmount", flakeAmount);
        _snowfallMaterial.SetFloat("_FlakeOpacity", flakeOpacity);

        RenderTexture snow = (RenderTexture) _meshRenderer.material.GetTexture("_Splat");
        RenderTexture temp = RenderTexture.GetTemporary(snow.width, snow.height, 0, RenderTextureFormat.ARGBFloat);

        Graphics.Blit(snow, temp, _snowfallMaterial);
        Graphics.Blit(temp, snow);

        _meshRenderer.material.SetTexture("_Splat", snow);

        RenderTexture.ReleaseTemporary(temp);
    }
}
