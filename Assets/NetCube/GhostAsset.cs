using Unity.Entities;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct GhostAsset : IComponentData
{
    [GhostField]
    public SandboxGuid userIdGUID;
    [GhostField]
    public SandboxGuid assetIdGUID;
    [GhostField]
    public bool isAvatar;
}
