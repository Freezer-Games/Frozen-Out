using System;

namespace Scripts.Level.Mission
{
    [Serializable]
    public class MissionInfo : MissionBase
    {
        public bool IsDone;

        public void SetDone()
        {
            IsDone = true;
            isActive = false;
        }
    }

    [Serializable]
    public class MissionBase
    {
        public string VariableName;
        public bool isActive;

        public override bool Equals(object obj)
        {
            if ((obj == null) || !(obj is MissionBase))
            {
                return false;
            }

            MissionBase other = (MissionBase)obj;
            return this.VariableName.Equals(other.VariableName);
        }

        public bool IsActive() { return isActive; }

        public void SetActive() { isActive = true; }

        public void SetInactive() { isActive = false; }
    }
}