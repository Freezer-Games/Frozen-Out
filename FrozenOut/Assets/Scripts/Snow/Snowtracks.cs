using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowtracks : MonoBehaviour
{
    public Shader drawShader;

    public int mapResolution;
    private RenderTexture trackMap;
    private Material drawMaterial;
    private Material[] snowMaterial;

    public GameObject[] terrains;
    public Transform[] colliders;

    RaycastHit groundHit;
    int layerMask;

    [Range(0, 2)]
    public float brushSize;
    [Range(0, 1)]
    public float brushStrength;
    [Range(0,10)]
    public float recoveryTime;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Ground");
        drawMaterial = new Material(drawShader);
        snowMaterial = new Material[terrains.Length];
        trackMap = new RenderTexture(mapResolution, mapResolution, 0, RenderTextureFormat.ARGBFloat);
        for (int i = 0; i < terrains.Length; i++)
        {
            snowMaterial[i] = terrains[i].GetComponent<MeshRenderer>().material;
            snowMaterial[i].SetTexture("_TrackMap", trackMap);
        }
    }

    // Update is called once per frame
    void Update()
    {
        drawMaterial.SetFloat("_RecoveryTime", recoveryTime);
        drawMaterial.SetFloat("_Timer", Time.deltaTime);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] != null)
            {
                if (Physics.Raycast(colliders[i].position, -Vector3.up, out groundHit))
                {
                    drawMaterial.SetVector("_Coordinate", new Vector4(groundHit.textureCoord.x, groundHit.textureCoord.y, 0, 0));
                    drawMaterial.SetFloat("_Size", brushSize);
                    drawMaterial.SetFloat("_Strength", brushStrength);
                    RenderTexture temp = RenderTexture.GetTemporary(trackMap.width, trackMap.height, 0, RenderTextureFormat.ARGBFloat);
                    Graphics.Blit(trackMap, temp);
                    Graphics.Blit(temp, trackMap, drawMaterial);
                    RenderTexture.ReleaseTemporary(temp);
                }
            }
        }
    }
}
