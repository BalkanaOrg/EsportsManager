using Godot;

public partial class SelectionButton : Button
{
    [Export] public bool IsTeamButton = true; // Set in Godot editor!

    private MainMenu _mainMenu;

    public override void _Ready()
    {
        // Get the MainMenu node - adjust path to your scene structure
        _mainMenu = GetNode<MainMenu>("/root/MainMenu");

        // Correct signal connection
        Pressed += OnButtonPressed;

        // Initialize pressed state - use SetPressedNoSignal to avoid triggering event
        if (IsTeamButton)
            SetPressedNoSignal(true);
    }

    private void OnButtonPressed()
    {
        // Notify MainMenu of the selection
        _mainMenu?.OnSelectionButtonPressed(IsTeamButton ? 0 : 1);

        // Visual feedback (optional)
        Modulate = ButtonPressed ? new Color(0.9f, 0.9f, 1.0f) : Colors.White;
    }
}