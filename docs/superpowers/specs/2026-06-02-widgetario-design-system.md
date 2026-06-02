# Widgetario Design System — Design

**Date:** 2026-06-02
**Status:** Draft, pending user review
**Owner:** Elton Stoneman
**Targets:** `src/web/dotnet` (.NET 10, ASP.NET Core MVC)

## Goal

Replace the bare-Bootstrap UI of the Widgetario web service with a coherent, opinionated design system in the "Playful Pop" / neobrutalist style: bright yellow + pink + black, chunky borders, drop-shadow stamps, characterful typography. Single-page scope — header, hero, product list, footer — but built so further pages would inherit the system cleanly.

## Brand Direction

**Playful Pop / neobrutalist.** Bright primary yellow (page) + magenta-pink (accent) + black ink. Chunky 2–3 px black borders on every surface. Drop-shadow "stamp" effect on cards and buttons. Slightly rotated product tiles for energy. Reads as fun, consumer, loud — fits a sample/demo store with personality.

## In Scope

- **`src/web/dotnet`** only:
  - `Views/Shared/_Layout.cshtml` (new fonts, drop Bootstrap, partials for header/footer)
  - `Views/Home/Index.cshtml` (rewrite to new components)
  - `v2/Index.cshtml` (rewrite — adds Buy button + toast)
  - `Models/Product.cs` (add `Emoji` + `TileColor` computed properties)
  - New `Views/Shared/_Header.cshtml`, `_Footer.cshtml` partials
  - New `wwwroot/css/tokens.css`, `wwwroot/css/components.css`
  - New `wwwroot/js/theme.js`, `wwwroot/js/toast.js`
  - Replace `wwwroot/css/site.css` with a 3-line `@import` shell
  - Delete `wwwroot/css/themes/light.css`, `themes/dark.css`
  - Delete `wwwroot/lib/bootstrap/`, `wwwroot/lib/jquery*`, `wwwroot/lib/jquery-validation*`
  - Delete `Views/Shared/_ValidationScriptsPartial.cshtml`

## Out of Scope

- Backend services (products-api, stock-api, db) — untouched.
- New routes / pages beyond Index.cshtml + v2/Index.cshtml.
- Real cart state or any HTTP mutation on Buy click.
- Real product images. Emoji is the visual.
- Screen-reader `aria-live` plumbing for the toast (visual demo only — documented as follow-up).
- Build tooling. No Tailwind, no Open Props, no Node step.

## File Structure

```
src/web/dotnet/
├── Widgetario.Web/
│   ├── Views/
│   │   ├── Shared/
│   │   │   ├── _Layout.cshtml          [modified]
│   │   │   ├── _Header.cshtml          [new]
│   │   │   ├── _Footer.cshtml          [new]
│   │   │   ├── _ValidationScriptsPartial.cshtml   [DELETED]
│   │   │   └── Error.cshtml            [unchanged]
│   │   └── Home/
│   │       └── Index.cshtml            [rewritten]
│   ├── Models/
│   │   └── Product.cs                  [modified — add Emoji, TileColor]
│   └── wwwroot/
│       ├── css/
│       │   ├── tokens.css              [new]
│       │   ├── components.css          [new]
│       │   ├── site.css                [emptied → 3-line @import shell]
│       │   └── themes/                 [DELETED dir]
│       ├── js/
│       │   ├── theme.js                [new]
│       │   ├── toast.js                [new]
│       │   └── site.js                 [unchanged or emptied]
│       └── lib/
│           ├── bootstrap/              [DELETED dir]
│           ├── jquery/                 [DELETED dir]
│           └── jquery-validation*/     [DELETED dirs]
└── v2/
    └── Index.cshtml                    [rewritten]
```

## Design Tokens (`wwwroot/css/tokens.css`)

CSS custom properties on `:root` for light, `[data-theme="dark"]` for dark.

