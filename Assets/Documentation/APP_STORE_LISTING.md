# App Store Connect Listing, Resonance Chamber

Prepared for App Store Connect submission. Primary localization is English (U.S.),
`en-US`. Every line of copy below is original, written for this app. No text in
this file uses an em dash or an en dash. Only the plain hyphen is used, and only
where it reads naturally.

Last refreshed: September 8, 2026.

---

## Apple compliance checklist (App Review Guidelines and App Store Connect help)

* Description contains no price, no sale, no promotion, and no "download now" or
  "get it today" filler.
* No mention of other platforms, other stores, Android, Google Play, Steam, web,
  or any competitor or trademarked product.
* No claims that cannot be verified in the build (no "best", no "award winning").
* Keywords are comma separated with no spaces, under 100 characters, no competitor
  names, no trademarks, no repetition of words already in the app name.
* Subtitle is under 30 characters, describes the app, and is not a keyword stuff.
* Screenshots and the app preview are captured from the real build, at the exact
  store resolutions, with no added device frames and no invented marketing text.
* Age rating answers are truthful. Expected result is 4+.
* App Privacy answers are truthful. Expected selection is Data Not Collected.
* Support URL and Privacy Policy URL are required and must resolve before the app
  can be submitted.
* Landscape only orientation is declared and is intentional for the puzzle layout.

---

## App name decision (kept from September 7, 2026)

The working title "Echo Forge" was set aside for the store after a public App
Store check.

| Candidate | Public App Store collisions | Verdict |
| --- | --- | --- |
| Echo Forge | "EchoForge: Voice Changer", "Arrow Echo Forge", the "Echo Forge Gaming" publisher, an "Echo Forge" Steam publisher. Apple's name check ignores case and spacing, so "Echo Forge" reads as the existing "EchoForge". | High confusion and rejection risk |
| Echo Forge: Acoustic Puzzles | Still leads with "Echo Forge", so the same collision applies, and "Echo" is a crowded prefix in this genre. | Still risky |
| Resonance Chamber | No App Store app is titled "Resonance Chamber". "Resonance" shows up in unrelated apps, never as this two word phrase, and there is no same name publisher. | Chosen |

"Resonance Chamber" is the most distinct of the three, it steps around the whole
"Echo Forge" and "EchoForge" family, it avoids the saturated "Echo" prefix, and it
is literally the thing you do in the game: you tune a resonance chamber. "Echo
Forge" survives only as the internal codename for C# namespaces, editor menus,
asset filenames, and the source repository.

Confirm the name is free in App Store Connect (My Apps, the plus button, New App)
before you finalize anything else.

---

## Identity

* App name (max 30): **Resonance Chamber** (17 characters)
* Subtitle (max 30): see the options below
* Company or developer name: **Resonance Chamber Co**
* Bundle ID: **com.resonancechamberco.resonancechamber**
  (set September 7, 2026. It becomes permanent on first submission.
  `com.resonancechamber.game` is a shorter alternative if you prefer it, but
  decide before you submit.)
* SKU: **RESCHAMBER-1000**
* Version: **1.0.0**
* Copyright: **2026 Resonance Chamber Co**
* Primary language: **English (U.S.)**
* Home screen and product name: **Resonance Chamber** (Unity `productName`)

---

## Subtitle (max 30 characters), pick one

| Option | Characters | Angle |
| --- | --- | --- |
| Acoustic reflection puzzles | 27 | Plain genre label, best for search |
| Tune the room, not the hero | 27 | The core hook, no character to control |
| One pulse, every listener | 25 | The goal in five words |
| Route sound through the room | 28 | Action plus place |
| A calm puzzle of sound | 22 | Mood first, good for the "relaxing" crowd |

Recommendation: **Acoustic reflection puzzles** for the search value on the first
day, or **Tune the room, not the hero** if you would rather lead with the idea
that sets the game apart. The subtitle can be changed with any future version, so
starting with the search friendly one is low risk.

---

## Promotional text (max 170 characters), pick one

Promotional text sits above the description and can be updated any time without a
new build.

1. You never move a character. You shape a room, release one pulse of sound, and
   watch it bend off your reflectors to reach every listener in time. Calm and
   fully offline. (168)
2. Set the reflectors, splitters, and delay plates, then release a sound pulse
   and follow the wavefront. Thirteen chambers of quiet acoustic logic, no ads,
   no internet. (165)
3. A slow puzzle about sound and space. Route a single pulse through a room you
   build, at or under par for a Clean Echo, perfectly placed for a Pure
   Resonance. (156)

Recommendation: **Option 1**. It states the one thing a browsing player needs to
understand about the game in the first sentence.

---

## Keywords (max 100 characters, comma separated, no spaces), pick one set

Apple already indexes the words in the app name, so "resonance" and "chamber" are
deliberately left out of every set. "echo" is now free to use as a keyword since
it is no longer in the name.

