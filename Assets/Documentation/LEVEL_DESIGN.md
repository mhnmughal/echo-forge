# LEVEL_DESIGN — Echo Forge

All 13 stages authored as `Assets/EchoForge/Levels/Level_NN.asset`
(`EchoLevelDefinition`). Validator: **13/13 VALID** — every `optimalComponents`
config solves via the deterministic simulator and no initial config pre-solves.
Coordinates are chamber cells (x east, y north). Reflector orient: 0=`/` 1=`\`
2=`|` 3=`-`. Splitter forks the incoming pulse into its two 45 deg neighbours.

## Interpretation decision (master prompt §2)
Forbidden receivers are visually introduced at **L3** (they are the reason an
absorber matters in the deterministic model) and fully combined with the other
components at **L6** per the spec. Recorded because the original spec lists the
forbidden receiver under L6 only.

## Series A — Reflection
- **L0 Sound Check** (tutorial, 9×7) — emitter (1,3)→E, receiver (5,5), one
  tunable reflector (5,3). Rotate it to `/` so E→N reaches the receiver. Teaches
  select / rotate / PULSE / observe. No rating; persists tutorial-complete.
- **L1 First Echo** (9×7, par 2 / opt 1) — reflector (6,1) `/`: single redirect.
- **L2 Corner Tone** (9×9, par 3 / opt 2) — reflectors (7,1)`/` + (7,7)`\`: two
  turns around a corner.
- **L3 Silent Wall** (9×7, par 2 / opt 1) — fixed splitter (4,3) forks toward a
  good receiver (7,6) and a forbidden one (7,0). Move the absorber onto (6,1) to
  silence the bad branch.
- **L4 Echo Path** (11×9, par 3 / opt 2) — wall (5,4) blocks the straight route.
  Rotate reflector (5,1) to `/` and drop the movable reflector into socket (5,3)
  `\`; fixed reflector (2,3) carries the pulse up to receiver (2,8). First
  rotate+move mastery under a tight par.

## Series B — Resonance
- **L5 Split Signal** (11×9, par 3 / opt 2) — fixed splitter (5,4); rotate both
  branch reflectors (3,6) and (7,6) to `-` so each branch reaches its receiver.
- **L6 Two Voices** (11×10, par 3 / opt 2) — two splitters. Rotate reflector
  (6,2) to `-` to steer the south branch to receiver (9,5); move the absorber to
  (8,6) to stop the east sub-branch before the forbidden receiver (9,6). Both
  good receivers (6,8) and (9,5) must ring.
- **L7 Late Echo** (11×7, par 2 / opt 1) — splitter (3,3); delay plate (2 beats)
  moved onto (4,2) makes receiver (6,0) ring exactly one beat after (6,6).
  Timing constraint enforced by the simulator.
- **L8 Harmonic Gate** (11×11, par 3 / opt 2) — splitter (5,5); rotate both
  reflectors (7,3)/(3,3) to `|` so the two branches converge on resonator (5,1)
  on the same beat (Simultaneous rule). Logic, not music.

## Series C — Phase
- **L9 Crossing Echoes** (9×10, par 3 / opt 2) — two emitters fire NE and SE and
  physically cross at (3,4) with no interaction. Rotate reflector (4,5) to `/`
  and (4,3) to `\` (pass-through) so each pulse reaches its receiver. Rule shown
  explicitly in the Sound Guide.
- **L10 Double Beat** (11×9, par 2 / opt 1) — splitter (5,3) + fixed reflectors
  (3,5)/(7,5) converge both branches on resonator (5,7). Drop the 2-beat delay
  plate onto (4,6) so the branches arrive 2 beats apart (BeatDelta 2).
- **L11 Resonance Network** (13×12, par 4 / opt 2) — two splitters, a fixed delay
  plate, three receivers (6,10)/(8,4)/(10,2), one forbidden (7,0). Rotate
  reflector (8,7) to `\` for receiver (8,4); move the absorber to (7,1). The
  readability + performance milestone.
- **L12 Final Resonance** (13×11, par 3 / opt 2) — two emitters, a fixed 2-beat
  delay on the upper line, two forbidden receivers (10,2)/(10,8). Rotate
  reflector (6,2) to `/` and (6,8) to `\` so both pulses converge on the final
  resonator (6,5) two beats apart. Completion → **CHAMBER IN TUNE**.
