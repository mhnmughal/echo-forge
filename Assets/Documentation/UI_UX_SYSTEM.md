# UI_UX_SYSTEM — Echo Forge

Status: DRAFT (bootstrap).

## Principle
Contextual, spatial, integrated into the acoustic chamber. The chamber dominates
the screen. No full-width top HUD, no permanent tool dock, no status rail, no
rectangular level grid, no generic menu cards.

## Screens / elements
- **Resonance Hall** — circular acoustic room. Centerpiece: Suspended Resonance
  Ring (tap → vibrate + ripple). Primary: START SESSION / RESUME CHAMBER.
- **Resonance Dial** — large radial dial; 12 chambers as frequency positions.
  Rotate to bring a chamber to focus. Shows number, title, completion quality,
  lock state. Sound Check accessed separately.
- **Echo Chamber** — puzzle scene. Contextual selected-component readout near
  lower-left safe region (e.g. `REFLECTOR / ANGLE 45`). Manipulation controls
  appear at the object.
- **PULSE actuator** — physical Resonance Actuator at lower-center chamber frame.
  Not a generic button.
- **Adjustment counter** — small engraved `TUNES 03`.
- **Sound Damping pause** — damping panels slide inward, ambience filters down.
  Resume / Restart Chamber / Sound Guide / Exit Session.
- **Tuning Strip settings** — Music/Ambience fader, Effects fader, Haptics toggle,
  recessed Reset Progress, Credits maker plate.
- **Sound Guide** — acoustic reference wall explaining each component.
- **Success** — RESONANCE ACHIEVED (or CHAMBER IN TUNE on L12). Rating as
  waveform marks: Resolved / Clean Echo / Pure Resonance. Actions: Next Chamber /
  Retune / Resonance Dial.
- **Failure** — OUT OF PHASE / SIGNAL LOST + reason. Retune.

## Palette
Midnight navy, graphite charcoal, warm bronze, matte black, soft ivory,
restrained pale violet (wave energy), muted amber (receiver glow), low-sat
crimson (invalid resonance).

## Typography
TextMeshPro. Max ~2 legally-safe typefaces: one restrained architectural display,
one neutral body. Not reusing prior-project font pairings.

## Responsive / safe area
Landscape. Test 16:9 phone, wide iPhone, extra-wide phone, standard iPad, iPad
Pro ratio. Critical UI inside safe area (notch / Dynamic Island / home indicator);
decorative chamber content may extend past it.

## Difference from previous projects
No document/correction metaphor (Misprint Department). No Control Room / Card
Vault / Status Rail / Tool Dock / punch-card editor / industrial console language
(Punchcard Operator). No Light Studio / Pattern Rack / translucent plate tabs /
PROJECT control / glass-drafting-table language (Stencil Relay). Navigation is a
radial acoustic dial, not a grid, list, rack, or carousel.

## UI QA
See UI_QA.md.
