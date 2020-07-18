using System;

namespace Scripts.Level.Mission
{
    [Serializable]
    public class MissionInfo
    {
        public string Name;
        public string Description;
        public bool IsDone;
    }

    [Serializable]
    public class MissionBase
    {
        public string VariableName;

        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is MissionBase))
            {
                return false;
            }

            MissionBase other = (MissionBase)obj;
            return this.VariableName.Equals(other.VariableName);
        }
    }
}