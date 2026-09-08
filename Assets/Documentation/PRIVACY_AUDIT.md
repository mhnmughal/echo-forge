# PRIVACY_AUDIT - Resonance Chamber (codename Echo Forge)

Audit of the app against Apple's App Store Review Guidelines section 5.1
(Privacy), the Apple privacy manifest policy, and the App Store Connect App
Privacy questionnaire.

Date: September 8, 2026. Method: full scan of `Assets/EchoForge/Runtime/**`,
`Assets/EchoForge/Editor/**`, `ProjectSettings/**`, `Packages/manifest.json`, and
the store documents. US English. No em dash used in this file.

References checked:
- https://developer.apple.com/app-store/review/guidelines/ (section 5.1)
- https://developer.apple.com/documentation/bundleresources/privacy_manifest_files
- https://docs.unity3d.com/Manual/apple-privacy-manifest-policy.html
- https://developer.apple.com/help/app-store-connect/manage-app-information/manage-app-privacy

---

## Verdict

**PASS on design and code. No code changes are required for privacy compliance.**

The app collects nothing, sends nothing, requests no permissions, and links no
tracking or advertising frameworks. "Data Not Collected" is the correct and
honest App Privacy answer.

Remaining work is not code. It is hosting (a privacy policy URL and a support
URL, both required to submit) plus two small recommended hardening steps. See
Action Items.

---

## A. App Store Review Guideline 5.1 (Privacy)

| Clause | Requirement | Status |
| --- | --- | --- |
| 5.1.1(i) Privacy policy | Link in App Store Connect metadata, and in the app | METADATA URL NOT YET HOSTED. In-app link recommended, see Action 4. |
| 5.1.1(ii) Permission requests | Request only data needed for core function, with a clear purpose string | PASS. No permissions requested. All iOS usage description strings are empty. |
| 5.1.1(iii) Data minimization | Collect only what is relevant | PASS. Nothing is collected. |
| 5.1.1(iv) Access | Respect user permission settings | PASS. Not applicable, no protected resources touched. |
| 5.1.1(v) Account sign-in | If the app does not need an account, allow use without one. Offer account deletion if accounts exist | PASS. No account, no login, no sign-in of any kind. |
| 5.1.2(i) Data use | Use data only as disclosed in the privacy policy | PASS. Only local save data, disclosed. |
| 5.1.2(ii) Third-party sharing | No sharing without consent | PASS. No third parties. |
| 5.1.2(iii) Repurposing | No undisclosed repurposing of data | PASS. |
| 5.1.2(iv) Data brokers | No selling data | PASS. |
| 5.1.2(v) Tracking | Must use App Tracking Transparency to track | PASS. No tracking. `AppTrackingTransparency` and `AdSupport` are not linked. `NSUserTrackingUsageDescription` is not set. |
| 5.1.3 Health and HealthKit | N/A | PASS. Not used. |
| 5.1.4 Kids Category | Extra rules for the Kids Category | N/A. The app targets Games / Puzzle, not the Kids Category. Age rating 4+ with no objectionable content, no ads, no external links. |
| 5.1.5 Location services | N/A | PASS. Not used. |

### Evidence

* **Runtime network scan** (`Assets/EchoForge/Runtime/**`): zero matches for
  `UnityWebRequest`, `HttpClient`, `System.Net`, `Socket` / `TcpClient` /
  `UdpClient` / `WebClient`, `DownloadHandler`, `Application.OpenURL`, `WebView`.
  The only "Socket" hits are the game's acoustic component sockets.
* **Device identifiers**: zero use of `SystemInfo.deviceUniqueIdentifier`,
  `advertisingIdentifier` / IDFA / IDFV, `deviceModel`, `systemLanguage`,
  `PlayerPrefs`.
