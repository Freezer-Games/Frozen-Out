namespace Scripts.Localisation
{
    [System.Serializable]
    public class LocalisationData
    {
        public LocalisationItem[] items;
    }

    [System.Serializable]
    public class LocalisationItem
    {
        public string key;
        public string value;
    }
}