### Color — light (default `:root`)
| Token | Value | Used by |
|---|---|---|
| `--color-bg` | `#ffeb6b` | page background |
| `--color-surface` | `#ffffff` | cards, rows, footer text panel |
| `--color-surface-alt` | `#fff9d4` | header band (slightly tinted) |
| `--color-ink` | `#1a1a1a` | text, borders, shadow stamp |
| `--color-ink-muted` | `#5a5a5a` | secondary text, prices |
| `--color-accent` | `#ff5fa2` | Buy button fill, Sold-out badge fill |
| `--color-accent-ink` | `#1a1a1a` | text on accent surfaces |
| `--color-positive` | `#1a1a1a` | "In stock" badge fill |
| `--color-positive-ink` | `#ffeb6b` | "In stock" badge text |
| `--color-warning` | `#fb923c` | "Few left" badge fill |
| `--color-warning-ink` | `#1a1a1a` | text on warning |

### Color — dark (`[data-theme="dark"]`)
| Token | Value |
|---|---|
| `--color-bg` | `#1a1a1a` |
| `--color-surface` | `#262626` |
| `--color-surface-alt` | `#1f1f1f` |
| `--color-ink` | `#ffeb6b` (yellow becomes ink) |
| `--color-ink-muted` | `#b8b8b8` |
| `--color-accent` | `#ff5fa2` (unchanged) |
| `--color-accent-ink` | `#1a1a1a` |
| `--color-positive` | `#ffeb6b` |
| `--color-positive-ink` | `#1a1a1a` |
| `--color-warning` | `#fb923c` |
| `--color-warning-ink` | `#1a1a1a` |

### Tile palette (vivid in both modes — not inverted)
`--tile-1: #ff5fa2` · `--tile-2: #7cdfff` · `--tile-3: #a3e635` · `--tile-4: #fb923c` · `--tile-5: #b794f4` · `--tile-6: #34d399`

### Typography
```
--font-display:    'Archivo Black', system-ui, sans-serif
--font-body:       'Inter Tight', system-ui, sans-serif
--text-xs:         0.75rem
--text-sm:         0.875rem
--text-base:       1rem
--text-lg:         1.25rem
--text-xl:         1.75rem
--text-2xl:        2.5rem
--leading-tight:   1.1
--leading-normal:  1.5
--tracking-tight:  -0.02em
--tracking-wide:   0.08em
```

Fonts loaded via Google Fonts `<link>` in `_Layout.cshtml` `<head>`:
```
https://fonts.googleapis.com/css2?family=Inter+Tight:wght@500;700&family=Archivo+Black&display=swap
```

### Spacing (8-pt scale)
`--space-1: 0.25rem` · `--space-2: 0.5rem` · `--space-3: 0.75rem` · `--space-4: 1rem` · `--space-5: 1.5rem` · `--space-6: 2rem` · `--space-7: 3rem` · `--space-8: 4rem`

### Radius
`--radius-sm: 4px` · `--radius-md: 8px` · `--radius-lg: 12px` · `--radius-pill: 999px`

### Shadow (the stamp)
`--shadow-stamp-sm: 3px 3px 0 var(--color-ink)`
`--shadow-stamp-md: 5px 5px 0 var(--color-ink)`
`--shadow-stamp-lg: 8px 8px 0 var(--color-ink)`
`--shadow-press: 1px 1px 0 var(--color-ink)` (pressed buttons)

### Border
`--border-thin: 2px solid var(--color-ink)`
`--border-thick: 3px solid var(--color-ink)`

### Motion
`--ease-out: cubic-bezier(0.16, 1, 0.3, 1)`
`--dur-fast: 120ms` · `--dur-base: 220ms` · `--dur-slow: 400ms`

## Component Inventory (`wwwroot/css/components.css`)

