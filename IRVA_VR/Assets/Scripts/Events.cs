using UnityEngine;

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit {}
}

public record LockUnlocked(string lockTag);

public record DrawerOpened(string drawerTag);
public record DrawerClosed(string drawerTag);
public record PuzzleFinished(int number, Vector3 position);

public record SecondPuzzleCodeEntered(string value);
public record FirstPuzzleButtonPressed(int Number);

