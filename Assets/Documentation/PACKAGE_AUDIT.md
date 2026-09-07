# PACKAGE_AUDIT — Echo Forge

Status: **APPLIED 2026-09-07** — trim done from a normal editor launch (no `-noUpm`).
`Packages/manifest.json` went from **53 → 29** dependencies. After `Client.Resolve()`:
0 compile errors, 17/17 EditMode, 13/13 `Validate Levels`, 17/17 PlayMode, 3 scenes
rebuild clean. Committed `f335774`.

## Keep (used at runtime or by required tooling)
- com.unity.render-pipelines.universal 17.3.0 — rendering
- com.unity.inputsystem 1.19.0 — all input
- com.unity.ugui 2.0.0 — UI + bundled TextMeshPro
- com.unity.test-framework 1.6.0 — EditMode + PlayMode tests
- com.unity.2d.sprite / 2d.tilemap — near-core, cheap, kept
- com.unity.modules kept: accessibility, androidjni, animation, audio, director,
  imageconversion, imgui, jsonserialize, particlesystem, physics, physics2d,
  screencapture, tilemap, ui, uielements, umbra, unitywebrequest,
  unitywebrequesttexture, vectorgraphics, video
- com.unity.ide.rider / ide.visualstudio — editor only
- com.anklebreaker.unity-mcp — **editor-only dev tooling**; remove before the
  final production export (master prompt §34) once no MCP work remains

## Removed (unused; verified safe)
- com.unity.collab-proxy, com.unity.multiplayer.center, com.unity.visualscripting
- com.unity.timeline (finale is coroutine-driven, not sequenced) — kept `director` module
- com.unity.2d.animation / 2d.aseprite / 2d.psdimporter / 2d.spriteshape /
  2d.tilemap.extras / 2d.tooling
- com.unity.modules: unityanalytics, assetbundle, ai, cloth, terrain,
  terrainphysics, vehicles, vr, xr, wind, unitywebrequestassetbundle,
  unitywebrequestaudio, unitywebrequestwww

## Findings log
- 2026-09-07: trial trim under `-noUpm` reverted (Package Manager disabled).
- 2026-09-07: editor relaunched WITHOUT `-noUpm`; trim applied and verified; committed.
- Remaining §34 item: strip `com.anklebreaker.unity-mcp` for the final store export.
