namespace Scripts.Level.Dialogue
{
    public class DialogueCinematic : DialogueActer
    {
        void Awake()
        {
            SetBlocking();
            SetAutomatic();
        }

        public override void OnStartTalk() {}

        public override void OnEndTalk() {}

        public override void OnPlayerClose() {}

        public override void OnPlayerAway() {}

        public override void OnSelected() {}

        public override void OnDeselected() {}
    }
}