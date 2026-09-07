# ARCHITECTURE — Echo Forge

Status: DRAFT (bootstrap). Updated as systems land.

## Assemblies
- `EchoForge.Runtime` (Assets/EchoForge/Runtime) — all gameplay/runtime code.
- `EchoForge.Editor` (Assets/EchoForge/Editor) — level validator, tooling, asset gen.
- `EchoForge.Tests` (Assets/EchoForge/Tests) — EditMode + PlayMode tests.

No third-party runtime assemblies. MCP plugin is Editor-only tooling.

## Deterministic simulation (authoritative)
The puzzle result is computed entirely by `PulseSimulator`, a pure C# discrete
stepper. Unity physics/particles/audio are presentation only and cannot change
the logical outcome.

- **Grid**: integer cell coordinates on a 2D plane (`Vector2Int`).
- **Direction**: 4- or 8-way enum (`Dir`), fixed unit steps.
- **PulseState**: `{ cell, dir, beat, channel, branchId, alive }`.
- **AcousticGraph**: immutable snapshot of a chamber configuration — component at
  each occupied cell + its oriented behavior — built from an `EchoLevelDefinition`
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
`EchoLevelDefinition` ScriptableObject — see LEVEL_DESIGN.md for full field list.
Levels 0-12 are individual assets under `Assets/EchoForge/Levels/`. No switch
statements on level id in gameplay code.

## Scene flow
`Bootstrap` (load save, route) → `ResonanceHall` (menu + Resonance Dial) →
`EchoChamber` (loads a level definition, builds board, runs simulation on PULSE).
`SceneFlow` handles additive/single loads and transitions.

## Save
`SaveController` — versioned JSON in `Application.persistentDataPath`. Handles
missing / corrupt / newer-version saves. Never downgrades an earned rating.

## Audio
`AudioController` + AudioMixer (Master / Ambience / SFX / UI). Clips are
project-generated (see AUDIO_PROVENANCE.md). Mixer exposed params persist via save.
