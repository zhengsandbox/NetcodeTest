using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
public class DisconnectSystem : SystemBase
{
	protected override void OnUpdate()
	{
		int disconnectId = 0;
		Entities
			.WithAll<NetworkStreamDisconnected>()
			.ForEach((in NetworkIdComponent networkId) => {
				disconnectId = networkId.Value;
				Debug.LogError($"Found disconneced comp! {networkId}");
			}).Run();
	}
}