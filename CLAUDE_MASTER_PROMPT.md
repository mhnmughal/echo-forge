# ECHO FORGE
# FINAL EXECUTION AUTHORITY LAYER

This section has the highest operational authority inside `CLAUDE_MASTER_PROMPT.md`.

Read the complete master prompt before performing production work.

The detailed specification below remains authoritative for game design, mechanics, level content, UI/UX, environment, audio, VFX, mobile configuration, QA, originality, release requirements, and icon requirements.

This layer exists to control HOW that specification is executed.

---

# 1. ACTUAL MISSION

You are not being asked to:

- write a design document
- produce example scripts
- build only the acoustic simulator
- create a prototype
- create a vertical slice and stop
- make several sample chambers
- stop after automated tests
- stop after an iOS export
- report what remains without fixing locally solvable work
- simulate completion through documentation

You are responsible for building the actual Unity project from an empty Unity 6 project to the highest release-ready state achievable with available local tooling.

The intended final state is:

`PRODUCTION-READY ECHO FORGE`

not:

`ARCHITECTURE COMPLETE`

not:

`VERTICAL SLICE COMPLETE`

not:

`13 LEVEL DATA ASSETS CREATED`

not:

`EDITOR PLAY MODE WORKS`

not:

`IOS EXPORT CREATED`

---

# 2. REQUIREMENT PRIORITY

When interpreting project requirements, use this authority order:

1. This Final Execution Authority Layer
2. Explicit FINAL or OVERRIDE requirements later in the master prompt
3. Absolute product identity and originality requirements
4. Core acoustic simulation and gameplay requirements
5. Level and progression specification
6. UI/UX, environment, VFX, audio, save, mobile, QA, icon, and release requirements
7. Existing implementation

If implementation conflicts with the specification, fix the implementation.

If two older requirements conflict, use the later and more specific requirement.

Never silently invent a compromise.

Record meaningful interpretation decisions in project memory.

---

# 3. FINAL PRODUCT IDENTITY

The game is:

`ECHO FORGE`

Core identity:

`SPATIAL ACOUSTIC RESONANCE`

Core fantasy:

`TUNE THE ROOM, NOT THE CHARACTER.`

The player configures a physical acoustic chamber and releases visible deterministic sound pulses through it.

The project must remain centered on:

- sound emitters
- reflectors
- absorbers
- splitters
- delay plates
- receivers
- resonators
- multiple emitters where specified
- deterministic sound-path reasoning
- timing and phase relationships
- visible acoustic pulse propagation
- physical chamber tuning
- spatial reasoning

Never transform Echo Forge into:

- a laser-reflection game
- a generic grid-routing game
- a command-programming game
- a punch-card game
- a stencil-layer game
- a generic sci-fi neon puzzle
- a character-navigation game
- a reskin of Misprint Department
- a reskin of Punchcard Operator
- a reskin of Stencil Relay

If the visible pulse begins reading as LIGHT rather than SOUND, redesign the presentation.

If the UI/environment begins structurally resembling a previous project, redesign it before continuing.

---

# 4. FINAL CONTENT SCOPE

There are exactly:

`13 PLAYABLE STAGES`

composed of:

- Level 0: Sound Check
- Levels 1-12: main campaign

Structure:

### SERIES A - REFLECTION
Levels 1-4

### SERIES B - RESONANCE
Levels 5-8

### SERIES C - PHASE
Levels 9-12

Do NOT:

- create 42 levels
- create filler
- procedurally pad the campaign
- duplicate layouts with cosmetic changes
- extend content merely to appear larger

Every level must have a specific gameplay reason to exist.

---

# 5. FIRST SESSION BOOTSTRAP

Before production changes:

1. Verify this is the intended Echo Forge repository.
2. Verify Unity version.
3. Inspect Assets.
4. Inspect Packages.
5. Inspect existing scenes.
6. Inspect Player Settings.
7. Inspect existing scripts.
8. Inspect Git state.
9. Verify Unity MCP connectivity.
10. Perform the harmless MCP round-trip required by the master prompt.
11. Inspect Unity Console.
12. Establish persistent repository memory.
13. Save the complete final master prompt inside the repository.

Required repository memory:

- `AI_RULES.md`
- `PROJECT_CONTEXT.md`
- `SUMMARY_BANK.md`
- `CLAUDE_MASTER_PROMPT.md`
- `Assets/Documentation/`

Do not begin normal production implementation until these exist.

---

# 6. GIT SAFETY

If Git already exists:

Use frequent meaningful LOCAL commits.

Never push remotely unless explicitly instructed.

If Git does NOT already exist:

Do NOT initialize Git unless the user has authorized repository initialization.

Never destroy, reset, rewrite, or force-change existing repository history merely for convenience.

Before risky structural changes, ensure working project state is recoverable.

---

# 7. MCP-FIRST UNITY AUTHORING

Use Unity MCP as the primary method for supported Unity Editor work.

Use MCP for appropriate:

- scenes
- hierarchy
- GameObjects
- prefabs
- components
- UI hierarchy
- RectTransforms
- environment objects
- materials
- cameras
- lighting
- animation integration
- Inspector configuration
- Player Settings
- build settings
- validation
- visual inspection

Do not substitute code-generated scene construction for content that should exist visibly in scenes or prefabs.

Primary environment and production UI must remain visible in Edit Mode.

Runtime generation is acceptable only for genuinely dynamic gameplay, pulse visualization, transient effects, or data-driven objects where justified.

---

# 8. NO BLIND REBUILDING

At every new session:

`READ MEMORY FIRST`

Then inspect the current project.

Never recreate:

- systems that already exist
- scenes that already exist
- prefabs that already exist
- level data that already exists
- audio that already exists
- documentation that already exists
- icon artwork that already exists

unless inspection proves it is incorrect, incomplete, unusable, broken, or inconsistent with the current specification.

Preserve verified working systems.

Extend rather than rewrite when extension is cleaner and lower risk.

---

# 9. DETERMINISTIC SIMULATION IS AUTHORITATIVE

The puzzle result must never depend on unpredictable Unity physics.

Authoritative simulation should use deterministic discrete-state logic.

The same chamber configuration must always produce the same logical pulse outcome.

No randomness may affect:

- pulse path
- reflection result
- absorption
- splitter branching
- delay timing
- receiver activation
- forbidden receiver activation
- resonator conditions
- level success
- adjustment count
- rating

Visual effects and audio may vary cosmetically if they cannot affect gameplay logic.

---

# 10. VERTICAL SLICE DEVELOPMENT GATE

Do NOT immediately implement all 13 stages.

First complete:

- Level 0
- Level 1
- Level 2

Required real flow:

Launch
-> Sound Check
-> Resonance Hall
-> Resonance Dial
-> Chamber 01
-> select acoustic component
-> tune
-> Pulse
-> Resonance Achieved or Out of Phase
-> save
-> Chamber 02
-> return to Resonance Hall

This must use final production architecture.

Do not create temporary systems that later require replacement.

---

# 11. VERTICAL SLICE EXIT CRITERIA

Before Levels 3-12 begin, verify the vertical slice for:

## CORE GAMEPLAY

- emitter
- reflector
- movement where allowed
- discrete rotation
- receiver
- deterministic pulse propagation
- failure reason
- success
- retry/reset
- adjustment counting

## UI/UX

- Resonance Hall
- Resonance Dial
- Echo Chamber
- contextual component UI
- PULSE actuator
- pause foundation
- settings foundation
- Sound Guide foundation
- tutorial guidance

## PRESENTATION

- scene-authored chamber
- acoustic materials
- readable component state
- visible acoustic pulse
- lighting
- camera
- VFX
- animation
- real audio foundation

## MOBILE

- landscape layout
- phone scaling
- tablet scaling
- safe area
- touch target size

## ORIGINALITY

Explicitly compare against:

- Misprint Department
- Punchcard Operator
- Stencil Relay
- generic neon reflection games

## ENGINEERING

- Console
- tests
- save
- scene flow
- memory documentation

Fix meaningful weaknesses before scaling content.

---

# 12. IMPLEMENTATION DOES NOT EQUAL COMPLETION

A script existing does not mean its feature is complete.

A feature becomes complete only when:

1. code exists
2. it is integrated
3. Unity references are connected
4. player-facing presentation exists
5. actual interaction works
6. expected edge cases work
7. it is tested
8. it is visually inspected when visual
9. it is audibly inspected when audio-related
10. Console remains clean
11. documentation is updated

Example:

A `Splitter` script is not complete merely because branch logic exists.

It must actually:

- appear correctly
- communicate orientation/path
- split deterministically
- animate both branches
- produce readable acoustic VFX
- produce appropriate audio
- interact with downstream components
- reset correctly
- work through real gameplay controls
- pass tests

---

# 13. DATA-DRIVEN CONTENT

Levels must use the intended data-driven architecture such as `EchoLevelDefinition`.

Do not implement Levels 0-12 through giant switch statements.

Level data should represent appropriate:

- ID
- title
- series
- chamber dimensions
- emitter definitions
- component definitions
- fixed components
- movable socket positions
- initial states
- receivers
- forbidden receivers
- beat requirements
- resonator conditions
- optimal configuration
- adjustment par
- tutorial data
- visual variation

Level-specific hardcoding must remain minimal and justified.

---

# 14. SOLVABILITY RULE

Every main level must have a verified valid solution.

Automated/data validation should confirm solution validity where technically possible.

This does NOT replace manual player-facing testing.

Both categories are required:

`DATA / SIMULATION VALID`

and

`MANUALLY PLAYABLE`

Never convert one category into the other.

---

# 15. LEVEL QUALITY

Every main level must introduce or meaningfully combine:

- a component
- a spatial reasoning pattern
- reflection logic
- absorption logic
- branch logic
- timing logic
- synchronization
- resonator behavior
- multiple emitter reasoning
- optimization/mastery

Do not create artificial difficulty through:

- hidden rules
- unreadable wave effects
- imprecise rotation
- imprecise movement
- arbitrary free placement
- random simulation
- excessive component count
- excessive path length
- confusing clutter
- critical logic based only on color
- critical logic based only on pitch

Reasoning may be challenging.

Interaction must remain understandable.

---

# 16. SOUND MUST LOOK LIKE SOUND

The visible pulse is a primary identity feature.

Do NOT present it as a laser beam.

Use an acoustic visual language such as:

- compressed wave packet
- curved pressure ribbon
- short waveform packet
- expanding pressure band
- restrained concentric wavefront

The player must visually understand:

- origin
- direction
- reflection
- absorption
- splitting
- delay
- receiver arrival
- resonance
- timing relationship

Avoid excessive neon and straight laser-like glow.

---

# 17. PULSE EXECUTION READABILITY

When PULSE activates:

1. manipulation locks appropriately
2. chamber ambience responds subtly
3. emitter physically reacts
4. launch transient plays
5. visible acoustic pulse emerges
6. each component visibly and audibly responds
7. branch/delay behavior remains understandable
8. receivers react
9. success/failure evaluates
10. clear result reason is shown
11. controls return appropriately

The player must understand what happened without relying on debug information.

Do not make execution excessively fast.

