# App Store Connect Listing, Resonance Chamber

Prepared for App Store Connect submission. US English (en-US) is the primary
localization. Copy is authored for this app. No text below contains an em dash.

Apple guidance followed: no price or promotion in the description, no mention of
other platforms or storefronts, no competitor or trademark terms, no "download
now" filler, keywords comma separated with no spaces, screenshots and preview to
match the real build, accurate age rating, privacy answers truthful.

---

## App name decision (2026-09-07)

The working title "Echo Forge" was set aside for the store listing after a public
App Store check:

| Candidate | Public App Store collisions | Verdict |
| --- | --- | --- |
| Echo Forge | "EchoForge: Voice Changer" (com.lapnito.echoforge), "Arrow Echo Forge", the "Echo Forge Gaming" publisher, an "Echo Forge" Steam publisher. Apple's name check ignores case and spaces, so "Echo Forge" reads as the existing "EchoForge". | High rejection / confusion risk |
| Echo Forge: Acoustic Puzzles | Still leads with "Echo Forge", so the same collision applies. "Echo" is also a very crowded prefix in this genre (Echo Maze, Echo Resonance, Echo Resonator, Echo Chamber Games). | Still risky |
| **Resonance Chamber** | No app titled "Resonance Chamber" on the App Store. "Resonance" appears in unrelated apps (Bass Resonance, Schumann Resonance, a small itch.io "Resonance"), never as this two word phrase. No same name publisher. | **Chosen** |

"Resonance Chamber" is the most unique of the three, sidesteps the whole
"Echo Forge" / "EchoForge" family, avoids the saturated "Echo" prefix, and is
literally what the game is about: you tune a resonance chamber. "Echo Forge"
stays only as the internal codename (C# namespaces, editor menus, asset
filenames, the git repository).

Reserve the name in App Store Connect (My Apps, plus, New App) to confirm it is
free before finalizing.

---

## Identity

* **App name (max 30):** Resonance Chamber   (17 characters)
* **Company / developer name:** Resonance Chamber Co
* **Bundle ID:** com.resonancechamberco.resonancechamber
  (set 2026-09-07, supersedes the master prompt's placeholder
  `com.rehmanmughal.echoforge`. Not yet reserved with Apple; it becomes permanent
  on first submission. `com.resonancechamber.game` is a shorter alternative if
  preferred.)
* **SKU:** RESCHAMBER-1000
* **Version:** 1.0.0
* **Copyright:** 2026 Resonance Chamber Co
* **Primary language:** English (U.S.)
* **Home-screen / product name:** Resonance Chamber (Unity `productName`)

---

## Subtitle (max 30 characters), pick one

| Option | Characters | Angle |
| --- | --- | --- |
| Acoustic reflection puzzles | 27 | Plain genre label, strong for search |
| Tune the room, not the hero | 27 | The core hook, no character control |
| One pulse, every listener | 25 | The goal, stated plainly |
| Bend sound, solve the room | 26 | Action plus goal |
| A puzzle of sound and space | 27 | Mood forward |

Recommended: **Acoustic reflection puzzles** for search value, or
**Tune the room, not the hero** if you want the hook up front.

---

## Promotional text (max 170 characters), pick one

1. You never move a character. You tune a chamber until one pulse of sound bends
   off your reflectors and reaches every listener in time. Deterministic, calm,
   and fully offline. (167)
2. Set the reflectors, splitters, and delay plates, then release a visible sound
   pulse and watch the wavefront travel. Thirteen chambers of quiet acoustic
   logic. No ads, no internet. (169)
3. A quiet puzzle about sound and space. Route one pulse through a room you shape,
   under par for a Clean Echo, perfectly for a Pure Resonance. (140)

---

## Keywords (max 100 characters, comma separated, no spaces), pick one set

Words already in the app name ("resonance", "chamber") are indexed by Apple, so
they are left out of every set. "echo" and "forge" are now free to use as
keywords since they are not in the name.

* **Set A, mechanic first (98 chars):**
  `puzzle,logic,sound,acoustic,echo,reflection,mirror,wave,pulse,brain teaser,offline,minimalist`
* **Set B, audience first (95 chars):**
  `brain,logic puzzle,sound,acoustics,path,routing,relaxing,zen,thinky,strategy,offline,no ads`
* **Set C, short tail heavy (96 chars):**
  `puzzle,logic,sound,echo,acoustic,brain,mirror,reflect,offline,calm,strategy,tune,spatial`

Recommended: **Set A**. It covers the exact terms a player types for this genre
and reclaims "echo" now that it is out of the name.

Selected draft: subtitle **Acoustic reflection puzzles**, keyword **Set A**,
promotional text **Option 1**. Alternatives remain available above.

---

## Category

App Store Connect asks for a primary category and an optional secondary category.
For a game the category is Games, and you then choose subcategories.

* **Primary category:** Games
* **Primary subcategory:** Puzzle
* **Secondary subcategory:** Strategy

Reasoning: the moment to moment activity is solving a self contained logic puzzle,
so Puzzle is the honest primary. Every chamber also rewards planning a full route
before you commit, and later chambers layer timing and resonance rules, so
Strategy is the better secondary than Board. If you prefer a spatial framing,
Board is a reasonable alternative secondary.

---

## Description (max 4000 characters)

Resonance Chamber is a quiet puzzle game about sound and space. You never move a
character. You tune a room.

Every chamber holds an emitter that releases one visible pulse of sound. Between
that emitter and the listening points sit acoustic parts you can adjust. Rotate a
reflector and the pulse turns. Move it to a new socket and the path changes
again. A splitter turns one pulse into two. When the room looks right, you
release the pulse and watch the wavefront travel the path you built. If it
reaches every receiver, and satisfies the timing or resonance rules a chamber
asks for, the chamber resolves.

