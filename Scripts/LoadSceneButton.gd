extends Button

@export
var ScenePath: String = "res://Scenes/!MainGameScene/Game.tscn"
@export
var MainMenu: Node
var been_pressed = false

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	ResourceLoader.load_threaded_request(ScenePath)

func _pressed() -> void:
	been_pressed = true
	%MainMenu.visible = false
	%LoadingScreen.visible = true

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if been_pressed and ResourceLoader.load_threaded_get_status(ScenePath)==ResourceLoader.THREAD_LOAD_LOADED:
		var MainLevel = ResourceLoader.load_threaded_get(ScenePath).instantiate()
		var root = get_tree().get_root()
		MainMenu.queue_free()
		root.add_child(MainLevel)