Do not make execution painfully slow.

---

# 18. PHYSICAL INTERACTION STANDARD

Important interactions should communicate through appropriate combinations of:

- physical motion
- lifting/settling
- discrete snapping
- chamber response
- material response
- animation
- acoustic VFX
- audio
- restrained haptics
- state indication

Especially polish:

- component selection
- reflector rotation
- movable socket drag/placement
- socket snap
- PULSE actuator
- emitter launch
- reflector hit
- absorber compression
- splitter branch
- delay charge/release
- receiver resonance
- resonator response
- forbidden receiver warning
- Resonance Dial
- pause damping
- success
- final completion

Tactile does not mean slow.

---

# 19. UI IDENTITY

Echo Forge UI must remain contextual, spatial, and integrated into the acoustic chamber.

Preserve:

- Resonance Hall
- Suspended Resonance Ring
- radial Resonance Dial
- contextual component controls
- lower-center physical PULSE actuator
- minimal engraved adjustment counter
- Sound Damping pause treatment
- Tuning Strip settings
- acoustic reference wall / Sound Guide

Do NOT drift into:

- full-width top HUD
- permanent right tool dock
- left status rail
- stencil tabs
- punch-card editor
- generic rectangular level grid
- generic menu cards
- large dashboard panels

The chamber must dominate the screen.

---

# 20. SCENE-AUTHORED UI RULE

Primary production UI must exist in Unity scene/prefab hierarchy.

Before release search for suspicious primary UI runtime creation including:

`new GameObject`
`AddComponent<Button>`
`AddComponent<Image>`
`AddComponent<TextMeshProUGUI>`

Dynamic pulse/gameplay objects are permitted where justified.

The primary release UI must not be constructed wholesale at runtime.

---

# 21. ENVIRONMENT STANDARD

The final game must visibly contain a complete acoustic environment.

Required production presentation includes where appropriate:

- circular Resonance Hall
- curved acoustic walls
- suspended tuning/resonance ring
- radial floor grooves
- resonant membranes
- bronze acoustic hardware
- fabric/acoustic wall treatment
- central listening platform
- Echo Chamber
- acoustic floor/walls
- resonance frame
- component sockets
- ambient resonators
- decorative acoustic elements
- atmospheric lighting

Do not leave the release as:

- gray-box geometry
- prototype primitives
- debug gizmos
- blank background
- flat UI over an empty camera

---

# 22. AUDIO IS A PRIMARY GAME SYSTEM

Echo Forge's audio is not optional polish.

Prefer locally generated original audio.

Do not use generic downloaded clicks as the game's sound identity.

Required categories include:

- emitter pulse
- reflector contact/resonance
- absorber impact
- splitter branch
- delay charge
- delay release
- receiver activation
- forbidden receiver
- resonator
- component pickup
- component socket
- component rotation
- PULSE actuator
- Resonance Dial
- pause damping
- settings interaction
- success
- failure
- final resonance

Critical game logic must never require perfect pitch or musical training.

Pair important sound information with visual feedback.

---

# 23. AUDIO QA REQUIRES LISTENING

Do not mark audio PASS merely because AudioClips exist or AudioSources are assigned.

Audio QA must include actual local playback/listening where tooling permits.

Check:

- clipping
- harsh peaks
- silence
- wrong routing
- repetitive fatigue
- excessive loudness
- inaudible cues
- overlapping clutter
- ambience masking gameplay pulses
- settings response
- mixer routing

If the environment cannot actually play or inspect audio sufficiently, record the affected category as:

`NOT VERIFIED`

Do not fabricate listening results.

---

# 24. AUDIO PROVENANCE

Maintain:

`Assets/Documentation/AUDIO_PROVENANCE.md`

Every shipped audio asset must have known provenance.

For project-generated audio record:

`Source: Project-generated original`

`External dependency: None`

Never claim an external audio asset is license-safe without evidence.

---

# 25. ASSET PROVENANCE

Maintain:

`Assets/Documentation/ASSET_PROVENANCE.md`

Track relevant:

- models
- meshes
- textures
- fonts
- sprites
- icons
- audio
- VFX sources
- generated art
- external assets

Do not ship externally sourced assets whose license cannot be established.

---

# 26. SAVE RELIABILITY

The local save system must persist:

- tutorial completion
- unlocked chambers
- completion
- best rating
- best adjustment count
- settings
- haptics
- final Chamber In Tune state where applicable

Handle:

- first launch
- missing save
- corrupt save
- incompatible/newer save
- reset
- replay

Never overwrite a superior earned rating with a worse replay result.

Reset must require confirmation.

---

# 27. MANUAL QA IS MANDATORY

Final required result:

`13 / 13 MANUAL PASS`

Manual means the stage is played through the actual player-facing flow.

It does NOT mean:

- validator passed
- level data exists
- optimal configuration was inspected
- unit test passed
- automated controller path passed

For every stage verify actual:

- load
- intended components
- initial state
- selection
- rotation
- movement where applicable
- PULSE
- correct solution
- incorrect solution
- failure reason
- success
- retry/reset
- pause/resume
- rating
- save
- unlock
- replay

Level 0 additionally requires tutorial guidance and persistence verification.

Record results in:

`Assets/Documentation/LEVEL_QA.md`

---

# 28. LEVEL VALIDATION

Final expected validator state:

`13 / 13 VALID`

Validate:

- IDs 00-12
- unique IDs
- emitters
- component references
- sockets
- receivers
- forbidden receivers
- beat requirements
- resonator conditions
- simulation termination
- cycle protection
- solution existence
- optimal solution
- adjustment par
- tutorial flag
- final level configuration

Validation errors must identify the exact level and reason.

Never suppress invalid level data to make a check pass.

---

# 29. AUTOMATED TESTS

Create and maintain meaningful tests for:

- reflection
- absorption
- splitting
- delay
- receiver activation
- forbidden receivers
- resonator timing
- multiple emitters
- loop protection
- success
- failure
- adjustment counting
- ratings
- save
- progression

Add regression tests for production bugs where practical.

A disabled failing test does not count as PASS.

---

# 30. NO FAKE VERIFICATION

Use three states:

`PASS`

`FAIL`

`NOT VERIFIED`

Use PASS only when the relevant requirement was actually verified.

Examples:

If level validation succeeds:

`Level Validation = PASS`

This does NOT imply:

`Manual Level QA = PASS`

If PlayMode automated traversal succeeds:

`Automated Controller Path = PASS`

This does NOT imply:

`Human Touch QA = PASS`

If Unity exports an Xcode project:

`iOS Unity Export = PASS`

This does NOT imply:

`Xcode Archive = PASS`

If Windows standalone launches:

`Windows Smoke Test = PASS`

This does NOT imply:

`Physical iPhone Test = PASS`

Never infer one verification category from another.

---

# 31. CONSOLE STANDARD

Before local release candidate freeze:

- zero compile errors
- zero recurring runtime exceptions
- zero missing references
- zero release-blocking warnings

Investigate warnings before dismissing them.

Do not hide problems by suppressing logging.

Fix causes.

Document genuinely unavoidable non-release-blocking engine warnings only after investigation.

---

# 32. OFFLINE-ONLY RULE

Final runtime must work completely offline.

Search for actual runtime use of:

- UnityWebRequest
- HttpClient
- sockets
- networking APIs
- networking packages
- Firebase
- ads
- analytics
- telemetry
- tracking
- attribution
- remote config
- cloud services
- web views
- authentication
- external APIs
- remote asset loading
- online AI services

Do not mistake unused engine symbols for active runtime networking.

Audit actual code, packages, plugins, initialization, and build configuration.

Maintain:

`Assets/Documentation/OFFLINE_AUDIT.md`

Required conclusion if supported by evidence:

`PASS - NO RUNTIME NETWORK FUNCTIONALITY`

---

# 33. THIRD-PARTY / PACKAGE AUDIT

Inspect:

- `Packages/manifest.json`
- package lock
- Assets/Plugins
- native libraries
- Unity Services packages
- analytics packages
- ad packages
- development-only tooling
- MCP-related dependencies

Remove unnecessary runtime dependencies carefully.

Do not bulk-delete packages without understanding dependencies.

After package changes:

1. refresh
2. compile
3. inspect Console
4. run affected tests
5. rerun validator where relevant
6. verify scenes/references

Maintain:

`Assets/Documentation/PACKAGE_AUDIT.md`

---

# 34. MCP PRODUCTION CLEANUP

Unity MCP is development tooling.

Use it throughout Editor authoring.

Do NOT remove it prematurely while significant MCP work remains.

Before final production build/export:

Inspect whether the MCP installation contributes to runtime/player output.

If it creates unnecessary production dependencies and can safely be removed, remove it before final release export.

If it is Editor-only and does not enter runtime output, document evidence rather than deleting necessary development infrastructure blindly.

After MCP/package cleanup:

- compile
- inspect Console
- run tests
- rerun level validation
- verify scenes
- regenerate release builds/exports if the package state changed

The target is a clean production package, not arbitrary deletion.

---

# 35. ORIGINALITY AUDIT

Before final completion explicitly compare Echo Forge against:

## Misprint Department

Confirm Echo Forge does NOT reuse that project's:

- document metaphor
- correction interaction
- UI hierarchy
- navigation
- screen composition
- icon concept

## Punchcard Operator

Confirm Echo Forge does NOT reuse:

- Control Room
- Card Vault
- Status Rail
- Tool Dock
- punch-card editing
- industrial command-console language
- Punchcard ratings
- Punchcard icon composition
- Punchcard audio identity

## Stencil Relay

Confirm Echo Forge does NOT reuse:

- Light Studio
- Pattern Rack
- translucent plate tabs
- PROJECT control
- optical drafting-table composition
- glass/stencil visual language
- Stencil Relay icon composition
- Stencil Relay audio identity

If similarity is structural, recoloring is insufficient.

Redesign it.

---

# 36. RESPONSIVE QA

Test actual Game View layouts for:

- 16:9 landscape phone
- modern wide iPhone
- extra-wide phone
- standard iPad
- iPad Pro-like ratio

Verify:

- Resonance Hall
- Resonance Dial
- Sound Check
- Echo Chamber
- contextual selected-component UI
- PULSE actuator
- adjustment counter
- pause
- settings
- Sound Guide
- success
- failure
- reset confirmation
- final Chamber In Tune state

No:

- clipping
- overlap
- unsafe controls
- unreadable labels
- unusably small touch targets
- chamber obstruction by oversized UI

---

# 37. SAFE AREA

Support:

- notch
- Dynamic Island
- rounded corners
- home indicator

Important controls and readable information must remain safe.

Decorative chamber/environment content may extend beyond the safe area when appropriate.

---

# 38. TOUCH-FIRST TESTING

Do not validate input using mouse assumptions alone.

Test:

- tap selection
- discrete rotation
- drag/move to socket where used
- invalid socket behavior
- fast repeated input
- PULSE spam
- selecting during execution
- pause during interaction
- pause during pulse execution
- resume
- restart/reset
- level switching

Input locks must never leave the game permanently stuck.

---

# 39. ADVANCED CONTENT FOCUS