The simulation is fully deterministic. The same layout always gives the same
result, so you can learn from each attempt. There is no luck, no timing
execution, and no reflex test. There is only the shape of the room and the way
sound moves through it.

WHAT YOU WORK WITH

Reflectors turn the pulse in even steps. Absorbers stop it. Splitters fork it
into two branches. Delay plates hold a pulse for a set number of beats and then
release it, which lets you line up arrivals. Receivers need a pulse. Forbidden
receivers must stay silent. Resonators need two pulses in a specific
relationship, by count, beat difference, or simultaneous arrival.

HOW IT IS BUILT

A Sound Check chamber teaches the controls with no rating pressure. After that,
twelve main chambers run in three short series. The first set is pure reflection
and routing. The second adds splitters, forbidden receivers, delay plates, and
resonators. The third brings in a second emitter, crossing paths, and timing
patterns, and ends with a chamber that brings the whole room into tune.

Finishing a chamber unlocks the next one. Finished chambers stay open so you can
return and look for a cleaner solution. Every rotate or move counts as one tune.
Solve at or under par for a Clean Echo. Find the tightest possible layout for a
Pure Resonance.

There is no game over. If a pulse misses, the room tells you why, and you keep
tuning.

PRIVACY

Resonance Chamber is completely offline. It collects nothing and sends nothing.
There is no analytics, no advertising, no account, and no login. Your progress is
saved on your device. The app asks for no camera, microphone, location, photo, or
tracking permission, even though sound is the whole subject of the game.

Resonance Chamber is designed for landscape play on iPhone and iPad, with a
restrained palette of deep blue, bronze, and warm amber, and an original
soundscape generated for this game.

(Description body is roughly 2500 characters, within the 4000 limit. Verify after
any edit.)

---

## What's New for version 1.0.0

First release. Twelve main acoustic reflection chambers and a Sound Check
tutorial, three performance ratings per main chamber, local save, and a full
settings panel. Fully offline.

---

## App Review notes

Resonance Chamber is a single player offline puzzle game. No account, no login,
and no special configuration are required to review it.

How to play, in short:
1. Launch the app. It opens on the main menu.
2. Tap START SESSION to enter the first chamber, which is a guided Sound Check.
3. Tap a bronze component to select it, then tap the selected reflector again to
   rotate it, or tap a highlighted socket to move it.
4. Tap the round PULSE button at the bottom center to run the simulation.
5. When the pulse reaches the receiver, the chamber resolves and the next chamber
   unlocks. RESONANCE DIAL on the menu lets you jump between unlocked chambers.

Networking and privacy: the app makes no network connections. There is no
analytics, advertising, tracking, or third party SDK. Unity engine diagnostics
and analytics are disabled in the project. The recommended App Privacy answer is
Data Not Collected.

Permissions: none are requested. The game is about acoustics but it does not use
the microphone.

Orientation: landscape only, which is intentional for the puzzle layout.

Codename note: some engine identifiers and the source repository use the internal
codename "Echo Forge". The product, the icon, and every on screen title read
"Resonance Chamber".

---

## App Privacy answers

* Data collection: **Data Not Collected.**
* Tracking: **No, this app does not track.**
* Third party partners: none.
* Privacy policy URL: required by App Store Connect. Host the text in
  PRIVACY_POLICY.md at a public URL and enter that URL. Suggested location is a
  GitHub Pages or personal site page.

---

## Age rating questionnaire, expected answers

All violence, sexual content, profanity, horror, gambling, and mature theme
questions: **None.**
Unrestricted web access: **No.**
User generated content: **No.**
Contests: **No.**
Expected result: **4+.**

---

## Required URLs

* **Support URL:** a public support website with actual contact information. Do
  not use this private repository as the URL. Hosting is deferred by the user.
* **Marketing URL:** optional. Can be left blank for 1.0.
* **Privacy Policy URL:** required. See PRIVACY_POLICY.md.
  Hosting is deferred by the user (September 7, 2026).

---

## Asset checklist for submission

* App icon 1024 by 1024, no alpha, no rounded corners. Source is
  `Assets/EchoForge/Art/Icon/EchoForge_Icon_1024.png` (codename filename; the art
  is the Resonance Chamber icon).
* **Screenshots: DONE.** `StoreShots` (Echo Forge/QA menu, from Play Mode) renders
  five views at the two mandatory landscape sizes into `Screenshots/AppStore/`
  (gitignored): iPhone 6.9 in 2868x1320 and iPad 13 in 2752x2064, 8-bit RGB PNG,
  no alpha. Providing the largest sizes is enough; App Store Connect scales down.
  The five views: menu (title), an early chamber, the pulse wavefront in flight,
  the deep "Resonance Network" chamber, and the chamber-select dial. Upload 3 to
  10 per size; reorder as you like. Regenerate any time from a focused editor.
* Optional app preview video, landscape, 15 to 30 seconds. Not captured.
* Screenshots are captured from the real build with no added device frames and no
  marketing text. If you add caption bars later, keep claims accurate.

## Official references checked September 7, 2026

- https://developer.apple.com/help/app-store-connect/reference/app-information/platform-version-information
- https://developer.apple.com/help/app-store-connect/reference/app-information/screenshot-specifications/
- https://developer.apple.com/help/app-store-connect/reference/app-information/app-information

Metadata is prepared locally, not submitted. App Store Connect assigns the actual
age rating and Apple makes the final review decision.
