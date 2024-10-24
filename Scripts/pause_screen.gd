extends CanvasLayer

signal pauseToggle

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	visible = false
	$Blur/Darken.modulate.a=0

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func _input(event: InputEvent) -> void:
	if event.is_action_pressed("pause"):
		PauseToggle()


func PauseToggle() -> void:
	var tree = get_tree()
	var blurMaterial = $Blur.material
	var set_blur = func (blur:float) -> void:
		blurMaterial.set_shader_parameter("blur_strength", blur)
	if !tree.paused:
		visible=true
		tree.paused = !tree.paused
		var tween = create_tween();
		tween.set_ease(Tween.EASE_OUT).tween_method(
			set_blur,0.0 ,1.0, 0.25
			)
		tween.parallel().tween_property($Blur/Darken, "modulate:a", 1.0, 0.25)
		await tween.chain().finished
	else:
		var tween = create_tween();
		tween.set_ease(Tween.EASE_IN).tween_method(
			set_blur,1.0 ,0.0, 0.25
			)
		tween.parallel().tween_property($Blur/Darken, "modulate:a", 0.0, 0.25)
		await tween.chain().finished
		visible=false
		tree.paused = !tree.paused
	pauseToggle.emit()
