using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public float[] positionplayer;
    public float[] rotationplayer;
    public float[] positioncamera;
    public float[] positionauxcamera;
    public List<string> YarnKeys;
    public List<float> YarnValues;
    public int level;
    public List<int> nextpos;
    public List<string> nombres;
    public List<float> positions;
    public List<float> rotations;


    public GameData(Game game)
    {
        //creamos los arrays
        positionplayer = new float[3];
        rotationplayer = new float[4];
        positioncamera = new float[3];
        positionauxcamera = new float[3];
        nextpos = new List<int>();
        nombres = new List<string>();
        positions = new List<float>();
        rotations = new List<float>();
        YarnKeys = new List<string>();
        YarnValues = new List<float>();
        //asignamos los arrays
        positionplayer[0] = game.playerpos.position.x;
        positionplayer[1] = game.playerpos.position.y;
        positionplayer[2] = game.playerpos.position.z;
        rotationplayer[0] = game.playerpos.rotation.x;
        rotationplayer[1] = game.playerpos.rotation.y;
        rotationplayer[2] = game.playerpos.rotation.z;
        rotationplayer[3] = game.playerpos.rotation.w;
        positioncamera[0] = game.camerapos.position.x;
        positioncamera[1] = game.camerapos.position.y;
        positioncamera[2] = game.camerapos.position.z;
        positionauxcamera[0] = game.auxcamerapos.position.x;
        positionauxcamera[1] = game.auxcamerapos.position.y;
        positionauxcamera[2] = game.auxcamerapos.position.z;
        foreach(GameObject objeto in game.ListaObjetos)
        {
            positions.Add(objeto.transform.position.x);
            positions.Add(objeto.transform.position.y);
            positions.Add(objeto.transform.position.z);
            rotations.Add(objeto.transform.rotation.x);
            rotations.Add(objeto.transform.rotation.y);
            rotations.Add(objeto.transform.rotation.z);
            rotations.Add(objeto.transform.rotation.w);
            nombres.Add(objeto.name);
            if (objeto.GetComponent<patrullar>() != null)
            nextpos.Add(objeto.GetComponent<patrullar>().destPoint);
        }
        YarnKeys = game.YarnKeys;
        YarnValues = game.YarnValues;
        level = SceneManager.GetActiveScene().buildIndex;
    }
}