| Class root | Job | Notes |
|---|---|---|
| `.app-header` | Top bar — logo + wordmark + theme toggle | Sticky top. `--color-bg`, 3 px ink bottom border. |
| `.theme-toggle` | 44×44 square button, ☀/🌙 icon | Black border + stamp shadow. Toggles `<html data-theme>` + localStorage. `aria-label`, `aria-pressed`. |
| `.hero` | Title band below header | Eyebrow pill + display headline + tagline. Optional `.env-chip` on the right. |
| `.env-chip` | Small pill showing env name | Only renders if `ViewData["Environment"]` is set. Accent fill, ink text. |
| `.product-list` | Vertical flex stack | Max-width 880 px, centered, gap `--space-3`. |
| `.product-row` | One product as a horizontal row | White surface, 2 px ink border, stamp shadow. Hover translates -1 px. Layout: tile · info · badge · button. |
| `.product-tile` | 56×56 colored square with emoji | `border-radius: --radius-md`, 2 px ink border, `rotate(-3deg)`. Background = `--tile-N` from Product.TileColor. `aria-hidden`. |
| `.product-badge` | Pill, three variants | `.product-badge--positive`, `--warning`, `--negative`. Pill radius. |
| `.button` | Primary action button | Pink fill, ink text, ink border, stamp shadow. Hover: translate -1 px / shadow grows. Active: translate +2 px / shadow → `--shadow-press`. Focus: 3 px ink outline, 2 px offset. |
| `.button--ghost` | Transparent variant | Ink border, ink text, no fill. |
| `.toast` | Fixed bottom-right notification | 320 px max width. Yellow fill, ink border + stamp. Slides up + fades in via `--ease-out`. Auto-dismisses 2.4 s. One at a time (no stacking). |
| `.app-footer` | Bottom panel | Ink fill, `--color-bg` text. Centered single line: `© 2026 Widgetario · Stocked, shipped, sorted.` `--text-xs`, `--tracking-wide`. |

## Page Layout — Home/Index.cshtml

```
┌─ <header class="app-header"> ──────────────────────────┐
│  [logo] Widgetario                  [theme toggle]     │
├────────────────────────────────────────────────────────┤
│  <section class="hero">                                │
│    [WIDGETARIO]                       [PROD env chip?] │
│    Tools for builders.                                 │
│    Stocked, shipped, sorted.                           │
├────────────────────────────────────────────────────────┤
│  <main class="product-list">                           │
│   ┌─ .product-row ────────────────────────────────┐    │
│   │ [🔌] Arm64 SoC         [IN STOCK]   [Buy]    │    │
│   │      $30.00                                  │    │
│   └──────────────────────────────────────────────┘    │
│   ┌─ .product-row ────────────────────────────────┐    │
│   │ [🤖] IoT breakout board [IN STOCK]  [Buy]    │    │
│   │      $8.00                                   │    │
│   └──────────────────────────────────────────────┘    │
│   ...                                                  │
├────────────────────────────────────────────────────────┤
│  <footer class="app-footer">                           │
│  © 2026 WIDGETARIO · STOCKED, SHIPPED, SORTED          │
└────────────────────────────────────────────────────────┘
```

## Mobile (`@media (max-width: 640px)`)

- Header: logo + toggle only. Wordmark hides if it crowds (`display: none` below ~480 px).
- Hero headline: `--text-xl` (was `--text-2xl`).
- Product row stacks: row 1 = tile + name + price; row 2 = badge + button. Padding tightens to `--space-3`.

## Behaviour

### Theme toggle (`wwwroot/js/theme.js`)
**Resolution order at load:**
1. `localStorage["widgetario-theme"]` if present
2. Server-rendered `data-theme` on `<html>` (from `Widgetario:Theme` config via `ViewData["Theme"]`)
3. `window.matchMedia('(prefers-color-scheme: dark)')`

**No-flash inline `<script>`** in `_Layout.cshtml` `<head>` (before any CSS link):
```html
<script>
  (function () {
    var t = localStorage.getItem('widgetario-theme');
    if (!t) t = document.documentElement.dataset.theme;  // server seed
    if (!t) t = matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    document.documentElement.dataset.theme = t;
  })();
</script>
```

**Toggle button** (in `_Header.cshtml`): on click, reads current `<html data-theme>`, flips it, writes to localStorage, updates its icon + `aria-pressed`. Vanilla JS — `theme.js` is ~30 lines.

