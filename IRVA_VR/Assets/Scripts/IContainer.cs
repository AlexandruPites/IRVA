using UnityEngine;

public interface IContainer
{
    int Capacity { get; }
    
    float ScaleModifier { get; }
    Transform GetSpawnPoint();
}