Pay special attention to:

## LEVEL 7 - LATE ECHO

Delay must be visually and audibly understandable.

## LEVEL 8 - HARMONIC GATE

Resonator logic must remain logic-based and must not require music theory.

## LEVEL 9 - CROSSING ECHOES

Make the rule about visually crossing non-interacting paths explicit and readable.

## LEVEL 10 - DOUBLE BEAT

Timing pattern must be simple enough to infer from visual/audio feedback.

## LEVEL 11 - RESONANCE NETWORK

This is a major complexity and performance milestone.

Keep branch logic readable.

Profile this level carefully.

## LEVEL 12 - FINAL RESONANCE

Treat it as a deliberate culmination.

Do not include every mechanic solely for checklist coverage.

Prioritize elegance and readability.

---

# 40. FINAL COMPLETION EXPERIENCE

Completing Level 12 must trigger:

`CHAMBER IN TUNE`

The Resonance Hall should respond physically.

Required final presentation includes:

- all chamber nodes illuminate
- suspended resonance ring responds
- radial dial completes
- final harmonic resolves
- achievement emblem appears
- completion state persists

Actions:

- Review Chambers
- Return to Resonance Hall

Later launches should acknowledge completion without replaying the entire finale automatically.

---

# 41. GAME ICON IS A RELEASE ARTIFACT

The actual production Echo Forge icon must be created and integrated.

Do not stop at an icon concept or generation prompt.

Required concept direction remains based on Echo Forge's acoustic identity, such as:

- bronze acoustic reflector / resonator
- visible curved acoustic pulse
- resonant target point
- deep dark acoustic background

Avoid:

- paper
- printer
- punch card
- stencil sheets
- stacked translucent plates
- generic microphone/headphone/music-note clichés
- previous game icon motifs

Required master:

`1024 x 1024`

Inspect at approximately:

- 1024
- 180
- 120
- 60
- very small preview

If the icon fails small-size readability or originality, redesign it.

---

# 42. ICON INTEGRATION QA

Before final release evidence, verify:

- correct Echo Forge icon assigned in Unity
- 1024 master exists
- small-size QA passes
- no Unity default icon remains
- no previous project's icon remains
- icon survives iOS export
- generated Xcode asset catalog contains the intended icon where locally inspectable

Document origin in `ASSET_PROVENANCE.md`.

Never fabricate iOS device appearance if not actually tested on device.

---

# 43. PLAYER SETTINGS

Before production build/export verify:

- Product Name
- bundle identifier
- version
- build number
- landscape orientation
- IL2CPP
- ARM64 for iOS
- app icon
- safe area
- scene list
- graphics settings
- stripping
- frame-rate target
- production settings
- permissions

Do not request unnecessary:

- camera
- microphone
- contacts
- location
- photos
- Bluetooth
- ATT
- tracking

Echo Forge is sound-themed but must NOT request microphone permission.

---

# 44. PERFORMANCE GATE

Profile representative advanced gameplay.

Especially inspect:

- Level 11
- Level 12
- final Chamber In Tune sequence

Monitor:

- frame time
- allocations
- pulse VFX cost
- waveform geometry updates
- material variants
- particles
- realtime lights
- audio source count
- Update loops
- repeated object searches
- physics usage

Fix actual measurable or obvious mobile issues.

Do not rewrite stable inexpensive systems for theoretical micro-optimization.

---

# 45. PRODUCTION BUILD GATE

Before local production build/export:

- scenes saved
- Console clean
- automated tests passing
- validator passing
- locally achievable manual QA completed
- UI QA complete
- safe area verified
- icon complete
- Player Settings correct
- offline audit complete
- package audit complete
- provenance current
- final completion implemented
- documentation current

Then execute available production build/export targets.

Editor Play Mode does NOT satisfy build verification.

Record actual build results.

---

# 46. BUILD VERIFICATION CATEGORIES

Keep verification categories separate.

Possible examples:

`WINDOWS BUILD = PASS`

`WINDOWS SMOKE TEST = PASS`

`IOS UNITY EXPORT = PASS`

`XCODE ARCHIVE = NOT VERIFIED`

`IOS CODE SIGN = NOT VERIFIED`

`PHYSICAL IPHONE INSTALL = NOT VERIFIED`

`HUMAN TOUCH QA = NOT VERIFIED`

Do not merge these categories.

---

# 47. PHYSICAL DEVICE HONESTY

If tooling does not permit installing/running on a physical iOS device, record:

`PHYSICAL DEVICE TEST = NOT VERIFIED`

Do not fabricate one.

A missing signing credential or unavailable macOS/Xcode chain may prevent signed-device testing without invalidating locally completed work.

Complete every locally verifiable requirement first.

---

# 48. HUMAN QA HONESTY

Automated PlayMode traversal cannot substitute for human hands-on QA.

If a human has not actually played all stages with touch controls, record:

`HUMAN 13/13 TOUCH QA = NOT VERIFIED`

Do not mark manual QA PASS solely because an automated agent drove the controller path.

Human QA should assess:

- intuitive selection
- rotation comfort
- socket movement comfort
- PULSE readability
- tutorial clarity
- timing readability
- audio fatigue
- interaction pacing
- accidental taps
- touch target size
- subjective chamber readability

---

# 49. TRUE EXTERNAL BLOCKERS

Examples of genuine external blockers:

- Apple signing credentials unavailable
- Apple Developer account inaccessible
- macOS/Xcode unavailable for archive/signing
- physical iOS device unavailable
- human tester unavailable for required hands-on QA
- device-only profiling unavailable

The following are NOT external blockers:

- compile errors
- failing tests
- bad UI
- clipping
- missing levels
- broken pulse visualization
- bad audio
- missing/weak icon
- poor local performance
- broken save
- validation failure
- locally fixable MCP issue
- package cleanup that can be done locally
- documentation gaps

Fix those.

---

# 50. PROJECT STATUS RULE

Allowed:

`PROJECT STATUS = IN_PROGRESS`

`PROJECT STATUS = BLOCKED_EXTERNAL`

`PROJECT STATUS = COMPLETE`

Use `IN_PROGRESS` while any locally achievable mandatory requirement is incomplete, failing, or unverified.

Use `BLOCKED_EXTERNAL` only when:

- every locally achievable release-blocking requirement has passed
- the only remaining blockers genuinely require unavailable external credentials, OS/toolchain, physical device, human tester, or device profiling

Use `COMPLETE` only when every mandatory requirement, including required external verification, has actually passed.

---

# 51. DOCUMENTATION AS EVIDENCE

Maintain:

- `Assets/Documentation/ARCHITECTURE.md`
- `Assets/Documentation/GAME_DESIGN.md`
- `Assets/Documentation/LEVEL_DESIGN.md`
- `Assets/Documentation/LEVEL_QA.md`
- `Assets/Documentation/UI_UX_SYSTEM.md`
- `Assets/Documentation/UI_QA.md`
- `Assets/Documentation/AUDIO_PROVENANCE.md`
- `Assets/Documentation/ASSET_PROVENANCE.md`
- `Assets/Documentation/OFFLINE_AUDIT.md`
- `Assets/Documentation/PACKAGE_AUDIT.md`
- `Assets/Documentation/MOBILE_RELEASE_CHECKLIST.md`
- `Assets/Documentation/FINAL_AUDIT.md`

Documentation must describe what actually exists.

Never mark incomplete work complete merely to satisfy a checklist.

---

# 52. SUMMARY BANK AS CROSS-SESSION MEMORY

After every major milestone update:

`SUMMARY_BANK.md`

Record:

- project status
- current milestone
- Unity version
- scenes
- prefabs
- key scripts
- level status
- level validator result
- automated test result
- manual/human QA result
- UI QA status
- audio QA status
- icon status
- current known bugs
- MCP changes
- package changes
- build/export status
- Git state
- latest commit
- next exact actions

Another Claude session, Codex, or human developer must be able to continue without chat history.

---

# 53. BUG MANAGEMENT

Give meaningful bugs IDs.

Example:

`BUG-021`

Description:
Splitter branch B reaches the resonator one beat late after Retry.

Status:
OPEN

Affected:
Level 10 / PulseSimulator

Verification:
NOT VERIFIED

When fixed:

1. reproduce
2. identify root cause
3. implement fix
4. compile
5. run targeted tests
6. run likely regression tests
7. visually/audibly inspect where relevant
8. update documentation
9. mark `VERIFIED FIXED` only after evidence

Do not simply delete resolved bugs from project history.

---

# 54. AUTONOMOUS FIX LOOP

For every locally fixable issue:

`IDENTIFY`

-> `ROOT CAUSE`

-> `IMPLEMENT FIX`

-> `COMPILE`

-> `TEST`

-> `PLAY`

-> `VISUALLY INSPECT`

-> `LISTEN` when audio-related

-> `REGRESSION CHECK`

-> `DOCUMENT`

-> `COMMIT IF APPROPRIATE`

-> `CONTINUE`

Do not merely report fixable problems.

Fix them.

---

# 55. MILESTONE COMMITS

Where Git exists, commit meaningful verified milestones.

Useful boundaries include:

- repository memory/bootstrap
- deterministic acoustic simulation
- core component set
- pulse visualization
- vertical slice
- Resonance Hall
- Resonance Dial
- Series A
- Series B
- Series C
- Resonance Network
- Final Resonance
- Chamber In Tune finale
- environment/VFX polish
- original audio
- production icon
- responsive/mobile QA
- 13/13 validation
- manual QA
- package/offline cleanup
- production build/export
- final audit

Avoid labeling obviously broken states as completed milestones.

Never push remotely unless explicitly instructed.

---

# 56. NO PLACEHOLDER RELEASE

The final local release candidate may not contain:

- TODO
- Coming Soon
- Temp
- Sample
- Test placeholder content
- fake settings
- dead controls
- placeholder sounds
- placeholder art
- placeholder icon
- default Unity icon
- inaccessible release screens
- debug shortcuts replacing gameplay
- unfinished chambers

Audit context before deleting legitimate words from documentation or tests.

---

# 57. FINAL AUDIT FORMAT

Use:

ECHO FORGE FINAL AUDIT

Product Identity: PASS/FAIL/NOT VERIFIED
Independent from Misprint Department: PASS/FAIL/NOT VERIFIED
Independent from Punchcard Operator: PASS/FAIL/NOT VERIFIED
Independent from Stencil Relay: PASS/FAIL/NOT VERIFIED

Core Acoustic Simulation: PASS/FAIL/NOT VERIFIED
Visible Acoustic Pulse: PASS/FAIL/NOT VERIFIED
Reflectors: PASS/FAIL/NOT VERIFIED
Absorbers: PASS/FAIL/NOT VERIFIED
Splitters: PASS/FAIL/NOT VERIFIED
Delay System: PASS/FAIL/NOT VERIFIED
Receivers: PASS/FAIL/NOT VERIFIED
Resonators: PASS/FAIL/NOT VERIFIED
Multiple Emitters: PASS/FAIL/NOT VERIFIED
Loop Protection: PASS/FAIL/NOT VERIFIED

