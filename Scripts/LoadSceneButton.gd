extends Button

@export
var ScenePath: String = "res://Scenes/!MainGameScene/Game.tscn"
@export
var MainMenu: Node
@export
var ControllerDefault: bool = false
var been_pressed = false
var MainLevel = null

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	if Input.get_connected_joypads().size()!=0 and ControllerDefault:
		grab_focus()
	if ResourceLoader.load_threaded_get_status(ScenePath)==ResourceLoader.THREAD_LOAD_INVALID_RESOURCE:
		ResourceLoader.load_threaded_request(ScenePath)

func _pressed() -> void:
	been_pressed = true
	%MainMenu.visible = false
	%LoadingScreen.visible = true

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if been_pressed and ResourceLoader.load_threaded_get_status(ScenePath)==ResourceLoader.THREAD_LOAD_LOADED:
		MainLevel = ResourceLoader.load_threaded_get(ScenePath).instantiate()
		var root = get_tree().get_root()
		#MainMenu.queue_free()
		MainMenu.visible = false
		root.add_child(MainLevel)


func _on_main_menu_trigger_link() -> void:
	been_pressed = false
	%MainMenu.visible = true
	%LoadingScreen.visible = false
	MainMenu.visible = true
	MainLevel.queue_free()
	_ready()
