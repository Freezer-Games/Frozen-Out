using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Localization;

using InstagramConnection;
using InstagramConnection.Model;
using System.Linq;
using CsvHelper.Configuration;
using System;
using Cinemachine;

namespace Scripts.Level.Dialogue.System.Instagram
{
    public class InstagramSystem : SecondaryDialogueSystem
    {
        public DialogueManager DialogueManager;

        private InstagramService Service;

        private bool Running;
        private bool RequestedNextLine;

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
            catch (global::System.InvalidOperationException)
            {
            }

        }

        public override bool IsRunning()
        {
            return Running;
        }

        public override void RequestNextLine()
        {
            RequestedNextLine = true;
        }

        public override void Switch()
        {
            StartDialogue();
        }

        public override void SetLanguage(Locale locale)
        {
            return;
        }

        public override void StartDialogue()
        {
            Running = true;

            StartCoroutine(DoShowLastComments());
        }

        public override void Stop()
        {
            StopAllCoroutines();
            Running = false;

            DialogueManager.OnDialogueEnded();
        }

        private IEnumerator DoShowLastComments()
        {
            if(Service != null)
            {
                ICollection<Comment> lastComments = null;
                try
                {
                    lastComments = Service.GetLatestPostComments();
                }
                catch(InvalidOperationException)
                {
                }

                if(lastComments == null)
                {
                    DialogueManager.MainDialogueSystem.SetVariable<bool>("instagramempty", false);
                    DialogueManager.MainDialogueSystem.SetVariable<bool>("instagramerror", true);

                    DialogueManager.OnLineStarted();
                    DialogueManager.OnLineDialogueUpdated("...");

                    while (!RequestedNextLine)
                    {
                        yield return null;
                    }
                    RequestedNextLine = false;

                    yield return new WaitForEndOfFrame();
                }
                else if(lastComments.Count() == 0)
                {
                    DialogueManager.MainDialogueSystem.SetVariable<bool>("instagramempty", true);
                    DialogueManager.MainDialogueSystem.SetVariable<bool>("instagramerror", false);

                    DialogueManager.OnLineStarted();
                    DialogueManager.OnLineDialogueUpdated("...");

                    while (!RequestedNextLine)
                    {
                        yield return null;
                    }
                    RequestedNextLine = false;

                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    DialogueManager.MainDialogueSystem.SetVariable<bool>("instagramempty", false);
                    DialogueManager.MainDialogueSystem.SetVariable<bool>("instagramerror", false);

                    foreach (Comment comment in lastComments.Take(3))
                    {
                        string nameText = comment.Username;
                        nameText = CleanComment(nameText);

                        string commentText = comment.Text;
                        commentText = CleanComment(commentText);
                        commentText = TrimComment(commentText);

                        string commentFormatted = "<i>" + nameText + "</i>: " + "\"" + commentText + "\"";
                        DialogueManager.OnLineStarted();
                        DialogueManager.OnLineDialogueUpdated(commentFormatted);

                        while (!RequestedNextLine)
                        {
                            yield return null;
                        }
                        RequestedNextLine = false;

                        yield return new WaitForEndOfFrame();
                    }
                }
            }
            else
            {
                DialogueManager.MainDialogueSystem.SetVariable<bool>("instagramempty", false);
                DialogueManager.MainDialogueSystem.SetVariable<bool>("instagramerror", true);

                DialogueManager.OnLineStarted();
                DialogueManager.OnLineDialogueUpdated("...");

                while (!RequestedNextLine)
                {
                    yield return null;
                }
                RequestedNextLine = false;

                yield return new WaitForEndOfFrame();
            }

            Running = false;
            DialogueManager.SwitchToMain();
        }

        private string CleanComment(string comment)
        {
            // Regex para quitar emojis ;( no soportados por Unity.Text
            return Regex.Replace(comment, @"\p{Cs}", "");
        }

        private string TrimComment(string comment)
        {
            string trimmedComment = comment;
            if (comment.Length > 80)
            {
                trimmedComment = comment.Substring(0, 75) + "...";
            }

            return trimmedComment;
        }
    }
}