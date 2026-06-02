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
