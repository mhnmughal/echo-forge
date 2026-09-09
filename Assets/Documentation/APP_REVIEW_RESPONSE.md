# APP_REVIEW_RESPONSE - Resonance Chamber

Response to: **Guideline 2.1 - Information Needed - New App Submission**
(the standard information request Apple sends to developer accounts with a
limited review history). Date received: September 9, 2026.

This is not a bug rejection. Apple wants a screen recording plus written answers
to six questions. Reply in the App Store Connect Resolution Center with Part A,
and paste Part B into App Store Connect under App Information > App Review
Information > Notes so it is on file for future submissions.

US English. No em dash used.

---

## What you must do

1. **Record your screen on a physical iPhone or iPad** running the current iOS
   version. See "Screen recording instructions" below. This cannot be the Unity
   editor capture; Apple requires a real-device recording.
2. Attach that recording to the Resolution Center reply.
3. Paste **Part A** as the reply text.
4. Paste **Part B** into the App Review Information Notes field.
5. Make sure the build under review is the clean 12-package build, not the one
   that briefly had `com.unity.recorder` and `com.anklebreaker.unity-mcp`. If the
   build in review is the recorder build, upload a fresh build and set it as the
   build for this version before replying.

---

## Screen recording instructions

The app has **no** account registration, login, or account deletion (it has no
accounts), **no** user-generated content, and **no** paid content, in-app
purchases, or paid features. Every chamber is free. So the recording only needs
to show the normal gameplay flow.

Set up iOS screen recording: Settings > Control Center > add **Screen Recording**.
Then open Control Center and tap the record button. Record for 1 to 3 minutes:

1. Start on the Home Screen. Tap the Resonance Chamber icon to launch.
2. Let the title beat play; the main menu ("Resonance Hall") appears.
3. Tap **START SESSION**. Play the Sound Check tutorial to the end: tap the
   reflector to select it, tap it again to rotate it, tap **PULSE**, watch the
   pulse reach the receiver, tap through to the next chamber.
4. Play **Level 1** and **Level 2** the same way: select a component, rotate or
   move it, press PULSE, solve, continue.
5. Back on the menu, open **RESONANCE DIAL** (chamber selector) and **SOUND
   GUIDE** (component reference), then close them.
6. Optionally open **TUNING** to show the settings and the progress reset.

Keep the device on the current iOS version. No editing, captions, or music
needed. Export the video and attach it.

---

## Part A - Resolution Center reply

> Thank you for the review. Resonance Chamber is a single player, fully offline
> puzzle game. It has no account, no login, no user-generated content, no in-app
> purchases, no advertising, and no network connectivity. Answers to the six
> points follow, and a screen recording captured on a physical iPhone running the
> current iOS version is attached, beginning with app launch and showing the
> typical gameplay flow.
>
> **1. Screen recording.** Attached. It starts by launching the app from the Home
> Screen and shows: the main menu, the Sound Check tutorial played to completion,
> Levels 1 and 2 solved (select a component, rotate or move it, press PULSE, the
> pulse travels and reaches the receiver), and the Resonance Dial and Sound Guide
> menus. There are no account, login, account deletion, user-generated content,
> or paid content flows in the app, so none appear.
>
> **2. Purpose and target audience.** Resonance Chamber is an acoustic logic
> puzzle game. The player never controls a character. They shape a room by
> rotating and repositioning acoustic parts (reflectors, splitters, absorbers,
> delay plates), then release one visible pulse of sound and route it so it
> reaches every listening point and satisfies that chamber's timing or resonance
> rule. The value it provides is a calm, deterministic thinking game with no time
> pressure, no reflexes, no randomness, and no internet requirement: a solved
> puzzle stays solved. Target audience: puzzle and logic game players who enjoy
> spatial reasoning, ages 4 and up. The app is rated 4+ and contains no violence,
> profanity, or other objectionable content.
>
> **3. Setup and access.** No setup, no login, no account, no configuration, and
> no sample files are required. The full app is available offline on first
> launch. To reach the main features: launch the app, which opens on the main
> menu; tap START SESSION for the guided Sound Check tutorial; tap a bronze
> component to select it, tap a selected reflector again to rotate it, or tap a
> highlighted socket to move a movable part; tap the round PULSE button at the
> bottom center to run the simulation. Clearing the tutorial unlocks Level 1, and
> each solved chamber unlocks the next of the 13 total chambers. From the main
> menu, RESONANCE DIAL jumps to any unlocked chamber, SOUND GUIDE explains every
> component, and TUNING holds the audio and haptics settings and a full progress
> reset. No demo credentials are needed because there is no login.
>
> **4. External services, tools, or platforms.** None are used to deliver the
> app's functionality. There is no backend, no data provider, and no API. There
> is no authentication service (no accounts). There is no payment processor (no
> in-app purchases, no advertising). There are no AI services. There is no
> analytics, telemetry, crash reporting, or attribution SDK; Unity engine
> diagnostics and analytics are disabled in the project. The app makes zero
> network connections. All content (levels, audio, artwork, icon, text) is
> bundled in the app and was generated by us. The only third-party technology is
> the Unity game engine used to build the app, and the LiberationSans font that
> ships with Unity's UI package under the SIL Open Font License 1.1.
>
> **5. Regional differences.** None. The app behaves identically in every region.
> There are no regional feature flags, no geo-restricted or geo-varied content,
> and no regional pricing (the app is free with no in-app purchases). The
> interface is English (U.S.) only. The app does not use location services.
>
> **6. Regulated industry or protected third-party material.** Not applicable.
> Resonance Chamber is a puzzle game and is not part of any regulated industry.
> Every asset in the app is our original work: the level designs, the
> deterministic acoustic simulation, all sound effects and ambience (synthesized
> offline by our own code into WAV files), all artwork and the app icon (rendered
> procedurally by our own code), and all on-screen text. We use no licensed or
> third-party music, art, trademarks, or protected material. The only external
> asset is the open-licensed LiberationSans font bundled with the Unity engine.
> No documentation or credentials are required. Note: some internal engine
> identifiers and our source code repository use the working codename "Echo
> Forge"; everything the user sees, including the app name, icon, and every title,
> reads "Resonance Chamber."
>
> Please let us know if anything else is needed.

