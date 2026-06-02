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
