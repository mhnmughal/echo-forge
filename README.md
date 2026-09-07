# Echo Forge

Echo Forge is an offline 2.5D acoustic puzzle game. You never move a character.
Instead you tune a resonance chamber so that a single pulse of sound travels the
room, bends off the surfaces you set, and reaches every listening point in time.

Built with Unity 6 and the Universal Render Pipeline. Single player, fully
offline, landscape orientation, designed for phone and tablet.

## The idea

Every chamber holds an emitter that releases one visible pulse. Between the
emitter and the receivers sit acoustic components you can adjust. Rotate a
reflector, slide it to a new socket, place movable parts, and the pulse follows a
new path. Press PULSE to run the room and watch the wavefront travel. If it
reaches every receiver, and satisfies any timing or resonance rules, the chamber
resolves.

Nothing about the outcome is random. The same chamber layout always produces the
same result, so a puzzle that looks solved is solved.

## Core loop

Listen, select a component, tune it, pulse the room, watch, adjust, resolve.

Each rotate or move counts as one TUNE. Solving at or under par earns a Clean Echo.
Finding the optimal layout earns a Pure Resonance. Any valid solution earns a
Resolve. There is no game over. A failed pulse tells you why (a receiver was
missed, a forbidden receiver was hit, the timing was off, a resonance was
incomplete) and you keep tuning.

## Components

| Component | Behavior |
| --- | --- |
| Emitter | Releases one pulse in a fixed direction. Some chambers use more than one. |
| Reflector | Turns the pulse in 45 degree steps. Rotate it to route the sound. |
| Absorber | Stops a pulse. |
| Splitter | Forks one pulse into two. |
| Delay Plate | Holds a pulse for a set number of beats, then releases it. |
| Receiver | Needs a pulse to arrive. Forbidden receivers must stay silent. |
| Resonator | Needs two pulses in the right relationship (count, beat difference, or simultaneous arrival). |

## Content

Level 0 is a Sound Check that teaches the controls with no rating pressure.
Levels 1 through 12 run in three series:

* A Reflection: reflectors, spatial routing, the first absorber.
* B Resonance: splitters, forbidden receivers, delay plates, resonators.
* C Phase: two emitters, crossing paths, timing patterns, and the finale.

Completing a chamber unlocks the next one. Finished chambers stay open for replay
and better ratings. Clearing Level 12 brings the whole hall into tune.

## Technology

* Unity 6000.3.19f1, Universal Render Pipeline 17.3, linear color space.
* Deterministic discrete event simulation in pure C#. Unity physics, particles,
  and audio are presentation only and cannot change a puzzle result.
* Scriptable Object level data. No level id switch statements in gameplay code.
* All three scenes are authored at edit time by builder scripts and serialized
  into the scene files. The runtime never constructs primary UI.
* Versioned local JSON save with recovery for missing, corrupt, and newer files.
* Original audio, generated offline by C# synthesis into 16 bit WAV.
* Original app icon, rendered procedurally.
* One AudioMixer with Master, Ambience, SFX, and UI buses.

## Privacy and networking

Echo Forge collects nothing and sends nothing. There is no analytics, no ads, no
telemetry, no cloud save, no account, and no login. The save file stays on the
device. The app requests no camera, microphone, location, photo, Bluetooth, or
tracking permission. Unity engine diagnostics and analytics are disabled in the
project settings.

## Repository layout

```
Assets/EchoForge/Runtime      gameplay and runtime code
Assets/EchoForge/Editor        level validator, build tools, asset generators
Assets/EchoForge/Editor/Build  scene and asset builders (EchoBuild, EchoBuildScenes,
                               AudioGen, IconGen, PlayerConfig, EchoPolish)
Assets/EchoForge/Levels        Level_00 to Level_12 definition assets
Assets/EchoForge/Scenes        Bootstrap, ResonanceHall, EchoChamber
Assets/EchoForge/Tests         EditMode and PlayMode test assemblies
Assets/Documentation           design, QA, audit, and provenance documents
```

## Building

1. Open the project in Unity 6000.3.19f1.
2. Run the menu item `Echo Forge/Build/Everything` to regenerate assets and the
   three scenes.
3. Run `Echo Forge/Validate Levels`. The target is 13 of 13 valid.
4. Run the EditMode and PlayMode test suites. The target is 17 of 17 each.
5. Build for the target platform. The scene list is Bootstrap, ResonanceHall,
   EchoChamber.

## Verification status

* Level validation: 13 of 13 valid.
* Automated tests: 17 EditMode and 17 PlayMode passing, including every level
  solved through the real chamber scene.
* Windows build: clean, no errors or warnings, smoke tested.
* Offline audit: no runtime network functionality.
* iOS export, device install, human touch testing, and audio listening are not
  yet verified because they need a Mac, a device, a tester, or an audio output
  device.

## License

Original work. All design, code, audio, and art in this repository are
project generated for Echo Forge.
