# SUMMARY_BANK.md — Echo Forge cross-session memory

> Any agent (Claude, Codex, human) must be able to resume from this file alone.

## PROJECT STATUS
`IN_PROGRESS` — core game complete & verified; remaining work is presentation
polish, listening QA, package trim (blocked by -noUpm), and iOS/device checks.

## CURRENT MILESTONE
M7 — visual polish pass + honest final audit. Core (M1-M6) done.

## LAST VERIFIED BUILD
Windows StandaloneWindows64: **PASS** (2026-09-07) — 0 errors, 0 warnings,
~105 MB. Smoke test: launches on D3D12, Input System + PhysX init, runs with no
exceptions. iOS export: NOT ATTEMPTED (needs macOS/Xcode; see below).

## ENVIRONMENT
- Unity 6000.3.19f1, URP 17.3, Linear color space, New Input System, TMP, Test FW.
- Editor runs with `-noUpm` → Package Manager disabled, manifest edits don't
  resolve. Package trim is documented in PACKAGE_AUDIT.md, not applied.
- MCP: AnkleBreaker plugin 2.39.5, instance "Echo Forge" port 7890.
- Tests via advanced tool `unity_testing_run_tests` (assemblies
  ["EchoForge.Tests"] / ["EchoForge.PlayTests"]).

## GIT
Local-only (user-authorized 2026-09-07), never pushed. ~20 commits.
`git log -1` for latest.

## SCENES  (Assets/EchoForge/Scenes/, rebuilt by `Echo Forge/Build/Everything`)
| Scene | Status |
|-------|--------|
| Bootstrap | built — title, AudioRig, routes to Hall |
| ResonanceHall | built — circular room, Suspended Resonance Ring (tap ripple), |
|               | radial Resonance Dial (12 nodes), Tuning Strip, Sound Guide, |
|               | Chamber In Tune finale + emblem |
| EchoChamber | built — data-driven board, contextual UI, circular PULSE actuator, |
|             | Sound Damping pause, result/tutorial layers, post-processing |
| SampleScene (Assets/Scenes) | template leftover — DELETE in a normal editor |

## GAMEPLAY SYSTEMS  (all in Assets/EchoForge/Runtime/)
| System | Status |
|--------|--------|
| Deterministic pulse sim (Sim/AcousticSim.cs) | DONE + 17 EditMode tests |
| EchoLevelDefinition + ChamberRuntime | DONE |
| LevelController / ChamberInput / ChamberUI / PulseVisualizer | DONE |
| SaveSystem (versioned JSON, recovery, no rating downgrade, reset) | DONE |
| SceneFlow / GameSession / Haptics | DONE |
| AudioController + SoundBank (mixer-optional, category volumes) | DONE |
| Menu: Bootstrap/Hall/Dial/TuningStrip/SoundGuide/RingTapRelay | DONE |
| LevelValidator (Editor) | DONE — 13/13 VALID |
| Build tools: EchoBuild, EchoBuildScenes, AudioGen, IconGen, PlayerConfig | DONE |

## LEVELS
`Assets/EchoForge/Levels/Level_00..12.asset` — **13/13 VALID**.
Solutions in Assets/Documentation/LEVEL_DESIGN.md.

## TESTS
- EditMode `EchoForge.Tests`: **17/17 PASS** (sim mechanics, ratings, adjustments,
  every shipped level's optimal config solves + no pre-solve).
- PlayMode `EchoForge.PlayTests`: **17/17 PASS** — incl. all 13 levels solved
  through the real EchoChamber scene pipeline, fail+retune, pause/resume.
- This is AUTOMATED CONTROLLER QA. Human touch QA = NOT VERIFIED (no device/human).

## AUDIO
20 procedural WAVs (`Assets/EchoForge/Audio/Generated/`) via AudioGen.
Objective QA PASS (levels/DC/no-NaN). Listening QA = NOT VERIFIED (no audio out).

## ICON
`Assets/EchoForge/Art/Icon/EchoForge_Icon_1024.png` procedural + 180/120/60.
Assigned to Standalone/iOS/Android slots. Small-size QA: ring + point readable.

## MOBILE / PLAYER SETTINGS
Product "Echo Forge", com.rehmanmughal.echoforge, v1.0.0/build 1, landscape-only,
IL2CPP, Android ARM64 minSdk25, Linear, no camera/mic/location descriptions.

## AUDITS
- OFFLINE_AUDIT: **PASS — no runtime network functionality** (code scan clean,
  no SDKs, services disabled, local save only).
- PACKAGE_AUDIT: plan written, NOT applied (`-noUpm`).
- MCP cleanup (§34): not done — MCP still needed for authoring. Editor-only plugin.

## KNOWN ISSUES / REMAINING WORK
- BUG-NONE open.
- Visual polish: environment is on-theme but sparse (frame + walls + posts);
  no curved membranes / rich acoustic detail. Reads "premium gray-box + bloom".
- UI: functional & contextual; fonts are LiberationSans throughout (safe, plain).
- Responsive/safe-area QA across 5 aspect ratios: NOT VERIFIED (needs Game View
  aspect testing).
- Package trim, SampleScene delete, iOS export, Xcode archive/sign, physical
  device, human touch QA, audio listening QA — all require a normal editor
  launch and/or external toolchain/device/human.

## NEXT EXACT ACTIONS
1. From a NORMAL editor launch (no -noUpm): apply PACKAGE_AUDIT trim, delete
   Assets/Scenes/SampleScene, re-verify (compile/tests/validator/scenes/build).
2. Environment/VFX polish: curved wall membranes, floor detail, better pulse
   wave-packet (leading ring), reflector/absorber/splitter shape language,
   selection + socket visuals, camera feedback on PULSE.
3. Responsive QA: Game View at 16:9 / 19.5:9 / 21:9 / 4:3 / 4.3:3, verify safe
   area + no clipping; screenshot each. Record in UI_QA.md.
4. Audio listening QA on a machine with output; fill AUDIO_PROVENANCE listening.
5. iOS: switch platform, Unity export to Xcode, verify icon in asset catalog.
6. Update FINAL_AUDIT.md with real per-line results each pass.
```
