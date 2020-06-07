using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Level.NPC
{
    public class NPCManager : MonoBehaviour
    {
        public LevelManager LevelManager;

        public List<NPCInfo> NPCs;

        public void StartAnimation(string npcName, string animation)
        {
            NPCInfo selectedNPC = GetNPCInfoByName(npcName);

            if(selectedNPC != null)
            {
                selectedNPC.StartAnimation(animation);
            }
        }

        public virtual void StopAnimation(string npcName)
        {
            NPCInfo selectedNPC = GetNPCInfoByName(npcName);

            if (selectedNPC != null)
            {
                selectedNPC.StopAnimation();
            }
        }

        public NPCInfo GetNPCInfoByName(string name)
        {
            NPCInfo selectedNPC = NPCs.SingleOrDefault<NPCInfo>(npc => npc.Name == name);

            return selectedNPC;
        }
    }
}