### Toast (`wwwroot/js/toast.js`)
- Exposes `window.widgetario.toast(message)`.
- Creates the `.toast` element on demand, single instance. If a toast is already showing, the existing element's timer is cancelled and its text is replaced (no stacking).
- Adds `.toast--enter` class for slide+fade in (uses `--dur-base` + `--ease-out`).
- Auto-dismisses at 2400 ms total: 2000 ms visible, 400 ms exit transition, then `remove()`.
- Buy buttons in `v2/Index.cshtml` get `onclick="widgetario.toast('Added! 🎉')"`.
- Loaded site-wide via `_Layout.cshtml` (~30 lines, harmless on pages without Buy).

### Product emoji + tile color (server-side in `Product.cs`)
**`Emoji` (read-only, computed):**
Lowercase-name keyword match, first hit wins:
```
"soc" | "cpu" | "chip"          → 🔌
"iot" | "breakout"              → 🤖
"dac" | "audio"                 → 🎛
"comms" | "radio" | "mars"      → 📡
"board"                         → 🧩
"sensor"                        → 📟
(no match)                      → 📦
```

**`TileColor` (read-only, computed):**
Sum of ASCII char codes in `Name`, modulo 6, mapped to a CSS variable:
```
0 → var(--tile-1)   (pink)
1 → var(--tile-2)   (sky)
2 → var(--tile-3)   (lime)
3 → var(--tile-4)   (orange)
4 → var(--tile-5)   (lavender)
5 → var(--tile-6)   (mint)
```
Property returns the CSS-var string literal; view uses it as `style="background: @Model.TileColor"`. Deterministic — same name → same color across renders.

### Environment chip
Renders in `.hero` (right side) only if `ViewData["Environment"]` is a non-empty string. Replaces the old `clearfix` aquamarine div. Small pill, accent fill, ink text, `--text-xs` + `--tracking-wide`.

## Accessibility

- All buttons: visible focus ring (3 px ink outline, 2 px offset).
- `.theme-toggle`: `aria-label="Toggle dark mode"`, `aria-pressed="true|false"` reflecting state.
- `.product-tile`: `aria-hidden="true"` (emoji is decorative; product name carries meaning).
- Contrast:
  - Ink on yellow (`#1a1a1a` on `#ffeb6b`): 16.2 : 1 — AAA
  - Yellow on dark ink (`#ffeb6b` on `#1a1a1a`): 16.2 : 1 — AAA
  - Pink button text: ink on `#ff5fa2` = 4.5 : 1 — AA for bold text
- Skipped (documented follow-up): `aria-live="polite"` toast region. The toast is currently visual only.

## Razor / cshtml mechanics

- Header + footer are partials (`_Header.cshtml`, `_Footer.cshtml`) called from `_Layout.cshtml`:
  ```cshtml
  @await Html.PartialAsync("_Header")
  ...
  @await Html.PartialAsync("_Footer")
  ```
- Both `Index.cshtml` files stay focused on `<main>` content. Each is ~30 lines.
- `Index.cshtml` default vs `v2/Index.cshtml`: only difference is v2 renders a `<button class="button" onclick="widgetario.toast('Added! 🎉')">Buy</button>` per in-stock product; default leaves that slot empty.

## Risks & Open Questions

- **Google Fonts CDN dependency:** the page now requires network access to fonts.googleapis.com at load. If the container runs in an offline demo environment this becomes a regression. Fallback: `font-family: system-ui, sans-serif` is already declared, so the page is usable without the fonts — just less characterful.
- **No-flash script trusts localStorage:** if a user manually corrupts localStorage, the script falls through to the server seed. Safe.
- **Bootstrap deletion** is irreversible by this work. Anyone using `Error.cshtml` (the scaffolded error page) inherits Bootstrap classes that now have no styles. Acceptable since Error.cshtml is rarely seen and degrades to unstyled text — but worth noting.
- **`v2/` directory placement:** `v2/Index.cshtml` is outside `Widgetario.Web/` and is copied into the build via `Dockerfile.v2`'s `COPY v2/Index.cshtml ./Views/Home/`. The new file must still live at `src/web/dotnet/v2/Index.cshtml` so the existing COPY line still works without Dockerfile changes.