---

## Part B - App Review Information Notes (permanent, paste into App Store Connect)

> Resonance Chamber is a single player, fully offline acoustic puzzle game.
>
> No account, no login, no account deletion (there are no accounts). No
> user-generated content. No in-app purchases, no paid features, no advertising.
> No network connections of any kind. No analytics, telemetry, crash reporting,
> or attribution SDKs (Unity diagnostics and analytics are disabled in the
> project). App Privacy: Data Not Collected. Does not use App Tracking
> Transparency. Requests no permissions (no camera, microphone, location,
> Bluetooth, photos, or tracking), even though the theme is acoustics.
>
> External services used to deliver functionality: none. The only third-party
> technology is the Unity game engine and its bundled open-licensed LiberationSans
> font. All levels, audio, art, and the icon are original and were generated by
> our own code, bundled in the app.
>
> How to review: launch the app; it opens on the main menu. Tap START SESSION for
> the guided tutorial. Tap a bronze component to select it, tap a selected
> reflector again to rotate it, tap a highlighted socket to move a movable part,
> then tap the round PULSE button to run the simulation. Solving a chamber unlocks
> the next; 13 chambers total. RESONANCE DIAL is the chamber selector, SOUND GUIDE
> explains the components, TUNING has settings and a progress reset. No demo
> credentials are needed.
>
> The app functions identically in all regions. English (U.S.) only. Rated 4+, no
> objectionable content. Landscape orientation only, by design.
>
> Some internal engine identifiers and the source repository use the working
> codename "Echo Forge." Everything the user sees reads "Resonance Chamber."

---

## Also check before you resubmit

* **Guideline 2.3.3 (screenshots)**: your App Store screenshots must show the app
  in use, not just the menu or title. The chamber, pulse-wavefront, and
  network-chamber screenshots are fine. If the plain main-menu screenshot is in
  the set, either drop it or keep it only after the gameplay screenshots, never
  as the first one.
* **Physical-device test pass**: before replying, install the build on a real
  device (TestFlight is fine) and play the tutorial plus a few chambers, in
  landscape, checking the PULSE button, the dial, and the settings reset. Apple
  reviews on real hardware.
* **Privacy Policy URL and Support URL** must be live and reachable over HTTPS
  before the app can be approved. Deploy `docs/privacy.html` and
  `docs/support.html` and enter both URLs. See `docs/README.md`.
* **Build identity**: confirm the build selected for this version is the clean
  12-package build (no `com.unity.recorder`, no `com.anklebreaker.unity-mcp`).
