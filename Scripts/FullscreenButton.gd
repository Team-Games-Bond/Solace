extends Button

func _pressed():
	var windowId = DisplayServer.get_window_list()[0]
	var mode = (
		DisplayServer.WindowMode.WINDOW_MODE_FULLSCREEN
		if DisplayServer.window_get_mode(windowId)==DisplayServer.WindowMode.WINDOW_MODE_WINDOWED 
		else DisplayServer.WindowMode.WINDOW_MODE_WINDOWED)
	DisplayServer.window_set_mode(mode, windowId)