* Set A, mechanic first (93 chars):
  `puzzle,logic,sound,acoustic,echo,reflection,mirror,wave,pulse,brain teaser,offline,minimalist`
* Set B, mood and audience first (86 chars):
  `brain,thinky,logic puzzle,sound,acoustics,routing,relaxing,zen,strategy,offline,no ads`
* Set C, short tail heavy (84 chars):
  `puzzle,logic,sound,echo,acoustic,mirror,reflect,brain,calm,offline,tune,zen,strategy`

Recommendation: **Set A**. It matches the exact phrases a player types for this
genre, it reclaims "echo" now that the name no longer carries it, and it still
leaves headroom under the 100 character limit for a later tweak. Set B is the
right pick if the store screenshots and preview lean hard on the calm, low
pressure framing.

Selected draft: subtitle **Acoustic reflection puzzles**, keywords **Set A**,
promotional text **Option 1**. The alternatives above stay on the table.

---

## Category

App Store Connect asks for one primary category and one optional secondary
category. For a game the top level category is Games, and then you choose
subcategories.

* Primary category: **Games**
* Primary subcategory: **Puzzle**
* Secondary subcategory: **Strategy**

Reasoning: minute to minute, the player is solving a self contained logic puzzle,
so Puzzle is the honest primary and it is where players who want this kind of
game look first. Every chamber also rewards planning the whole route before you
commit a single move, and the later chambers layer timing and resonance
constraints on top, so Strategy is a stronger secondary than Board. If you would
rather signal the spatial, grid on a table feel, Board is a reasonable
alternative secondary. Do not pick Simulation or Educational; neither matches
what the game is.

---

## Description (max 4000 characters)

Resonance Chamber is a calm puzzle game about sound and space. You never move a
character and you never race a clock. You shape a room, release a single pulse of
sound, and watch where it goes.

Every chamber has one emitter and at least one listening point. Between them sit
acoustic parts you can adjust. Turn a reflector and the pulse changes direction.
Slide it to a new socket and the whole path shifts. Add a splitter and one pulse
becomes two. When the room looks right, you press the pulse button and the
wavefront runs the exact route you built. Reach every receiver, satisfy whatever
timing or resonance rule the chamber asks for, and the room resolves.

Nothing here is random. The same layout always produces the same result, so every
attempt teaches you something you can carry into the next one. There is no reflex
test, no hidden information, and no luck. There is only the shape of the room and
the way sound travels through it.

THE PARTS YOU WORK WITH

Reflectors turn the pulse in even steps. Absorbers stop it dead. Splitters fork it
into two branches. Delay plates hold a pulse for a set number of beats and then
release it, so you can line up two arrivals on purpose. Receivers need to be
reached. Forbidden receivers have to stay silent. Resonators want two pulses in a
particular relationship, whether that is a count, a beat difference, or a matched
arrival.

HOW THE GAME IS LAID OUT

A Sound Check chamber teaches the controls with no rating pressure. After that,
twelve chambers run in three short series. The first series is pure reflection and
routing. The second adds splitters, forbidden receivers, delay plates, and
resonators. The third brings in a second emitter, crossing paths, and timing
patterns, and closes with a chamber that tunes the whole room at once.

Finishing a chamber unlocks the next one, and finished chambers stay open so you
can go back for a cleaner solution. Every turn or move counts as one tune. Solve
at or under par for a Clean Echo. Find the tightest layout the chamber allows for
a Pure Resonance. If a pulse misses, the room tells you what went wrong and you
keep working. There is no game over and no way to get permanently stuck.

BUILT TO LEAVE YOU ALONE

Resonance Chamber runs fully offline. It collects nothing, sends nothing, and
carries no analytics, no ads, no account, and no login. Your progress lives on
your device. The whole game is about acoustics, and it still asks for no
microphone, no camera, no location, and no tracking.

It is designed for landscape play on iPhone and iPad, with a quiet palette of deep
blue, bronze, and warm amber, and an original score written for this game.

(Body is roughly 2,600 characters, well within the 4,000 limit. Recount after any
edit.)

---

## What's New for version 1.0.0

First release. A Sound Check tutorial and twelve acoustic reflection chambers
across three series, three ratings to chase on every main chamber, local progress
saving, and a full settings panel. Fully offline.

---

## App Review notes

Resonance Chamber is a single player, offline puzzle game. There is no account, no
login, no paywall, and no configuration. The whole game can be reviewed on a fresh
install.

Getting through it:

1. Launch the app. It opens on the main menu, titled "Resonance Hall".
2. On a fresh install the large button reads START SESSION. Tap it to enter the
   Sound Check, a guided tutorial chamber that introduces tapping, selecting,
   rotating a part, and releasing the pulse. After the tutorial this same button
   reads RESUME CHAMBER.
3. In any chamber, tap a bronze component to select it. Tap a selected reflector
   again to rotate it. Tap a highlighted socket to move a movable part.
