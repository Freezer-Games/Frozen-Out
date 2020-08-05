using UnityEngine;
using System;

namespace Scripts.Level.Dialogue.System.Ink
{
    [Serializable]
    public class InkTuple
    {
        public string Name => StoryJSON.name;
        public TextAsset StoryJSON;
    }
}