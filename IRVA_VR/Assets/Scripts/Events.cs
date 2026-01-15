using UnityEngine;

namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit {}
}

public record LockUnlocked(string lockTag);

public record DrawerOpened(string drawerTag);
public record DrawerClosed(string drawerTag);
public record FirstPuzzleFinished;
public record FirstPuzzleButtonPressed(int Number);

public record SecondPuzzleFinished;

public record SecondPuzzleCodeEntered(string value);

public record ThirdPuzzleFinished;

public record FourthPuzzleFinished;

public record FifthPuzzleFinished;

public record SixthPuzzleFinished;
