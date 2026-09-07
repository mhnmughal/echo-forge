# ASSET_PROVENANCE - Echo Forge

Every shipped non-code asset. Nothing external; all project-generated.

## Meshes / models
Unity built-in primitives (Cube/Cylinder/Sphere/Quad/Plane) only, arranged by
edit-time builder scripts and by `LevelController` at runtime for the data-driven
puzzle board. No imported models.

## Materials / textures
URP Lit materials authored by `EchoBuild.Mat` (`Assets/EchoForge/Art/Materials/`):
solid colours + metallic/smoothness/emission, project palette. No texture files.

## Fonts
TextMeshPro default **LiberationSans SDF** (SIL Open Font License 1.1, bundled
with `com.unity.ugui`). Used for all UI. Single family; weight/tracking/case give
hierarchy. No font files added to the repo, no prior-project font pairing reused.

## App icon
`Assets/EchoForge/Art/Icon/EchoForge_Icon_1024.png` - 1024×1024 master, plus
180/120/60 QA downscales. **Procedurally rendered** by
`Assets/EchoForge/Editor/Build/IconGen.cs` (2× supersampled raster): bronze
resonance ring + three restrained violet acoustic wavefronts curving in from the
lower-left + an amber resonant point with bloom, on a deep midnight ground.
Original raster art, no external source, no image-generation service.
Assigned to Player Settings (Standalone / iOS / Android icon slots) by the same
tool. Small-size QA (2026-09-07): ring silhouette + resonant point readable at
60 px; wavefront detail softens at that size (acceptable). No Unity default icon,
no prior-project motif, no text.

## Audio
See AUDIO_PROVENANCE.md - 20 project-generated procedural WAVs.

## External assets
NONE.
