extends ProgressBar

@export
var ScenePath: String = "res://Scenes/!MainGameScene/Game.tscn"
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	max_value=1


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	var progress = []
	var status = ResourceLoader.load_threaded_get_status(ScenePath, progress)
	value = progress[0]
