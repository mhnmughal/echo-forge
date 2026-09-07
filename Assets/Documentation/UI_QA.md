# UI_QA - Echo Forge

Actual Game View inspection per screen/state across representative aspect ratios.
Not PASS from Inspector hierarchy alone.

Ratios: 16:9 phone · wide iPhone (~19.5:9) · extra-wide phone · standard iPad
(4:3) · iPad Pro (~4.3:3).

Legend: `PHOTO` = a captured Game-View frame at that aspect exists and was inspected ·
`CAPTURED` = frame captured in the automated pass, not individually eyeballed ·
`ANALYZED` = layout math only.

The `ReleaseQA` capture pass (`Echo Forge/QA` menu, from Play Mode) renders all 13
states below at all 5 aspects to `QA/Responsive/` (65 frames). A representative
subset was inspected by hand: Resonance Hall, Resonance Dial, Tuning Strip,
Echo Chamber (16:9 and 4:3), Pause. The rest are `CAPTURED` (frames on disk).

| Screen / state | 16:9 | 19.5:9 | 21:9 | iPad 4:3 | iPad Pro |
|----------------|------|--------|------|----------|----------|
| Resonance Hall | PHOTO | CAPTURED | CAPTURED | CAPTURED | CAPTURED |
| Resonance Dial | PHOTO | CAPTURED | CAPTURED | CAPTURED | CAPTURED |
| Sound Check (L0) | CAPTURED | CAPTURED | CAPTURED | CAPTURED | CAPTURED |
| Echo Chamber (L11) | PHOTO | CAPTURED | CAPTURED | PHOTO | CAPTURED |
| Selected component | CAPTURED | CAPTURED | CAPTURED | CAPTURED | CAPTURED |
| Pause (Sound Damping) | PHOTO | CAPTURED | CAPTURED | CAPTURED | CAPTURED |
| Chamber Sound Guide | CAPTURED | CAPTURED | CAPTURED | CAPTURED | CAPTURED |
| Settings (Tuning Strip) | PHOTO | CAPTURED | CAPTURED | CAPTURED | CAPTURED |
| Reset confirmation | CAPTURED | CAPTURED | CAPTURED | CAPTURED | CAPTURED |
| Hall Sound Guide | CAPTURED | CAPTURED | CAPTURED | CAPTURED | CAPTURED |
| Success | CAPTURED | CAPTURED | CAPTURED | CAPTURED | CAPTURED |
| Failure | CAPTURED | CAPTURED | CAPTURED | CAPTURED | CAPTURED |
| Chamber In Tune (Finale) | CAPTURED | CAPTURED | CAPTURED | CAPTURED | CAPTURED |

Issues found and fixed in this pass:
- Main-menu buttons and title showed through the Dial / Settings / Guide panels.
  Fixed: the Hall hides its menu elements while any modal panel is open
  (`ResonanceHallController.modalPanels` / `menuElements`), and Tuning Strip +
  Sound Guide now sit on a full-screen opaque scrim like the Dial.
- Dial "LOCKED" label overlapped the chamber number. Fixed: bottom-aligned with
  an 8px margin; node backgrounds are now rings.
- Dial numbers rotated with the ring. Fixed: each node counter-rotates.
- On larger boards the puzzle extended under the PULSE control. Fixed:
  `LevelController.FitCamera` dollies the chamber camera per aspect so all board
  corners stay inside a safe viewport rectangle (checked in an automated test for
  L11 and L12).
- Narrow settings controls (toggle, credits) had small touch areas. Fixed:
  `EchoPolish.ImproveTouchTargets` pads any control under 96x80 with an invisible
  child, after forcing a layout pass so it never balloons.

Checks per frame: no clipping / overlap / unsafe controls / unreadable labels /
tiny touch targets / chamber obstruction by oversized UI.

## Runtime-UI-construction audit - PASS (2026-09-07)
`grep` of `Assets/EchoForge/Runtime` for `AddComponent<Button|Image|
TextMeshProUGUI|Canvas|CanvasScaler|Slider|Toggle>`: **zero matches**.
`new GameObject` in runtime is used only for: the AudioSource pool, the
data-driven puzzle-board component views (justified per §7/§20 - content varies
per level), a gameplay glow light, and transient pulse-VFX packets. No primary
production UI is built at runtime - all three scenes' UI is authored at edit time
by `EchoBuildScenes` and serialized into the `.unity` files.

## UI system (2026-09-07 pass)
One factory in `EchoBuild.cs` drives every screen so panels + buttons align:
- `Button` - flat graphite fill (bronze fill + amber rule when `primary`), a bronze
  base rule, ivory label with +6 tracking, hover-warms / press-bronze color states.
- `Panel` / `Rule` / `Header` - consistent card fill, bronze accent rules, one
  header baseline (title + underline at a fixed offset).
- `ButtonStack` - equal-width, equal-gap vertical button columns (top-anchored),
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

## Aspect-ratio pass - CAPTURED across all 5 (2026-09-07)

Tooling: `ResponsiveQA` sets the Game View to each landscape target
(16:9 1920x1080 · 19.5:9 2340x1080 · 21:9 2520x1080 · 4:3 1440x1080 ·
4.3:3 1550x1080). `ReleaseQA` drives every UI state and screenshots all 5.

**Layout model - PASS.** Every canvas: `CanvasScaler` ScaleWithScreenSize,
ref 1920x1080, `matchWidthOrHeight = 1`. All UI under the `SafeArea` child driven
by `SafeAreaFitter`. Vertical is always exactly 1080 logical units, so nothing
clips vertically; wider aspects only add horizontal room; at the narrowest target
(4:3, half-width 720) the widest cluster still lands well inside the frame; corner
HUD sits inside the safe-area-inset rect so it clears a notch by construction.

**Capture pass - all 5 aspects, 13 states (65 frames in `QA/Responsive/`).** The
inspected subset shows no clipping, overlap, or unsafe controls. Remaining item:
a human should still eyeball the full set on a real notched device and Dynamic
Island, which needs hardware.