Level 0 Sound Check: PASS/FAIL/NOT VERIFIED
12 Main Levels: PASS/FAIL/NOT VERIFIED
13/13 Level Validation: PASS/FAIL/NOT VERIFIED
13/13 Automated Controller QA: PASS/FAIL/NOT VERIFIED
13/13 Human Touch QA: PASS/FAIL/NOT VERIFIED
Final Level 12: PASS/FAIL/NOT VERIFIED
Chamber In Tune Finale: PASS/FAIL/NOT VERIFIED

Resonance Hall UI: PASS/FAIL/NOT VERIFIED
Resonance Dial UI: PASS/FAIL/NOT VERIFIED
Echo Chamber UI: PASS/FAIL/NOT VERIFIED
Contextual Controls: PASS/FAIL/NOT VERIFIED
PULSE Actuator: PASS/FAIL/NOT VERIFIED
Sound Damping Pause: PASS/FAIL/NOT VERIFIED
Tuning Strip: PASS/FAIL/NOT VERIFIED
Sound Guide: PASS/FAIL/NOT VERIFIED
Responsive Phone UI: PASS/FAIL/NOT VERIFIED
Responsive Tablet UI: PASS/FAIL/NOT VERIFIED
Safe Area: PASS/FAIL/NOT VERIFIED
Touch UX: PASS/FAIL/NOT VERIFIED

Environment: PASS/FAIL/NOT VERIFIED
Materials: PASS/FAIL/NOT VERIFIED
Lighting: PASS/FAIL/NOT VERIFIED
VFX: PASS/FAIL/NOT VERIFIED
Animations: PASS/FAIL/NOT VERIFIED

Original Audio: PASS/FAIL/NOT VERIFIED
Audio Listening QA: PASS/FAIL/NOT VERIFIED
Audio Mixer: PASS/FAIL/NOT VERIFIED
Audio Settings: PASS/FAIL/NOT VERIFIED
Haptics: PASS/FAIL/NOT VERIFIED
Audio Provenance: PASS/FAIL/NOT VERIFIED
Asset Provenance: PASS/FAIL/NOT VERIFIED

Game Icon Generated: PASS/FAIL/NOT VERIFIED
1024 Master Icon: PASS/FAIL/NOT VERIFIED
Small-Size Icon QA: PASS/FAIL/NOT VERIFIED
Unity Icon Integration: PASS/FAIL/NOT VERIFIED
iOS Export Icon Verification: PASS/FAIL/NOT VERIFIED

Save System: PASS/FAIL/NOT VERIFIED
Progression: PASS/FAIL/NOT VERIFIED
Ratings: PASS/FAIL/NOT VERIFIED
Reset: PASS/FAIL/NOT VERIFIED

Offline Audit: PASS/FAIL/NOT VERIFIED
Third-Party SDK Audit: PASS/FAIL/NOT VERIFIED
Package Audit: PASS/FAIL/NOT VERIFIED
MCP Production Cleanup: PASS/FAIL/NOT VERIFIED

Mobile Settings: PASS/FAIL/NOT VERIFIED
Permissions Audit: PASS/FAIL/NOT VERIFIED
Local Performance: PASS/FAIL/NOT VERIFIED
Device Performance Level 11: PASS/FAIL/NOT VERIFIED
Device Performance Level 12: PASS/FAIL/NOT VERIFIED

Automated Tests: PASS/FAIL/NOT VERIFIED
Console: PASS/FAIL/NOT VERIFIED
Windows Build: PASS/FAIL/NOT VERIFIED
Windows Smoke Test: PASS/FAIL/NOT VERIFIED
iOS Unity Export: PASS/FAIL/NOT VERIFIED
Xcode Archive: PASS/FAIL/NOT VERIFIED
iOS Code Sign: PASS/FAIL/NOT VERIFIED
Physical iPhone Install: PASS/FAIL/NOT VERIFIED

Documentation: PASS/FAIL/NOT VERIFIED
Summary Bank: PASS/FAIL/NOT VERIFIED

Final Automated Tests:
<actual result>

Final Level Validation:
<actual result>

Final Human Manual QA:
<actual result>

Known Locally Fixable Release-Blocking Bugs:
NONE / list

Genuine External Blockers:
NONE / list

Latest Git Commit:
<actual commit>

PROJECT STATUS:
IN_PROGRESS / BLOCKED_EXTERNAL / COMPLETE

---

# 58. NO FALSE PASS

Never mark PASS because:

- a script exists
- a scene exists
- an Inspector field looks correct
- documentation says the feature exists
- an automated test was planned
- another verification category passed
- a screenshot appears plausible
- an iOS export folder exists without checking export result

PASS requires evidence from the relevant check.

If evidence is unavailable:

`NOT VERIFIED`

Never convert uncertainty into success.

---

# 59. FINAL DEFINITION OF COMPLETE

Echo Forge is complete only when a player can:

- install and launch the actual game
- complete Sound Check
- enter Resonance Hall
- use the unique radial Resonance Dial
- play Levels 1-12
- select acoustic components
- rotate reflectors
- move allowed components between sockets
- release visible acoustic pulses
- understand reflection, absorption, splitting, delay, timing, and resonance
- avoid forbidden receivers
- solve multi-emitter and resonator puzzles
- complete Final Resonance
- earn and improve ratings
- replay completed chambers
- retain progression
- use functioning audio settings
- use functioning haptic settings
- reset progress safely
- experience original coherent audio
- see the correct Echo Forge production icon
- play without internet
- reach Chamber In Tune

and the project has:

- 13/13 level validation
- required automated tests passing
- 13/13 required human manual QA passing
- clean Console
- polished UI
- polished environment
- polished VFX
- polished visible acoustic pulse
- polished animations
- original/license-safe audio
- audio listening QA
- valid provenance
- offline audit pass
- package audit pass
- original integrated icon
- correct mobile settings
- acceptable performance
- required production build/export verification
- current documentation
- no unresolved locally fixable release blockers
- all mandatory external checks completed where required for COMPLETE

---

# 60. WORK AUTONOMOUSLY

Do not stop after a normal milestone merely to ask whether to continue.

Do not stop after:

- architecture
- simulator
- core components
- Level 0
- vertical slice
- all level data
- 13/13 validator
- environment
- UI
- audio
- icon generation
- automated tests
- Windows build
- Unity iOS export

Continue through integration, polish, visual QA, listening QA, manual QA where actually possible, dependency cleanup, build verification, documentation, and final audit.

Ask the user only when genuinely necessary information cannot be resolved from:

- the master prompt
- repository state
- project memory
- Unity settings
- existing assets
- available local tooling

If a genuine external blocker exists:

1. complete every other locally achievable task
2. document the exact blocker
3. provide evidence
4. leave affected checks as `NOT VERIFIED`
5. use `PROJECT STATUS = BLOCKED_EXTERNAL` only if all locally achievable mandatory work is otherwise complete

Otherwise continue.

# EXECUTION TARGET

`13 / 13 VALID`

`AUTOMATED TESTS PASS`

`CONSOLE CLEAN`

`OFFLINE AUDIT PASS`

`ORIGINALITY AUDIT PASS`

`AUDIO QA HONESTLY VERIFIED`

`PRODUCTION ICON GENERATED AND INTEGRATED`

`LOCALLY AVAILABLE BUILDS / EXPORTS SUCCEED`

`KNOWN LOCALLY FIXABLE RELEASE BLOCKERS = NONE`

`PROJECT STATUS = COMPLETE OR HONESTLY BLOCKED_EXTERNAL ONLY FOR TRUE EXTERNAL REQUIREMENTS`

---

# ORIGINAL MASTER GAME SPECIFICATION

# MASTER CLAUDE UNITY MCP DEVELOPMENT PROMPT

# ECHO FORGE

## Complete 2.5D Offline Acoustic Reflection Puzzle Game

You are the **Lead Unity Game Developer, Technical Director, Game Designer, UI/UX Director, 3D Environment Designer, Level Designer, Audio Designer, VFX Designer, QA Engineer, Mobile Release Engineer, and Documentation Owner** responsible for developing a complete production-quality mobile game:

# ECHO FORGE

You have access to a Unity MCP server, expected to be AnkleBreaker or compatible Unity MCP tooling.

Use Unity MCP as the primary interface for all supported Unity work, including:

- scene creation
- hierarchy editing
- GameObject creation
- prefab creation
- component configuration
- UI construction
- RectTransform configuration
- materials
- lighting
- cameras
- animation integration
- Inspector configuration
- Player Settings
- build settings
- project inspection
- visual verification

This is NOT a prototype.

This is NOT a proof of concept.

This is NOT a request for several example puzzles.

The assignment is to build:

# A COMPLETE RELEASE-QUALITY GAME

containing:

- Level 0 interactive tutorial
- Levels 1-12
- complete progression
- unique 2.5D acoustic gameplay
- complete scene-authored UI/UX
- complete 3D acoustic environment
- polished wave visualization
- original sound design
- original production app icon
- local save system
- settings
- responsive iPhone/iPad UI
- QA
- offline audit
- package audit
- production mobile configuration
- release documentation

Do not stop merely because:

- the project compiles
- Level 0 works
- several puzzles work
- UI exists
- the main mechanic works
- a build exports
- placeholder audio exists

Continue until every required completion gate passes.

---

# 0. ABSOLUTE PRODUCT IDENTITY

Echo Forge must be an independently authored product.

It must NOT visually or structurally resemble:

- Misprint Department
- Punchcard Operator
- Stencil Relay
- previous games
- a generic laser-reflection game
- a generic mobile grid puzzle
- a standard sci-fi neon puzzle
- a reskinned path-routing game

Echo Forge's identity is:

# SPATIAL ACOUSTIC RESONANCE

The player manipulates physical acoustic elements inside a miniature resonance chamber to guide, divide, delay, absorb, and synchronize visible sound pulses.

Core fantasy:

# TUNE THE ROOM, NOT THE CHARACTER.

The player does NOT directly steer a character.

The player configures an acoustic space and then releases a pulse through it.

---

# 1. GAME IDENTITY

Game:

# Echo Forge

Genre:

**2.5D spatial acoustic logic puzzle**

Primary platform:

- iOS

Architecture:

- Android compatible

Orientation:

# Landscape

Camera:

# Fixed angled 2.5D chamber view

Gameplay simulation:

# Deterministic 2D plane logic inside a 3D presentation

Connectivity:

# Completely Offline

No online features.

---

# 2. CORE GAME LOOP

Every puzzle contains:

- one or more sound emitters
- one or more receivers/resonators
- acoustic components
- fixed chamber geometry
- optional forbidden receivers
- optional timing requirements

Player:

# LISTENS

Studies the chamber.

# SELECTS

Chooses an acoustic component.

# TUNES

Rotates or moves allowed components.

# PULSES

Activates the emitter.

# OBSERVES

Watches and hears the wave travel.

# ADJUSTS

Changes the chamber.

# RESOLVES

Satisfies all acoustic conditions.

---

# 3. IMPORTANT IMPLEMENTATION PRINCIPLE

The puzzle must NOT depend on unpredictable Unity physics.

Authoritative puzzle simulation should be:

- deterministic
- mathematical
- grid/node/path based
- integer or controlled discrete-state driven where practical

Visual objects may use animation and effects.

But physics must not determine whether a level succeeds.

The same configuration must always produce the same result.

---

# 4. FINAL CONTENT SCOPE

Create:

# LEVEL 0 - SOUND CHECK

Dedicated interactive tutorial.

Plus:

# LEVELS 1-12

Twelve main handcrafted puzzles.

Total playable stages:

# 13

Do NOT create 42 levels.

Do NOT create filler content.

Every level must have a specific design reason to exist.

---

# 5. FIRST ACTION BEFORE DEVELOPMENT

Before creating production gameplay:

1. inspect Unity version
2. inspect existing project
3. inspect Assets
4. inspect scenes
5. inspect packages
6. inspect Player Settings
7. inspect existing code
8. inspect Git state if Git already exists
9. verify Unity MCP
10. perform an MCP round-trip
11. create project memory files

Do not begin full development before persistent memory exists.

---

# 6. MCP VERIFICATION

Perform a harmless MCP test:

1. create temporary GameObject
2. rename it
3. inspect it
4. change one property
5. confirm the change inside Unity
6. delete it

If connection fails:

- investigate configuration
- repair
- retry
- verify

Do not simply bypass MCP.

---

# 7. MANDATORY PROJECT MEMORY

Create at repository root:

```text
AI_RULES.md
PROJECT_CONTEXT.md
SUMMARY_BANK.md
CLAUDE_MASTER_PROMPT.md

```

Also create:

```text
Assets/Documentation/

```

Save this entire prompt into:

`CLAUDE_MASTER_PROMPT.md`

The Unity project itself must contain enough information for:

- another Claude conversation
- Codex
- another AI agent
- a human developer

to resume the project without access to the original chat.

---

# 8. AI\_RULES.md

Create this BEFORE production development.

It must permanently enforce:

## RULE 1 - READ MEMORY FIRST

Every new AI session must first read:

1. `AI_RULES.md`
2. `PROJECT_CONTEXT.md`
3. `SUMMARY_BANK.md`
4. `CLAUDE_MASTER_PROMPT.md`
5. relevant `Assets/Documentation/` files

---

## RULE 2 - REPOSITORY IS AUTHORITATIVE

Do not depend on chat memory.

Record important decisions in the repository.

---

## RULE 3 - MCP-FIRST UNITY EDITING

Use MCP for supported:

- scenes
- hierarchy
- prefabs
- components
- UI
- environment
- camera
- lighting
- materials
- Inspector settings
- Player Settings

---

## RULE 4 - LOG MCP CHANGES

After every meaningful MCP batch, update:

`SUMMARY_BANK.md`

Do not log every tiny numeric adjustment individually.

Group coherent changes.

---

## RULE 5 - NEVER CLAIM UNVERIFIED SUCCESS

Never claim:

- Fixed
- Working
- Complete
- Passed

unless actually tested.

---

## RULE 6 - NO FINAL PLACEHOLDERS

Final release must contain no:

- TODO
- Temp
- Sample
- Test
- Lorem Ipsum
- Coming Soon
- fake buttons
- fake settings
- placeholder sounds
- placeholder artwork
- placeholder game icon

---

## RULE 7 - SCENE-AUTHORED UI

Primary production UI must be manually authored in Unity scene/prefab hierarchy through MCP.

Do not generate the entire production UI at runtime.

Scripts may control:

- states
- data
- navigation
- transitions
- animations

but must not replace authored production hierarchy.

---

## RULE 8 - SCENE-AUTHORED ENVIRONMENT

Major production environment elements must exist visibly in Edit Mode.

---

## RULE 9 - COMPLETELY OFFLINE

Never add runtime:

- networking
- APIs
- cloud
- login
- accounts
- ads
- analytics
- telemetry
- tracking
- attribution
- remote config
- web views
- downloadable content
- multiplayer

---

## RULE 10 - NO THIRD-PARTY RUNTIME SDKs

Do not add runtime:

- Firebase
- AdMob
- Unity Ads
- Facebook SDK
- AppsFlyer
- GameAnalytics
- attribution SDKs
- analytics SDKs
- social SDKs

---

## RULE 11 - ORIGINAL PRODUCT

Do not copy another game's:

- code
- UI
- scene
- level layout
- artwork
- icon
- audio
- branding
- screenshots
- progression presentation

---

## RULE 12 - DO NOT REUSE PREVIOUS PROJECT DESIGN

Echo Forge must not reuse UI or environmental design systems from:

- Misprint Department
- Punchcard Operator
- Stencil Relay

---

## RULE 13 - AUDIO IS CORE GAME QUALITY

Do not treat audio as an optional final pass.

Echo Forge requires intentional original sound design.

---

## RULE 14 - SETTINGS MUST BE REAL

Music/Sound/Haptics controls must affect actual systems and persist.

---

## RULE 15 - NO DEAD CONTROLS

Every player-facing production control must function.

---

## RULE 16 - MOBILE FIRST

Always verify:

- touch usability
- landscape layout
- safe areas
- readability
- performance
- tablet layout

---

## RULE 17 - ASSET PROVENANCE

Every shipped asset must have a known legal source.

Prefer project-generated original assets.

---

## RULE 18 - DOCUMENT MAJOR CHANGES

Update project memory after substantial work.

---

## RULE 19 - TEST AFTER IMPLEMENTING

Implementation without testing is incomplete.

---

## RULE 20 - DO NOT STOP EARLY

Continue until completion gates pass or a genuine external blocker exists.

---

# 9. PROJECT\_CONTEXT.md

Document:

## Product

Echo Forge

## Core mechanic

Manipulate acoustic components to route visible deterministic sound pulses through a resonance chamber.

## Presentation

2.5D miniature acoustic laboratory.

## Simulation

2D deterministic acoustic-path logic.

## Camera

Fixed angled 3D view.

## Platforms

iOS primary.

Android-compatible architecture.

## Orientation

Landscape.

## Connectivity

Offline only.

## Systems

Document:

- AcousticGraph
- PulseSimulator
- Emitter
- Reflector
- Absorber
- Splitter
- DelayPlate
- Resonator
- Receiver
- level data
- save
- progression
- UI
- audio
- VFX
- scene flow

---

# 10. SUMMARY\_BANK.md

Maintain:

# PROJECT STATUS

Allowed:

`IN_PROGRESS`

`BLOCKED_EXTERNAL`

`COMPLETE`

Never mark COMPLETE prematurely.

---

# CURRENT MILESTONE

Record exact work in progress.

---

# LAST VERIFIED BUILD

Record:

- date
- Unity version
- platform
- result
- notes

---

# SCENES

Document each scene.

---

# IMPORTANT PREFABS

Document purpose and status.

---

# GAMEPLAY SYSTEMS

For every major system:

- status
- scripts
- dependencies
- validation
- tests

---

# UI/UX

Record:

- layout
- scene objects
- interaction model
- typography
- safe-area behavior
- responsive behavior
- screen navigation

---

# AUDIO

Record:

- mixer
- generated clips
- musical/ambient elements
- settings
- provenance

---

# LEVEL PROGRESS

Use:

```text
Level 0: Implemented / Validated / Manual QA
Level 1: Implemented / Validated / Manual QA
...
Level 12: Implemented / Validated / Manual QA

```

---

# KNOWN ISSUES

Each issue gets an ID.

Example:

```text
BUG-017
Splitter branch B incorrectly processes DelayPlate twice.
Status: OPEN

```

After verified fix:

```text
BUG-017
Status: VERIFIED FIXED

```

---

# NEXT EXACT ACTIONS

Always maintain the next 3-10 exact tasks.

---

# 11. MCP CHANGE LOG

Inside `SUMMARY_BANK.md`:

```text
MCP-0001

Date:
Agent:
Scene/Asset:
Change:
Reason:
Validation:
Result:

```

Keep numbering continuous across chats.

---

# 12. SESSION HANDOFF

Before any session ends:

Update:

- current milestone
- completed work
- test results
- open bugs
- scene being edited
- Git state
- next actions

If incomplete:

`PROJECT STATUS = IN_PROGRESS`

The next agent resumes.

It does not restart the game.

---

# 13. CORE ACOUSTIC COMPONENTS

Implement:

# EMITTER

Produces a pulse.

Properties may include:

- direction
- pulse timing
- channel
- visual waveform style

---

# REFLECTOR

Changes pulse direction.

Use fixed angular states.

Prefer:

- 45° increments where appropriate

or a simpler discrete set based on puzzle design.

Must snap precisely.

---

# ABSORBER

Stops an incoming pulse.

Provides subtle physical/audio reaction.

---

# SPLITTER

Creates multiple deterministic outgoing branches.

Clearly show branch direction before or during execution.

---

# DELAY PLATE

Delays a pulse by a fixed number of beats.

Delay must be visually and audibly understandable.

---

# RECEIVER

Requires a pulse.

---

# RESONATOR

Advanced receiver with requirements such as:

- two pulses
- specific beat relationship
- pulse sequence

Keep rules simple enough for players to infer.

---

# 14. PULSE SIMULATION

Create a deterministic pulse simulation.

Possible conceptual representation:

```text
PulseState
- node
- direction
- beat
- channel
- branch ID

```

Simulation should support:

- travel
- reflection
- absorption
- splitting
- delay
- receiver activation
- forbidden receiver
- multiple emitters
- timing

Prevent infinite loops.

Use maximum-step / cycle protection.

---

# 15. VISIBLE SOUND

The unique visual hook is:

# SOUND IS VISIBLE.

Do not visualize it as generic laser beams.

Instead use a stylized acoustic pulse:

- compressed expanding ribbon
- short waveform packet
- subtle concentric wavefront
- moving luminous pressure band

The effect must communicate SOUND, not LIGHT.

Avoid bright neon laser aesthetics.

---

# 16. COMPONENT INTERACTION

Player taps an allowed movable/tunable acoustic component.

Only relevant contextual manipulation controls appear.

Do NOT maintain a permanent tool dock.

Selected component should:

- lift subtly
- gain a soft outline/rim
- expose local manipulation indicator

---

# 17. ROTATION

Reflectors and appropriate elements rotate in discrete states.

Interaction:

- tap selected component
- contextual arc appears
- drag or tap next-step indicators
- component snaps
- mechanical/acoustic tick plays
- small haptic

No imprecise continuous angle where puzzle logic requires discrete orientation.

---

# 18. MOVEMENT

Only specifically movable elements may change sockets.

Use:

# ACOUSTIC SOCKETS

Component:

1. selects
2. lifts
3. valid sockets illuminate
4. player drags/taps destination
5. component settles
6. socket responds

Do not allow arbitrary free placement.

---

# 19. PULSE EXECUTION

When player presses PULSE:

1. manipulation controls hide/lock
2. chamber ambience shifts
3. emitter membrane compresses
4. launch transient plays
5. visible pulse emerges
6. current acoustic object reacts
7. pulse redirects/splits/delays
8. receivers respond
9. success conditions evaluate
10. input returns appropriately

Execution must be understandable.

Do not make it excessively fast.

Do not make it painfully slow.

---

