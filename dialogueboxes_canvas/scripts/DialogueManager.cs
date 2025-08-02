using Godot;
using System;

public partial class DialogueManager : Node
{
	private DialogueData currentDialogue;
	private int currentLineIndex;

	public Label speakerLabel;
	public Label textLabel;
	private CanvasLayer uiInstance;
	private bool _awaitingChoice = false;
	[Signal]
	public delegate void EndDialEventHandler();
	[Signal]
	public delegate void AwaitingChoiceChangedEventHandler(bool isAwaiting);
	public bool AwaitingChoice
	{
		get => _awaitingChoice;
		set
		{
			if (_awaitingChoice != value)
			{
				_awaitingChoice = value;
				EmitSignal(SignalName.AwaitingChoiceChanged, value);
			}
		}
	}
	public void StartDialogue(DialogueData dialogue)
	{
		var uiScene = GD.Load<PackedScene>("res://scenes/db.tscn");
		uiInstance = (CanvasLayer)uiScene.Instantiate();
		AddSibling(uiInstance);
		

		speakerLabel = uiInstance.GetNode<Label>("Background/speakerLabel");
		textLabel = uiInstance.GetNode<Label>("Background/textLabel");

		currentDialogue = dialogue;
		currentLineIndex = 0;
		uiInstance.Visible = true;
		ShowLine(currentLineIndex);
	}

	public void NextLine()
	{
		var line = currentDialogue.lines[currentLineIndex];
		if (line.type == "dialogue_option")
		{
			ShowLine(currentLineIndex);
		}
		else if (line.next != null)
		{
			currentLineIndex = line.next.Value;
			ShowLine(currentLineIndex);
		}
		else
		{
			GD.Print("Debug4: the option SOMEHOW got here");
			EndDialogue();
		}
	}
	private void ShowLine(int index)
	{
		var line = currentDialogue.lines[index];
		speakerLabel.Text = line.speaker;
		textLabel.Text = line.text;

		var optionContainer = uiInstance.GetNode<VBoxContainer>("Background/db_choices");
		optionContainer.Visible = false;
		QueueFreeChildren(optionContainer);

		if (line.type == "dialogue_option")
		{	
			_awaitingChoice = true;
			optionContainer.Visible = true;
			GD.Print("Debug1: the option got here");
			GD.Print("Debug5: ", line.option);
			foreach (var opt in line.option)
			{
				GD.Print("Debug2: the option created");
				var button = new Button();
				button.Text = opt.b_text;
				button.Pressed += () => OnOptionSelected(opt.b_next);
				optionContainer.AddChild(button);
			}
		}	
		else
		{
			_awaitingChoice = false;
			GD.Print("Debug3: the option somehow got here");
		}
	}

	private void QueueFreeChildren(VBoxContainer container)
	{
		var child = container.GetChildren();
		foreach (var c in child)
		{
			c.QueueFree();
		}
	}
	private void OnOptionSelected(int nextIndex)
	{
		var optionContainer = uiInstance.GetNode<VBoxContainer>("Background/db_choices");
		optionContainer.Visible = false;
		_awaitingChoice = false;

		currentLineIndex = nextIndex;
		ShowLine(currentLineIndex);
	
	}

	private void EndDialogue()
	{
		uiInstance.QueueFree();
		EmitSignal(SignalName.EndDial);
	}
}
