using AOT;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using Unity.Burst;
using UnityEngine;

public struct EnableNetCubeGame : IComponentData
{}

// Control system updating in the default world
[UpdateInWorld(UpdateInWorld.TargetWorld.Default)]
[AlwaysSynchronizeSystem]
public class Game : SystemBase
{
    // Singleton component to trigger connections once from a control system
    struct InitGameComponent : IComponentData
    {
    }
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<InitGameComponent>();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "NetCube")
            return;
        // Create singleton, require singleton for update so system runs once
        EntityManager.CreateEntity(typeof(InitGameComponent));
    }

    protected override void OnUpdate()
    {
        // Destroy singleton to prevent system from running again
        EntityManager.DestroyEntity(GetSingletonEntity<InitGameComponent>());
        foreach (var world in World.All)
        {
            var network = world.GetExistingSystem<NetworkStreamReceiveSystem>();
            var epPort = (ushort)7979;
            if (world.GetExistingSystem<ClientSimulationSystemGroup>() != null)
            {
                world.EntityManager.CreateEntity(typeof(EnableNetCubeGame));
                // Client worlds automatically connect to localhost
                NetworkEndPoint ep = NetworkEndPoint.LoopbackIpv4;
                ep.Port = epPort;
#if UNITY_EDITOR
                ep = NetworkEndPoint.Parse(ClientServerBootstrap.RequestedAutoConnect, epPort);
#endif
                var result = network.Connect(ep);
                Debug.Log($"<color=green>client connect! result entity {result}</color>");
            }
            #if UNITY_EDITOR || UNITY_SERVER
            else if (world.GetExistingSystem<ServerSimulationSystemGroup>() != null)
            {
                world.EntityManager.CreateEntity(typeof(EnableNetCubeGame));
                // Server world automatically listen for connections from any host
                NetworkEndPoint ep = NetworkEndPoint.AnyIpv4;
                ep.Port = epPort;
                var result = network.Listen(ep);
                Debug.Log($"<color=red>server listen! result {result}</color>");
            }
            #endif
        }
    }
}



