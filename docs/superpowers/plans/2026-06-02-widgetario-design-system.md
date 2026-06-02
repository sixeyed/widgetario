# Widgetario Design System Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Replace the Widgetario web UI with a coherent "Playful Pop" / neobrutalist design system: tokens, components, theme toggle, toast, and a fully-restyled product list.

**Architecture:** Pure hand-rolled CSS using custom properties for tokens. Two new CSS files (`tokens.css`, `components.css`) loaded after Google Fonts. Two tiny vanilla JS files (`theme.js`, `toast.js`). Bootstrap + jQuery deleted. Header and footer move to Razor partials so they render on every page. Two computed properties on `Product` (`Emoji`, `TileColor`) keep the views dumb.

**Tech Stack:** ASP.NET Core MVC (.NET 10) · Razor (`.cshtml`) · Plain CSS3 with custom properties · Vanilla JS (no framework) · Google Fonts (Inter Tight + Archivo Black) · Docker Compose for build + run.

**Reference spec:** `docs/superpowers/specs/2026-06-02-widgetario-design-system.md`

---

## File Structure

**New files:**
- `src/web/dotnet/Widgetario.Web/wwwroot/css/tokens.css` — all CSS custom properties (light + dark)
- `src/web/dotnet/Widgetario.Web/wwwroot/css/components.css` — all component styles
- `src/web/dotnet/Widgetario.Web/wwwroot/js/theme.js` — theme toggle behaviour
- `src/web/dotnet/Widgetario.Web/wwwroot/js/toast.js` — `widgetario.toast()` function
- `src/web/dotnet/Widgetario.Web/Views/Shared/_Header.cshtml` — header partial
- `src/web/dotnet/Widgetario.Web/Views/Shared/_Footer.cshtml` — footer partial

**Modified:**
- `src/web/dotnet/Widgetario.Web/Views/Shared/_Layout.cshtml` — drop Bootstrap, add fonts, no-flash script, partials
- `src/web/dotnet/Widgetario.Web/Views/Home/Index.cshtml` — rewrite using new components
- `src/web/dotnet/v2/Index.cshtml` — rewrite + Buy button toast wire-up
- `src/web/dotnet/Widgetario.Web/Models/Product.cs` — add `Emoji` + `TileColor` computed properties
- `src/web/dotnet/Widgetario.Web/wwwroot/css/site.css` — emptied to a `@import` shell

**Deleted:**
- `src/web/dotnet/Widgetario.Web/wwwroot/css/themes/light.css`
- `src/web/dotnet/Widgetario.Web/wwwroot/css/themes/dark.css`
- `src/web/dotnet/Widgetario.Web/wwwroot/css/themes/` (empty directory after the two files go)
- `src/web/dotnet/Widgetario.Web/wwwroot/lib/bootstrap/` (whole directory)
- `src/web/dotnet/Widgetario.Web/wwwroot/lib/jquery/` (whole directory)
- `src/web/dotnet/Widgetario.Web/wwwroot/lib/jquery-validation/` (whole directory)
- `src/web/dotnet/Widgetario.Web/wwwroot/lib/jquery-validation-unobtrusive/` (whole directory)
- `src/web/dotnet/Widgetario.Web/Views/Shared/_ValidationScriptsPartial.cshtml`

**Untouched:** All controllers, services, backend APIs, compose files, Dockerfiles, `Error.cshtml`, `appsettings.json`.

---

## Cross-Cutting Notes

### Build + visual-verify cycle per task

The web service is currently running locally via:
```
docker compose -f src/docker-compose.yml -f src/docker-compose-build.yml up -d
```

After every task that changes web source:
1. From `src/`: `docker compose -f docker-compose.yml -f docker-compose-build.yml build web`
2. From `src/`: `docker compose -f docker-compose.yml -f docker-compose-build.yml up -d --force-recreate web`
3. `sleep 8` (web .NET startup)
4. `curl -fsS http://localhost:8080/ -o /tmp/widgetario.html && wc -c /tmp/widgetario.html` — confirm 200 + non-empty
5. `grep "<expected-marker>" /tmp/widgetario.html` — task-specific HTML check (see each task)

The visual eyeball check (does it look right) is the user's job at the end of the task. An implementer subagent reports DONE and the user looks; if anything is wrong, surface in code review.

### Commit message style

Short imperative, no Conventional Commits prefix. Examples from existing repo:
- `Add Elasticsearch & Kibana`
- `Bump products-api/dotnet to .NET 10`

