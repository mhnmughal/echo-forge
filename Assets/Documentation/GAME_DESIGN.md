# GAME_DESIGN — Echo Forge

Status: DRAFT (bootstrap).

## Pillar
Tune the room, not the character. Spatial acoustic resonance puzzles.

## Loop
Listen → Select → Tune (rotate/move) → Pulse → Observe → Adjust → Resolve.

## Components
| Component | Behavior | Player interaction |
|-----------|----------|--------------------|
| Emitter | Emits one pulse per level start, fixed direction/channel. May be multiple. | none (some levels: none) |
| Reflector | Redirects pulse by discrete angle state (default 45° steps). | tap to select → rotate arc → snap |
| Absorber | Kills incoming pulse. | usually fixed; sometimes movable |
| Splitter | Kills incoming, emits 2 deterministic branches. | rotate to orient branches |
| Delay Plate | Holds pulse N beats then re-emits same direction. | fixed N (shown), sometimes movable |
| Receiver | Requires a pulse arrival. | none |
| Resonator | Requires 2+ pulses in a defined relationship (count / beat-delta / order). | none |
| Forbidden Receiver | Must NOT be hit. | none |

## Adjustments ("TUNES")
Each rotate-snap or socket move = +1 TUNE. Par per level → CLEAN ECHO.
Optimal configuration → PURE RESONANCE. Any solve → RESOLVED.

## Failure states (no "Game Over")
OUT OF PHASE / SIGNAL LOST with a reason: Receiver Missed · Forbidden Receiver
Triggered · Timing Incorrect · Resonance Incomplete. Player retunes freely.

## Determinism
No randomness affects path, reflection, absorption, split, delay, receiver,
forbidden, resonator, success, adjustment count, or rating.

## Progression
First launch → Level 0 Sound Check (tutorial, no rating). Completion unlocks
Level 1. Linear 1→12. Completed chambers replayable. Level 12 → CHAMBER IN TUNE.

## Series
- A Reflection (1-4): reflectors, spatial routing, first absorber, first mastery.
- B Resonance (5-8): splitter, forbidden receiver, delay plate, resonator.
- C Phase (9-12): two emitters, crossing paths, timing patterns, network, finale.
