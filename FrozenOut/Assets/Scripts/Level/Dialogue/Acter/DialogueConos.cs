namespace Scripts.Level.Dialogue
{
    public class DialogueConos : DialogueActer
    {
        protected ILevelManager LevelManager => GameManager.Instance.CurrentLevelManager;

        void Start()
        {
            SetBlocking();
            SetNonAutomatic();
        }

        public override void OnStartTalk()
        {

        }

        public override void OnEndTalk()
        {

        }

        public override void OnPlayerClose()
        {

        }

        public override void OnPlayerAway()
        {

        }

        public override void OnSelected()
        {

        }

        public override void OnDeselected()
        {

        }
    }
}