4. Tap the round PULSE button at the bottom center to run the simulation and watch
   the wavefront travel.
5. When every receiver is reached, the chamber resolves and the next one unlocks.
   Back on the menu, RESONANCE DIAL opens a selector to jump to any unlocked
   chamber, SOUND GUIDE explains every component, and TUNING opens settings,
   including a full progress reset.

Reviewer demo recording: a gameplay capture is provided at 2868 by 1320, the
landscape resolution of the iPhone 17 Pro Max. It begins at app launch, moves
through the main menu and each of its buttons, plays the Sound Check tutorial from
start to finish, and then solves several main chambers, so the review covers real
play and not just menus. If it is hosted, the full walkthrough URL is listed with
the submission, and the short App Preview on the listing is a trimmed section of
the same capture.

Networking and privacy: the app makes zero network connections. There is no
analytics, advertising, tracking, or third party SDK. Unity engine diagnostics and
analytics are disabled in the project settings. The correct App Privacy selection
is Data Not Collected.

Permissions: none are requested. The subject is acoustics, but the game never uses
the microphone or any other sensor.

Orientation: landscape only, by design, because every puzzle is laid out wide.

Codename: some engine identifiers and the source repository still use the internal
codename "Echo Forge". Everything the player sees, including the app name, the
icon, and every on screen title, reads "Resonance Chamber".

---

## App Privacy answers

* Data collection: **Data Not Collected.**
* Tracking: **No, this app does not track.**
* Third party partners: none.
* Privacy policy URL: required by App Store Connect. Host the text in
  `PRIVACY_POLICY.md` at a public URL and enter that URL. A GitHub Pages page or a
  simple page on a personal site is enough.

---

## Age rating questionnaire, expected answers

All questions about violence, sexual content, profanity, horror, gambling, and
mature themes: **None.**
Unrestricted web access: **No.**
User generated content: **No.**
Contests: **No.**
Expected result: **4+.**

---

## Required URLs

* Support URL: a public page with real contact information. Do not use the private
  source repository. Hosting is deferred by the user.
* Marketing URL: optional, can be left blank for 1.0.
* Privacy Policy URL: required. See `PRIVACY_POLICY.md`. Hosting is deferred by the
  user (September 7, 2026).

---

## Screenshots (done)

`StoreShots` (the `Echo Forge/QA` menu, run from Play Mode) renders five views at
the two mandatory landscape sizes into `Screenshots/AppStore/` (gitignored):

* iPhone 6.9 inch: **2868 by 1320**, 8-bit RGB PNG, no alpha. This size covers the
  iPhone 17 Pro Max and the whole 6.9 inch class, and App Store Connect scales it
  down for smaller devices.
* iPad 13 inch: **2752 by 2064**, same format.

The five views: the title menu, an early chamber, the pulse wavefront in flight,
the deep "Resonance Network" chamber, and the chamber select dial. Upload 3 to 10
per size and reorder as you like. Regenerate any time from a focused editor.

---

## App preview video (optional, recommended)

A store App Preview is separate from the reviewer demo recording. Rules that
matter:

* 15 to 30 seconds. One to three previews per device size.
* Footage the app actually produces. No pricing, no "download" language, no other
  platforms, no fingers or hands on screen, no capture tool overlay.
* The first frame becomes the poster frame, so it should be a real gameplay frame,
  not a black frame or a logo.
* H.264 or Apple ProRes 422 HQ, up to 30 fps, with the game's own audio.

Sizes for this app (landscape only):

* iPhone 6.9 inch (iPhone 17 Pro Max and the 16 Pro Max class): **2868 by 1320**.
* iPad 13 inch: **2752 by 2064**.

The planned reviewer recording (launch, every menu button, the full Sound Check,
then several chambers) is longer than 30 seconds, so it cannot be the App Preview
as is. Keep the full version as the reviewer demo, host it, and link it in the
review notes. Cut one 25 to 30 second highlight from the same capture for the
actual App Preview: a few seconds of the menu, one reflector rotation, one PULSE
with the wavefront traveling, and one chamber resolving.

Unity Recorder is installed in the project for this capture. Record at the exact
pixel size above, disable any Recorder watermark, and export without a device
frame.

---

## Metadata uniqueness

* The app name has no App Store match (checked September 7, 2026).
* Every subtitle, promotional text, keyword set, and the full description are
  written for this app and are not adapted from a template or another listing.
* The description avoids generic filler and states only what the build does.
* Keywords contain no competitor names and no trademarks.

---

## Official references checked

* https://developer.apple.com/help/app-store-connect/reference/app-information/app-information
* https://developer.apple.com/help/app-store-connect/reference/app-information/screenshot-specifications/
* https://developer.apple.com/help/app-store-connect/reference/manage-app-information/app-previews/
* https://developer.apple.com/app-store/review/guidelines/

Metadata is prepared locally, not submitted. App Store Connect assigns the final
age rating and Apple makes the final review decision.
