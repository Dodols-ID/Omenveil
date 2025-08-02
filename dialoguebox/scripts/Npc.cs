using Godot;
using System;

public partial class Npc : StaticBody3D
{
	[Export] public Sprite3D Icon;
	[Export] public string iconPath;
	[Export] public string dialoguePath;
	public Area3D InteractionArea;
	private bool _playerInRange = false;
	private bool _inDialogue = false;
	private bool _justInDialogue = false;
	private bool _npcWaitingForChoice;
	private DialogueManager dialogueManager;
	
	
	public override void _Ready()
	{
		Icon.Visible = false;
		var texture = GD.Load<Texture2D>(iconPath);
		Icon.Texture = texture;
		InteractionArea = GetNode<Area3D>("npc_interactionarea");
		dialogueManager = (DialogueManager) GetNode("/root/DialogueManager");
		dialogueManager.Connect("EndDial", new Callable(this, nameof(OnDialogueEnd)));
		dialogueManager.Connect("AwaitingChoice", new Callable(this, nameof(OnAwaitingChoice)));
		if (InteractionArea != null)
		{
			InteractionArea.BodyEntered += OnBodyEntered;
			InteractionArea.BodyExited += OnBodyExited;
		}
	}

	public override void _Process(double delta)
	{
        if (_playerInRange && Input.IsActionJustPressed("dialogue_start") && !_justInDialogue)
		{
			var dialogue = DialogueLoader.LoadDialogue(dialoguePath);

			if (_inDialogue == false)
			{
				_inDialogue = true;
				dialogueManager.StartDialogue(dialogue);
			}	
			else
			{
				while (_npcWaitingForChoice == true)
				{
					_npcWaitingForChoice = true;
				}
				if (!_npcWaitingForChoice)	
					dialogueManager.NextLine();
			}
		}
	}

	public void OnBodyEntered(Node3D body)
	{
		if (body.Name == "player")
		{
			var player = body as Node;
			player.Set("current_npc", this);

			_playerInRange = true;
			if (Icon != null)	
				Icon.Visible = true;
		}
		_justInDialogue = false;
	}

	public void OnBodyExited(Node3D Body)
	{
		if (Body.Name == "player")
		{
			var player = Body as Node;
			if ((Node)player.Get("current_npc") == this)
				player.Set("current_npc", (Node3D)null);
		}
		_playerInRange = false;
			if (Icon != null)	
				Icon.Visible = false;
		_justInDialogue = false;
	}
	public void OnDialogueEnd()
	{
		_inDialogue = false;
		Icon.Visible = false;
		_justInDialogue = true;
	}
	public void OnAwaitingChoice(bool value)
	{
		_npcWaitingForChoice = value;
	}
}
