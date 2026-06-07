extends Control

@export var game: PackedScene

func _on_button_pressed() -> void:
	get_tree().change_scene_to_packed(game)
