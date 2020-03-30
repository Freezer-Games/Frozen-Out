using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Scripts.Localisation
{
    public class LocalisationManager : MonoBehaviour
    {

        public bool IsReady
        {
            get;
            private set;
        }
        
        private Dictionary<string, string> LocalisedText;
        private const string MissingTextString = "Localized text not found";

        private GameManager GameManager => GameManager.Instance;

        void start()
        {
            IsReady = false;
        }

        public void LoadLocalisedText(string fileName)
        {
            LocalisedText = new Dictionary<string, string>();
            string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                LocalisationData loadedData = JsonUtility.FromJson<LocalisationData>(dataAsJson);

                for (int i = 0; i < loadedData.items.Length; i++)
                {
                    LocalisedText.Add(loadedData.items[i].key, loadedData.items[i].value);
                }
            }

            IsReady = true;
        }

        public string GetLocalisedValue(string key)
        {
            string result = MissingTextString;
            if (LocalisedText.ContainsKey(key))
            {
                result = LocalisedText[key];
            }

            return result;
        }

    }
}