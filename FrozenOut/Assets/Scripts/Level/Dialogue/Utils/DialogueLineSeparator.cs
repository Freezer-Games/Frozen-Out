public class DialogueLineSeparator
{
    public DialogueLineSeparator(string styleSeparator, string dialogueSeparator)
    {
        this.StyleSeparator = styleSeparator;
        this.DialogueSeparator = dialogueSeparator;
    }

    private string StyleSeparator;
    private string DialogueSeparator;

    public void Separate(string text, out string style, out string name, out string dialogue)
    {
        int indexOfStyleSeparator = text.IndexOf(StyleSeparator);
        int indexOfDialogueSeparator = text.IndexOf(DialogueSeparator);

        if (indexOfDialogueSeparator == -1) // No se especifica nombre
        {
            name = "";
            style = "";
            dialogue = text;
        }
        else
        {
            if (indexOfStyleSeparator == -1) // No hay estilo adicional
            {
                name = text.Substring(0, indexOfDialogueSeparator);
                style = name;
            }
            else
            {
                name = text.Substring(0, indexOfStyleSeparator);

                int styleLength = indexOfDialogueSeparator - (indexOfStyleSeparator + StyleSeparator.Length);
                style = text.Substring(indexOfStyleSeparator + StyleSeparator.Length, styleLength);
            }
            dialogue = text.Substring(indexOfDialogueSeparator + DialogueSeparator.Length);
        }
    }
}
