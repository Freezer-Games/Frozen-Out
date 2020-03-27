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
        } = false;
        
        private Dictionary<string, string> LocalizedText;
        private const string MissingTextString = "Localized text not found";

        public void LoadLocalisedText(string fileName)
        {
            LocalizedText = new Dictionary<string, string>();
            string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                LocalisationData loadedData = JsonUtility.FromJson<LocalisationData>(dataAsJson);

                for (int i = 0; i < loadedData.items.Length; i++)
                {
                    LocalizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
                }
            }

            IsReady = true;
        }

        public string GetLocalisedValue(string key)
        {
            string result = MissingTextString;
            if (LocalizedText.ContainsKey(key))
            {
                result = LocalizedText[key];
            }

            return result;

        }
    }
}