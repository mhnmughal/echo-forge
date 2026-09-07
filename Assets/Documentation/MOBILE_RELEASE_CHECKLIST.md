# MOBILE_RELEASE_CHECKLIST - Echo Forge

Applied by `Echo Forge/Build/7 - Player Settings` (PlayerConfig.cs), verified via
PlayerSettings API readback 2026-09-07.

## Player Settings
- [x] Product Name = "Resonance Chamber" (App Store + home-screen name; codename "Echo Forge")
- [x] Company / developer name = "Resonance Chamber Co"
- [x] Bundle ID = com.resonancechamberco.resonancechamber (Standalone / iOS / Android)
- [x] Copyright = "2026 Resonance Chamber Co"
- [x] Version 1.0.0 / iOS build 1 / Android bundleVersionCode 1
- [x] Orientation: LandscapeLeft default, portrait autorotate OFF, both landscapes ON
- [x] iOS scripting backend: IL2CPP
- [x] iOS architecture: ARM64 (targetDevice iPhone+iPad, min iOS 15 - Unity 6000.3 floor)
- [x] Android: IL2CPP + ARM64, minSdk 25
- [x] Target frame rate 60 (BootstrapController)
- [x] Managed stripping: Medium (iOS + Android)
- [x] Graphics: URP, Linear color space
- [x] App icon assigned (Standalone/iOS/Android slots), no Unity default
- [ ] Splash screen - default (Unity personal license shows Unity logo); NOT VERIFIED

## Permissions (must NOT request)
- [x] No camera/microphone/location usage descriptions set (empty)
- [x] No analytics/ads/attribution packages present; network engine modules removed

## Store listing
- [x] APP_STORE_LISTING.md - name, subtitle options, keyword sets, description,
  category (Games / Puzzle primary, Strategy secondary), review notes, privacy
- [x] PRIVACY_POLICY.md - host at a public URL, enter it in App Store Connect
- [ ] Screenshots (iPhone 6.7 in + iPad 12.9 in, landscape) - NOT CAPTURED
- [ ] App preview video - optional, NOT CAPTURED
- [ ] Info.plist / AndroidManifest post-export inspection - NOT VERIFIED (no export)

## Safe area
- [x] SafeAreaFitter drives every canvas
- [ ] Verified on notch / Dynamic Island / home indicator - NOT VERIFIED (no device)

## Audio
- [x] AudioMixer `EchoForge.mixer` - Master → Ambience / SFX / UI, exposed
  MusicVol / EffectsVol / UIVol, wired into AudioController
- [ ] Audible balance check - NOT VERIFIED (no audio output device)

## Build
- [x] Scene list: Bootstrap, ResonanceHall, EchoChamber
- [x] Packages trimmed to 14; all network modules removed (PACKAGE_AUDIT.md)
- [x] Unity engine diagnostics + analytics disabled (UnityConnectSettings, submitAnalytics 0)
- [x] Windows build - PASS (0 errors / 0 warnings, ~99 MB)
- [x] Windows smoke test - PASS (launches, engine + input + physics init, no exception)
- [ ] iOS Unity export - NOT VERIFIED (needs macOS/Xcode)
- [ ] Xcode archive - NOT VERIFIED (external toolchain)
- [ ] iOS code sign - NOT VERIFIED (external credentials)
- [ ] Physical iPhone install - NOT VERIFIED (external device)
