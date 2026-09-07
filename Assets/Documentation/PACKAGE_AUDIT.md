# PACKAGE_AUDIT - Resonance Chamber (codename Echo Forge)

Status: **APPLIED 2026-09-07**. Four passes from a normal editor launch.
`Packages/manifest.json` went 53 -> 29 -> 19 -> 14 -> **12** dependencies. After
each `Client.Resolve()`: 0 compile errors, 17/17 EditMode, 13/13 `Validate
Levels`, 19/19 PlayMode, Windows build 0/0 (~99 MB), smoke test clean (PhysX +
Input init, scene transitions, no missing-module errors).

Pass 4 (14 -> 12) removed both remaining editor-only integrations:
`com.unity.ide.rider` and `com.anklebreaker.unity-mcp`. **The MCP editor bridge
is gone** - no more AI/editor automation; open the project in Unity to
re-resolve and confirm 0 errors before any store export.

## Current set (12)
- com.unity.ide.visualstudio - editor only (VS / VS Code csproj + intellisense)
- com.unity.inputsystem 1.19.0 - all input
- com.unity.render-pipelines.universal 17.3.0 - rendering (pulls core, shadergraph,
  universal-config transitively)
- com.unity.test-framework 1.6.0 - EditMode + PlayMode tests (pulls ext.nunit)
- com.unity.ugui 2.0.0 - UI + bundled TextMeshPro
- com.unity.modules: audio, imageconversion (IconGen editor tool), imgui,
  jsonserialize (save), physics (tap raycast), ui, uielements

### Removed in pass 3 (5) - 0 code refs, no package declares a dependency on them, resolve + tests + build + smoke all green
- com.unity.modules.animation - `Animator` / `Animation` (unused; UI uses ColorTint transitions, not the Animation transition)
- com.unity.modules.particlesystem - `ParticleSystem` (unused; pulse VFX uses LineRenderer + TrailRenderer)
- com.unity.modules.physics2d - 2D physics (unused; the 3D `EchoForgeForwardRenderer` needs no 2D physics)
- com.unity.modules.accessibility - `UnityEngine.Accessibility` (unused; no VoiceOver / screen-reader integration exists, so the module adds nothing - re-add + implement if that changes)
- com.unity.modules.androidjni - `AndroidJavaObject` / JNI (unused; the per-platform haptic impact generator TODO would re-add this. NOTE: not yet verified on an actual Android build.)

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
  diagnostics + analytics disabled. Verified.
- 2026-09-07: pass 3 applied (19 -> 14), 5 unused engine modules removed
  (animation, particlesystem, physics2d, accessibility, androidjni). Verified:
  17/17 + 19/19 tests, 13/13 validator, Windows build 0/0 ~99 MB, smoke clean.
- 2026-09-07: pass 4 applied (14 -> 12) at user request - removed
  `com.unity.ide.rider` (verified: resolve clean, 0 errors, 17/17 + 19/19) and
  `com.anklebreaker.unity-mcp` (MCP bridge; verified in manifest + packages-lock
  only - editor connection severed by the removal itself; re-open the project in
  Unity to confirm the resolve). com.unity.ext.nunit stays (Test Framework dep).
- Remaining: verify `androidjni` removal on the first real Android build.
