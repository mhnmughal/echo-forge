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

## Runtime-UI-construction audit
Search for `new GameObject` / `AddComponent<Button>` / `AddComponent<Image>` /
`AddComponent<TextMeshProUGUI>` in primary UI paths: NOT VERIFIED (no UI yet).
