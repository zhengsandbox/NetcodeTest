using Unity.Entities;
using Unity.NetCode;

[GenerateAuthoringComponent]
[GhostComponent(OwnerPredictedSendType =GhostSendType.All ,PrefabType = GhostPrefabType.All)]
public struct GameEntityInstanceGUID : IComponentData
{
    [GhostField]
    public SandboxGuid instanceGUID;
}
