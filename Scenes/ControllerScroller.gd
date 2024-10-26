extends ScrollContainer
@export
var Sensitivity: float = 500;
var Velocity: Vector2=Vector2.ZERO
var Fingers: Dictionary = {}
var v_scrollbar: ScrollBar
var h_scrollbar: ScrollBar
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	v_scrollbar = get_v_scroll_bar()
	h_scrollbar = get_v_scroll_bar()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if is_visible_in_tree():
		scroll_vertical += roundi(
			Input.get_axis("ScrollUp", "ScrollDown") * Sensitivity * delta
		)
		scroll_horizontal += roundi(
			Input.get_axis("ScrollLeft", "ScrollRight") * Sensitivity * delta
		)
	scroll_vertical += roundi(Velocity.y*delta)
	scroll_horizontal += roundi(Velocity.x*delta)
	if v_scrollbar.max_value<=scroll_vertical or scroll_vertical <=v_scrollbar.min_value:
		Velocity.y=0
	if h_scrollbar.max_value<=scroll_horizontal or scroll_horizontal <=h_scrollbar.min_value:
		Velocity.x=0
	Velocity = Velocity-(Velocity*.7*delta)

func _input(event: InputEvent) -> void:
	if event is InputEventScreenDrag and event.index in Fingers:
		scroll_vertical-=event.screen_relative.y
		scroll_horizontal-=event.screen_relative.x
		Fingers[event.index]=event.velocity
		
	if event is InputEventScreenTouch:
		if event.pressed:
			if is_visible_in_tree():
				Velocity=Vector2.ZERO
				Fingers[event.index]=null
		elif event.index in Fingers: # Released
			if Fingers[event.index] != null:
				Velocity-=Fingers[event.index]
				print(Fingers[event.index])
			Fingers.erase(event.index)
			
