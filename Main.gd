extends Node

var roll1 : MeshInstance
var roll2 : MeshInstance
var roll3 : MeshInstance

var rolling : bool

var rolling1 : bool
var rolling2 : bool
var rolling3 : bool

var lever : Spatial

var tween : Tween
var lever_timer : Timer
var rolling_timer : Timer

var counter : int

var speed = -8.5

func _ready():
	roll1 = $Rolls/Roll1
	roll2 = $Rolls/Roll2
	roll3 = $Rolls/Roll3
	tween = $Tween
	lever = $Lever/Spatial
	lever_timer = $LeverTimer
	rolling_timer = $RollingTimer
	rolling = false
	rolling1 = false
	rolling2 = false
	rolling3 = false
	counter = 1


func _process(delta):
	if rolling:
		if rolling1:
			roll1.rotate_z(speed * delta)
		if rolling2:
			roll2.rotate_z(speed * delta)
		if rolling3:
			roll3.rotate_z(speed * delta)


func _on_StaticBody_input_event(camera, event, click_position, click_normal, shape_idx):
	if event is InputEventMouseButton:
		if event.button_index == BUTTON_LEFT:
			if event.pressed:
				print("CLICK")
				tween.interpolate_property(lever, "rotation:z", -0.31, -1.57, 1, Tween.TRANS_BACK, Tween.EASE_IN)
				tween.start()
				lever_timer.start()
				rolling = true
				rolling1 = true
				rolling2 = true
				rolling3 = true
				rolling_timer.start(5)
				counter = 1


func _on_Timer_timeout():
	tween.interpolate_property(lever, "rotation:z", -1.57, -0.31, 0.3, Tween.TRANS_LINEAR, Tween.EASE_IN)
	tween.start()


func _on_RollingTimer_timeout():
	match counter:
		1:
			counter += 1
			rolling1 = false
			rolling_timer.start(randf() * 5)
		2:
			counter += 1
			rolling2 = false
			rolling_timer.start(randf() * 5)
		3:
			counter = 0
			rolling3 = false
