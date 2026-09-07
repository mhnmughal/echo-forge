# PACKAGE_AUDIT - Echo Forge

Status: **APPLIED 2026-09-07**. Two passes from a normal editor launch (no `-noUpm`).
`Packages/manifest.json` went 53 -> 29 -> **19** dependencies. After each
`Client.Resolve()`: 0 compile errors, 17/17 EditMode, 13/13 `Validate Levels`,
19/19 PlayMode, Windows build 0/0 (~99 MB). Committed `f335774`, then the offline pass.

## Pass 2 goal: network-free, minimal offline set

Removed everything that reaches the network or is unused by this game.

### Kept (19)
- com.anklebreaker.unity-mcp - **editor-only dev tooling**, localhost bridge, never
  in the player. Strip this last, right before the store export (master prompt §34).
- com.unity.ide.rider / ide.visualstudio - editor only
- com.unity.inputsystem 1.19.0 - all input
- com.unity.render-pipelines.universal 17.3.0 - rendering (pulls core, shadergraph,
  universal-config transitively)
- com.unity.test-framework 1.6.0 - EditMode + PlayMode tests
- com.unity.ugui 2.0.0 - UI + bundled TextMeshPro
- com.unity.modules: accessibility, androidjni, animation, audio, imageconversion,
  imgui, jsonserialize, particlesystem, physics, physics2d, ui, uielements

### Removed in pass 2 (10) - verified: 0 code references, resolve clean, tests + build green
- com.unity.modules.unitywebrequest - HTTP client (no code uses `UnityWebRequest`)
- com.unity.modules.unitywebrequesttexture - HTTP texture download
- com.unity.modules.video - `VideoPlayer` (unused, pulled unitywebrequest)
- com.unity.modules.director - `UnityEngine.Playables` (unused, timeline already gone)
- com.unity.modules.umbra - occlusion culling runtime (no baked occlusion)
- com.unity.modules.screencapture - `ScreenCapture` API (runtime unused)
- com.unity.modules.vectorgraphics - SVG runtime (unused)
- com.unity.2d.sprite - Sprite Editor window (single-sprite import works without it)
- com.unity.2d.tilemap - Tilemap tools (no tilemaps)
- com.unity.modules.tilemap - only referrer was com.unity.2d.tilemap

### Removed in pass 1 (24)
collab-proxy, multiplayer.center, visualscripting, timeline, six 2d.* packages
(2d.animation / aseprite / psdimporter / spriteshape / tilemap.extras / tooling),
and modules: unityanalytics, assetbundle, ai, cloth, terrain, terrainphysics,
vehicles, vr, xr, wind, unitywebrequestassetbundle, unitywebrequestaudio,
unitywebrequestwww.

## Analytics and diagnostics (disabled 2026-09-07)
`ProjectSettings/UnityConnectSettings.asset`: `m_Enabled 0`,
`InsightsSettings.m_EngineDiagnosticsEnabled 0`, `CrashReportingSettings`
cloud reporting 0 and `m_CaptureEditorExceptions 0`, all Analytics / Ads /
Purchasing / PerformanceReporting `m_Enabled 0` and `m_InitializeOnStartup 0`,
service URLs blanked. `ProjectSettings.asset`: `submitAnalytics 0`.

## Findings log
- 2026-09-07: trial trim under `-noUpm` reverted (Package Manager disabled).
- 2026-09-07: editor relaunched without `-noUpm`; pass 1 applied (53 -> 29).
- 2026-09-07: pass 2 applied (29 -> 19), network modules removed, engine
  diagnostics + analytics disabled. Verified. Build ~103 MB, 0/0.
- Remaining §34 item: strip `com.anklebreaker.unity-mcp` for the final store export
  (one manifest line, then re-resolve and rebuild).
