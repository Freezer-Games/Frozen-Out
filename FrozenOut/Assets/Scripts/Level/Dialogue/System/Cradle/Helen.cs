using UnityEngine;

using Cradle.StoryFormats.Sugar;

public partial class @Helen : SugarStory
{
    public Scripts.Level.Dialogue.System.Cradle.CradleDialogueSystem CradleSystem;

    #region Commands
    private void giveitem(string itemVariableName, string quantity)
    {
        int realQuantity = int.Parse(quantity);

        CradleSystem.PickItem(itemVariableName, realQuantity);
    }

    private void useitem(string itemVariableName, string quantity)
    {
        int realQuantity = int.Parse(quantity);

        CradleSystem.UseItem(itemVariableName, realQuantity);
    }

    private void setanim(string npcName, string animation)
    {
        CradleSystem.SetNPCAnimation(npcName, animation);
    }

    private void setanimall(string npcName, string animation)
    {
        CradleSystem.SetNPCAnimationWithSimilarName(npcName, animation);
    }

    private void stopanim(string npcName)
    {
        CradleSystem.StopNPCAnimation(npcName);
    }

    private void stopanimall(string npcName)
    {
        CradleSystem.StopNPCAnimationWithSimilarName(npcName);
    }

    private void startinstagram()
    {
        CradleSystem.SwitchToSecondary("Instagram");
    }
    #endregion

    #region Functions
    private bool has_item(string item)
    {
		return CradleSystem.IsItemInInventory(item);
	}

	private bool used_item(string item)
    {
		return CradleSystem.IsItemUsed(item);
	}

	private int quantity_item(string item)
    {
		return CradleSystem.QuantityOfItem(item);
	}

	private bool done_mission(string mission)
    {
		return CradleSystem.MarkMissionDone(mission);
	}

	private bool random_bool()
    {
		float random = Random.value;
		return random > 0.5f;
	}
    #endregion
}