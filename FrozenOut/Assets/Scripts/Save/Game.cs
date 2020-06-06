using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;
using Scripts.Level.Dialogue.Runner.YarnSpinner;

namespace Scripts.Save
{
    public class Game : MonoBehaviour
    {
        public Transform PlayerPos;
        public Transform CameraPos;
        public Transform auxcamerapos;
        [System.NonSerialized]
        public List<string> YarnKeys;
        [System.NonSerialized]
        public List<float> YarnValues;
        public static Game instance;
        public List<GameObject> ListaObjetos;
        [System.NonSerialized]
        public int level;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        }

        public void SaveGame()
        {
            level = SceneManager.GetActiveScene().buildIndex;
            YarnKeys = new List<string>();
            YarnValues = new List<float>();
            foreach (var item in FindObjectOfType<YarnVariableStorage>().Variables)
            {
                string llave = item.Key.Remove(0, 1);
                if (llave != "Yarn.ShuffleOptions")
                {
                    YarnKeys.Add(llave);
                    YarnValues.Add(item.Value.AsNumber);
                }
                Debug.Log(item.Key);
                Debug.Log(item.Value);
            }

            BinaryFormatter Formateador = new BinaryFormatter();

            SaveSystem.SaveGame(this);
        }

        public void LoadGame()
        {
            //print("Cargando...");
            SaveData data = SaveSystem.LoadGame();
            level = data.level;
            StartCoroutine(LoadLevel(level, data));
        }

        IEnumerator LoadLevel(int level, SaveData data)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);
            // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
            while (!async.isDone)
            {
                yield return null;
            }
            Time.timeScale = 1;
            Vector3 newposplayer = new Vector3(data.positionplayer[0], data.positionplayer[1], data.positionplayer[2]);
            Vector3 newposcamera = new Vector3(data.positioncamera[0], data.positioncamera[1], data.positioncamera[2]);
            Vector3 newauxposcamera = new Vector3(data.positionauxcamera[0], data.positionauxcamera[1], data.positionauxcamera[2]);
            GameObject.Find("Player").GetComponent<MoverAlCargar>().mover(newposplayer);
            GameObject.Find("Player").GetComponent<MoverAlCargar>().rotar(data.rotationplayer);
            GameObject.Find("MainCamera").GetComponent<MoverAlCargar>().mover(newposcamera);

            //cargar informacion de dialogos ya realizados
            for (int i = 0; i < data.YarnValues.Count; i++)
            {
                FindObjectOfType<YarnVariableStorage>().ResetToDefaults();
                Yarn.Value yarnValue = new Yarn.Value(data.YarnValues[i]);
                Debug.Log(data.YarnKeys[i]);
                Debug.Log(yarnValue);
                FindObjectOfType<YarnVariableStorage>().SetValue(data.YarnKeys[i], yarnValue);
            }

            //cargar posiciones de cada objeto que se haya movido
            for (int i = 0; i < data.nombres.Count; i++)
            {
                Vector3 newpos = new Vector3(data.positions[i * 3], data.positions[i * 3 + 1], data.positions[i * 3 + 2]);
                float[] rotaciones = new float[4];
                rotaciones[0] = data.rotations[i * 4];
                rotaciones[1] = data.rotations[i * 4 + 1];
                rotaciones[2] = data.rotations[i * 4 + 2];
                rotaciones[3] = data.rotations[i * 4 + 3];
                GameObject.Find(data.nombres[i]).GetComponent<MoverAlCargar>().mover(newpos);
                GameObject.Find(data.nombres[i]).GetComponent<MoverAlCargar>().rotar(rotaciones);
                if (GameObject.Find(data.nombres[i]).GetComponent<Patrulla>() != null) { GameObject.Find(data.nombres[i]).GetComponent<Patrulla>().SiguientePunto = data.nextpos[i]; }
            }
        }

        // Alternativa a OnLevelWasLoaded(int), el cual esta en desuso
        private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (GameObject.Find("Player") != null)
            {
                PlayerPos = GameObject.Find("Player").transform;
                CameraPos = GameObject.Find("MainCamera").transform;
            }
        }

    }
}