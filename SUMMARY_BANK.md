# SUMMARY_BANK.md — Echo Forge cross-session memory

> Any agent (Claude, Codex, human) must be able to resume from this file alone.

## PROJECT STATUS
`IN_PROGRESS` — core game complete & verified; package trim + AudioMixer + torus
ring DONE this session. Remaining work all needs a FOCUSED interactive editor
(real-time art pass, last responsive photos, profiler) or external hardware
(iOS toolchain/device, human tester, audio output device).

## CURRENT MILESTONE
M7 — visual polish pass + honest final audit. Core (M1-M6) done.

## LAST VERIFIED BUILD
Windows StandaloneWindows64: **PASS** (2026-09-07, post package-trim) — 0 errors,
0 warnings, ~103 MB. Smoke test (earlier build): launches on D3D12, Input System
+ PhysX init, no exceptions. iOS export: NOT ATTEMPTED (needs macOS/Xcode).

## ENVIRONMENT
- Unity 6000.3.19f1, URP 17.3, Linear color space, New Input System, TMP, Test FW.
- Editor now runs WITHOUT `-noUpm` (relaunched 2026-09-07). Package Manager works;
  trim APPLIED (manifest 53→29). If a future session sees `-noUpm` again, package
  ops will fail — check `Environment.GetCommandLineArgs()`.
- Known limit: this session's editor is unfocused, so the Game View renders only
  ~1 frame per play session (screenshot pipeline unreliable for sequences). Also
  no audio output device.
- MCP: AnkleBreaker plugin 2.39.5, instance "Echo Forge" port 7890.
- Tests via advanced tool `unity_testing_run_tests` (assemblies
  ["EchoForge.Tests"] / ["EchoForge.PlayTests"]).

## GIT
User-authorized 2026-09-07. ~21 commits. `git log -1` for latest.
Remote `origin` = https://github.com/mhnmughal/echo-forge (**PRIVATE**, user
authorized the push 2026-09-07). Push to it; keep it private; no other remotes.

## SCENES  (Assets/EchoForge/Scenes/, rebuilt by `Echo Forge/Build/Everything`)
| Scene | Status |
|-------|--------|
| Bootstrap | built — title, AudioRig, routes to Hall |
| ResonanceHall | built — circular room, Suspended Resonance Ring (tap ripple), |
|               | radial Resonance Dial (12 nodes), Tuning Strip, Sound Guide, |
|               | Chamber In Tune finale + emblem |
| EchoChamber | built — data-driven board, contextual UI, circular PULSE actuator, |
|             | Sound Damping pause, result/tutorial layers, post-processing |
| (SampleScene) | removed — Assets/Scenes/ is empty |

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
Real AudioMixer `Assets/EchoForge/Art/EchoForge.mixer` (Master→Ambience/SFX/UI,
exposed MusicVol/EffectsVol/UIVol) built by `EchoPolish.BuildMixer`, wired in
`AudioRig()`. Multiplier fallback kept.

## ICON
`Assets/EchoForge/Art/Icon/EchoForge_Icon_1024.png` procedural + 180/120/60.
Assigned to Standalone/iOS/Android slots. Small-size QA: ring + point readable.

## MOBILE / PLAYER SETTINGS
Product "Echo Forge", com.rehmanmughal.echoforge, v1.0.0/build 1, landscape-only,
IL2CPP, Android ARM64 minSdk25, Linear, no camera/mic/location descriptions.

## AUDITS
- OFFLINE_AUDIT: **PASS — no runtime network functionality** (code scan clean,
  no SDKs, services disabled, local save only).
- PACKAGE_AUDIT: **APPLIED** — 53→29 deps, re-verified. See PACKAGE_AUDIT.md.
- MCP cleanup (§34): not done — MCP still needed for authoring. Editor-only plugin.

## DONE THIS SESSION (2026-09-07, session_01HnKWKst1ksn91DWhcZytbv)
- Package trim 53→29 (`f335774`), SampleScene confirmed gone.
- Real AudioMixer asset + wiring; editor-compile warnings → 0 (`e6a214c`).
- `ResonanceRing.asset` procedural torus mesh, replaces flat-disc ring.
- Chamber component emissive + glow-light bump for board readability;
  Hall camera pull-back + layered warm/violet light; ambient rebalance.
- `ResponsiveQA.cs` Game-View aspect harness; UI_QA responsive = static-ANALYZED
  all 5 aspects + PHOTO 16:9/4:3 (see UI_QA.md).
- Windows build re-verified PASS (0/0, 103 MB); 17/17 + 13/13 + 17/17 green.

## KNOWN ISSUES / REMAINING WORK  (all need focused editor or external hardware)
- BUG-NONE open.
- Environment art: stylised-minimal, coherent but not §21 membrane/detail bar.
  Needs real-time iteration on a FOCUSED editor (blind screenshot loop can't
  converge it — this session tried and reverted a too-dark pass).
- Per-component particle VFX (charge/branch/membrane); pulse leading-ring.
- Fonts: LiberationSans throughout (safe, plain) — optional display-face upgrade.
- Responsive: middle 3 aspects + 4:3 corner-HUD photo recheck (layout math = PASS).
- Profiler pass; iOS export/archive/sign; physical device; human touch + audio
  listening QA.
- §34: strip `com.anklebreaker.unity-mcp` before the final store export.

## NEXT EXACT ACTIONS
1. On a FOCUSED interactive editor: real-time environment/VFX/animation art pass;
   capture the remaining responsive aspects; run the Unity Profiler on each scene.
2. Font: optional swap to a restrained architectural display face.
3. On macOS/Xcode: iOS platform switch → export → archive → sign → device install;
   verify icon in the asset catalog.
4. Audio listening QA on a machine with output; fill AUDIO_PROVENANCE listening.
5. Before final export: remove the MCP plugin package, rebuild, re-verify.
6. Update FINAL_AUDIT.md per-line each pass.
```
