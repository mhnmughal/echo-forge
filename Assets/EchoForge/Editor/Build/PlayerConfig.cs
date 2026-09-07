using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace EchoForge.EditorTools
{
    /// <summary>Re-runnable production Player Settings for Echo Forge (mobile-first, offline,
    /// landscape, no sensitive permissions). Master prompt §43 / §78-80.</summary>
    public static class PlayerConfig
    {
        [MenuItem("Echo Forge/Build/7 - Player Settings")]
        public static void Apply()
        {
            const string id = "com.rehmanmughal.echoforge";

            PlayerSettings.productName = "Echo Forge";
            PlayerSettings.companyName = "Rehman Mughal";
            PlayerSettings.bundleVersion = "1.0.0";

            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Standalone, id);
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.iOS, id);
            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, id);

            PlayerSettings.iOS.buildNumber = "1";
            PlayerSettings.Android.bundleVersionCode = 1;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel25;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;

            // landscape only
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
            PlayerSettings.allowedAutorotateToPortrait = false;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
            PlayerSettings.allowedAutorotateToLandscapeLeft = true;
            PlayerSettings.allowedAutorotateToLandscapeRight = true;
            PlayerSettings.useAnimatedAutorotation = true;

            // scripting / arch
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.iOS, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.iOS, ManagedStrippingLevel.Medium);
            PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.Android, ManagedStrippingLevel.Medium);

            PlayerSettings.iOS.targetOSVersionString = "13.0";
            PlayerSettings.iOS.appleEnableAutomaticSigning = false;
            PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneAndiPad;

            // NO sensitive permissions - leave every usage description empty
            PlayerSettings.iOS.cameraUsageDescription = "";
            PlayerSettings.iOS.locationUsageDescription = "";
            PlayerSettings.iOS.microphoneUsageDescription = "";

            PlayerSettings.colorSpace = ColorSpace.Linear;
            PlayerSettings.muteOtherAudioSources = false;
            PlayerSettings.gpuSkinning = true;

            AssetDatabase.SaveAssets();
            Debug.Log($"<b>Echo Forge: Player Settings applied ({id}, landscape, IL2CPP, no sensitive permissions).</b>");
        }
    }
}
