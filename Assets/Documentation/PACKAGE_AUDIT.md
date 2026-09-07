# PACKAGE_AUDIT — Echo Forge

Status: **planned, not yet applied** — this session's Unity editor runs with
`-noUpm` (Package Manager disabled), so `Packages/manifest.json` edits do not
resolve. A trial removal was reverted cleanly. Apply the removals below from a
normal editor launch, then: refresh → compile → console → run EchoForge.Tests →
`Echo Forge/Validate Levels` → open all 3 scenes → rebuild.

## Keep (used at runtime or by required tooling)
- com.unity.render-pipelines.universal 17.3.0 — rendering
- com.unity.inputsystem 1.19.0 — all input
- com.unity.ugui 2.0.0 — UI + bundled TextMeshPro
- com.unity.test-framework 1.6.0 — EditMode + PlayMode tests
- com.unity.modules: ui, uielements, imgui, audio, particlesystem, physics
  (raycast for tap selection), physics2d (URP 2D dep), animation, director,
  imageconversion (icon PNG encode), jsonserialize (save), screencapture (QA),
  vectorgraphics, video, umbra, accessibility, adaptiveperformance, androidjni
- com.unity.ide.rider / ide.visualstudio — editor only
- com.anklebreaker.unity-mcp — **editor-only dev tooling**; remove before the
  final production export (master prompt §34) once no MCP work remains

## Remove (unused; safe, high confidence)
- com.unity.collab-proxy — VCS UI, project uses local git
- com.unity.multiplayer.center — offline single-player game
- com.unity.visualscripting — unused
- com.unity.2d.animation / 2d.aseprite / 2d.psdimporter / 2d.spriteshape /
  2d.tilemap.extras / 2d.tooling — 2.5D game, primitives + procedural art only
  (keep 2d.sprite + 2d.tilemap, near-core and cheap)
- com.unity.modules: unityanalytics (offline), assetbundle, ai, cloth, terrain,
  terrainphysics, vehicles, vr, xr, wind, unitywebrequestassetbundle,
  unitywebrequestaudio, unitywebrequestwww
- Evaluate: com.unity.timeline (+ keep director) — currently unused; keep only if
  the Chamber In Tune finale ends up sequenced with Timeline, else remove.
- Evaluate: com.unity.modules.unitywebrequest / unitywebrequesttexture — keep only
  if URP/TMP importers still need them after the above removals.

## Findings log
- 2026-09-07: trial manifest trim attempted, reverted (editor is `-noUpm`).
  No package changes are in the repo yet.
