using System;
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
			.ForEach((in PredictedGhostComponent prediction, in GhostAsset ghostAsset, in GameEntityInstanceGUID entityInstance) =>
		{
			if (!GhostPredictionSystemGroup.ShouldPredict(tick, prediction))
				return;

			if (World.GetExistingSystem<ClientSimulationSystemGroup>() != null)
			{
				Debug.Log($"<color=green> GhostAssetId: {new Guid(ghostAsset.assetIdGUID.ToByteArray())} GhostUserId: {new Guid(ghostAsset.userIdGUID.ToByteArray())} entityInstance: {new Guid(entityInstance.instanceGUID.ToByteArray())} </color>");
			}
			else if (World.GetExistingSystem<ServerSimulationSystemGroup>() != null)
			{
				Debug.Log($"<color=red> GhostAssetId: {new Guid(ghostAsset.assetIdGUID.ToByteArray())} GhostUserId: {new Guid(ghostAsset.userIdGUID.ToByteArray())} entityInstance: {new Guid(entityInstance.instanceGUID.ToByteArray())} </color>");
			}
			
		}).Run();
	}
}
