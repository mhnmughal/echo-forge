# PACKAGE_AUDIT — Echo Forge

Status: DRAFT. Full pass required before release candidate.

## Current manifest (Unity 6000.3.19f1)
Keep:
- com.unity.render-pipelines.universal 17.3.0 — rendering
- com.unity.inputsystem 1.19.0 — input
- com.unity.ugui 2.0.0 — UI
- com.unity.test-framework 1.6.0 — tests
- com.unity.timeline 1.8.12 — finale sequencing (confirm use, else drop)
- com.unity.ide.* — Editor only
- com.anklebreaker.unity-mcp — Editor-only dev tooling (§34: remove or prove
  Editor-only before final export)
- TextMeshPro (bundled in ugui 2.0.0 / Unity 6)

Review / likely remove:
- com.unity.collab-proxy 2.12.4 — VCS UI, not needed (local git). REMOVE candidate.
- com.unity.multiplayer.center 1.0.1 — offline game. REMOVE candidate.
- com.unity.visualscripting 1.9.11 — unused. REMOVE candidate.
- com.unity.2d.* (animation, aseprite, psdimporter, spriteshape, tilemap, tooling)
  — 2.5D game; most unused. REMOVE candidates, keep only what art pipeline needs.
- com.unity.modules.unityanalytics — REMOVE (offline).
- com.unity.modules.unitywebrequest* — REMOVE if no runtime use.
- com.unity.modules.ai / vehicles / cloth / terrain / wind / xr / vr — likely
  removable; verify no dependency.

## Procedure after any change
1 refresh · 2 compile · 3 console · 4 affected tests · 5 rerun validator ·
6 verify scenes/references.

## Findings log
(empty)
