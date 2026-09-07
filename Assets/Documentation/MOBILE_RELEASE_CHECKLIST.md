# MOBILE_RELEASE_CHECKLIST — Echo Forge

Status: NOT STARTED. All items NOT VERIFIED until checked with evidence.

## Player Settings
- [ ] Product Name = "Echo Forge"
- [ ] Bundle ID = com.rehmanmughal.echoforge
- [ ] Version 1.0.0 / Build 1
- [ ] Orientation: Landscape Left + Right only
- [ ] iOS scripting backend: IL2CPP
- [ ] iOS architecture: ARM64
- [ ] Android: IL2CPP + ARM64 (compat)
- [ ] Target frame rate set (60)
- [ ] Managed stripping level appropriate
- [ ] Graphics: URP, no unused tiers
- [ ] App icon assigned (all iOS slots), no Unity default
- [ ] Splash screen configured / disabled per plan

## Permissions (must NOT request)
- [ ] No camera, microphone, contacts, location, photos, Bluetooth, ATT, tracking
- [ ] Confirm Info.plist / AndroidManifest clean after export

## Safe area
- [ ] SafeArea component drives all critical UI
- [ ] Verified on notch / Dynamic Island / home indicator layouts

## Build
- [ ] Scene list correct (Bootstrap, ResonanceHall, EchoChamber)
- [ ] Windows build (smoke test)
- [ ] iOS Unity export (Xcode project)
- [ ] Xcode archive — NOT VERIFIED (external toolchain)
- [ ] iOS code sign — NOT VERIFIED (external credentials)
- [ ] Physical iPhone install — NOT VERIFIED (external device)
