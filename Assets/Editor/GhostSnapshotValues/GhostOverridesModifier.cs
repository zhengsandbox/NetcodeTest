using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.NetCode;
using Unity.NetCode.Editor;
using UnityEngine;

public class GhostOverridesModifier : IGhostDefaultOverridesModifier
{
    void IGhostDefaultOverridesModifier.Modify(Dictionary<string, GhostComponentModifier> overrides)
    {
        var transAttrib = overrides["Unity.Transforms.Translation"].fields.First(attrib => attrib.name == "Value").attribute;
        transAttrib.Smoothing = SmoothingAction.Interpolate;
        transAttrib.Quantization = 1000;

        var rotAttrib = overrides["Unity.Transforms.Rotation"].fields.First(attrib => attrib.name == "Value").attribute;
        rotAttrib.Smoothing = SmoothingAction.Interpolate;
        rotAttrib.Quantization = 1000;

        // Only send to interpolated ones
        overrides["Unity.Transforms.Translation"].attribute.OwnerPredictedSendType = GhostSendType.Interpolated;
        overrides["Unity.Transforms.Rotation"].attribute.OwnerPredictedSendType = GhostSendType.Interpolated;
    }

    void IGhostDefaultOverridesModifier.ModifyAlwaysIncludedAssembly(HashSet<string> alwaysIncludedAssemblies)
    {
        
    }

    void IGhostDefaultOverridesModifier.ModifyTypeRegistry(TypeRegistry typeRegistry, string netCodeGenAssemblyPath)
    {
        var path = "../SandboxNetcodeTest/Assets/Editor/GhostSnapshotValues";
        typeRegistry.RegisterType(typeof(SandboxGuid), TypeAttribute.Empty(),
            new TypeTemplate
            {
                
                TemplatePath = $"{path}/CodeGenTemplates/GhostSnapshotValueGUID.txt"
            });
    }
}
