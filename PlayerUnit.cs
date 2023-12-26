using Godot;
using System;

public partial class PlayerUnit : Unit
{
    Sprite2D selectionVisual;
    public override void _Ready()
    {
        selectionVisual = GetNode<Sprite2D>("SelectionVisual");
    }

    public void ToggleSelectionVisual(bool toggle)
    {
        selectionVisual.Visible = toggle;
    }
}
