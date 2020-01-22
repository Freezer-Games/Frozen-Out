using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowtracks : MonoBehaviour
{
    public Shader drawShader;

    private RenderTexture trackMap;
    private Material snowMaterial, drawMaterial;

    public GameObject terrain;
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
        snowMaterial = terrain.GetComponent<MeshRenderer>().material;
        snowMaterial.SetTexture("_TrackMap", trackMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat));
    }

    // Update is called once per frame
    void Update()
    {
        drawMaterial.SetFloat("_RecoveryTime", recoveryTime);
        drawMaterial.SetFloat("_Timer", Time.deltaTime);

        for (int i = 0; i < colliders.Length; i++)
        {
            //algunos personajes desaparecen, hay que comprobar que aun existen
            if (colliders[i] != null)
            {
                if (Physics.Raycast(colliders[i].position, -Vector3.up, out groundHit, colliders[i].gameObject.GetComponent<SkinnedMeshRenderer>().bounds.size.y/2, layerMask))
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
