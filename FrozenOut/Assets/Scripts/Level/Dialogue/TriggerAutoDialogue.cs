namespace Scripts.Level.Dialogue
{
    public class TriggerAutoDialogue : TriggerActDialogue
    {
        private DialogueManager DialogueManager => GameManager.Instance.CurrentLevelManager.GetDialogueManager();

        protected override void OnPlayerEnter()
        {
            base.OnPlayerEnter();
            
            DialogueManager.StartDialogue(Acter);
        }

        protected override void OnPlayerExit()
        {
            base.OnPlayerExit();
            
            DialogueManager.StopDialogue();
        }
    }
}