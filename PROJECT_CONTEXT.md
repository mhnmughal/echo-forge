# PROJECT_CONTEXT.md - Resonance Chamber (codename Echo Forge)

## Product
**Resonance Chamber** - a 2.5D offline spatial acoustic logic puzzle game.
Player-facing name = Resonance Chamber (App Store, home screen, all titles).
"Echo Forge" is the codename kept for namespaces, editor menus, asset ids, the
bundle id, and the git repo. Chosen 2026-09-07 after an App Store name check
(see Assets/Documentation/APP_STORE_LISTING.md).

## Core fantasy
**Tune the room, not the character.** The player configures a physical acoustic
chamber and releases visible deterministic sound pulses through it.

## Core mechanic
Manipulate acoustic components (reflectors, absorbers, splitters, delay plates,
resonators) to route visible sound pulses from emitters to receivers/resonators,
satisfying spatial + timing conditions while avoiding forbidden receivers.

## Presentation
2.5D miniature acoustic laboratory. Fixed angled 3D camera, no free orbit.
Dark / warm / bronze / sculptural / acoustic visual language.

## Simulation
2D deterministic node/direction/beat path logic inside a 3D presentation.
No physics decides success. Cycle/step protection required.

## Platforms
iOS primary; Android-compatible architecture. Landscape only. Offline only.

## Identity constraints
Must NOT resemble Misprint Department, Punchcard Operator, or Stencil Relay, nor a
generic laser-reflection / neon grid puzzle. Pulse must read as SOUND, not LIGHT.

## Engine
Unity 6000.3.19f1, Universal Render Pipeline (URP), New Input System, TextMeshPro,
Unity Test Framework. MCP: com.anklebreaker.unity-mcp (plugin 2.39.5, port 7890).

## Content scope
Exactly 13 stages: Level 0 (Sound Check tutorial) + Levels 1-12.
- Series A - Reflection: 1-4
- Series B - Resonance: 5-8
- Series C - Phase: 9-12

## Scenes
- `Bootstrap` - entry, save load, routing
- `ResonanceHall` - circular acoustic main menu + Resonance Dial chamber select
- `EchoChamber` - the puzzle scene (data-driven per level)

## Systems (see Assets/Documentation/ARCHITECTURE.md)
AcousticGraph, PulseSimulator, Emitter, Reflector, Absorber, Splitter, DelayPlate,
Receiver, Resonator, EchoLevelDefinition (ScriptableObject), LevelController,
GameState, SaveController, AudioController, HapticController, SceneFlow.

## Ratings (waveform marks, not stars)
RESOLVED (completed) · CLEAN ECHO (within adjustment par) · PURE RESONANCE (optimal).

## Bundle
Product Name: `Resonance Chamber` · Company: `Resonance Chamber Co` ·
Bundle ID: `com.resonancechamberco.resonancechamber` · Version `1.0.0` · Build `1`.
(Codename "Echo Forge" for code / repo only. User set the name + id 2026-09-07.)

## Key terminology (Echo Forge specific)
Resonance Hall, Suspended Resonance Ring, Resonance Dial, Echo Chamber, PULSE
actuator (Resonance Actuator), TUNES counter, Sound Damping (pause), Tuning Strip
(settings), Sound Guide, Resonance Achieved / Out of Phase / Signal Lost,
Chamber In Tune (final completion).
