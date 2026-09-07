# UI_QA — Echo Forge

Actual Game View inspection per screen/state across representative aspect ratios.
Not PASS from Inspector hierarchy alone.

Ratios: 16:9 phone · wide iPhone (~19.5:9) · extra-wide phone · standard iPad
(4:3) · iPad Pro (~4.3:3).

| Screen / state | 16:9 | wide iPhone | extra-wide | iPad | iPad Pro |
|----------------|------|-------------|------------|------|----------|
| Resonance Hall | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED |
| Resonance Dial | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED |
| Sound Check | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED |
| Echo Chamber | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED |
| Selected component UI | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED |
| PULSE actuator | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED |
| Adjustment counter | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED |
| Pause (Sound Damping) | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED |
| Settings (Tuning Strip) | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED |
| Sound Guide | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED |
| Success | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED |
| Failure | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED |
| Reset confirmation | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED |
| Chamber In Tune | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED | NOT VERIFIED |

Checks: no clipping / overlap / unsafe controls / unreadable labels / tiny touch
targets / chamber obstruction by oversized UI.

## Runtime-UI-construction audit — PASS (2026-09-07)
`grep` of `Assets/EchoForge/Runtime` for `AddComponent<Button|Image|
TextMeshProUGUI|Canvas|CanvasScaler|Slider|Toggle>`: **zero matches**.
`new GameObject` in runtime is used only for: the AudioSource pool, the
data-driven puzzle-board component views (justified per §7/§20 — content varies
per level), a gameplay glow light, and transient pulse-VFX packets. No primary
production UI is built at runtime — all three scenes' UI is authored at edit time
by `EchoBuildScenes` and serialized into the `.unity` files.

## Screens built & wired (edit-time authored)
Resonance Hall (primary action, Sound Check, Dial, Tuning, Sound Guide, finale
emblem) · Resonance Dial (12 nodes, focus readout, prev/next/launch/close) ·
Echo Chamber (context readout, circular PULSE actuator, TUNES counter, circular
pause, tutorial caption, result layer, Sound Damping pause layer, dialog layer,
Sound Guide layer) · Tuning Strip (Music/Effects faders, Haptics toggle, reset
confirm, credits).

## Aspect-ratio pass — NOT VERIFIED
CanvasScaler is set to ScaleWithScreenSize / match-height=1 and a SafeAreaFitter
drives every canvas, but the Game View has not been exercised at 16:9 / 19.5:9 /
21:9 / 4:3 / 4.3:3 with screenshots. Do this from an interactive editor.
