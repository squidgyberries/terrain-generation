using Godot;
using System;

public partial class CameraController : Camera3D {
	[Export]
	public float sensitivity = 0.005f;
	[Export]
	public float speed = 0.2f;
	bool mouseCaptured = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		var move = new Vector3();
		if (Input.IsActionPressed("move_forward")) {
			move.Z -= 1.0f;
		}
		if (Input.IsActionPressed("move_backward")) {
			move.Z += 1.0f;
		}
		if (Input.IsActionPressed("move_left")) {
			move.X -= 1.0f;
		}
		if (Input.IsActionPressed("move_right")) {
			move.X += 1.0f;
		}
		Translate(move.Normalized() * speed);
	}

	public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("capture_camera")) {
			Input.MouseMode = Input.MouseModeEnum.Captured;
		} else if (@event.IsActionReleased("capture_camera")) {
			Input.MouseMode = Input.MouseModeEnum.Visible;
		} else if (@event is InputEventMouseMotion mouseMotion) {
			if (Input.MouseMode == Input.MouseModeEnum.Captured) {
				float yaw = -mouseMotion.ScreenRelative.X * sensitivity;
				float pitch = -mouseMotion.ScreenRelative.Y * sensitivity;
				pitch = Mathf.Clamp(pitch, Mathf.DegToRad(-90) - Rotation.X, Mathf.DegToRad(90) - Rotation.X);

				Rotate(Vector3.Up, yaw);
				RotateObjectLocal(Vector3.Right, pitch);
			}
		}
	}
}
