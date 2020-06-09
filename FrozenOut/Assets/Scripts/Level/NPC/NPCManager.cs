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
            NPCInfo selectedNPC = GetNPCInfo(npcName);

            if(selectedNPC != null)
            {
                selectedNPC.StartAnimation(animation);
            }
        }

        /// Para casos en los que se requiere más de un npc. Preferible usar StartAnimation
        public void StartAnimationWithSimilarName(string npcName, string animation)
        {
            IEnumerable<NPCInfo> selectedNPCs = GetNPCInfosBySimilarName(npcName);

            foreach(NPCInfo npc in selectedNPCs)
            {
                npc.StartAnimation(animation);
            }
        }

        public void StopAnimation(string npcName)
        {
            NPCInfo selectedNPC = GetNPCInfo(npcName);

            if (selectedNPC != null)
            {
                selectedNPC.StopAnimation();
            }
        }

        /// En casos en los que se requiere parar más de un NPC. Preferible usar StopAnimation.
        public void StopAnimationsWithSimilarName(string npcName)
        {
            IEnumerable<NPCInfo> selectedNPCs = GetNPCInfosBySimilarName(npcName);

            foreach(NPCInfo npc in selectedNPCs)
            {
                npc.StopAnimation();
            }
        }

        public NPCInfo GetNPCInfo(string name)
        {
            NPCInfo selectedNPC = NPCs.FirstOrDefault<NPCInfo>(npc => npc.Name == name);

            return selectedNPC;
        }

        public IEnumerable<NPCInfo> GetNPCInfosBySimilarName(string name)
        {
            IEnumerable<NPCInfo> selectedNPCs = NPCs.Where<NPCInfo>(npc => npc.Name.Contains(name));

            return selectedNPCs;
        }
    }
}