extends Control

func _switch(name: String):
	for child in get_children():
		child.visible = child.name==name