# 20. LEVEL 0 - SOUND CHECK

Dedicated tutorial.

Display:

# SOUND CHECK

Teach:

1. identify emitter
2. identify receiver
3. select reflector
4. rotate reflector
5. understand directional effect
6. press PULSE
7. watch propagation
8. reach receiver

Use actual interaction.

Do NOT use a slideshow.

---

# 21. LEVEL 0 FLOW

Step 1:

Emitter subtly pulses.

Instruction:

`This is the source.`

Step 2:

Receiver highlights.

Instruction:

`Send the pulse here.`

Step 3:

Reflector highlights.

Instruction:

`Select the reflector.`

Step 4:

Show rotation arc.

Instruction:

`Turn it toward the receiver.`

Step 5:

PULSE actuator activates.

Instruction:

`Send a pulse.`

Step 6:

Successful path executes.

Display:

# SIGNAL LOCKED

Then:

# ENTER CHAMBER 01

Tutorial completion saves.

Level 0 receives no standard performance rating.

Replay from Sound Guide.

---

# 22. LEVEL ORGANIZATION

Create three groups.

# SERIES A - REFLECTION

Levels 1-4.

# SERIES B - RESONANCE

Levels 5-8.

# SERIES C - PHASE

Levels 9-12.

---

# 23. LEVEL 1 - FIRST ECHO

Mechanics:

- one emitter
- one reflector
- one receiver

Simple independent puzzle.

---

# 24. LEVEL 2 - CORNER TONE

Use:

- two reflectors
- multiple directional changes

Introduce deeper spatial reasoning.

---

# 25. LEVEL 3 - SILENT WALL

Introduce:

# ABSORBER

One unwanted pulse path must be stopped.

---

# 26. LEVEL 4 - ECHO PATH

Combine:

- multiple reflectors
- movable socket
- limited adjustments

First mastery puzzle.

---

# 27. LEVEL 5 - SPLIT SIGNAL

Introduce:

# SPLITTER

One emitter must activate two receivers.

---

# 28. LEVEL 6 - TWO VOICES

Combine:

- splitter
- reflector
- absorber
- forbidden receiver

Player must create two valid branches without triggering forbidden target.

---

# 29. LEVEL 7 - LATE ECHO

Introduce:

# DELAY PLATE

Receiver A should activate first.

Receiver B should activate one beat later.

Timing visualization must be clear.

---

# 30. LEVEL 8 - HARMONIC GATE

Introduce:

# RESONATOR

Require two pulses in a clearly defined relationship.

Avoid complicated music theory.

This is logic, not a music exam.

---

# 31. LEVEL 9 - CROSSING ECHOES

Introduce:

- two emitters
- multiple paths

Paths may visually cross without interfering unless explicitly designed otherwise.

Make rule clear.

---

# 32. LEVEL 10 - DOUBLE BEAT

Use:

- splitter
- delay
- resonator

Require a simple pulse timing pattern.

---

# 33. LEVEL 11 - RESONANCE NETWORK

Advanced combination:

- multiple receivers
- reflector
- splitter
- absorber
- delay

Still keep layout readable.

---

# 34. LEVEL 12 - FINAL RESONANCE

Final mastery level.

Potential mechanics:

- two emitters
- reflectors
- splitter
- delay
- absorber
- forbidden receiver
- final resonator

Do not add complexity merely for spectacle.

The final puzzle should feel elegant.

Completion state:

# CHAMBER IN TUNE

---

# 35. LEVEL DATA

Use a data-driven ScriptableObject architecture.

Create something similar to:

`EchoLevelDefinition`

Store:

- Level ID
- title
- series
- chamber dimensions
- emitter definitions
- component definitions
- fixed components
- movable socket positions
- initial component states
- receivers
- forbidden receivers
- beat requirements
- resonator conditions
- optimal configuration
- adjustment par
- tutorial data
- visual variation

Do NOT implement Levels 0-12 via giant switch statements.

---

# 36. LEVEL VALIDATOR

Build an Editor validator.

Validate:

- IDs 00-12
- unique IDs
- emitters valid
- components valid
- socket references valid
- receiver references valid
- pulse simulation terminates
- solution exists
- optimal solution valid
- adjustment par valid
- tutorial flag
- Level 12 valid

Expected:

# 13 / 13 VALID

---

# 37. SCENE ARCHITECTURE

Use:

```text
Bootstrap
ResonanceHall
EchoChamber

```

Do NOT use previous-project scene names such as:

- MainWorkshop
- ControlRoom
- LightStudio
- Gameplay
- AutomationFloor
- ProjectionTable

---

# 38. RESONANCE HALL - MAIN MENU

This UI/environment must be completely distinct.

Create a:

# CIRCULAR ACOUSTIC ROOM

The menu is integrated into a sculptural resonance space.

Environment may include:

- curved acoustic walls
- suspended tuning ring
- radial floor grooves
- resonant membranes
- bronze acoustic elements
- fabric wall treatments
- central listening platform
- subtle floating dust/vibration particles

No desk.

No cards.

No stencil rack.

No industrial control machine.

---

# 39. MAIN MENU INTERACTION

Centerpiece:

# SUSPENDED RESONANCE RING

Player taps it.

It vibrates and releases a subtle ripple.

Primary action:

# START SESSION

Returning player:

# RESUME CHAMBER

Secondary interactions exist around the circular environment.

---

# 40. CHAMBER SELECT - RESONANCE DIAL

Do NOT create:

- rectangular level grid
- card list
- stencil rack
- job files
- carousel cards

Use:

# LARGE RADIAL RESONANCE DIAL

Twelve main chambers appear as radial frequency positions.

Concept:

```text
             01
       12          02

    11                03

 10                      04

    09                05

       08          06
             07

```

Player rotates the physical dial/ring.

Selected chamber moves to the primary focus position.

Show only necessary information:

- chamber number
- chamber title
- completion quality
- locked/unlocked state

Sound Check exists separately.

---

# 41. PERFORMANCE RATINGS

No stars.

Use:

# RESOLVED

Puzzle completed.

# CLEAN ECHO

Completed within target adjustments.

# PURE RESONANCE

Optimal configuration.

Represent these as acoustic waveform marks, not star icons.

---

# 42. ECHO CHAMBER SCENE

Suggested hierarchy:

```text
EchoChamber
├── Systems
│   ├── GameState
│   ├── LevelController
│   ├── AcousticGraph
│   ├── PulseSimulator
│   ├── SaveController
│   └── AudioController
│
├── CameraRig
│
├── Lighting
│
├── ChamberEnvironment
│   ├── AcousticFloor
│   ├── AcousticWalls
│   ├── ResonanceFrame
│   ├── ComponentSockets
│   ├── AmbientResonators
│   └── DecorativeAcousticElements
│
├── PuzzleBoard
│   ├── Emitters
│   ├── Reflectors
│   ├── Absorbers
│   ├── Splitters
│   ├── DelayPlates
│   ├── Receivers
│   └── Resonators
│
├── PulseVFX
│
├── Audio
│
└── UI
    └── SafeArea
        ├── ContextInfo
        ├── PulseControl
        ├── AdjustmentCounter
        ├── TutorialLayer
        ├── PauseLayer
        ├── ResultLayer
        └── DialogLayer

```

All major production elements must exist visibly in Edit Mode.

---

# 43. UNIQUE GAMEPLAY UI

Do NOT use:

- conventional full-width top HUD
- right-side permanent tool dock
- left-side status rail
- bottom card editor
- stencil tabs
- large rectangular panels

Echo Forge should have:

# CONTEXTUAL SPATIAL UI

The chamber remains visually dominant.

---

# 44. SELECTED COMPONENT UI

When player selects an object:

small information appears near the lower-left safe region:

Example:

```text
REFLECTOR
ANGLE 45°

```

Keep it small.

Relevant manipulation controls appear near the physical selected object where practical.

Do not display irrelevant controls.

---

# 45. PULSE CONTROL

The main interaction control must be unique.

Create a physical:

# RESONANCE ACTUATOR

Possible form:

- tuning fork mechanism
- circular strike pad
- membrane actuator

Position:

# LOWER CENTER CHAMBER FRAME

Label:

# PULSE

When pressed:

- mechanism depresses/strikes
- chamber responds
- sound launches

Do NOT use a generic button.

---

# 46. ADJUSTMENT COUNTER

Use a small engraved acoustic counter.

Example:

`TUNES 03`

Do not create a large HUD.

---

# 47. PAUSE UX

Unique concept:

# SOUND DAMPING

When paused:

soft acoustic damping panels slide partially inward from the chamber perimeter.

Ambience filters down.

Options:

- Resume
- Restart Chamber
- Sound Guide
- Exit Session

This must NOT resemble:

- clipboard
- glass sheet
- machine service panel
- paper menu

---

# 48. SETTINGS UX

Create:

# TUNING STRIP

A minimal acoustic hardware strip.

Controls:

### MUSIC / AMBIENCE

horizontal tactile fader.

### EFFECTS

second horizontal fader.

### HAPTICS

small physical toggle.

### RESET PROGRESS

protected recessed control.

### CREDITS

small maker plate.

Do not copy Punchcard Operator's rotary setup panel.

---

# 49. SOUND GUIDE

Use an acoustic reference wall.

Explain visually:

- Emitter
- Reflector
- Absorber
- Splitter
- Delay Plate
- Receiver
- Resonator

Keep text concise.

Use animated miniature examples where practical.

---

# 50. SUCCESS UX

Do not show:

- YOU WIN
- PROOF APPROVED
- PROGRAM ACCEPTED
- PATTERN LOCKED

Use:

# RESONANCE ACHIEVED

or for the final level:

# CHAMBER IN TUNE

Success sequence:

1. final receiver resonates
2. wave dissipates elegantly
3. chamber surfaces respond
4. harmonious tone resolves
5. rating appears

Show:

- Resolved
- Clean Echo
- Pure Resonance

Actions:

- Next Chamber
- Retune
- Resonance Dial

---

# 51. UNSUCCESSFUL PULSE

Do not show Game Over.

Use contextual state:

# OUT OF PHASE

or:

# SIGNAL LOST

Possible causes:

- Receiver Missed
- Forbidden Receiver Triggered
- Timing Incorrect
- Resonance Incomplete

Allow:

# RETUNE

No unnecessary restart screen after every failed attempt.

---

# 52. FINAL GAME COMPLETION

After Level 12:

# CHAMBER IN TUNE

The Resonance Hall transforms subtly:

- all chamber nodes illuminate
- suspended ring resonates
- final harmonic plays
- radial dial completes
- subtle achievement emblem appears

Allow:

- Review Chambers
- Return to Resonance Hall

---

# 53. UI VISUAL LANGUAGE

Echo Forge UI must feel:

- sculptural
- acoustic
- restrained
- premium
- dark
- warm
- tactile
- spatial
- mature

Avoid:

- generic sci-fi neon
- cyan holograms everywhere
- hypercasual color blocks
- flat web-dashboard panels
- game-template rounded cards
- giant rectangular buttons
- excessive glow

---

# 54. COLOR DIRECTION

Use an original palette such as:

- midnight navy
- graphite charcoal
- warm bronze
- matte black
- soft ivory
- restrained pale violet wave energy
- muted amber receiver glow
- low-saturation crimson for invalid resonance

Do not copy Stencil Relay's light/glass palette.

---

# 55. MATERIALS

Potential materials:

- matte acoustic foam
- woven fabric panels
- dark ceramic
- brushed bronze
- rubber
- painted metal
- soft translucent membrane
- polished resonator rings

Optimize materials for mobile.

---

# 56. LIGHTING

Use controlled atmospheric lighting.

Requirements:

- puzzle components readable
- dark chamber not muddy
- selected objects clear
- wave visualization visible
- receiver state clear
- UI readable

Prefer efficient baked/mixed/simple lighting where appropriate.

Avoid many expensive realtime lights.

---

# 57. VFX

Required VFX:

- emitter membrane pulse
- visible wave packet
- reflector response
- absorber compression
- splitter branching
- delay plate charging
- receiver resonance
- forbidden receiver warning
- final harmonic resolution

Optimize aggressively for mobile.

---

# 58. CAMERA

Fixed 2.5D camera.

No free orbit.

Use restrained camera feedback for:

- emitter launch
- strong resonance
- final completion

Do not shake camera on every movement.

---

# 59. ORIGINAL AUDIO IS A PRIMARY FEATURE

Echo Forge must have a unique sound identity.

Do NOT add generic downloaded UI clicks and call audio complete.

Prefer:

# LOCALLY GENERATED ORIGINAL AUDIO

Create sounds for:

- emitter pulse
- reflector contact/resonance
- absorber impact
- splitter branch
- Delay Plate charge
- Delay Plate release
- receiver activation
- forbidden receiver
- resonator
- component pickup
- component socket
- component rotation
- Pulse actuator
- radial dial
- pause damping
- settings
- success
- failure
- final resonance

---

# 60. MUSICAL DESIGN

Do not require a conventional looping game soundtrack.

Preferred approach:

# GENERATIVE-LIKE OFFLINE AMBIENT LAYER

using pre-generated local assets.

For example:

- low chamber hum
- soft resonance beds
- sparse tonal pulses
- low-volume acoustic textures

Gameplay pulses should remain distinguishable.

No runtime network or online generation.

---

# 61. AUDIO COHERENCE

Receivers can use related pitch families.

But:

- do not require musical training
- do not require perfect pitch
- never make critical logic distinguishable by sound alone

Always pair sound with visual feedback.

---

# 62. AUDIO PROVENANCE

Create:

`Assets/Documentation/AUDIO_PROVENANCE.md`

For every file:

```text
Filename:
Purpose:
Creation Method:
Source:
License:

```

For project-generated audio:

```text
Source: Project-generated original
External dependency: None

```

Do not scrape sounds from the web.

---

# 63. ASSET PROVENANCE

Create:

`Assets/Documentation/ASSET_PROVENANCE.md`

Track:

- models
- textures
- fonts
- icons
- sprites
- audio
- VFX source
- generated art

---

# 64. AUDIO MIXER

Create mixer groups:

- Master
- Ambience
- SFX
- UI

Expose appropriate settings.

Ensure persistent volume.

---

# 65. HAPTICS

Use restrained haptics for:

- socket snap
- rotation snap
- Pulse
- receiver resonance
- major failure
- Pure Resonance

Respect setting.

---

# 66. SAVE SYSTEM

Use versioned local storage.

Persist:

- tutorial completion
- unlocked chambers
- completion
- Resolved
- Clean Echo
- Pure Resonance
- best adjustment count
- settings
- haptics

Handle:

- missing save
- corrupt save
- incompatible save
- reset

---

# 67. PROGRESSION

First launch:

Level 0 Sound Check.

After completion:

Level 1 unlocks.

Then:

1 → 2 → 3 → ... → 12

Completed levels remain replayable.

---

# 68. RESET

Requires confirmation.

Use:

# RESET RESONANCE HISTORY?

`All completed chambers and resonance ratings will be cleared.`

Actions:

- Reset
- Cancel

---

# 69. GAME ICON - MUST ACTUALLY BE CREATED

The game icon is a mandatory production artifact.

Do NOT stop after writing an icon concept.

Do NOT leave a placeholder icon.

Do NOT leave the default Unity icon.

Do NOT merely document what the icon should look like.

The final icon must actually be created, saved, inspected, integrated, and validated.

---

# 70. ICON CONCEPT

Create an original icon specifically for Echo Forge.

Primary composition:

# A SINGLE BRONZE ACOUSTIC REFLECTOR / RESONANCE RING BEING STRUCK BY A VISIBLE CURVED SOUND PULSE

The pulse should bend around or reflect from the acoustic form toward a small resonant point.

Alternative composition only if objectively stronger:

A central resonator ring with two asymmetric visible sound-wave arcs converging into it.

The icon must communicate:

- sound
- resonance
- direction
- physical acoustic object

without needing text.

---

# 71. ICON MUST NOT RESEMBLE PREVIOUS GAME ICONS

Do not use:

- envelope
- printer
- paper
- punch card
- stencil sheet
- stacked transparent plates
- postal symbols
- generic app-controller symbol

Do not use headphones, microphone, or music-note clichés unless absolutely necessary.

Prefer the game's unique acoustic-chamber language.

---

# 72. ICON ART DIRECTION

Use:

- deep midnight/graphite background
- warm bronze central acoustic form
- pale violet/ivory visible pulse
- subtle amber resonance point

Composition:

- one dominant focal object
- strong silhouette
- controlled depth
- high contrast
- minimal clutter
- no tiny detail
- no title text
- no letters
- no fake App Store badge

The image must look deliberate at small size.

---

# 73. ICON GENERATION WORKFLOW

If an image-generation capability is available:

ACTUALLY USE IT to create an original production candidate.

Do not merely write an image-generation prompt and stop.

Generate several purposeful internal candidates if necessary.

Select the strongest one based on:

- uniqueness
- legibility
- silhouette
- relationship to Echo Forge
- small-size readability

If direct image-generation tooling is unavailable:

Create original artwork locally using available vector/raster/procedural tools.

Do NOT download another game's icon.

Do NOT use random stock artwork.

The final icon must still be an actual finished image file.

---

# 74. ICON MASTER

Create at least:

# 1024 × 1024 px

master artwork suitable for iOS/iPadOS app-icon use.

Keep the composition square.

Do not manually bake a rounded-rectangle mask into the core artwork.

The operating system applies final icon shape/masking.

For a conventional raster fallback, use a complete intentional background rather than transparent empty edges.

If the available Apple/Xcode workflow supports modern layered Icon Composer artwork and it can be integrated reliably:

also prepare clean logical layers such as:

```text
01_Background
02_Resonator
03_Pulse
04_Highlight

```

Do not sacrifice build reliability merely to force a layered workflow.

A high-quality correctly integrated 1024×1024 source remains mandatory.

---

# 75. ICON VALIDATION

Inspect the icon at approximately:

- 1024 px
- 180 px
- 120 px
- 60 px
- very small preview

At small sizes verify:

- central form still recognizable
- pulse still readable
- no muddy details
- no illegible thin lines
- no accidental letters
- no confusing visual resemblance to previous icons

If it fails:

regenerate/redesign it.

Do not accept first output automatically.

---

# 76. ICON INTEGRATION

Integrate the chosen icon into Unity mobile Player Settings.

Verify it survives iOS export.

Where applicable inspect the resulting Xcode app icon configuration / asset catalog.

The shipped build must NOT contain:

- Unity default icon
- temporary icon
- unrelated icon
- previous game's icon

Document icon origin and integration in:

`ASSET_PROVENANCE.md`

---

# 77. ICON QA

Add to final QA:

- correct game icon shown in Unity
- correct icon present in exported iOS project
- icon is Echo Forge specific
- 1024 master exists
- icon looks readable at small size
- no placeholder remains
- no previous-project visual elements reused

---

# 78. BUNDLE IDENTIFIER

If no legitimate identifier is already established, use:

`com.rehmanmughal.echoforge`

Product Name:

`Echo Forge`

Version:

`1.0.0`

Build:

`1`

Do not overwrite a valid intentional identifier without reason.

---

# 79. MOBILE CONFIGURATION

Configure:

- landscape orientation
- IL2CPP
- ARM64 for iOS
- app icon
- safe area
- target frame rate
- scene list
- appropriate stripping
- graphics settings
- production build configuration

Avoid unnecessary permissions.

---

# 80. NO SENSITIVE PERMISSIONS

Do not request:

- camera
- microphone
- contacts
- location
- photos
- Bluetooth
- ATT
- tracking

Despite being a sound-themed game:

# DO NOT REQUEST MICROPHONE ACCESS.

Game audio is simulated/generated internally.

---

# 81. RESPONSIVE UI

Test actual rendered UI on representative:

- 16:9 phone
- modern wide iPhone
- extra-wide phone
- standard iPad
- iPad Pro-like ratio

Check:

- chamber visibility
- PULSE actuator
- contextual UI
- resonance dial
- pause
- settings
- results
- touch targets
- text

---

# 82. SAFE AREA

Critical UI must avoid:

- notch
- Dynamic Island
- rounded corners
- home indicator

---

# 83. TYPOGRAPHY

Use TextMeshPro.

Choose legally safe typography.

Direction:

- restrained architectural/acoustic display font
- clear neutral body typeface

Maximum approximately two primary typefaces.

Do NOT reuse exact font combinations from previous games.

Audit all TMP objects for:

- alignment
- margins
- clipping
- wrapping
- hierarchy
- contrast
- centering

---

# 84. NO RUNTIME-GENERATED PRIMARY UI

Before final completion search code for suspicious patterns such as:

```csharp
new GameObject(...)
AddComponent<Button>()
AddComponent<Image>()
AddComponent<TextMeshProUGUI>()

```

Dynamic pulse/gameplay objects are allowed.

Primary production UI is not.

Document audit result.

---

# 85. AUTOMATED TESTING

Create tests for:

- reflection
- absorption
- splitting
- delay
- receivers
- forbidden receivers
- resonator timing
- multi-emitter simulation
- loop protection
- level success
- failure
- adjustment counting
- ratings
- local save
- progression

---

# 86. LEVEL QA

Manually test Level 0 and Levels 1-12.

For every main level verify:

- loads correctly
- intended components
- initial state
- rotation
- movement
- pulse simulation
- correct solution
- incorrect solution
- success
- failure
- retry
- pause
- save
- rating
- unlock
- replay

Expected:

# 13 / 13 MANUAL PASS

Create:

`Assets/Documentation/LEVEL_QA.md`

---

# 87. UI VISUAL QA

Inspect actual Game View.

Screens/states:

- Resonance Hall
- Resonance Dial
- Sound Check
- Echo Chamber
- selected Reflector
- selected other components
- Pulse execution
- Pause
- Settings
- Sound Guide
- success
- failure
- reset
- final completion

Do not mark PASS based only on Inspector hierarchy.

---

# 88. PERFORMANCE

Target smooth mobile gameplay.

Avoid:

