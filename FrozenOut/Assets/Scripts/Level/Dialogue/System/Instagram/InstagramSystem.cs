using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Localization;

using InstagramConnection;
using InstagramConnection.Model;
using System.Linq;

namespace Scripts.Level.Dialogue.Runner.Instagram
{
    public class InstagramSystem : DialogueSystem
    {
        public DialogueManager DialogueManager;

        private InstagramService Service;

        private bool Running;
        private bool RequestedNextLine;

        private const string NoInternetDialogue = "Parece que este cacharro no funciona hoy";

        private void Start()
        {
            Running = false;
            InitService();
        }

        private void InitService()
        {
            try
            {
                Service = new InstagramService();
            }
            catch (System.InvalidOperationException)
            {
            }

        }

        public override bool GetBoolVariable(string variableName, bool includeLeading = true)
        {
            return false;
        }

        public override float GetNumberVariable(string variableName, bool includeLeading = true)
        {
            return 0.0f;
        }

        public override string GetStringVariable(string variableName, bool includeLeading = true)
        {
            return "";
        }

        public override bool IsRunning()
        {
            return Running;
        }

        public override void RequestNextLine()
        {
            RequestedNextLine = true;
        }

        public override void SetLanguage(Locale locale)
        {
            return;
        }

        public override void SetVariable<T>(string variableName, T value, bool includeLeading = true)
        {
            return;
        }

        public override void StartDialogue(DialogueActer acter)
        {
            DialogueManager.OnDialogueStarted();
            Running = true;

            StartCoroutine(DoShowLastComments(acter.TalkToNode));
        }

        public override void Stop()
        {
            StopAllCoroutines();

            DialogueManager.OnDialogueEnded();
        }

        private IEnumerator DoShowLastComments(string name)
        {
            DialogueManager.OnLineStyleUpdated(name);
            DialogueManager.OnLineNameUpdated(name);
            DialogueManager.OnLineDialogueUpdated("Ehem... vamos a ver que dice la gente");

            while (!RequestedNextLine)
            {
                yield return null;
            }
            RequestedNextLine = false;
            yield return new WaitForEndOfFrame();

            if(Service != null)
            {
                ICollection<Comment> lastComments = Service.GetLatestPostComments();

                foreach (Comment comment in lastComments.Take(3))
                {
                    // Regex para quitar emojis ;( no soportados por Unity.Text
                    string commentTextClean = Regex.Replace(comment.Text, @"\p{Cs}", "");
                    if (comment.Text.Length > 80)
                    {
                        commentTextClean = commentTextClean.Substring(0, 75) + "...";
                    }

                    string commentFormatted = "Dice <i>" + comment.Username + "</i> que " + "\"" + commentTextClean + "\"";
                    DialogueManager.OnLineStyleUpdated(name);
                    DialogueManager.OnLineNameUpdated(name);
                    DialogueManager.OnLineDialogueUpdated(commentFormatted);

                    while (!RequestedNextLine)
                    {
                        yield return null;
                    }
                    RequestedNextLine = false;

                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                DialogueManager.OnLineStyleUpdated(name);
                DialogueManager.OnLineNameUpdated(name);
                DialogueManager.OnLineDialogueUpdated("Vaya");

                while (!RequestedNextLine)
                {
                    yield return null;
                }
                RequestedNextLine = false;
                yield return new WaitForEndOfFrame();
                
                DialogueManager.OnLineStyleUpdated(name);
                DialogueManager.OnLineNameUpdated(name);
                DialogueManager.OnLineDialogueUpdated(NoInternetDialogue);

                while (!RequestedNextLine)
                {
                    yield return null;
                }
                RequestedNextLine = false;
                yield return new WaitForEndOfFrame();
            }

            Running = false;
            DialogueManager.OnDialogueEnded();
        }
    }
}