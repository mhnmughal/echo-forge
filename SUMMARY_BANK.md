# SUMMARY_BANK.md — Echo Forge cross-session memory

> Any agent (Claude, Codex, human) must be able to resume from this file alone.

## PROJECT STATUS
`IN_PROGRESS`

## CURRENT MILESTONE
M0 — Bootstrap: project memory, local git, MCP verification. IN PROGRESS.

## LAST VERIFIED BUILD
None yet.

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
None implemented yet. Next: deterministic PulseSimulator + AcousticGraph.

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
1. Commit bootstrap (memory files, .gitignore, master prompt, doc stubs).
2. Write Assets/Documentation/ stubs: ARCHITECTURE, GAME_DESIGN, LEVEL_DESIGN,
   UI_UX_SYSTEM, and the audit/QA docs.
3. Create Assets/EchoForge/ folder structure + assembly definitions
   (Runtime, Editor, Tests).
4. Implement deterministic core: AcousticGraph, PulseSimulator, PulseState,
   Direction enum, component step-logic (Reflector/Absorber/Splitter/DelayPlate/
   Receiver/Resonator), loop protection.
5. Implement EchoLevelDefinition ScriptableObject + a couple of test level assets.
6. EditMode tests for reflection/absorption/split/delay/receiver/forbidden/
   resonator/multi-emitter/loop/adjustment/rating. Run them.
7. Build Level Validator (Editor). Target 13/13 once levels exist.
8. Then scene work: Bootstrap → ResonanceHall → EchoChamber vertical slice
   (Levels 0,1,2), production architecture, real audio foundation, save.
```
