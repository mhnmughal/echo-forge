# ARCHITECTURE - Echo Forge

Status: CURRENT (2026-09-07). All systems below implemented + tested.

## Build / authoring tools (Editor, `Assets/EchoForge/Editor/Build/`)
- `EchoBuild` - palette, URP material factory, UI factory, post-process profile,
  generated UI sprites, primitive/light helpers.
- `EchoBuildScenes` - `Echo Forge/Build/*` menu: rebuilds the 3 scenes + catalog
  + soundbank at edit time and serialises them (scene-authored, not runtime).
- `AudioGen` - offline procedural WAV synthesis -> SoundBank.
- `IconGen` - procedural 1024 icon render -> Player Settings.
- `PlayerConfig` - production Player Settings.
- `LevelValidator` - `Echo Forge/Validate Levels`, target 13/13.

## Assemblies
- `EchoForge.Runtime` (Assets/EchoForge/Runtime) - all gameplay/runtime code.
- `EchoForge.Editor` (Assets/EchoForge/Editor) - validator, tooling, asset gen,
  `ResponsiveQA` (Game View aspect harness).
- `EchoForge.Tests` / `EchoForge.PlayTests` - EditMode + PlayMode tests.

No third-party runtime assemblies. MCP plugin is Editor-only tooling.
Packages trimmed to 14, network modules removed (see PACKAGE_AUDIT.md).

## Deterministic simulation (authoritative)
The puzzle result is computed entirely by `PulseSimulator`, a pure C# discrete
stepper. Unity physics/particles/audio are presentation only and cannot change
the logical outcome.

- **Grid**: integer cell coordinates on a 2D plane (`Vector2Int`).
- **Direction**: 4- or 8-way enum (`Dir`), fixed unit steps.
- **PulseState**: `{ cell, dir, beat, channel, branchId, alive }`.
- **AcousticGraph**: immutable snapshot of a chamber configuration - component at
  each occupied cell + its oriented behavior - built from an `EchoLevelDefinition`
  plus the player's current adjustments.
- **Step loop**: BFS/queue over active pulses, advancing one cell per beat.
  Components transform pulses on entry:
  - Reflector: redirect by fixed angle state.
  - Absorber: kill pulse (records absorption).
  - Splitter: kill incoming, spawn 2 branch pulses with new branchIds.
  - DelayPlate: hold pulse N beats, then re-emit same dir.
  - Receiver: mark hit with arrival beat; pulse ends.
  - Resonator: accumulate hits; evaluate condition (count / beat-delta / order).
  - Forbidden receiver: mark triggered → level fail.
  - Wall / bounds: kill pulse.
- **Loop protection**: max total steps and per-(cell,dir,beat,channel) visited set;
  exceeding caps → simulation terminates, level marked unsolved for that config.

## Result model
`SimResult { success, failReason, receiverHits[], forbiddenTriggered,
resonatorStates[], totalBeats, endedByCap }`.
`failReason ∈ { ReceiverMissed, ForbiddenReceiver, TimingIncorrect,
ResonanceIncomplete }`.

## Level data
`EchoLevelDefinition` ScriptableObject - see LEVEL_DESIGN.md for full field list.
Levels 0-12 are individual assets under `Assets/EchoForge/Levels/`. No switch
statements on level id in gameplay code.

## Scene flow
`Bootstrap` (load save, route) → `ResonanceHall` (menu + Resonance Dial) →
`EchoChamber` (loads a level definition, builds board, runs simulation on PULSE).
`SceneFlow` handles additive/single loads and transitions.

## Save
`SaveController` - versioned JSON in `Application.persistentDataPath`. Handles
missing / corrupt / newer-version saves. Never downgrades an earned rating.

## Audio
`AudioController` + `EchoForge.mixer` AudioMixer (Master → Ambience / SFX / UI,
exposed MusicVol / EffectsVol / UIVol). Clips are project-generated (see
AUDIO_PROVENANCE.md). Saved 0-1 slider values drive the exposed params in dB;
a multiplier fallback covers `mixer == null`.

## Presentation assets (edit-time generated, `Assets/EchoForge/Art/`)
- `ResonanceRing.asset` - procedural torus mesh for the Hall's suspended ring
  (`EchoPolish.BuildTorus`), replaced the flat scaled-cylinder stand-in.
- `EchoForge.mixer` - see Audio, above.
- `EchoForge_Post.asset` - URP post profile (bloom / ACES / color / vignette).
Menu: `Echo Forge/Build/8 - Ring Mesh + Mixer`.
