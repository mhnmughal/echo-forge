# LEVEL_QA - Echo Forge

Manual player-facing QA per stage. Manual = played through the real flow
(load, select, rotate, move, PULSE, correct + incorrect solution, fail reason,
success, retry/reset, pause/resume, rating, save, unlock, replay).

Target: `13 / 13 HUMAN TOUCH PASS`.

Two categories, both required, never substituted for each other:
- **DATA / SIMULATION VALID** - `LevelValidator` runs the deterministic sim on
  each level's optimal config. **13/13 VALID**.
- **AUTOMATED CONTROLLER QA** - `EchoForge.PlayTests` loads each level in the real
  EchoChamber scene and drives it to the solve by tapping, moving, and rotating
  the parts through `LevelController`, then retunes and re-pulses.
  **13/13 PASS** (2026-09-07, part of the 19-test PlayMode suite).
- **HUMAN TOUCH QA** - a person plays each stage with touch controls.
  **0/13 - NOT VERIFIED** (no device / no human tester available).

| Stage | Data valid | Automated controller | Human touch |
|-------|-----------|----------------------|-------------|
| L0 Sound Check | VALID | PASS | NOT VERIFIED |
| L1 First Echo | VALID | PASS | NOT VERIFIED |
| L2 Corner Tone | VALID | PASS | NOT VERIFIED |
| L3 Silent Wall | VALID | PASS | NOT VERIFIED |
| L4 Echo Path | VALID | PASS | NOT VERIFIED |
| L5 Split Signal | VALID | PASS | NOT VERIFIED |
| L6 Two Voices | VALID | PASS | NOT VERIFIED |
| L7 Late Echo | VALID | PASS | NOT VERIFIED |
| L8 Harmonic Gate | VALID | PASS | NOT VERIFIED |
| L9 Crossing Echoes | VALID | PASS | NOT VERIFIED |
| L10 Double Beat | VALID | PASS | NOT VERIFIED |
| L11 Resonance Network | VALID | PASS | NOT VERIFIED |
| L12 Final Resonance | VALID | PASS | NOT VERIFIED |

Also PlayMode-tested: fail path + Retune re-enables input; pause/resume restores
time + input; tutorial step advance; progression unlock; save persistence.
