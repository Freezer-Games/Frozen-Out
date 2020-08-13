using UnityEngine;

using Cradle.StoryFormats.Sugar;

public partial class @Lv1_Dialogue : SugarStory
{
    public Scripts.Level.Dialogue.System.Cradle.CradleDialogueSystem CradleSystem;

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
}