# MOBILE_RELEASE_CHECKLIST — Echo Forge

Applied by `Echo Forge/Build/7 - Player Settings` (PlayerConfig.cs), verified via
PlayerSettings API readback 2026-09-07.

## Player Settings
- [x] Product Name = "Echo Forge"
- [x] Bundle ID = com.rehmanmughal.echoforge (Standalone / iOS / Android)
- [x] Version 1.0.0 / iOS build 1 / Android bundleVersionCode 1
- [x] Orientation: LandscapeLeft default, portrait autorotate OFF, both landscapes ON
- [x] iOS scripting backend: IL2CPP
- [x] iOS architecture: ARM64 (targetDevice iPhone+iPad, min iOS 13)
- [x] Android: IL2CPP + ARM64, minSdk 25
- [x] Target frame rate 60 (BootstrapController)
- [x] Managed stripping: Medium (iOS + Android)
- [x] Graphics: URP, Linear color space
- [x] App icon assigned (Standalone/iOS/Android slots), no Unity default
- [ ] Splash screen — default (Unity personal license shows Unity logo); NOT VERIFIED

## Permissions (must NOT request)
- [x] No camera/microphone/location usage descriptions set (empty)
- [x] No analytics/ads/attribution packages present
- [ ] Info.plist / AndroidManifest post-export inspection — NOT VERIFIED (no export)

## Safe area
- [x] SafeAreaFitter drives every canvas
- [ ] Verified on notch / Dynamic Island / home indicator — NOT VERIFIED (no device)

## Build
- [x] Scene list: Bootstrap, ResonanceHall, EchoChamber
- [x] Windows build — PASS (0 errors / 0 warnings)
- [x] Windows smoke test — PASS (launches, engine + input + physics init, no exception)
- [ ] iOS Unity export — NOT VERIFIED (needs macOS/Xcode)
- [ ] Xcode archive — NOT VERIFIED (external toolchain)
- [ ] iOS code sign — NOT VERIFIED (external credentials)
- [ ] Physical iPhone install — NOT VERIFIED (external device)