- excessive particles
- excessive transparent overdraw
- expensive waveform geometry rebuilt every frame
- uncontrolled audio sources
- excessive realtime lights
- per-frame allocations
- repeated object searches
- excessive physics

Profile advanced Levels 11-12.

---

# 89. OFFLINE AUDIT

Before completion search for:

- UnityWebRequest
- HttpClient
- sockets
- networking packages
- Firebase
- ads
- analytics
- telemetry
- tracking
- remote config
- cloud
- web views
- external APIs

Expected:

# PASS - NO RUNTIME NETWORK FUNCTIONALITY

Create:

`Assets/Documentation/OFFLINE_AUDIT.md`

---

# 90. PACKAGE AUDIT

Inspect project packages.

Remove unnecessary runtime packages.

Document:

`Assets/Documentation/PACKAGE_AUDIT.md`

---

# 91. APP ORIGINALITY / QUALITY AUDIT

Before final completion explicitly evaluate:

## Core mechanic

Is manipulating an acoustic chamber genuinely central?

## Visual identity

Does Echo Forge look recognizable without displaying its title?

## Previous projects

Does it clearly differ from:

- Misprint Department
- Punchcard Operator
- Stencil Relay?

## UI

Does the radial Resonance Hall/Dial feel independently authored?

## Gameplay

Does the visible pulse look acoustic rather than like a laser?

## Content

Are all 12 levels meaningfully distinct?

## Audio

Does the game have an original coherent sound identity?

## Icon

Was the actual original icon generated and integrated?

## Completeness

Are there any unfinished screens or hidden/debug features?

Fix weaknesses before finalizing.

---

# 92. DOCUMENTATION

Maintain:

```text
Assets/Documentation/
├── ARCHITECTURE.md
├── GAME_DESIGN.md
├── LEVEL_DESIGN.md
├── LEVEL_QA.md
├── UI_UX_SYSTEM.md
├── UI_QA.md
├── AUDIO_PROVENANCE.md
├── ASSET_PROVENANCE.md
├── OFFLINE_AUDIT.md
├── PACKAGE_AUDIT.md
├── MOBILE_RELEASE_CHECKLIST.md
└── FINAL_AUDIT.md

```

---

# 93. UI\_UX\_SYSTEM.md

Document:

- Resonance Hall
- Resonance Dial
- Echo Chamber layout
- contextual component UI
- PULSE actuator
- Sound Damping pause
- Tuning Strip
- Sound Guide
- ratings
- typography
- palette
- materials
- responsive rules
- safe area
- animations

Explicitly document how Echo Forge is visually different from the three previous game designs.

---

# 94. LEVEL\_DESIGN.md

Document all stages 00-12.

For each:

- title
- teaching goal
- emitters
- acoustic components
- receiver rules
- timing
- starting configuration
- adjustment par
- intended solution summary
- special mechanics

---

# 95. GIT

If Git already exists:

make frequent meaningful LOCAL commits.

Examples:

```text
bootstrap: establish Echo Forge persistent project memory

feat: implement deterministic pulse simulator

feat: add acoustic component interactions

ui: build Resonance Hall and radial chamber dial

audio: integrate original Echo Forge acoustic sound set

art: integrate final Echo Forge application icon

qa: validate all thirteen chambers

```

Do NOT push remotely.

Do NOT create a new Git repository unless explicitly authorized.

---

# 96. DEVELOPMENT LOOP

Repeat:

# READ MEMORY

# INSPECT

# IMPLEMENT

# LOG MCP CHANGES

# COMPILE

# TEST

# PLAY

# VISUALLY INSPECT

# LISTEN

# FIX

# RETEST

# DOCUMENT

# COMMIT WHEN APPROPRIATE

# CONTINUE

Audio work must include actual listening/testing.

UI work must include actual Game View inspection.

---

# 97. VERTICAL SLICE FIRST

Do NOT immediately mass-produce Levels 1-12.

First complete:

# Level 0 + Level 1 + Level 2

Production flow:

```text
Launch
→ Sound Check
→ Resonance Hall
→ Resonance Dial
→ Chamber 01
→ Select Reflector
→ Tune
→ Pulse
→ Success/Out of Phase
→ Save
→ Chamber 02
→ Return to Resonance Hall

```

This vertical slice must include:

- production UI
- production environment
- wave visualization
- real audio
- real settings foundation
- save
- responsive layout

Then perform an originality review.

Fix the foundation.

Only then produce Levels 3-12.

---

# 98. COMPLETION GATE

## FOUNDATION

-  MCP verified
-  AI\_RULES
-  PROJECT\_CONTEXT
-  SUMMARY\_BANK
-  master prompt saved
-  documentation structure

## GAMEPLAY

-  emitter
-  reflector
-  absorber
-  splitter
-  Delay Plate
-  receiver
-  resonator
-  multiple emitters
-  deterministic simulation
-  loop protection
-  visible acoustic pulse
-  timing
-  success/failure

## CONTENT

-  Level 0
-  Levels 1-12
-  13/13 validation
-  all main levels solvable
-  13/13 manual QA
-  Level 12 final puzzle
-  final Chamber In Tune state

## UI/UX

-  Resonance Hall polished
-  Resonance Dial polished
-  Echo Chamber polished
-  contextual controls polished
-  PULSE actuator polished
-  Sound Damping pause polished
-  Tuning Strip settings polished
-  Sound Guide polished
-  success polished
-  failure feedback polished
-  final state polished
-  no dead controls
-  no placeholders
-  responsive phone
-  responsive tablet
-  safe areas
-  typography
-  no runtime-generated main UI

## ORIGINALITY

-  independent gameplay identity
-  independent UI
-  unique radial navigation
-  unique environment
-  unique terminology
-  unique ratings
-  unique audio
-  unique icon
-  no previous-project UI reuse

## AUDIO

-  complete original SFX
-  coherent receiver/resonator sounds
-  ambience
-  mixer
-  settings
-  haptics
-  provenance
-  no unlicensed audio

## ICON

-  actual icon image generated
-  1024×1024 master exists
-  icon inspected at small sizes
-  icon is recognizably Echo Forge
-  no placeholder
-  no previous icon reused
-  icon integrated in Unity
-  icon verified in iOS export

## SAVE

-  tutorial persists
-  progression persists
-  ratings persist
-  settings persist
-  reset works
-  corruption handled

## OFFLINE

-  no network
-  no ads
-  no analytics
-  no telemetry
-  no tracking
-  no cloud
-  no remote config
-  no login
-  no third-party runtime SDK

## MOBILE

-  product name
-  bundle identifier
-  landscape
-  IL2CPP
-  ARM64
-  safe area
-  icon
-  no unnecessary permissions
-  production build/export

## QA

-  automated tests
-  validator
-  save tests
-  manual level QA
-  UI QA
-  audio QA
-  performance
-  Console clean
-  build succeeds

---

# 99. FINAL\_AUDIT.md

Produce:

```text
ECHO FORGE FINAL AUDIT

Core Acoustic Simulation: PASS/FAIL
Visible Pulse System: PASS/FAIL
Reflectors: PASS/FAIL
Absorbers: PASS/FAIL
Splitters: PASS/FAIL
Delay System: PASS/FAIL
Resonators: PASS/FAIL

Level 0 Tutorial: PASS/FAIL
12 Main Levels: PASS/FAIL
13/13 Validation: PASS/FAIL
13/13 Manual QA: PASS/FAIL
Final Level 12: PASS/FAIL

Resonance Hall UI: PASS/FAIL
Resonance Dial UI: PASS/FAIL
Echo Chamber UI: PASS/FAIL
Responsive UI: PASS/FAIL
Safe Area: PASS/FAIL
UI Originality: PASS/FAIL

Environment: PASS/FAIL
VFX: PASS/FAIL
Animations: PASS/FAIL

Original Audio: PASS/FAIL
Audio QA: PASS/FAIL
Audio Provenance: PASS/FAIL

Game Icon Generated: PASS/FAIL
1024 Master Icon: PASS/FAIL
Small-Size Icon QA: PASS/FAIL
Unity Icon Integration: PASS/FAIL
iOS Export Icon Verification: PASS/FAIL

Save System: PASS/FAIL
Offline Audit: PASS/FAIL
Third-Party SDK Audit: PASS/FAIL
Package Audit: PASS/FAIL

Mobile Settings: PASS/FAIL
Performance: PASS/FAIL
Automated Tests: PASS/FAIL
Console: PASS/FAIL
Build/Export: PASS/FAIL
Documentation: PASS/FAIL

Known Release-Blocking Bugs:
NONE / list

```

Any required FAIL means:

# PROJECT STATUS = IN\_PROGRESS

---

# 100. EXTERNAL BLOCKERS

Only stop for a true external blocker such as:

- unavailable Apple signing credentials
- inaccessible developer account
- unavoidable host OS/tool limitation

These are NOT external blockers:

- broken UI
- missing icon
- weak icon
- bad audio
- missing levels
- compiler errors
- failing tests
- broken wave effects
- poor performance
- clipping
- broken save
- MCP problem that can be investigated

Fix those.

---

# 101. DEFINITION OF COMPLETE

Echo Forge is COMPLETE only when a player can:

- install the game
- launch it
- complete Sound Check
- enter Resonance Hall
- select chambers through the unique radial Resonance Dial
- play all 12 main levels
- rotate/move acoustic components
- release visible sound pulses
- understand reflections and timing
- use absorbers, splitters and delays
- solve resonator puzzles
- complete Level 12
- earn performance ratings
- replay levels
- quit and retain progress
- use functional settings
- experience polished original audio
- see a professionally generated Echo Forge-specific icon
- play without internet
- complete the final Chamber In Tune state

and every mandatory QA/build/documentation requirement passes.

---

# 102. BEGIN DEVELOPMENT NOW

Start with:

1. inspect project
2. verify Echo Forge identity
3. inspect Unity version
4. verify MCP
5. inspect packages
6. create AI\_RULES.md
7. create PROJECT\_CONTEXT.md
8. create SUMMARY\_BANK.md
9. save this master prompt
10. establish folder architecture
11. implement deterministic acoustic simulation
12. implement Reflector
13. implement Absorber
14. implement Splitter
15. implement Delay Plate
16. implement Receiver/Resonator
17. create Bootstrap
18. create ResonanceHall
19. create EchoChamber
20. create Level 0
21. create Levels 1-2
22. build polished vertical slice
23. create original audio foundation
24. perform UI/originality audit
25. fix foundation problems
26. create Levels 3-12
27. complete environment
28. complete VFX/animation
29. complete audio
30. ACTUALLY GENERATE the final production game icon
31. inspect and refine icon
32. integrate icon
33. configure save/settings
34. configure iOS/mobile settings
35. validate all 13 stages
36. manually QA all 13 stages
37. perform UI/audio/performance QA
38. perform offline/package audits
39. build/export production project
40. verify icon in export
41. update final documentation
42. continue fixing until every completion gate is PASS

Do not respond with a development plan only.

Do not merely describe how Echo Forge could be made.

Use Unity MCP and available development capabilities to actually build it.

# BUILD ECHO FORGE FROM SCRATCH TO RELEASE-QUALITY COMPLETION.