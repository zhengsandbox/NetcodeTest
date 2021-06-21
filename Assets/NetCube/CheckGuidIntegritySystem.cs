using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[UpdateInGroup(typeof(GhostPredictionSystemGroup))]
public class CheckGuidIntegritySystem : SystemBase
{
	GhostPredictionSystemGroup m_GhostPredictionSystemGroup;
	protected override void OnCreate()
	{
		m_GhostPredictionSystemGroup = World.GetExistingSystem<GhostPredictionSystemGroup>();
	}
	
	protected override void OnUpdate()
	{
		var tick = m_GhostPredictionSystemGroup.PredictingTick;
		var deltaTime = Time.DeltaTime;
		
		Entities
			.WithoutBurst()
			.ForEach((in PredictedGhostComponent prediction, in GhostAsset ghostAsset) =>
		{
			if (!GhostPredictionSystemGroup.ShouldPredict(tick, prediction))
				return;

			if (World.GetExistingSystem<ClientSimulationSystemGroup>() != null)
			{
				Debug.Log($"<color=green> GhostAssetId: {ghostAsset.assetIdGUID.GetHashCode()} GhostUserId: {ghostAsset.userIdGUID.GetHashCode()} isAvatar: {ghostAsset.isAvatar} </color>");
			}
			else if (World.GetExistingSystem<ServerSimulationSystemGroup>() != null)
			{
				Debug.Log($"<color=red> GhostAssetId: {ghostAsset.assetIdGUID.GetHashCode()} GhostUserId: {ghostAsset.userIdGUID.GetHashCode()} isAvatar: {ghostAsset.isAvatar}</color>");
			}
			
		}).Run();
	}
}
