using Godot;
using System;

public partial class Camerarig : Node3D
{
	[Export] public Node3D Player;
	[Export] public float FollowSpeed = 5.0f;
	[Export] public Vector3 FollowOffset = new Vector3(0, 3, 3);


    public override void _PhysicsProcess(double delta)
    {
        if (Player == null)
			return;
		
		Vector3 targetPos = Player.GlobalTransform.Origin + FollowOffset;
		GlobalTransform = new Transform3D(GlobalTransform.Basis,
			GlobalTransform.Origin.Lerp(targetPos, (float)delta * FollowSpeed));
    }

	


	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
