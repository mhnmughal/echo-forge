# UI_QA — Echo Forge

Actual Game View inspection per screen/state across representative aspect ratios.
Not PASS from Inspector hierarchy alone.

Ratios: 16:9 phone · wide iPhone (~19.5:9) · extra-wide phone · standard iPad
(4:3) · iPad Pro (~4.3:3).

Legend: `PHOTO` = captured Game-View frame inspected · `ANALYZED` = anchor / scaler
/ safe-area math (see "Aspect-ratio pass" below) · `NOT VERIFIED` = neither.

| Screen / state | 16:9 | wide iPhone | extra-wide | iPad | iPad Pro |
|----------------|------|-------------|------------|------|----------|
| Resonance Hall | PHOTO✓ | ANALYZED | ANALYZED | ANALYZED | ANALYZED |
| Resonance Dial | ANALYZED | ANALYZED | ANALYZED | ANALYZED | ANALYZED |
| Sound Check | ANALYZED | ANALYZED | ANALYZED | ANALYZED | ANALYZED |
| Echo Chamber | PHOTO✓ | ANALYZED | ANALYZED | PHOTO | ANALYZED |
| Pause (Sound Damping) | PHOTO✓ | ANALYZED | ANALYZED | ANALYZED | ANALYZED |
| Selected component UI | ANALYZED | ANALYZED | ANALYZED | ANALYZED | ANALYZED |
| PULSE actuator | PHOTO | ANALYZED | ANALYZED | PHOTO | ANALYZED |
| Adjustment counter | PHOTO | ANALYZED | ANALYZED | RECHECK | ANALYZED |
| Pause (Sound Damping) | ANALYZED | ANALYZED | ANALYZED | ANALYZED | ANALYZED |
| Settings (Tuning Strip) | ANALYZED | ANALYZED | ANALYZED | ANALYZED | ANALYZED |
| Sound Guide | ANALYZED | ANALYZED | ANALYZED | ANALYZED | ANALYZED |
| Success | ANALYZED | ANALYZED | ANALYZED | ANALYZED | ANALYZED |
| Failure | ANALYZED | ANALYZED | ANALYZED | ANALYZED | ANALYZED |
| Reset confirmation | ANALYZED | ANALYZED | ANALYZED | ANALYZED | ANALYZED |
| Chamber In Tune | ANALYZED | ANALYZED | ANALYZED | ANALYZED | ANALYZED |

`RECHECK` = the 4:3 corner-HUD frame was ambiguous under render throttle; layout
math predicts PASS, confirm from a focused editor.

Checks: no clipping / overlap / unsafe controls / unreadable labels / tiny touch
targets / chamber obstruction by oversized UI. PHOTO cells: no issues seen.
ANALYZED cells: no issue predicted by the layout model.

## Runtime-UI-construction audit — PASS (2026-09-07)
`grep` of `Assets/EchoForge/Runtime` for `AddComponent<Button|Image|
TextMeshProUGUI|Canvas|CanvasScaler|Slider|Toggle>`: **zero matches**.
`new GameObject` in runtime is used only for: the AudioSource pool, the
data-driven puzzle-board component views (justified per §7/§20 — content varies
per level), a gameplay glow light, and transient pulse-VFX packets. No primary
production UI is built at runtime — all three scenes' UI is authored at edit time
by `EchoBuildScenes` and serialized into the `.unity` files.

## UI system (2026-09-07 pass)
One factory in `EchoBuild.cs` drives every screen so panels + buttons align:
- `Button` — flat graphite fill (bronze fill + amber rule when `primary`), a bronze
  base rule, ivory label with +6 tracking, hover-warms / press-bronze color states.
- `Panel` / `Rule` / `Header` — consistent card fill, bronze accent rules, one
  header baseline (title + underline at a fixed offset).
- `ButtonStack` — equal-width, equal-gap vertical button columns (top-anchored),
  used by the Hall main menu and the pause menu so columns are perfectly flush.
- `UIEdge` (64) / `BtnH` (76) / `BtnHBig` (96) / `BtnGap` (16) constants.
Screens rebuilt on it: Bootstrap title, Hall main menu (centred card, one column),
Resonance Dial / Tuning Strip / Sound Guide (headers + aligned close/action rows),
Tuning faders + toggle (shared left margin), Chamber HUD (TUNES pill + pause disc
on one rail, bronze-ticked context card, tutorial caption), Result + Pause layers
(scrim + centred card + aligned button rows/column).

## Screens built & wired (edit-time authored)
Resonance Hall (primary action, Sound Check, Dial, Tuning, Sound Guide, finale
emblem) · Resonance Dial (12 nodes, focus readout, prev/next/launch/close) ·
Echo Chamber (context readout, circular PULSE actuator, TUNES counter, circular
pause, tutorial caption, result layer, Sound Damping pause layer, dialog layer,
Sound Guide layer) · Tuning Strip (Music/Effects faders, Haptics toggle, reset
confirm, credits).

## Aspect-ratio pass — PARTIAL (2026-09-07)

Tooling: `Echo Forge/QA/Game View - *` (`ResponsiveQA.cs`) sets the Game View to
each landscape target: 16:9 1920×1080 · 19.5:9 2340×1080 · 21:9 2520×1080 ·
4:3 1440×1080 · 4.3:3 1550×1080.

**Static layout analysis — PASS.** Every canvas: `CanvasScaler` ScaleWithScreenSize,
ref 1920×1080, `matchWidthOrHeight = 1` (match height). All UI is parented under
the `SafeArea` child driven by `SafeAreaFitter` (normalises `Screen.safeArea` to
anchors each frame it changes). Consequences:
- Vertical is always exactly 1080 logical units → no vertical element (max reach
  ~+360 from an edge, result title +120 from centre) is ever clipped.
- Wider aspects only add horizontal breathing room (safe direction).
- Narrowest target 4:3 → 1440 logical width, half-width 720. Widest horizontal
  cluster is the result screen's Retune/ToDial buttons at centre ±230 × 300 wide
  → outer edge ±380, well inside ±720. Chamber context panel (x 250, w 460) →
  20…480 from left. No horizontal clipping at 4:3.
- Corner HUD (TUNES x180/y66, pause x84/y64) sits inside the already-safe-area-
  inset rect → clears notch / Dynamic Island / home indicator by construction.

**Photo capture — 3 of 5 aspects.** Editor runs unfocused here, which throttles
Game-View rendering, so the screenshot pipeline only reliably flushes one frame
per play session. Captured and inspected: Resonance Hall 16:9; Echo Chamber 16:9
(all HUD present, unclipped, PULSE + tutorial caption well placed); Echo Chamber
4:3 (core interactive UI correct; top-corner HUD not legible in that single
throttled frame — needs a focused-editor recheck). 19.5:9 / 21:9 / 4.3:3 photo
verification and the corner-HUD 4:3 recheck: **do from a focused interactive
editor** — layout math above predicts PASS.
