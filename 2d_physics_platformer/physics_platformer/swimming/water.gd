extends Node2D

signal body_entered_water(body: Node2D)
signal body_exited_water(body: Node2D)

func _on_area_2d_body_entered(body: Node2D) -> void:
	body_entered_water.emit(body)

func _on_area_2d_body_exited(body: Node2D) -> void:
	body_exited_water.emit(body)
