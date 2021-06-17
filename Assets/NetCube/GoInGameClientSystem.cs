using Unity.Collections;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

// RPC request from client to server for game to go "in game" and send snapshots / inputs
public struct GoInGameRequest : IRpcCommand
{
}

// When client has a connection with network id, go in game and tell server to also go in game
[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
[AlwaysSynchronizeSystem]
public class GoInGameClientSystem : SystemBase
{
	protected override void OnCreate()
	{
		RequireSingletonForUpdate<EnableNetCubeGame>();
		RequireForUpdate(GetEntityQuery(ComponentType.ReadOnly<NetworkIdComponent>(), ComponentType.Exclude<NetworkStreamInGame>()));
	}

	protected override void OnUpdate()
	{
		var commandBuffer = new EntityCommandBuffer(Allocator.Temp);
		Entities.WithNone<NetworkStreamInGame>().ForEach((Entity ent, in NetworkIdComponent id) =>
		{
			commandBuffer.AddComponent<NetworkStreamInGame>(ent);
			var req = commandBuffer.CreateEntity();
			commandBuffer.AddComponent<GoInGameRequest>(req);
			commandBuffer.AddComponent(req, new SendRpcCommandRequestComponent { TargetConnection = ent });
			Debug.Log($"<color=green>GAME REQUEST SENT. Target connection entity {ent}</color>");
		}).Run();
		commandBuffer.Playback(EntityManager);
	}
}