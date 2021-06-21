//THIS FILE IS AUTOGENERATED BY GHOSTCOMPILER. DON'T MODIFY OR ALTER.
using System;
using AOT;
using Unity.Burst;
using Unity.Networking.Transport;
using Unity.NetCode.LowLevel.Unsafe;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Collections;
using Unity.NetCode;
using Unity.Transforms;
using Unity.Mathematics;

namespace NetCube.Generated
{
    [BurstCompile]
    public struct GhostAssetGhostComponentSerializer
    {
        static GhostComponentSerializer.State GetState()
        {
            // This needs to be lazy initialized because otherwise there is a depenency on the static initialization order which breaks il2cpp builds due to TYpeManager not being initialized yet
            if (!s_StateInitialized)
            {
                s_State = new GhostComponentSerializer.State
                {
                    GhostFieldsHash = 11266163746584454747,
                    ExcludeFromComponentCollectionHash = 0,
                    ComponentType = ComponentType.ReadWrite<GhostAsset>(),
                    ComponentSize = UnsafeUtility.SizeOf<GhostAsset>(),
                    SnapshotSize = UnsafeUtility.SizeOf<Snapshot>(),
                    ChangeMaskBits = ChangeMaskBits,
                    SendMask = GhostComponentSerializer.SendMask.Interpolated | GhostComponentSerializer.SendMask.Predicted,
                    SendToOwner = SendToOwnerType.All,
                    SendForChildEntities = 1,
                    VariantHash = 0,
                    CopyToSnapshot =
                        new PortableFunctionPointer<GhostComponentSerializer.CopyToFromSnapshotDelegate>(CopyToSnapshot),
                    CopyFromSnapshot =
                        new PortableFunctionPointer<GhostComponentSerializer.CopyToFromSnapshotDelegate>(CopyFromSnapshot),
                    RestoreFromBackup =
                        new PortableFunctionPointer<GhostComponentSerializer.RestoreFromBackupDelegate>(RestoreFromBackup),
                    PredictDelta = new PortableFunctionPointer<GhostComponentSerializer.PredictDeltaDelegate>(PredictDelta),
                    CalculateChangeMask =
                        new PortableFunctionPointer<GhostComponentSerializer.CalculateChangeMaskDelegate>(
                            CalculateChangeMask),
                    Serialize = new PortableFunctionPointer<GhostComponentSerializer.SerializeDelegate>(Serialize),
                    Deserialize = new PortableFunctionPointer<GhostComponentSerializer.DeserializeDelegate>(Deserialize),
                    #if UNITY_EDITOR || DEVELOPMENT_BUILD
                    ReportPredictionErrors = new PortableFunctionPointer<GhostComponentSerializer.ReportPredictionErrorsDelegate>(ReportPredictionErrors),
                    #endif
                };
                #if UNITY_EDITOR || DEVELOPMENT_BUILD
                s_State.NumPredictionErrorNames = GetPredictionErrorNames(ref s_State.PredictionErrorNames);
                #endif
                s_StateInitialized = true;
            }
            return s_State;
        }
        private static bool s_StateInitialized;
        private static GhostComponentSerializer.State s_State;
        public static GhostComponentSerializer.State State => GetState();
        public struct Snapshot
        {
            public SandboxGuid userIdGUID;
            public SandboxGuid assetIdGUID;
            public uint isAvatar;
        }
        public const int ChangeMaskBits = 3;
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.CopyToFromSnapshotDelegate))]
        private static void CopyToSnapshot(IntPtr stateData, IntPtr snapshotData, int snapshotOffset, int snapshotStride, IntPtr componentData, int componentStride, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                ref var snapshot = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotData, snapshotOffset + snapshotStride*i);
                ref var component = ref GhostComponentSerializer.TypeCast<GhostAsset>(componentData, componentStride*i);
                ref var serializerState = ref GhostComponentSerializer.TypeCast<GhostSerializerState>(stateData, 0);
                snapshot.userIdGUID = component.userIdGUID;
                snapshot.assetIdGUID = component.assetIdGUID;
                snapshot.isAvatar = component.isAvatar?1u:0;
            }
        }
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.CopyToFromSnapshotDelegate))]
        private static void CopyFromSnapshot(IntPtr stateData, IntPtr snapshotData, int snapshotOffset, int snapshotStride, IntPtr componentData, int componentStride, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                var deserializerState = GhostComponentSerializer.TypeCast<GhostDeserializerState>(stateData, 0);
                ref var snapshotInterpolationData = ref GhostComponentSerializer.TypeCast<SnapshotData.DataAtTick>(snapshotData, snapshotStride*i);
                ref var snapshotBefore = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotInterpolationData.SnapshotBefore, snapshotOffset);
                ref var snapshotAfter = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotInterpolationData.SnapshotAfter, snapshotOffset);
                //Compute the required owner mask for the components and buffers by retrievieng the ghost owner id from the data for the current tick.
                if (snapshotInterpolationData.GhostOwner > 0)
                {
                    var requiredOwnerMask = snapshotInterpolationData.GhostOwner == deserializerState.GhostOwner
                        ? SendToOwnerType.SendToOwner
                        : SendToOwnerType.SendToNonOwner;
                    if ((deserializerState.SendToOwner & requiredOwnerMask) == 0)
                        continue;
                }
                deserializerState.SnapshotTick = snapshotInterpolationData.Tick;
                float snapshotInterpolationFactorRaw = snapshotInterpolationData.InterpolationFactor;
                float snapshotInterpolationFactor = snapshotInterpolationFactorRaw;
                ref var component = ref GhostComponentSerializer.TypeCast<GhostAsset>(componentData, componentStride*i);
                component.userIdGUID = snapshotBefore.userIdGUID;
                component.assetIdGUID = snapshotBefore.assetIdGUID;
                component.isAvatar = snapshotBefore.isAvatar != 0;

            }
        }


        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.RestoreFromBackupDelegate))]
        private static void RestoreFromBackup(IntPtr componentData, IntPtr backupData)
        {
            ref var component = ref GhostComponentSerializer.TypeCast<GhostAsset>(componentData, 0);
            ref var backup = ref GhostComponentSerializer.TypeCast<GhostAsset>(backupData, 0);
            component.userIdGUID = backup.userIdGUID;
            component.assetIdGUID = backup.assetIdGUID;
            component.isAvatar = backup.isAvatar;
        }

        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.PredictDeltaDelegate))]
        private static void PredictDelta(IntPtr snapshotData, IntPtr baseline1Data, IntPtr baseline2Data, ref GhostDeltaPredictor predictor)
        {
            ref var snapshot = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotData);
            ref var baseline1 = ref GhostComponentSerializer.TypeCast<Snapshot>(baseline1Data);
            ref var baseline2 = ref GhostComponentSerializer.TypeCast<Snapshot>(baseline2Data);
            snapshot.isAvatar = (uint)predictor.PredictInt((int)snapshot.isAvatar, (int)baseline1.isAvatar, (int)baseline2.isAvatar);
        }
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.CalculateChangeMaskDelegate))]
        private static void CalculateChangeMask(IntPtr snapshotData, IntPtr baselineData, IntPtr bits, int startOffset)
        {
            ref var snapshot = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotData);
            ref var baseline = ref GhostComponentSerializer.TypeCast<Snapshot>(baselineData);
            uint changeMask;
            changeMask = snapshot.userIdGUID.Equals(baseline.userIdGUID) ? 0 : 1u;
            changeMask |= snapshot.assetIdGUID.Equals(baseline.assetIdGUID) ? 0 : (1u << 1);
            changeMask |= (snapshot.isAvatar != baseline.isAvatar) ? (1u<<2) : 0;
            GhostComponentSerializer.CopyToChangeMask(bits, changeMask, startOffset, 3);
        }
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.SerializeDelegate))]
        private static void Serialize(IntPtr snapshotData, IntPtr baselineData, ref DataStreamWriter writer, ref NetworkCompressionModel compressionModel, IntPtr changeMaskData, int startOffset)
        {
            ref var snapshot = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotData);
            ref var baseline = ref GhostComponentSerializer.TypeCast<Snapshot>(baselineData);
            uint changeMask = GhostComponentSerializer.CopyFromChangeMask(changeMaskData, startOffset, ChangeMaskBits);
            if ((changeMask & (1 << 0)) != 0)
                snapshot.userIdGUID.WriteDataStream(ref writer);
            if ((changeMask & (1 << 1)) != 0)
                snapshot.assetIdGUID.WriteDataStream(ref writer);
            if ((changeMask & (1 << 2)) != 0)
                writer.WritePackedUIntDelta(snapshot.isAvatar, baseline.isAvatar, compressionModel);
        }
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.DeserializeDelegate))]
        private static void Deserialize(IntPtr snapshotData, IntPtr baselineData, ref DataStreamReader reader, ref NetworkCompressionModel compressionModel, IntPtr changeMaskData, int startOffset)
        {
            ref var snapshot = ref GhostComponentSerializer.TypeCast<Snapshot>(snapshotData);
            ref var baseline = ref GhostComponentSerializer.TypeCast<Snapshot>(baselineData);
            uint changeMask = GhostComponentSerializer.CopyFromChangeMask(changeMaskData, startOffset, ChangeMaskBits);
            if ((changeMask & (1 << 0)) != 0)
                snapshot.userIdGUID.ReadDataStream(ref reader);
            else
                snapshot.userIdGUID = baseline.userIdGUID;
            if ((changeMask & (1 << 1)) != 0)
                snapshot.assetIdGUID.ReadDataStream(ref reader);
            else
                snapshot.assetIdGUID = baseline.assetIdGUID;
            if ((changeMask & (1 << 2)) != 0)
                snapshot.isAvatar = reader.ReadPackedUIntDelta(baseline.isAvatar, compressionModel);
            else
                snapshot.isAvatar = baseline.isAvatar;
        }
        #if UNITY_EDITOR || DEVELOPMENT_BUILD
        [BurstCompile]
        [MonoPInvokeCallback(typeof(GhostComponentSerializer.ReportPredictionErrorsDelegate))]
        private static void ReportPredictionErrors(IntPtr componentData, IntPtr backupData, ref UnsafeList<float> errors)
        {
            ref var component = ref GhostComponentSerializer.TypeCast<GhostAsset>(componentData, 0);
            ref var backup = ref GhostComponentSerializer.TypeCast<GhostAsset>(backupData, 0);
            int errorIndex = 0;
            errors[errorIndex] = math.max(errors[errorIndex], (component.isAvatar != backup.isAvatar) ? 1 : 0);
            ++errorIndex;
        }
        private static int GetPredictionErrorNames(ref FixedString512 names)
        {
            int nameCount = 0;
            if (nameCount != 0)
                names.Append(new FixedString32(","));
            names.Append(new FixedString64("isAvatar"));
            ++nameCount;
            return nameCount;
        }
        #endif
    }
}
