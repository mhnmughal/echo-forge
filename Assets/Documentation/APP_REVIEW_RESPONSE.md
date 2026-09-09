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

1. **Record your screen on a physical iPhone running iOS 26.6.1** (the current
   release). See "Screen recording instructions" below. This cannot be the Unity
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

Record on iPhone with iOS 26.6.1. No editing, captions, or music needed. Export
the video and attach it. If the file is large, trim only the head and tail, keep
it under about 500 MB, and use .mp4 or .mov.

---

## Part A - Resolution Center reply

Fits Apple's 4000 character limit (about 3,200 characters). Answers only what was
asked, no filler.

> Resonance Chamber is a single player, fully offline puzzle game. No account, no
> login, no user-generated content, no in-app purchases, no advertising, no
> network connections. A screen recording captured on a physical iPhone running
> iOS 26.6.1 is attached.
>
> 1. Screen recording: attached. It starts at app launch and shows the main menu,
> the Sound Check tutorial completed, Levels 1 and 2 solved (select a component,
> rotate or move it, press PULSE, the pulse reaches the receiver), and the
> Resonance Dial and Sound Guide menus. The app has no account, login, account
> deletion, user-generated content, or paid-content flows, so none appear.
>
> 2. Purpose and audience: an acoustic logic puzzle. The player shapes a room by
> rotating and moving acoustic parts (reflectors, splitters, absorbers, delay
> plates), then releases one pulse of sound and routes it to reach every listening
> point and satisfy that chamber's timing or resonance rule. It is a calm,
> deterministic puzzle with no time pressure, no reflexes, no randomness, and no
> internet requirement. Audience: puzzle and logic players, ages 4 and up. Rated
> 4+, no objectionable content.
>
> 3. Setup and access: none required. No login, account, configuration, or sample
> files. The full app works offline on first launch. Launch it to reach the main
> menu; tap START SESSION for the tutorial; tap a component to select it, tap a
> selected reflector again to rotate it, tap a highlighted socket to move a
> movable part; tap PULSE to run the simulation. Clearing the tutorial unlocks
> Level 1; each solved chamber unlocks the next (13 total). RESONANCE DIAL selects
> any unlocked chamber, SOUND GUIDE explains the components, TUNING has settings
> and a progress reset. No demo credentials needed.
>
> 4. External services, tools, or platforms: none. No backend, data provider, or
> API. No authentication service. No payment processor, in-app purchases, or
> advertising. No AI services. No analytics, telemetry, crash reporting, or
> attribution SDK; Unity diagnostics and analytics are disabled in the project.
> Zero network connections. The app is built with the Unity game engine; all
> levels, audio, artwork, the icon, and text are bundled in the app and were
> created by us.
>
> 5. Regional differences: none. The app behaves identically in every region, with
> no regional feature flags, geo-varied content, or regional pricing. Price:
> USD 0.00 (free), no in-app purchases. No location services.
>
> 6. Regulated industry or protected third-party material: not applicable.
> Resonance Chamber is a puzzle game in no regulated industry. Every asset is our
> original work: level designs, the deterministic acoustic simulation, all audio
> (synthesized offline by our code into WAV), all artwork and the icon (rendered
> procedurally by our code), and all text. The app contains no third-party
> content and uses no licensed music, art, trademarks, or other protected
> material. No documentation or credentials required. Some internal engine
> identifiers and our source repository use the working codename "Echo Forge";
> everything the user sees reads "Resonance Chamber".

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
> Bluetooth, photos, or tracking), even though the theme is acoustics. Price:
> USD 0.00 (free), no in-app purchases.
>
> External services used to deliver functionality: none. The app is built with
> the Unity game engine, contains no third-party content, and uses no licensed or
> protected material. All levels, audio, art, and the icon are original and were
> generated by our own code, bundled in the app.
>
> How to review: launch the app; it opens on the main menu. Tap START SESSION for
> the guided tutorial. Tap a bronze component to select it, tap a selected
> reflector again to rotate it, tap a highlighted socket to move a movable part,
> then tap the round PULSE button to run the simulation. Solving a chamber unlocks
> the next; 13 chambers total. RESONANCE DIAL is the chamber selector, SOUND GUIDE
> explains the components, TUNING has settings and a progress reset. No demo
> credentials are needed.
>
> The app functions identically in all regions. Rated 4+, no objectionable
> content. Landscape orientation only, by design.
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

---

## Note on Content Rights and the engine font

App Information > Content Rights is answered **"No, this app does not contain,
show, or access third-party content"**, and the reply above is written to match.

That question, and Guideline 5.2, are about licensed media or intellectual
property (songs, video, images, branded or protected material) where your
authorization could be in question. It does not mean the engine, its runtime, or
its default components.

The app's UI text renders in LiberationSans, the default font bundled with Unity's
UI package, under the SIL Open Font License 1.1. The OFL explicitly permits
bundling the font in an application with no notice or action required, so it is
not "third-party content" in the Content Rights sense and does not need
authorization. It is recorded in `ASSET_PROVENANCE.md` for completeness. There is
no reason to raise it in the review reply, and doing so would only look like it
contradicts the Content Rights answer, so it is left out.