* **Local persistence**: one file, `Application.persistentDataPath/echoforge_save.json`,
  written with `File.WriteAllText` / `File.Replace` / `File.Move`. Contents of
  `SaveData`: schema version, tutorial-complete flag, highest unlocked level,
  per-level completion and best rating and best tune count, music volume,
  effects volume, haptics on/off, two finale flags. No name, email, contacts,
  identifiers, timestamps, coarse or fine location, or free text. The file never
  leaves the device.
* **Unity services and diagnostics**: `ProjectSettings/UnityConnectSettings.asset`
  has UnityConnect, Engine Diagnostics, Crash Reporting, Analytics, Ads,
  Purchasing, and Performance Reporting all `m_Enabled: 0` with
  `m_InitializeOnStartup: 0` and every service URL blanked.
  `ProjectSettings.asset`: `submitAnalytics: 0`.
* **iOS capabilities**: `iOSBackgroundModes: 0`. No push notifications, iCloud,
  CloudKit, HealthKit, HomeKit, Sign in with Apple, associated domains, or
  keychain sharing. `stripEngineCode: 1`.
* **Haptics**: `Haptics.cs` calls `Handheld.Vibrate()` only, gated by a user
  setting. On iOS this is a system sound, needs no permission, and carries no
  privacy implication.

---

## B. Apple privacy manifest (PrivacyInfo.xcprivacy)

**Handled automatically by Unity. One post-export check remains.**

* The project runs Unity 6000.3.19f1. Unity Engine has auto-generated the
  app-level `PrivacyInfo.xcprivacy` on iOS builds since 2023.2.7f1, so this build
  produces one without any manual file. It declares the four required-reason API
  categories the engine uses: file timestamp (C617.1, 0A2A.1), user defaults
  (CA92.1), system boot time (35F9.1), and disk space (E174.1). It sets
  `NSPrivacyTracking` to `false` and leaves `NSPrivacyCollectedDataTypes` empty.
* **This app's own C# code adds no required-reason API usage beyond that set.**
  The save code does not read file timestamps (`File.GetLastWriteTime` and
  friends are never called), does not use `PlayerPrefs` / `NSUserDefaults`
  directly, and does not query boot time or disk space. So no custom
  `Assets/Plugins/PrivacyInfo.xcprivacy` is needed.
* **Third-party SDKs**: the 12-package shipping manifest is URP, Input System,
  uGUI with TextMeshPro, the Test Framework (editor), the Visual Studio editor
  integration (editor), and Unity engine modules. None collect data. None ship
  their own `.xcprivacy` in this editor install, because Unity's engine manifest
  covers first-party rendering and input. No additional privacy manifest is
  required.
* **Temporary tooling note**: `com.unity.recorder` and
  `com.anklebreaker.unity-mcp` are currently in `manifest.json` for a one-off
  gameplay capture. Both are editor-only (`includePlatforms: ["Editor"]`) and are
  never compiled into an iOS player, so they do not affect the manifest or the
  privacy posture. They are to be removed before the store build (see
  PACKAGE_AUDIT.md).

Post-export check (Section H): open the generated Xcode project and confirm the
manifest is present and unchanged from the description above.

---

## C. App Store Connect App Privacy questionnaire

Enter these answers in App Store Connect under App Privacy:

* **Data collection**: select **"Data Not Collected."**
  Justification: no data is transmitted off the device. The local save is not
  used for tracking, advertising, or analytics, and under Apple's definition
  local-only storage that never leaves the device is not "collection." Unity
  telemetry is disabled. There are no third-party partners.
* **Tracking**: **"No, this app does not track."**
* No data types to configure, because nothing is collected.
* Privacy Policy URL: required field. See Action 1.

---

## D. Permissions and Info.plist

* `PlayerSettings.iOS.cameraUsageDescription`,
  `locationUsageDescription`, and `microphoneUsageDescription` are all empty
  (`PlayerConfig.cs` sets them empty and the project asset confirms it). Unity
  only writes an `NS...UsageDescription` key when the string is non-empty, so the
  exported Info.plist will contain none of them.
