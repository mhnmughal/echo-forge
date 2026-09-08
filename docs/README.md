# docs/ - hostable pages for the App Store submission

App Store Connect requires two public URLs that resolve before you can submit:

| Field | Page | Why |
| --- | --- | --- |
| Privacy Policy URL | `privacy.html` | Required for every app (Guideline 5.1.1(i)). |
| Support URL | `support.html` | Required. Must have a real contact method. |

`index.html` is a small landing page that links to both. All three are plain
static HTML with inline CSS, no build step, no framework, no external requests.
They match the game's dark blue / bronze / amber theme.

## Fill these in before hosting

Search and replace across all three files:

| Placeholder | Replace with |
| --- | --- |
| `{{CONTACT_EMAIL}}` | A monitored email address (e.g. `support@yourdomain.com`). Used in a `mailto:` link. |
| `{{LAST_UPDATED_DATE}}` | The date you publish, written out (e.g. `March 3, 2026`). |
| `{{APP_VERSION}}` | `1.0.0` for the first release. |
| `{{APP_STORE_URL}}` | The App Store product URL once the app is live. Until then, point it at `./support.html` or remove that link. |
| `{{RESPONSE_TIME}}` | A realistic window (e.g. `3 business days`). |
| `{{YEAR}}` | `2026`. |

Keep `privacy.html` in sync with `../PRIVACY_POLICY.md` (same wording). If you
change one, change the other.

## Hosting options (any static host works)

- **GitHub Pages**: move this folder's contents to a public repo (this repo is
  private; Pages on a private repo needs a paid plan), or a `gh-pages` branch.
  Settings > Pages > Deploy from branch > `/docs`. URLs become
  `https://<user>.github.io/<repo>/privacy.html` and `.../support.html`.
- **Netlify / Cloudflare Pages / Vercel**: drag the `docs/` folder into a new
  site. Free, custom domain optional.
- **Any web server**: upload the three files to a folder and use those URLs.

## After hosting

1. Enter the two URLs in App Store Connect (App Information > Privacy Policy URL;
   and the Support URL field for the version).
2. Confirm both load over HTTPS in a browser with no login.
3. Optional: add a link to the privacy page inside the app (Tuning Strip or
   Credits) to fully satisfy the "within the app" half of Guideline 5.1.1(i).
   See PRIVACY_AUDIT.md action item 4.
