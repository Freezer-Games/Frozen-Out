using Assets.Scripts.Dialogue;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using Yarn.Unity;


public class Game : MonoBehaviour
{
    public Transform playerpos;
    public Transform camerapos;
    public Transform auxcamerapos;
    public Dictionary<string, Mision> Missions;
    [System.NonSerialized]
    public List<string> YarnKeys;
    [System.NonSerialized]
    public List<float> YarnValues;
    public static Game instance;
    public List<GameObject> ListaObjetos;
    [System.NonSerialized]
    public int level;
    [System.NonSerialized]
    public bool loading = false;
    private int loading2 = 0;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    public void SaveGame() {
        Missions = GameObject.FindObjectOfType<MissionsCanvas>().Missions;
        level = SceneManager.GetActiveScene().buildIndex;
        YarnKeys = new List<string>();
        YarnValues = new List<float>();
        foreach (var item in FindObjectOfType<VariableStorageYarn>().Variables)
        {
            string llave = item.Key.Remove(0,1);
            if (llave != "Yarn.ShuffleOptions")
            {
                YarnKeys.Add(llave);
                YarnValues.Add(item.Value.AsNumber);
            }
            Debug.Log(item.Key);
            Debug.Log(item.Value);
        }

        BinaryFormatter Formateador = new BinaryFormatter();
        string pathM = Application.persistentDataPath + "/Misions.sv";
        FileStream streamM = new FileStream(pathM, FileMode.Create);
        Formateador.Serialize(streamM, Missions);
        streamM.Close();


        SaveSystem.SaveGame(this);
    }

    public void LoadGame()
    {
        //print("Cargando...");
        GameData data = SaveSystem.LoadGame();
        level = data.level;
        loading = true;
        loading2 = 2;
        StartCoroutine(LoadLevel(level, data));
    }

    IEnumerator LoadLevel(int level, GameData data)
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
        GameObject.Find("POL").GetComponent<moveralcargar>().mover(newposplayer);
        GameObject.Find("POL").GetComponent<moveralcargar>().rotar(data.rotationplayer);
        GameObject.Find("CameraBase").GetComponent<moveralcargar>().mover(newposcamera);
        GameObject.Find("AuxCamPos").GetComponent<moveralcargar>().mover(newauxposcamera);
        //if (level == 2) { }else if (level == 3) { }
        string pathM = Application.persistentDataPath + "/Misions.sv";
        if (File.Exists(pathM))
        {
            using (FileStream stream = new FileStream(pathM, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Missions = formatter.Deserialize(stream) as Dictionary<string, Mision>;
            }
            GameObject.FindObjectOfType<MissionsCanvas>().Missions = Missions;
            GameObject.FindObjectOfType<MissionsCanvas>().ActualizarMisiones();
        }
        else
        {
            Debug.LogError("Save file not found in " + pathM);
        }
        print(data.YarnValues.Count);
        for (int i = 0; i < data.YarnValues.Count; i++)
        {
            FindObjectOfType<VariableStorageYarn>().ResetToDefaults();
            Yarn.Value yarnValue = new Yarn.Value(data.YarnValues[i]);
            Debug.Log(data.YarnKeys[i]);
            Debug.Log(yarnValue);
            FindObjectOfType<VariableStorageYarn>().SetValue(data.YarnKeys[i], yarnValue);
        }

        for (int i = 0; i < data.nombres.Count; i++)
        {
            Vector3 newpos = new Vector3(data.positions[i * 3], data.positions[i * 3 + 1], data.positions[i * 3 + 2]);
            float[] rotaciones = new float[4];
            rotaciones[0] = data.rotations[i * 4];
            rotaciones[1] = data.rotations[i * 4 + 1];
            rotaciones[2] = data.rotations[i * 4 + 2];
            rotaciones[3] = data.rotations[i * 4 + 3];
            GameObject.Find(data.nombres[i]).GetComponent<moveralcargar>().mover(newpos);
            GameObject.Find(data.nombres[i]).GetComponent<moveralcargar>().rotar(rotaciones);
            if (GameObject.Find(data.nombres[i]).GetComponent<patrullar>() != null) { GameObject.Find(data.nombres[i]).GetComponent<patrullar>().destPoint = data.nextpos[i]; }
        }
        loading2 = 1;
    }

    // Alternativa a OnLevelWasLoaded(int), el cual esta en desuso
    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (GameObject.Find("POL") != null)
        {
            playerpos = GameObject.Find("POL").transform;
            camerapos = GameObject.Find("CameraBase").transform;
            auxcamerapos = GameObject.Find("AuxCamPos").transform;
        }
        if (loading2 == 0) { loading2 = 1; }
        else if (loading2 == 1) { Destroy(this.gameObject); }
    }

}