# SUMMARY_BANK.md — Echo Forge cross-session memory

> Any agent (Claude, Codex, human) must be able to resume from this file alone.

## PROJECT STATUS
`IN_PROGRESS`

## CURRENT MILESTONE
M1 complete — deterministic simulator + core data model + tests.
Next: M2 — author Level 0/1/2 assets, run validator, then vertical-slice scenes.

## LAST VERIFIED BUILD
No player build yet. EditMode tests 13/13 PASS (2026-09-07, job c2e48c7fdf3e).
Clean compile: EchoForge.Runtime, EchoForge.Editor, EchoForge.Tests.

## ENVIRONMENT
- Unity 6000.3.19f1, URP, New Input System, TMP, Test Framework 1.6.0
- MCP: com.anklebreaker.unity-mcp, plugin 2.39.5, instance "Echo Forge" port 7890
- Platform target currently: StandaloneWindows64 (to switch to iOS later)
- Project was a fresh Unity URP 2D-ish template: only `Assets/Scenes/SampleScene.unity`

## MCP VERIFICATION
`PASS` (2026-09-07) — round-trip: create Empty GO → set transform (1,2,3) →
inspect (confirmed) → delete. Console clean, 0 compile errors.

## GIT
- Local repo initialized 2026-09-07 with explicit user authorization. Local-only, never push.
- Latest commit: (see `git log -1`)

## SCENES
| Scene | Status |
|-------|--------|
| SampleScene | template leftover, to be removed once Bootstrap/Hall/Chamber exist |
| Bootstrap | not created |
| ResonanceHall | not created |
| EchoChamber | not created |

## IMPORTANT PREFABS
None yet.

## GAMEPLAY SYSTEMS
| System | Status | Files |
|--------|--------|-------|
| Deterministic pulse sim | IMPLEMENTED + TESTED | Runtime/Sim/AcousticSim.cs |
| Level data model | IMPLEMENTED | Runtime/Level/EchoLevelDefinition.cs |
| Chamber tunable state / TUNES | IMPLEMENTED + TESTED | Runtime/Level/ChamberRuntime.cs |
| Level validator (Editor) | IMPLEMENTED (no level assets yet) | Editor/LevelValidator.cs |
| Scene flow / GameState / Save / Audio / VFX | NOT STARTED | — |

Sim design notes: 8-way Dir; reflector 4 orient states (/, \, |, -); splitter forks
±45°; delay adds beats; loop guard = MaxSteps 5000 + per-(cell,dir) cap 8.
Fail priority: Forbidden > ReceiverMissed > Timing > ResonanceIncomplete.

## UI/UX
Not started. Design in Assets/Documentation/UI_UX_SYSTEM.md (to be written).

## AUDIO
Not started.

## LEVEL PROGRESS
```
Level 0:  Not implemented / Not validated / Not QA'd
Level 1-12: Not implemented / Not validated / Not QA'd
```
Level validator: NOT BUILT. Automated tests: NOT BUILT. Manual QA: NOT VERIFIED.

## KNOWN ISSUES
None recorded.

## MCP CHANGE LOG
```
MCP-0001
Date: 2026-09-07
Agent: Claude (Sonnet 5)
Scene/Asset: SampleScene (transient)
Change: Created + transformed + deleted __MCP_RoundTrip_Probe empty GameObject.
Reason: Mandatory MCP connectivity round-trip (master prompt §6).
Validation: unity_gameobject_info confirmed position (1,2,3); delete confirmed.
Result: PASS. No scene left dirty.
```

## NEXT EXACT ACTIONS
1. Author Level_00/01/02 EchoLevelDefinition assets (Editor tool or execute_code
   to create + populate .asset). Run `Echo Forge/Validate Levels`, get them VALID.
2. Add a PlayMode/EditMode test that loads each shipped level asset and asserts
   optimal config solves (data-solvability gate).
3. Runtime scene systems: SceneFlow, GameState, SaveController (versioned JSON),
   LevelController (drives ChamberRuntime + fires sim on PULSE), AudioController
   stub, HapticController stub.
4. Build 3 scenes via MCP: Bootstrap, ResonanceHall, EchoChamber. Scene-authored
   UI + environment (no runtime-built primary UI).
5. Pulse VFX: acoustic wave-packet visualization (NOT laser) driven by SimResult
   playback timeline.
6. Vertical slice flow: Launch → Sound Check → Resonance Hall → Resonance Dial →
   Chamber 01 → tune → PULSE → result → save → Chamber 02 → back to Hall.
7. Originality review vs Misprint Department / Punchcard Operator / Stencil Relay.
8. Then Levels 3-12, environment/VFX/audio polish, icon, mobile config, QA, builds.

## SCOPE NOTE
This is a multi-session build (release-quality 13-level mobile game). Each session:
read memory → inspect → implement next slice → test → commit → update this file.
Do NOT restart or rebuild verified systems (see AI_RULES RULE 1 / master §8).
```
