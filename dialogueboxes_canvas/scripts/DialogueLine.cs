using System.Collections.Generic;

public class DialogueLine
{
    public string type { get; set; }
    public string speaker { get; set; }
    public string text { get; set; }
    public int? next { get; set; }
    public List<DialogueOption> option { get; set; }
}