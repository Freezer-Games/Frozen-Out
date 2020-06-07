using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Save
{
    [System.Serializable]
    public class SaveData
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


        public SaveData(Game game)
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
            positionplayer[0] = game.PlayerPos.position.x;
            positionplayer[1] = game.PlayerPos.position.y;
            positionplayer[2] = game.PlayerPos.position.z;
            rotationplayer[0] = game.PlayerPos.rotation.x;
            rotationplayer[1] = game.PlayerPos.rotation.y;
            rotationplayer[2] = game.PlayerPos.rotation.z;
            rotationplayer[3] = game.PlayerPos.rotation.w;
            positioncamera[0] = game.CameraPos.position.x;
            positioncamera[1] = game.CameraPos.position.y;
            positioncamera[2] = game.CameraPos.position.z;
            positionauxcamera[0] = game.auxcamerapos.position.x;
            positionauxcamera[1] = game.auxcamerapos.position.y;
            positionauxcamera[2] = game.auxcamerapos.position.z;
            foreach (GameObject objeto in game.ListaObjetos)
            {
                positions.Add(objeto.transform.position.x);
                positions.Add(objeto.transform.position.y);
                positions.Add(objeto.transform.position.z);
                rotations.Add(objeto.transform.rotation.x);
                rotations.Add(objeto.transform.rotation.y);
                rotations.Add(objeto.transform.rotation.z);
                rotations.Add(objeto.transform.rotation.w);
                nombres.Add(objeto.name);
                if (objeto.GetComponent<Patrulla>() != null)
                    nextpos.Add(objeto.GetComponent<Patrulla>().SiguientePunto);
            }
            YarnKeys = game.YarnKeys;
            YarnValues = game.YarnValues;
            level = SceneManager.GetActiveScene().buildIndex;
        }
    }
}