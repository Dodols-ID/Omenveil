using System.Text.Json;
using Godot;

public static class DialogueLoader
{
    public static DialogueData LoadDialogue(string path)
    {
        var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        string json = file.GetAsText();
        file.Close();
        return JsonSerializer.Deserialize<DialogueData>(json);
    }
}
