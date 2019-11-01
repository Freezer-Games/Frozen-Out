using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowtracks : MonoBehaviour
{
    public GameObject _ground;
    public Shader _snowtrackShader;
    public Transform PolPalo;
    [Range(0, 2)]
    public float _brushSize = 0.5f;
    [Range(0, 1)]
    public float _brushStrength = 0.8f;

    private RenderTexture _splatMap;
    private Material _snowMaterial;
    private Material _drawMaterial;
    private int _terrainMask;
    // Start is called before the first frame update
    void Start()
    {
        _terrainMask = LayerMask.GetMask("Ground");

        _drawMaterial = new Material(_snowtrackShader);

        _snowMaterial = _ground.GetComponent<MeshRenderer>().material;
        _splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        _snowMaterial.SetTexture("_Splat", _splatMap);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit groundHit;
        if(Physics.Raycast(PolPalo.position, Vector3.down, out groundHit, 0.5f, _terrainMask))
        {
            _drawMaterial.SetVector("_Coordinate", new Vector4(groundHit.textureCoord.x, groundHit.textureCoord.y, 0, 0));
            _drawMaterial.SetFloat("_Size", _brushSize);
            _drawMaterial.SetFloat("_Strength", _brushStrength);
            RenderTexture temp = RenderTexture.GetTemporary(_splatMap.width, _splatMap.height, 0, RenderTextureFormat.ARGBFloat);
            Graphics.Blit(_splatMap, temp);
            Graphics.Blit(temp, _splatMap, _drawMaterial);
            RenderTexture.ReleaseTemporary(temp);
        }
    }
}