Include the standard trailer (use the model you're actually running as):
```
Co-Authored-By: Claude Sonnet 4.6 <noreply@anthropic.com>
```

### Branch

All work happens on the existing `upgrade-26.06` branch. Do not switch branches, do not create a new one.

---

## Task 1: Tokens + base CSS shell

**Files:**
- Create: `src/web/dotnet/Widgetario.Web/wwwroot/css/tokens.css`
- Create: `src/web/dotnet/Widgetario.Web/wwwroot/css/components.css`
- Modify: `src/web/dotnet/Widgetario.Web/wwwroot/css/site.css` (full replace)
- Modify: `src/web/dotnet/Widgetario.Web/Views/Shared/_Layout.cshtml`

- [ ] **Step 1: Create `tokens.css` with full token set**

Create `src/web/dotnet/Widgetario.Web/wwwroot/css/tokens.css`:
```css
:root {
  /* Color — light */
  --color-bg: #ffeb6b;
  --color-surface: #ffffff;
  --color-surface-alt: #fff9d4;
  --color-ink: #1a1a1a;
  --color-ink-muted: #5a5a5a;
  --color-accent: #ff5fa2;
  --color-accent-ink: #1a1a1a;
  --color-positive: #1a1a1a;
  --color-positive-ink: #ffeb6b;
  --color-warning: #fb923c;
  --color-warning-ink: #1a1a1a;

  /* Tile palette (constant in both modes) */
  --tile-1: #ff5fa2;
  --tile-2: #7cdfff;
  --tile-3: #a3e635;
  --tile-4: #fb923c;
  --tile-5: #b794f4;
  --tile-6: #34d399;

  /* Typography */
  --font-display: 'Archivo Black', system-ui, sans-serif;
  --font-body: 'Inter Tight', system-ui, sans-serif;
  --text-xs: 0.75rem;
  --text-sm: 0.875rem;
  --text-base: 1rem;
  --text-lg: 1.25rem;
  --text-xl: 1.75rem;
  --text-2xl: 2.5rem;
  --leading-tight: 1.1;
  --leading-normal: 1.5;
  --tracking-tight: -0.02em;
  --tracking-wide: 0.08em;

  /* Spacing */
  --space-1: 0.25rem;
  --space-2: 0.5rem;
  --space-3: 0.75rem;
  --space-4: 1rem;
  --space-5: 1.5rem;
  --space-6: 2rem;
  --space-7: 3rem;
  --space-8: 4rem;

  /* Radius */
  --radius-sm: 4px;
  --radius-md: 8px;
  --radius-lg: 12px;
  --radius-pill: 999px;

  /* Shadow (the stamp) */
  --shadow-stamp-sm: 3px 3px 0 var(--color-ink);
  --shadow-stamp-md: 5px 5px 0 var(--color-ink);
  --shadow-stamp-lg: 8px 8px 0 var(--color-ink);
  --shadow-press: 1px 1px 0 var(--color-ink);

  /* Border */
  --border-thin: 2px solid var(--color-ink);
  --border-thick: 3px solid var(--color-ink);

  /* Motion */
  --ease-out: cubic-bezier(0.16, 1, 0.3, 1);
  --dur-fast: 120ms;
  --dur-base: 220ms;
  --dur-slow: 400ms;
}

[data-theme="dark"] {
  --color-bg: #1a1a1a;
  --color-surface: #262626;
  --color-surface-alt: #1f1f1f;
  --color-ink: #ffeb6b;
  --color-ink-muted: #b8b8b8;
  --color-accent: #ff5fa2;
  --color-accent-ink: #1a1a1a;
  --color-positive: #ffeb6b;
  --color-positive-ink: #1a1a1a;
  --color-warning: #fb923c;
  --color-warning-ink: #1a1a1a;
}

/* Reset / base */
*, *::before, *::after { box-sizing: border-box; }
html, body { margin: 0; padding: 0; }
body {
  background: var(--color-bg);
  color: var(--color-ink);
  font-family: var(--font-body);
  font-size: var(--text-base);
  line-height: var(--leading-normal);
  min-height: 100vh;
}
button { font-family: inherit; cursor: pointer; }
img { max-width: 100%; display: block; }
```

- [ ] **Step 2: Create `components.css` with an empty file**

Create `src/web/dotnet/Widgetario.Web/wwwroot/css/components.css`:
```css
/* Widgetario component styles. Populated by subsequent tasks. */
```

- [ ] **Step 3: Replace `site.css` with a 3-line import shell**

Replace the entire contents of `src/web/dotnet/Widgetario.Web/wwwroot/css/site.css` with:
```css
@import url("tokens.css");
@import url("components.css");
```

- [ ] **Step 4: Update `_Layout.cshtml` — fonts, no-flash script, new CSS, drop Bootstrap**

Replace the entire contents of `src/web/dotnet/Widgetario.Web/Views/Shared/_Layout.cshtml` with:
```cshtml
<!DOCTYPE html>
<html lang="en" data-theme="@(ViewData["Theme"] ?? "light")">
    <head>
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <title>@ViewData["Title"] - Widgetario</title>
        <script>
          (function () {
            var t = localStorage.getItem('widgetario-theme');
            if (!t) t = document.documentElement.dataset.theme;
            if (!t) t = matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
            document.documentElement.dataset.theme = t;
          })();
        </script>
        <link rel="preconnect" href="https://fonts.googleapis.com">
        <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
        <link href="https://fonts.googleapis.com/css2?family=Inter+Tight:wght@500;700&family=Archivo+Black&display=swap" rel="stylesheet">
        <link rel="stylesheet" href="~/css/site.css" />
    </head>
    <body>
        <main role="main">
            @RenderBody()
        </main>
        @RenderSection("Scripts", required: false)
    </body>
</html>
```

(Header and footer partials are added in later tasks — leave the body as just `<main>` for now.)

- [ ] **Step 5: Build the web image**

Run from `src/`:
```
docker compose -f docker-compose.yml -f docker-compose-build.yml build web
```
Expected: build succeeds. The publish step will succeed even though some `<link href="~/lib/bootstrap/...">` and jquery references no longer have files behind them — they're no longer referenced after Step 4.

- [ ] **Step 6: Recreate the web container with the new image**

Run from `src/`:
```
docker compose -f docker-compose.yml -f docker-compose-build.yml up -d --force-recreate web
sleep 8
```

- [ ] **Step 7: Verify the page returns 200 and includes the new markers**

Run:
```
curl -fsS http://localhost:8080/ -o /tmp/widgetario.html
wc -c /tmp/widgetario.html
grep -c "Inter Tight" /tmp/widgetario.html
grep -c "Archivo Black" /tmp/widgetario.html
grep -c "widgetario-theme" /tmp/widgetario.html
grep -c "~/css/site.css\|/css/site.css" /tmp/widgetario.html
! grep -q "bootstrap" /tmp/widgetario.html && echo "Bootstrap link removed ✓"
```
Expected:
- File size > 500 bytes
- `Inter Tight` matched (1+ times)
- `Archivo Black` matched (1+ times)
- `widgetario-theme` matched (1, from the no-flash script)
- `/css/site.css` matched (1)
- "Bootstrap link removed ✓" prints (no `bootstrap` references in served HTML)

- [ ] **Step 8: Commit**

```
git add src/web/dotnet/Widgetario.Web/wwwroot/css/tokens.css \
        src/web/dotnet/Widgetario.Web/wwwroot/css/components.css \
        src/web/dotnet/Widgetario.Web/wwwroot/css/site.css \
        src/web/dotnet/Widgetario.Web/Views/Shared/_Layout.cshtml
git commit -m "Add design tokens & base CSS shell"
```

---

## Task 2: Delete Bootstrap, jQuery, and ValidationScripts partial

**Files:**
- Delete: `src/web/dotnet/Widgetario.Web/wwwroot/lib/bootstrap/` (entire directory)
- Delete: `src/web/dotnet/Widgetario.Web/wwwroot/lib/jquery/` (entire directory)
- Delete: `src/web/dotnet/Widgetario.Web/wwwroot/lib/jquery-validation/` (entire directory)
- Delete: `src/web/dotnet/Widgetario.Web/wwwroot/lib/jquery-validation-unobtrusive/` (entire directory)
- Delete: `src/web/dotnet/Widgetario.Web/Views/Shared/_ValidationScriptsPartial.cshtml`
- Delete: `src/web/dotnet/Widgetario.Web/wwwroot/css/themes/` (entire directory — `light.css` + `dark.css`)

- [ ] **Step 1: Confirm nothing left references these files**

Run from the repo root:
```
grep -r "bootstrap\|jquery\|themes/light\|themes/dark\|_ValidationScriptsPartial" \
  src/web/dotnet/Widgetario.Web/Views \
  src/web/dotnet/Widgetario.Web/Controllers \
  src/web/dotnet/Widgetario.Web/wwwroot/css/site.css \
  src/web/dotnet/Widgetario.Web/wwwroot/css/tokens.css \
  src/web/dotnet/Widgetario.Web/wwwroot/css/components.css \
  src/web/dotnet/v2/Index.cshtml \
  2>/dev/null || echo "no references"
```
Expected: only matches inside `wwwroot/lib/*` (the files being deleted) and possibly inside `Views/Home/Index.cshtml` (still mentions theme files — those go away in a later task). Anything outside these is a blocker — report and stop.

- [ ] **Step 2: Delete the library directories**

Run from `src/web/dotnet/Widgetario.Web/wwwroot/lib/`:
```
rm -rf bootstrap jquery jquery-validation jquery-validation-unobtrusive
```

- [ ] **Step 3: Delete the validation partial**

Run from `src/web/dotnet/Widgetario.Web/Views/Shared/`:
```
rm _ValidationScriptsPartial.cshtml
```

- [ ] **Step 4: Delete the old theme CSS files**

Run from `src/web/dotnet/Widgetario.Web/wwwroot/css/`:
```
rm -rf themes/
```

- [ ] **Step 5: Build & verify**

From `src/`:
```
docker compose -f docker-compose.yml -f docker-compose-build.yml build web
docker compose -f docker-compose.yml -f docker-compose-build.yml up -d --force-recreate web
sleep 8
curl -fsS http://localhost:8080/ -o /tmp/widgetario.html
wc -c /tmp/widgetario.html
```
Expected: build succeeds, image rebuilds smaller than before, page returns 200 with content.

- [ ] **Step 6: Commit**

Stage the deletions with `git add -A` scoped to the web dotnet tree (this picks up rm'd files):
```
git add -A src/web/dotnet/Widgetario.Web/wwwroot/lib
git add -A src/web/dotnet/Widgetario.Web/wwwroot/css/themes
git add -A src/web/dotnet/Widgetario.Web/Views/Shared/_ValidationScriptsPartial.cshtml
git status   # verify only deletions are staged
git commit -m "Delete Bootstrap, jQuery & old theme CSS"
```

---

## Task 3: Product model — Emoji + TileColor computed properties

**Files:**
- Modify: `src/web/dotnet/Widgetario.Web/Models/Product.cs`

- [ ] **Step 1: Add `Emoji` and `TileColor` to `Product.cs`**

Replace the entire contents of `src/web/dotnet/Widgetario.Web/Models/Product.cs` with:
```csharp
namespace Widgetario.Web.Models
{
    public class Product
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int Stock { get;  set; }

        public string StockMessage
        {
            get
            {
                var message = "Plenty";
                if (Stock == 0)
                {
                    message = "SOLD OUT!";
                }
                else if (Stock < 50)
                {
                    message = "Last few...";
                }
                return message;
            }
        }

        public string DisplayPrice
        {
            get
            {
                return $"${Price.ToString("#.00")}";
            }
        }

        public string Emoji
        {
            get
            {
                var lower = (Name ?? string.Empty).ToLowerInvariant();
                if (lower.Contains("soc") || lower.Contains("cpu") || lower.Contains("chip")) return "🔌";
                if (lower.Contains("iot") || lower.Contains("breakout")) return "🤖";
                if (lower.Contains("dac") || lower.Contains("audio")) return "🎛";
                if (lower.Contains("comms") || lower.Contains("radio") || lower.Contains("mars")) return "📡";
                if (lower.Contains("board")) return "🧩";
                if (lower.Contains("sensor")) return "📟";
                return "📦";
            }
        }

        public string TileColor
        {
            get
            {
                var name = Name ?? string.Empty;
                int sum = 0;
                foreach (var c in name) sum += (int)c;
                int idx = (Math.Abs(sum) % 6) + 1;
                return $"var(--tile-{idx})";
            }
        }
    }
}
```

(Note: needs `using System;` only if the file doesn't already have implicit usings. .NET 10 default templates have implicit usings; this project's Program.cs uses Startup pattern but implicit usings remain enabled by default. If `Math.Abs` does not resolve, add `using System;` at the top.)

- [ ] **Step 2: Build & verify the model is wired**

From `src/`:
```
docker compose -f docker-compose.yml -f docker-compose-build.yml build web
docker compose -f docker-compose.yml -f docker-compose-build.yml up -d --force-recreate web
sleep 8
curl -fsS http://localhost:8080/ -o /tmp/widgetario.html
wc -c /tmp/widgetario.html
```
Expected: build succeeds (no compile errors on new properties). Page returns 200 — visual layout unchanged at this point (we haven't applied the new properties to a view yet), but the model now exposes Emoji and TileColor for later tasks.

- [ ] **Step 3: Commit**

```
git add src/web/dotnet/Widgetario.Web/Models/Product.cs
git commit -m "Add Emoji & TileColor computed properties to Product"
```

---

## Task 4: Header + Footer partials

**Files:**
- Create: `src/web/dotnet/Widgetario.Web/Views/Shared/_Header.cshtml`
- Create: `src/web/dotnet/Widgetario.Web/Views/Shared/_Footer.cshtml`
- Modify: `src/web/dotnet/Widgetario.Web/Views/Shared/_Layout.cshtml`
- Modify: `src/web/dotnet/Widgetario.Web/wwwroot/css/components.css` (append header + footer styles)

- [ ] **Step 1: Create `_Header.cshtml`**

Create `src/web/dotnet/Widgetario.Web/Views/Shared/_Header.cshtml`:
```cshtml
<header class="app-header">
    <a class="app-header__brand" href="/">
        <span class="app-header__logo" aria-hidden="true">⚙️</span>
        <span class="app-header__wordmark">Widgetario</span>
    </a>
    <button type="button"
            class="theme-toggle"
            aria-label="Toggle dark mode"
            aria-pressed="false"
            data-theme-toggle>
        <span class="theme-toggle__icon" data-theme-icon>☀</span>
    </button>
</header>
```

- [ ] **Step 2: Create `_Footer.cshtml`**

Create `src/web/dotnet/Widgetario.Web/Views/Shared/_Footer.cshtml`:
```cshtml
<footer class="app-footer">
    © @DateTime.UtcNow.Year Widgetario · Stocked, shipped, sorted.
</footer>
```

- [ ] **Step 3: Wire partials into `_Layout.cshtml`**

Replace the `<body>` block of `src/web/dotnet/Widgetario.Web/Views/Shared/_Layout.cshtml` with:
```cshtml
    <body>
        @await Html.PartialAsync("_Header")
        <main role="main">
            @RenderBody()
        </main>
        @await Html.PartialAsync("_Footer")
        @RenderSection("Scripts", required: false)
    </body>
```

- [ ] **Step 4: Append header + footer styles to `components.css`**

Append to `src/web/dotnet/Widgetario.Web/wwwroot/css/components.css`:
```css
/* ---------- App header ---------- */
.app-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: var(--space-4) var(--space-5);
  background: var(--color-surface-alt);
  border-bottom: var(--border-thick);
  position: sticky;
  top: 0;
  z-index: 10;
}
.app-header__brand {
  display: flex;
  align-items: center;
  gap: var(--space-3);
  text-decoration: none;
  color: var(--color-ink);
}
.app-header__logo {
  width: 40px;
  height: 40px;
  background: var(--color-accent);
  border: var(--border-thin);
  border-radius: var(--radius-md);
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 20px;
  transform: rotate(-6deg);
  box-shadow: var(--shadow-stamp-sm);
}
.app-header__wordmark {
  font-family: var(--font-display);
  font-size: var(--text-xl);
  letter-spacing: var(--tracking-tight);
  line-height: 1;
}

/* ---------- Theme toggle ---------- */
.theme-toggle {
  width: 44px;
  height: 44px;
  background: var(--color-bg);
  border: var(--border-thin);
  border-radius: var(--radius-md);
  box-shadow: var(--shadow-stamp-sm);
  display: inline-flex;
  align-items: center;
  justify-content: center;
  color: var(--color-ink);
  transition: transform var(--dur-fast) var(--ease-out), box-shadow var(--dur-fast) var(--ease-out);
}
.theme-toggle:hover {
  transform: translate(-1px, -1px);
  box-shadow: var(--shadow-stamp-md);
}
.theme-toggle:active {
  transform: translate(2px, 2px);
  box-shadow: var(--shadow-press);
}
.theme-toggle:focus-visible {
  outline: 3px solid var(--color-ink);
  outline-offset: 2px;
}
.theme-toggle__icon { font-size: 18px; line-height: 1; }

/* ---------- App footer ---------- */
.app-footer {
  margin-top: var(--space-7);
  padding: var(--space-4) var(--space-5);
  background: var(--color-ink);
  color: var(--color-bg);
  text-align: center;
  font-size: var(--text-xs);
  letter-spacing: var(--tracking-wide);
  text-transform: uppercase;
  border-top: var(--border-thick);
}

/* ---------- Mobile ---------- */
@media (max-width: 480px) {
  .app-header__wordmark { display: none; }
}
```

- [ ] **Step 5: Build & verify**

From `src/`:
```
docker compose -f docker-compose.yml -f docker-compose-build.yml build web
docker compose -f docker-compose.yml -f docker-compose-build.yml up -d --force-recreate web
sleep 8
curl -fsS http://localhost:8080/ -o /tmp/widgetario.html
grep -c 'class="app-header"' /tmp/widgetario.html
grep -c 'data-theme-toggle' /tmp/widgetario.html
grep -c 'class="app-footer"' /tmp/widgetario.html
```
Expected: each grep returns 1.

- [ ] **Step 6: Commit**

```
git add src/web/dotnet/Widgetario.Web/Views/Shared/_Header.cshtml \
        src/web/dotnet/Widgetario.Web/Views/Shared/_Footer.cshtml \
        src/web/dotnet/Widgetario.Web/Views/Shared/_Layout.cshtml \
        src/web/dotnet/Widgetario.Web/wwwroot/css/components.css
git commit -m "Add header & footer partials"
```

---

## Task 5: theme.js — toggle behaviour

**Files:**
- Create: `src/web/dotnet/Widgetario.Web/wwwroot/js/theme.js`
- Modify: `src/web/dotnet/Widgetario.Web/Views/Shared/_Layout.cshtml` (load the script)

- [ ] **Step 1: Create `theme.js`**

Create `src/web/dotnet/Widgetario.Web/wwwroot/js/theme.js`:
```javascript
(function () {
  'use strict';
  var STORAGE_KEY = 'widgetario-theme';
  var btn = document.querySelector('[data-theme-toggle]');
  var iconEl = document.querySelector('[data-theme-icon]');
  if (!btn || !iconEl) return;

  function paintIcon(theme) {
    iconEl.textContent = theme === 'dark' ? '🌙' : '☀';
    btn.setAttribute('aria-pressed', theme === 'dark' ? 'true' : 'false');
  }

  paintIcon(document.documentElement.dataset.theme || 'light');

  btn.addEventListener('click', function () {
    var current = document.documentElement.dataset.theme === 'dark' ? 'dark' : 'light';
    var next = current === 'dark' ? 'light' : 'dark';
    document.documentElement.dataset.theme = next;
    try { localStorage.setItem(STORAGE_KEY, next); } catch (e) { /* private mode */ }
    paintIcon(next);
  });
})();
```

- [ ] **Step 2: Load `theme.js` from `_Layout.cshtml`**

In `src/web/dotnet/Widgetario.Web/Views/Shared/_Layout.cshtml`, replace the line:
```cshtml
        @RenderSection("Scripts", required: false)
```
With:
```cshtml
        <script src="~/js/theme.js" asp-append-version="true"></script>
        @RenderSection("Scripts", required: false)
```

- [ ] **Step 3: Build & verify**

From `src/`:
```
docker compose -f docker-compose.yml -f docker-compose-build.yml build web
docker compose -f docker-compose.yml -f docker-compose-build.yml up -d --force-recreate web
sleep 8
curl -fsS http://localhost:8080/ -o /tmp/widgetario.html
grep -c '/js/theme.js' /tmp/widgetario.html
curl -fsS http://localhost:8080/js/theme.js | grep -c 'widgetario-theme'
```
Expected: first grep = 1 (script tag present), second grep ≥ 1 (file is served and contains the storage key).

- [ ] **Step 4: Commit**

```
git add src/web/dotnet/Widgetario.Web/wwwroot/js/theme.js \
        src/web/dotnet/Widgetario.Web/Views/Shared/_Layout.cshtml
git commit -m "Add theme toggle behaviour"
```

---

## Task 6: Hero section + env chip

**Files:**
- Modify: `src/web/dotnet/Widgetario.Web/Views/Home/Index.cshtml`
- Modify: `src/web/dotnet/Widgetario.Web/wwwroot/css/components.css` (append hero styles)

- [ ] **Step 1: Replace the top of `Index.cshtml` with the hero block**

Replace the entire contents of `src/web/dotnet/Widgetario.Web/Views/Home/Index.cshtml` with:
```cshtml
@model ProductViewModel
@{
    ViewData["Title"] = "Products";
}

<section class="hero">
    <div class="hero__copy">
        <span class="hero__eyebrow">Widgetario</span>
        <h1 class="hero__title">Tools for builders.</h1>
        <p class="hero__tag">Stocked, shipped, sorted.</p>
    </div>
    @if (ViewData["Environment"] != null)
    {
        <span class="env-chip">@ViewData["Environment"]</span>
    }
</section>

@if (Model.Products == null)
{
    <p class="loading">Loading…</p>
}
else
{
    <ul class="product-list">
        @foreach (var product in Model.Products)
        {
            <li class="product-row">
                <span class="product-row__name">@product.Name</span>
                <span class="product-row__price">@product.DisplayPrice</span>
            </li>
        }
    </ul>
}
```

(The product list is intentionally bare here — the row component gets fleshed out in Task 7.)

- [ ] **Step 2: Append hero + env chip styles to `components.css`**

Append to `src/web/dotnet/Widgetario.Web/wwwroot/css/components.css`:
```css
/* ---------- Hero ---------- */
.hero {
  max-width: 880px;
  margin: 0 auto;
  padding: var(--space-7) var(--space-5) var(--space-5);
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: var(--space-4);
}
.hero__copy { flex: 1; min-width: 0; }
.hero__eyebrow {
  display: inline-block;
  font-family: var(--font-body);
  font-weight: 700;
  font-size: var(--text-xs);
  letter-spacing: var(--tracking-wide);
  text-transform: uppercase;
  background: var(--color-surface);
  color: var(--color-ink);
  border: var(--border-thin);
  border-radius: var(--radius-sm);
  padding: var(--space-1) var(--space-3);
  margin-bottom: var(--space-3);
}
.hero__title {
  font-family: var(--font-display);
  font-size: var(--text-2xl);
  line-height: var(--leading-tight);
  letter-spacing: var(--tracking-tight);
  color: var(--color-ink);
  margin: 0;
}
.hero__tag {
  font-size: var(--text-base);
  color: var(--color-ink-muted);
  margin: var(--space-2) 0 0;
}

/* ---------- Env chip ---------- */
.env-chip {
  font-family: var(--font-body);
  font-weight: 800;
  font-size: var(--text-xs);
  letter-spacing: var(--tracking-wide);
  text-transform: uppercase;
  background: var(--color-accent);
  color: var(--color-accent-ink);
  border: var(--border-thin);
  border-radius: var(--radius-pill);
  padding: var(--space-1) var(--space-3);
  flex-shrink: 0;
}

/* ---------- Loading ---------- */
.loading {
  text-align: center;
  font-family: var(--font-body);
  color: var(--color-ink-muted);
  padding: var(--space-7);
}

/* ---------- Mobile ---------- */
@media (max-width: 640px) {
  .hero { padding: var(--space-5) var(--space-4); flex-direction: column; }
  .hero__title { font-size: var(--text-xl); }
}
```

- [ ] **Step 3: Build & verify**

From `src/`:
```
docker compose -f docker-compose.yml -f docker-compose-build.yml build web
docker compose -f docker-compose.yml -f docker-compose-build.yml up -d --force-recreate web
sleep 8
curl -fsS http://localhost:8080/ -o /tmp/widgetario.html
grep -c 'class="hero"' /tmp/widgetario.html
grep -c 'Tools for builders.' /tmp/widgetario.html
grep -c '<table' /tmp/widgetario.html
```
Expected: first two greps = 1, third grep = 0 (the old `<table>` is gone).

- [ ] **Step 4: Commit**

```
git add src/web/dotnet/Widgetario.Web/Views/Home/Index.cshtml \
        src/web/dotnet/Widgetario.Web/wwwroot/css/components.css
git commit -m "Add hero section & env chip"
```

---

## Task 7: Product row component (tile + name + price)

**Files:**
- Modify: `src/web/dotnet/Widgetario.Web/Views/Home/Index.cshtml`
- Modify: `src/web/dotnet/Widgetario.Web/wwwroot/css/components.css` (append product-row styles)

- [ ] **Step 1: Flesh out the product row markup in `Index.cshtml`**

Replace the existing `<ul class="product-list">…</ul>` block in `src/web/dotnet/Widgetario.Web/Views/Home/Index.cshtml` with:
```cshtml
    <ul class="product-list">
        @foreach (var product in Model.Products)
        {
            <li class="product-row @(product.Stock == 0 ? "product-row--sold" : "")">
                <span class="product-tile" aria-hidden="true" style="background: @product.TileColor">@product.Emoji</span>
                <div class="product-row__info">
                    <span class="product-row__name">@product.Name</span>
                    <span class="product-row__price">@product.DisplayPrice</span>
                </div>
            </li>
        }
    </ul>
```

(Badge + button slots get added in Task 8.)

- [ ] **Step 2: Append product-list + product-row + product-tile styles**

Append to `src/web/dotnet/Widgetario.Web/wwwroot/css/components.css`:
```css
/* ---------- Product list ---------- */
.product-list {
  max-width: 880px;
  margin: 0 auto;
  padding: 0 var(--space-5) var(--space-5);
  list-style: none;
  display: flex;
  flex-direction: column;
  gap: var(--space-3);
}

/* ---------- Product row ---------- */
.product-row {
  background: var(--color-surface);
  border: var(--border-thin);
  border-radius: var(--radius-md);
  box-shadow: var(--shadow-stamp-sm);
  padding: var(--space-3) var(--space-4);
  display: flex;
  align-items: center;
  gap: var(--space-4);
  transition: transform var(--dur-fast) var(--ease-out), box-shadow var(--dur-fast) var(--ease-out);
}
.product-row:hover {
  transform: translate(-1px, -1px);
  box-shadow: var(--shadow-stamp-md);
}
.product-row--sold .product-tile { opacity: 0.5; }
.product-row__info { flex: 1; min-width: 0; display: flex; flex-direction: column; }
.product-row__name {
  font-family: var(--font-display);
  font-size: var(--text-lg);
  letter-spacing: var(--tracking-tight);
  color: var(--color-ink);
  line-height: var(--leading-tight);
}
.product-row__price {
  font-size: var(--text-sm);
  color: var(--color-ink-muted);
  margin-top: 2px;
}

/* ---------- Product tile ---------- */
.product-tile {
  width: 56px;
  height: 56px;
  border: var(--border-thin);
  border-radius: var(--radius-md);
  display: inline-flex;
  align-items: center;
  justify-content: center;
  font-size: 24px;
  line-height: 1;
  transform: rotate(-3deg);
  flex-shrink: 0;
}

/* ---------- Mobile ---------- */
@media (max-width: 640px) {
  .product-row { flex-wrap: wrap; gap: var(--space-3); padding: var(--space-3); }
  .product-row__info { flex-basis: 100%; }
}
```

- [ ] **Step 3: Build & verify**

From `src/`:
```
docker compose -f docker-compose.yml -f docker-compose-build.yml build web
docker compose -f docker-compose.yml -f docker-compose-build.yml up -d --force-recreate web
sleep 8
curl -fsS http://localhost:8080/ -o /tmp/widgetario.html
grep -c 'class="product-tile"' /tmp/widgetario.html
grep -c 'product-row__name' /tmp/widgetario.html
grep -c 'background: var(--tile-' /tmp/widgetario.html
```
Expected: each grep returns at least 1 (depends on how many products are in the catalog — there should be 4+).

- [ ] **Step 4: Commit**

```
git add src/web/dotnet/Widgetario.Web/Views/Home/Index.cshtml \
        src/web/dotnet/Widgetario.Web/wwwroot/css/components.css
git commit -m "Add product row component with tile, name & price"
```

---

## Task 8: Product badge + button (no Buy wiring yet)

**Files:**
- Modify: `src/web/dotnet/Widgetario.Web/Views/Home/Index.cshtml`
- Modify: `src/web/dotnet/Widgetario.Web/wwwroot/css/components.css` (append badge + button styles)

- [ ] **Step 1: Add badge to the product row markup**

In `src/web/dotnet/Widgetario.Web/Views/Home/Index.cshtml`, replace the `<li class="product-row …">` block inside the `@foreach` with:
```cshtml
            <li class="product-row @(product.Stock == 0 ? "product-row--sold" : "")">
                <span class="product-tile" aria-hidden="true" style="background: @product.TileColor">@product.Emoji</span>
                <div class="product-row__info">
                    <span class="product-row__name">@product.Name</span>
                    <span class="product-row__price">@product.DisplayPrice</span>
                </div>
                @{
                    string badgeText;
                    string badgeMod;
                    if (product.Stock == 0) { badgeText = "Sold out"; badgeMod = "product-badge--negative"; }
                    else if (product.Stock < 50) { badgeText = "Few left"; badgeMod = "product-badge--warning"; }
                    else { badgeText = "In stock"; badgeMod = "product-badge--positive"; }
                }
                <span class="product-badge @badgeMod">@badgeText</span>
            </li>
```

(The Buy button is in `v2/Index.cshtml` only — added in Task 9. The default `Index.cshtml` shows the badge but no Buy button.)

- [ ] **Step 2: Append badge + button styles to `components.css`**

Append to `src/web/dotnet/Widgetario.Web/wwwroot/css/components.css`:
```css
/* ---------- Product badge ---------- */
.product-badge {
  display: inline-flex;
  align-items: center;
  font-family: var(--font-body);
  font-weight: 700;
  font-size: var(--text-xs);
  letter-spacing: var(--tracking-wide);
  text-transform: uppercase;
  padding: var(--space-1) var(--space-3);
  border-radius: var(--radius-pill);
  flex-shrink: 0;
}
.product-badge--positive { background: var(--color-positive); color: var(--color-positive-ink); }
.product-badge--warning  { background: var(--color-warning);  color: var(--color-warning-ink);  }
.product-badge--negative { background: var(--color-accent);   color: var(--color-accent-ink);   }

/* ---------- Button ---------- */
.button {
  font-family: var(--font-body);
  font-weight: 800;
  font-size: var(--text-sm);
  background: var(--color-accent);
  color: var(--color-accent-ink);
  border: var(--border-thin);
  border-radius: var(--radius-md);
  padding: var(--space-2) var(--space-4);
  box-shadow: var(--shadow-stamp-sm);
  transition: transform var(--dur-fast) var(--ease-out), box-shadow var(--dur-fast) var(--ease-out);
  flex-shrink: 0;
}
.button:hover {
  transform: translate(-1px, -1px);
  box-shadow: var(--shadow-stamp-md);
}
.button:active {
  transform: translate(2px, 2px);
  box-shadow: var(--shadow-press);
}
.button:focus-visible {
  outline: 3px solid var(--color-ink);
  outline-offset: 2px;
}
.button[disabled] {
  opacity: 0.35;
  pointer-events: none;
  box-shadow: var(--shadow-press);
  transform: translate(2px, 2px);
}
.button--ghost {
  background: transparent;
  color: var(--color-ink);
}
```

- [ ] **Step 3: Build & verify**

From `src/`:
```
docker compose -f docker-compose.yml -f docker-compose-build.yml build web
docker compose -f docker-compose.yml -f docker-compose-build.yml up -d --force-recreate web
sleep 8
curl -fsS http://localhost:8080/ -o /tmp/widgetario.html
grep -c 'product-badge--positive\|product-badge--warning\|product-badge--negative' /tmp/widgetario.html
```
Expected: grep returns at least 1 — every product produces a badge.

- [ ] **Step 4: Commit**

```
git add src/web/dotnet/Widgetario.Web/Views/Home/Index.cshtml \
        src/web/dotnet/Widgetario.Web/wwwroot/css/components.css
git commit -m "Add product badge variants & button styles"
```

---

## Task 9: v2 Index — Buy button + toast.js

**Files:**
- Create: `src/web/dotnet/Widgetario.Web/wwwroot/js/toast.js`
- Modify: `src/web/dotnet/Widgetario.Web/Views/Shared/_Layout.cshtml` (load toast.js)
- Modify: `src/web/dotnet/v2/Index.cshtml` (rewrite with Buy buttons)
- Modify: `src/web/dotnet/Widgetario.Web/wwwroot/css/components.css` (append toast styles)

- [ ] **Step 1: Create `toast.js`**

Create `src/web/dotnet/Widgetario.Web/wwwroot/js/toast.js`:
```javascript
(function () {
  'use strict';
  window.widgetario = window.widgetario || {};

  var current = null;
  var currentTimer = null;

  function dismiss(el) {
    if (!el) return;
    el.classList.remove('toast--enter');
    el.classList.add('toast--exit');
    setTimeout(function () { if (el.parentNode) el.parentNode.removeChild(el); }, 400);
  }

  window.widgetario.toast = function (message) {
    if (current) {
      clearTimeout(currentTimer);
      current.textContent = message;
    } else {
      current = document.createElement('div');
      current.className = 'toast';
      current.setAttribute('role', 'status');
      current.textContent = message;
      document.body.appendChild(current);
      // Force reflow so the entrance transition runs:
      void current.offsetHeight;
      current.classList.add('toast--enter');
    }
    var el = current;
    currentTimer = setTimeout(function () {
      dismiss(el);
      if (current === el) { current = null; currentTimer = null; }
    }, 2000);
  };
})();
```

- [ ] **Step 2: Load `toast.js` from `_Layout.cshtml`**

In `src/web/dotnet/Widgetario.Web/Views/Shared/_Layout.cshtml`, replace the line:
```cshtml
        <script src="~/js/theme.js" asp-append-version="true"></script>
```
With:
```cshtml
        <script src="~/js/theme.js" asp-append-version="true"></script>
        <script src="~/js/toast.js" asp-append-version="true"></script>
```

- [ ] **Step 3: Append toast styles**

Append to `src/web/dotnet/Widgetario.Web/wwwroot/css/components.css`:
```css
/* ---------- Toast ---------- */
.toast {
  position: fixed;
  bottom: var(--space-5);
  right: var(--space-5);
  max-width: 320px;
  background: var(--color-bg);
  color: var(--color-ink);
  border: var(--border-thin);
  border-radius: var(--radius-md);
  box-shadow: var(--shadow-stamp-md);
  padding: var(--space-3) var(--space-4);
  font-family: var(--font-body);
  font-weight: 700;
  font-size: var(--text-sm);
  opacity: 0;
  transform: translateY(8px);
  transition: opacity var(--dur-base) var(--ease-out), transform var(--dur-base) var(--ease-out);
  z-index: 50;
}
.toast--enter {
  opacity: 1;
  transform: translateY(0);
}
.toast--exit {
  opacity: 0;
  transform: translateY(8px);
}
```

- [ ] **Step 4: Rewrite `v2/Index.cshtml` with Buy buttons**

Replace the entire contents of `src/web/dotnet/v2/Index.cshtml` with:
```cshtml
@model ProductViewModel
@{
    ViewData["Title"] = "Products";
}

<section class="hero">
    <div class="hero__copy">
        <span class="hero__eyebrow">Widgetario</span>
        <h1 class="hero__title">Tools for builders.</h1>
        <p class="hero__tag">Stocked, shipped, sorted.</p>
    </div>
    @if (ViewData["Environment"] != null)
    {
        <span class="env-chip">@ViewData["Environment"]</span>
    }
</section>

@if (Model.Products == null)
{
    <p class="loading">Loading…</p>
}
else
{
    <ul class="product-list">
        @foreach (var product in Model.Products)
        {
            <li class="product-row @(product.Stock == 0 ? "product-row--sold" : "")">
                <span class="product-tile" aria-hidden="true" style="background: @product.TileColor">@product.Emoji</span>
                <div class="product-row__info">
                    <span class="product-row__name">@product.Name</span>
                    <span class="product-row__price">@product.DisplayPrice</span>
                </div>
                @{
                    string badgeText;
                    string badgeMod;
                    if (product.Stock == 0) { badgeText = "Sold out"; badgeMod = "product-badge--negative"; }
                    else if (product.Stock < 50) { badgeText = "Few left"; badgeMod = "product-badge--warning"; }
                    else { badgeText = "In stock"; badgeMod = "product-badge--positive"; }
                }
                <span class="product-badge @badgeMod">@badgeText</span>
                @if (product.Stock > 0)
                {
                    <button type="button" class="button" onclick="widgetario.toast('Added! 🎉')">Buy</button>
                }
                else
                {
                    <button type="button" class="button" disabled>Buy</button>
                }
            </li>
        }
    </ul>
}
```

- [ ] **Step 5: Build the default web image (no v2 file change needed for this image)**

The default `widgetario/web:26.06` image does NOT include `v2/Index.cshtml` — that's only the `Dockerfile.v2`. We need to test BOTH:

```
docker compose -f docker-compose.yml -f docker-compose-build.yml build web
docker compose -f docker-compose.yml -f docker-compose-build.yml up -d --force-recreate web
sleep 8
curl -fsS http://localhost:8080/ -o /tmp/widgetario.html
grep -c '/js/toast.js' /tmp/widgetario.html
grep -c 'widgetario.toast' /tmp/widgetario.html
```
Expected: first grep = 1 (toast script loaded site-wide). Second grep = 0 (no Buy buttons in the default Index — `onclick="widgetario.toast(...)"` is only in v2).

- [ ] **Step 6: Build & verify the v2 variant**

```
docker compose -f docker-compose-build.yml build web-v2
```
(The web-v2 service in `docker-compose-build.yml` uses `Dockerfile.v2` which COPYs `v2/Index.cshtml` into the build.)

Run it standalone to verify:
```
docker run -d --rm --name widgetario-web-v2-check \
  --network src_app-net \
  -p 18080:80 \
  -e Widgetario__ProductsApi__Url=http://products-api/products \
  -e Widgetario__StockApi__Url=http://stock-api:8080/stock \
  widgetario/web:26.06-v2
sleep 8
curl -fsS http://localhost:18080/ -o /tmp/widgetario-v2.html
grep -c 'widgetario.toast' /tmp/widgetario-v2.html
docker stop widgetario-web-v2-check
```
Expected: grep returns at least 1 (in-stock products get a Buy button wired to the toast).

- [ ] **Step 7: Commit**

```
git add src/web/dotnet/Widgetario.Web/wwwroot/js/toast.js \
        src/web/dotnet/Widgetario.Web/Views/Shared/_Layout.cshtml \
        src/web/dotnet/v2/Index.cshtml \
        src/web/dotnet/Widgetario.Web/wwwroot/css/components.css
git commit -m "Add toast & v2 Buy button wire-up"
```

---

## Task 10: Final visual verification (user eyeball + cleanup)

**Files:** None — verification only. Any fixes go into a single follow-up commit if needed.

- [ ] **Step 1: Confirm the default stack is still up**

```
docker compose -f /Users/elton/scm/github/sixeyed/widgetario/src/docker-compose.yml \
               -f /Users/elton/scm/github/sixeyed/widgetario/src/docker-compose-build.yml ps
```
Expected: products-db, products-api, stock-api, web all running.

- [ ] **Step 2: Hit each endpoint to confirm nothing regressed**

```
curl -fsS -o /dev/null -w "web   /         HTTP %{http_code}\n" http://localhost:8080/
curl -fsS -o /dev/null -w "api   /products HTTP %{http_code}\n" http://localhost:8081/products
curl -fsS -o /dev/null -w "stock /stock/1  HTTP %{http_code}\n" http://localhost:8082/stock/1
```
Expected: all 200.

- [ ] **Step 3: Page-source sanity sweep**

```
curl -fsS http://localhost:8080/ -o /tmp/widgetario.html
echo "--- Markers ---"
for marker in "Inter Tight" "Archivo Black" "data-theme-toggle" "Tools for builders." "product-tile" "product-row__info" "product-badge--" "app-footer"; do
  c=$(grep -c "$marker" /tmp/widgetario.html)
  echo "$marker → $c"
done
echo "--- Absences (these should all be 0) ---"
for marker in "bootstrap" "jquery" "<table"; do
  c=$(grep -c "$marker" /tmp/widgetario.html)
  echo "$marker → $c"
done
```
Expected: every "Markers" line ≥ 1, every "Absences" line = 0.

- [ ] **Step 4: User visual check**

Open `http://localhost:8080` in a browser and confirm:
- Yellow page with a sticky header (logo + "Widgetario" wordmark on left, theme toggle ☀ on right).
- Hero with eyebrow pill, big black headline, muted tagline.
- Product rows: colored emoji tiles (slightly rotated), name in heavy display font, price muted, badge on right.
- Clicking the toggle flips to dark mode (yellow text on dark background); refresh persists the choice.
- Black footer band at the bottom.

If anything looks wrong, fix in the offending source file and commit:
```
git commit -m "Fix design-system visual regressions"
```
If everything looks right, no commit needed for this task.

- [ ] **Step 5 (optional): Verify v2 buy + toast in the same browser session**

Bring up the v2 image temporarily (see Task 9 Step 6 for the docker-run command) and load `http://localhost:18080`. Click a Buy button on an in-stock product — a yellow toast should slide in from bottom-right, say "Added! 🎉", and dismiss after ~2s. Clicking quickly across products should replace the toast text without stacking.

Tear down with `docker stop widgetario-web-v2-check`.

---

## Self-Review

1. **Spec coverage:**
   - Tokens (color light + dark, type, spacing, radius, shadow, motion, tile palette) → Task 1 ✓
   - Bootstrap + jQuery + old themes deleted → Task 2 ✓
   - `Emoji` + `TileColor` on Product → Task 3 ✓
   - `_Header.cshtml` + `_Footer.cshtml` + sticky header + theme toggle markup → Task 4 ✓
   - `theme.js` (localStorage, no-flash, aria-pressed) → Task 5 (with the no-flash inline script landing in Task 1's _Layout) ✓
   - Hero + eyebrow + headline + tag + env chip → Task 6 ✓
   - Product list + row + tile (with TileColor + Emoji) → Task 7 ✓
   - Badge variants (positive/warning/negative) + Button (hover/active/focus/disabled) → Task 8 ✓
   - Toast (single instance, slide+fade, auto-dismiss 2s) + v2 Buy wire-up → Task 9 ✓
   - Final integration check → Task 10 ✓
   - Mobile breakpoints → folded into Tasks 4 (header), 6 (hero), 7 (row) ✓
   - Accessibility (focus rings, aria-label, aria-pressed, aria-hidden on tile) → folded into the components.css + cshtml across Tasks 4, 7, 8 ✓

2. **Placeholder scan:** None. Every step has exact file content, exact commands, expected output.

3. **Type consistency:**
   - `product.Emoji` and `product.TileColor` defined in Task 3, used in Tasks 7, 8, 9 — same names. ✓
   - `[data-theme-toggle]` and `[data-theme-icon]` attribute selectors defined in Task 4 markup, consumed in Task 5 theme.js. ✓
   - `widgetario.toast(message)` defined in Task 9 toast.js, called in Task 9 v2/Index.cshtml. ✓
   - CSS class names consistent: `.product-row`, `.product-row__info`, `.product-row--sold`, `.product-tile`, `.product-badge`, `.product-badge--positive`/`--warning`/`--negative`, `.button`, `.toast`, `.toast--enter`/`--exit`. All BEM-style, all match between markup and CSS. ✓
