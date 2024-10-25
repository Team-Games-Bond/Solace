extends Control

@export
var ControllerDefault: bool = false

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	if ControllerDefault:
		visibility_changed.connect(grab_if_applicable)
		grab_if_applicable()
	
func grab_if_applicable() -> void:
	if Input.get_connected_joypads().size()!=0:
		grab_focus()
		