* No `NSUserTrackingUsageDescription`, no `NSBluetoothAlwaysUsageDescription`, no
  `NSPhotoLibraryUsageDescription`.
* Verify after export with a grep, see Section H.

---

## E. Privacy policy document review (PRIVACY_POLICY.md)

The draft is accurate and covers the right topics: no data collected, no
permissions, no networking, local-only save, backups handled by the OS, a
children note, a change process, and a contact section.

Fixes needed before it goes live:

1. The contact section still reads "Public contact details and hosting will be
   added before submission" and "This is an unpublished draft." Replace with a
   working contact method (an email address or a contact form URL) and remove the
   draft disclaimer.
2. Set a real "Last updated" date when it is published.
3. Optional wording: state plainly that the app is not directed at children under
   13 and collects no data from anyone, which keeps the children section simple.

---

## F. Required URLs (both block submission)

* **Privacy Policy URL**: host `PRIVACY_POLICY.md` at a stable public HTTPS page
  (GitHub Pages or a personal site). Must be a viewable web page, not a file
  download. Enter it in App Store Connect.
* **Support URL**: a public page with a real contact method. The private source
  repository is not acceptable.

---

## G. Action items

### Blocking (cannot submit without these)

1. Host the privacy policy at a public HTTPS URL and enter it in App Store
   Connect.
2. Provide a Support URL with real contact information.
3. Finalize PRIVACY_POLICY.md per Section E (contact method, date, remove the
   draft disclaimer).

### Recommended (lowers rejection risk and per-submission friction)

4. Add an in-app privacy notice that is easy to reach, to satisfy the "within the
   app" half of Guideline 5.1.1(i). Offline-friendly options, in order of
   preference for a no-network app:
   a. Embed the full policy text in a scrollable panel opened from the Tuning
      Strip or the Credits plate.
   b. An `Application.OpenURL` link to the hosted policy. This is a
      user-initiated action and is accepted, but it means the review notes
      should say "the only network action is the user tapping the privacy policy
      link," rather than "no network connections at all."
5. Add `ITSAppUsesNonExemptEncryption` set to `NO` in the exported Info.plist.
   The app uses no encryption and no networking, so this is accurate and it
   removes the export compliance question on every submission. Do it in Xcode
   after export, or with a small `PostProcessBuild` script.

### Housekeeping (not a privacy issue)

6. `SaveSystem.Recover` writes `echoforge_save.json.bak-<ticks>` on every
   recovery and never deletes them. Consider keeping only the most recent one.

---

## H. Post-export verification checklist

Run these against the generated Xcode project before archiving:

* [ ] `PrivacyInfo.xcprivacy` exists in the Unity-iPhone target. `NSPrivacyTracking`
  is `false`. `NSPrivacyCollectedDataTypes` is empty. `NSPrivacyAccessedAPITypes`
  lists only the four Unity engine categories (C617/0A2A file timestamp, CA92
  user defaults, 35F9 boot time, E174 disk space).
* [ ] `grep -i "UsageDescription" Info.plist` returns nothing.
* [ ] `AppTrackingTransparency.framework` and `AdSupport.framework` are not in the
  Link Binary With Libraries phase.
* [ ] No `NSUserTrackingUsageDescription` in Info.plist.
* [ ] `com.unity.recorder` and `com.anklebreaker.unity-mcp` were removed from
  `manifest.json` before the build (PACKAGE_AUDIT.md).
* [ ] Launch the build in airplane mode and play start to finish with no network
  reachability. Confirms OFFLINE_AUDIT on device.

---

## Summary

Nothing in the app's behavior or code violates Apple's privacy guidelines, and
the App Privacy answer is a clean "Data Not Collected." The submission is gated
only by hosting the two required URLs and finalizing the policy text. Items 4 and
5 are worth doing before the first submission to avoid a predictable reviewer
note and a repeated export-compliance prompt.
