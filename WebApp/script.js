(function () {
  'use strict';

  var API_BASE = ''; // тот же домен
  var API_URL = API_BASE + '/api/heroes';
  var LIMIT = 10;                // строк в таблице по умолчанию
  var ALL_LIMIT = 200;           // лимит для «Показать всех героев»

  // ---------- контракт с API ----------
  // Единственное место, где страница знает имена полей JSON.
  // Если в вашем классе Hero свойства называются иначе, поправьте только эту функцию.
  // API отдаёт винрейт/пикрейт и их приросты долями (0.52), на странице показываем в процентах (52%)
  function num(v) { var x = Number(v); return isFinite(x) ? x : 0; }
  function toRow(h) {
    return {
      id: h.id,
      img: API_BASE + '/api/heroes/' + encodeURIComponent(h.id) + '/avatar.webp',
      name: String(h.name),
      role: h.role,
      wr: num(h.winRate) * 100,
      pr: num(h.pickRate) * 100,
      rating: Math.round(num(h.rating) * 100000),
      dwr: num(h.winRateDelta) * 100,
      dpr: num(h.pickRateDelta) * 100,
      drt: num(h.ratingDelta)
    };
  }

  function query(o) {
    var p = new URLSearchParams();
    var ranks = Array.isArray(o.ranks) ? o.ranks : (o.rank ? [o.rank] : []);
    var roles = Array.isArray(o.roles) ? o.roles : (o.role ? [o.role] : []);
    if (ranks.length) p.set('rank', ranks.join(','));
    if (roles.length) p.set('role', roles.join(','));
    p.set('sort', o.sort);
    p.set('dir', o.dir);
    p.set('limit', String(o.limit));
    return p.toString();
  }

  // Вся фильтрация, сортировка и расчёты выполняются на сервере.
  async function getHeroes(o, signal) {
    var res = await fetch(API_URL + '?' + query(o), { signal: signal, headers: { 'Accept': 'application/json' } });
    if (!res.ok) throw new Error('HTTP ' + res.status);
    var data = await res.json();
    return {
      total: num(data.total),
      updatedAt: data.updatedAt,
      items: (data.items || []).map(toRow)
    };
  }

  // ---------- отображение ----------
  function esc(v) {
    return String(v).replace(/[&<>"']/g, function (c) {
      return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c];
    });
  }
  function seedOf(s) {
    var h = 2166136261;
    for (var i = 0; i < s.length; i++) { h ^= s.charCodeAt(i); h = Math.imul(h, 16777619); }
    return h >>> 0;
  }
  function initials(name) {
    var parts = name.split(/[\s-]+/);
    var s = parts.length > 1 ? parts[0][0] + parts[1][0] : name.slice(0, 2);
    return s.toUpperCase();
  }
  function avatar(name, img) {
    var base = '<span class="av" style="--h:' + (seedOf(name) % 360) + '"><i>' + esc(initials(name)) + '</i>';
    if (!img) return base + '</span>';
    return base + '<img src="' + esc(img) + '" alt="" loading="lazy" onerror="this.remove()"></span>';
  }
  function signed(v, suffix) {
    var s = Math.abs(v).toFixed(2) + suffix;
    return v < 0 ? '-' + s : '+' + s;
  }
  function signedInt(v) { return (v < 0 ? '-' : '+') + Math.abs(Math.round(v)); }
  function cls(v) { return v > 0 ? 'pos' : (v < 0 ? 'neg' : 'zero'); }

  function setStamp(iso) {
    var d = new Date(iso);
    if (!iso || isNaN(d.getTime())) return;
    document.getElementById('stamp').textContent =
      d.toLocaleString('ru-RU', { day: 'numeric', month: 'short', year: 'numeric', hour: '2-digit', minute: '2-digit' }).replace(' г.', '');
  }

  // ---------- состояние страницы ----------
  // Это только выбор кнопок. Серверу уходит каждый раз целиком.
  // Ранги и роли — массивы: можно выбрать несколько значений сразу.
  var state = { ranks: [], roles: [], sort: 'rating', dir: 'desc', all: false };

  function toggleValue(list, value) {
    var i = list.indexOf(value);
    if (i === -1) list.push(value); else list.splice(i, 1);
    return list;
  }

  function rowHtml(s, i) {
    var trend = s.drt >= 0 ? 'up' : 'down';
    return '<tr>' +
      '<td><div class="hero"><span class="num">' + (i + 1) + '</span>' + avatar(s.name, s.img) +
      '<span class="nm">' + esc(s.name) + '</span></div></td>' +
      '<td><span class="rt">' + s.rating.toLocaleString('ru-RU', { maximumFractionDigits: 0 }) + '<span class="tri ' + trend + '"></span></span></td>' +
      '<td><span class="' + (s.wr > 50 ? 'pos' : (s.wr < 50 ? 'neg' : 'zero')) + '">' + s.wr.toFixed(2) + '%</span></td>' +
      '<td>' + s.pr.toFixed(2) + '%</td>' +
      '<td class="' + cls(s.dwr) + '">' + signed(s.dwr, '%') + '</td>' +
      '<td class="' + cls(s.dpr) + '">' + signed(s.dpr, '%') + '</td>' +
      '<td class="' + cls(s.drt) + '">' + signedInt(s.drt) + '</td>' +
      '</tr>';
  }

  function drawRows(data) {
    var html = data.items.map(rowHtml).join('');
    if (!data.items.length) html = '<tr><td class="empty" colspan="7">Героев по этим фильтрам нет.</td></tr>';
    document.getElementById('rows').innerHTML = html;
    document.getElementById('count').textContent = 'Показано ' + data.items.length + ' из ' + data.total;
    var more = document.getElementById('more');
    more.hidden = data.total <= LIMIT;
    more.textContent = state.all ? 'Свернуть до ' + LIMIT : 'Показать всех героев';
    setStamp(data.updatedAt);
  }

  function showTableError() {
    document.getElementById('rows').innerHTML =
      '<tr><td class="empty" colspan="7">Не удалось загрузить данные. <button class="more" type="button" id="retry">Повторить</button></td></tr>';
    document.getElementById('count').textContent = '';
    document.getElementById('more').hidden = true;
  }

  var tableCtl = null;
  async function loadRows() {
    if (tableCtl) tableCtl.abort();          // быстрые клики: старый запрос отменяем
    var ctl = tableCtl = new AbortController();
    var board = document.querySelector('.board');
    board.classList.add('loading');
    board.setAttribute('aria-busy', 'true');
    try {
      var data = await getHeroes({
        ranks: state.ranks, roles: state.roles, sort: state.sort, dir: state.dir,
        limit: state.all ? ALL_LIMIT : LIMIT
      }, ctl.signal);
      drawRows(data);
    } catch (e) {
      if (e.name === 'AbortError') return;
      showTableError();
    } finally {
      if (tableCtl === ctl) { board.classList.remove('loading'); board.removeAttribute('aria-busy'); }
    }
  }

  function renderControls() {
    document.querySelectorAll('[data-rank]').forEach(function (b) {
      b.setAttribute('aria-pressed', String(state.ranks.indexOf(b.getAttribute('data-rank')) !== -1));
    });
    document.querySelectorAll('[data-role]').forEach(function (b) {
      b.setAttribute('aria-pressed', String(state.roles.indexOf(b.getAttribute('data-role')) !== -1));
    });
    document.querySelectorAll('[data-sort]').forEach(function (b) {
      var on = b.getAttribute('data-sort') === state.sort;
      b.setAttribute('aria-pressed', String(on));
      if (on) b.setAttribute('data-dir', state.dir); else b.removeAttribute('data-dir');
    });
    document.querySelectorAll('th[data-th]').forEach(function (th) {
      var on = th.getAttribute('data-th') === state.sort;
      th.setAttribute('aria-sort', on ? (state.dir === 'desc' ? 'descending' : 'ascending') : 'none');
    });
  }

  function render() { renderControls(); loadRows(); }

  // ---------- маршрутизация ----------
  var VIEWS = ['heroes', 'cabinet', 'about'];
  function currentView() {
    var v = (location.hash || '').slice(1);
    return VIEWS.indexOf(v) === -1 ? 'heroes' : v;
  }
  function route() {
    var v = currentView();
    VIEWS.forEach(function (x) { document.getElementById('view-' + x).hidden = x !== v; });
    document.querySelectorAll('.nav a').forEach(function (a) {
      if (a.getAttribute('data-view') === v) a.setAttribute('aria-current', 'page'); else a.removeAttribute('aria-current');
    });
    document.getElementById('heroband').hidden = v !== 'heroes';
  }

  // ---------- события ----------
  document.addEventListener('click', function (e) {
    var t = e.target;
    if (!t.closest) return;
    var b;
    if ((b = t.closest('[data-rank]'))) {
      toggleValue(state.ranks, b.getAttribute('data-rank'));
      render();
    } else if ((b = t.closest('[data-role]'))) {
      toggleValue(state.roles, b.getAttribute('data-role'));
      render();
    } else if ((b = t.closest('[data-sort]'))) {
      var s = b.getAttribute('data-sort');
      if (state.sort === s) state.dir = state.dir === 'desc' ? 'asc' : 'desc';
      else { state.sort = s; state.dir = 'desc'; }
      render();
    } else if (t.closest('#reset')) {
      state = { ranks: [], roles: [], sort: 'rating', dir: 'desc', all: false };
      render();
    } else if (t.closest('#more')) {
      state.all = !state.all;
      loadRows();
    } else if (t.closest('#retry')) {
      loadRows();
    } else if (t.closest('#refresh')) {
      var btn = document.getElementById('refresh');
      btn.classList.remove('spin');
      void btn.offsetWidth;
      btn.classList.add('spin');
      loadRows();
    }
  });

  window.addEventListener('hashchange', route);

  render();
  route();
})();
