# AI_RULES.md — Echo Forge

Permanent operating rules for any AI/human agent working on this project.

## RULE 1 — READ MEMORY FIRST
Every new session reads, in order:
1. `AI_RULES.md`
2. `PROJECT_CONTEXT.md`
3. `SUMMARY_BANK.md`
4. `CLAUDE_MASTER_PROMPT.md`
5. Relevant `Assets/Documentation/*.md`

## RULE 2 — REPOSITORY IS AUTHORITATIVE
Do not rely on chat history. Record decisions in the repo.

## RULE 3 — MCP-FIRST UNITY EDITING
Use the AnkleBreaker Unity MCP (`unity_*` tools, port 7890) for scenes, hierarchy,
prefabs, components, UI, RectTransforms, environment, camera, lighting, materials,
Inspector config, Player Settings, build settings, inspection. Never call the HTTP
bridge directly.

## RULE 4 — LOG MCP CHANGES
After each meaningful MCP batch, append an `MCP-XXXX` entry to `SUMMARY_BANK.md`.
Group coherent changes; do not log every numeric tweak.

## RULE 5 — NEVER CLAIM UNVERIFIED SUCCESS
Use only PASS / FAIL / NOT VERIFIED. PASS requires evidence from the actual check.
Never infer one verification category from another.

## RULE 6 — NO FINAL PLACEHOLDERS
Release candidate contains no TODO / Temp / Sample / Test placeholder content,
fake buttons, fake settings, placeholder audio/art, default Unity icon.

## RULE 7 — SCENE-AUTHORED UI
Primary production UI authored in scene/prefab hierarchy via MCP. Scripts control
state/data/navigation/transitions only — not wholesale runtime construction.

## RULE 8 — SCENE-AUTHORED ENVIRONMENT
Major environment elements exist visibly in Edit Mode.

## RULE 9 — COMPLETELY OFFLINE
No runtime networking, APIs, cloud, login, ads, analytics, telemetry, tracking,
attribution, remote config, web views, DLC, multiplayer.

## RULE 10 — NO THIRD-PARTY RUNTIME SDKs
No Firebase / AdMob / Unity Ads / Facebook / AppsFlyer / GameAnalytics / social SDKs.

## RULE 11 — ORIGINAL PRODUCT
Do not copy any other game's code, UI, layout, art, icon, audio, or branding.

## RULE 12 — DO NOT REUSE PREVIOUS PROJECT DESIGN
No UI/environment systems, terminology, or icon motifs from Misprint Department,
Punchcard Operator, or Stencil Relay.

## RULE 13 — AUDIO IS CORE QUALITY
Intentional original sound design, not a final afterthought.

## RULE 14 — SETTINGS MUST BE REAL
Music / Effects / Haptics controls affect real systems and persist.

## RULE 15 — NO DEAD CONTROLS
Every player-facing production control functions.

## RULE 16 — MOBILE FIRST
Verify touch usability, landscape, safe areas, readability, performance, tablet.

## RULE 17 — ASSET PROVENANCE
Every shipped asset has a known legal source. Prefer project-generated originals.

## RULE 18 — DOCUMENT MAJOR CHANGES
Update project memory after substantial work.

## RULE 19 — TEST AFTER IMPLEMENTING
Implementation without testing is incomplete.

## RULE 20 — DO NOT STOP EARLY
Continue until completion gates pass or a genuine external blocker exists.

## RULE 21 — LOCAL COMMIT SYSTEM
Git is local-only (initialized 2026-09-07 with user authorization). Commit every
meaningful change with a clear message. NEVER push to a remote. Never rewrite or
force-reset existing history.

## RULE 22 — DETERMINISM IS AUTHORITATIVE
Puzzle outcomes come from deterministic discrete-state simulation, never Unity
physics or randomness. Same chamber config → same logical result, always.
