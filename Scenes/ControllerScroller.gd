extends ScrollContainer
@export
var Sensitivity: float = 500;


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if is_visible_in_tree():
		scroll_vertical += roundi(
			Input.get_axis("ScrollUp", "ScrollDown") * Sensitivity * delta
		)
		scroll_horizontal += roundi(
			Input.get_axis("ScrollLeft", "ScrollRight") * Sensitivity * delta
		)
