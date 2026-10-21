<script setup>
import { computed, ref } from 'vue'

const screen = ref('list')
const search = ref('')
const notice = ref('')
const login = ref('')
const password = ref('')
const form = ref({ title: '', siteId: 2, description: '' })
const nextId = ref(3)

const requests = ref([
  {
    id: 1,
    number: 'VR-00001',
    title: 'Планирование отпусков участка связи',
    siteId: 2,
    description: 'Подготовить проект графика отпусков работников участка связи.',
    status: 'New',
  },
  {
    id: 2,
    number: 'VR-00002',
    title: 'Проверка обеспеченности смен',
    siteId: 1,
    description: 'Проверить явочную численность на период летних отпусков.',
    status: 'InProgress',
  },
])

const statusLabels = {
  New: 'Новая',
  InProgress: 'В работе',
  Closed: 'Завершена',
  Cancelled: 'Отменена',
}

const filteredRequests = computed(() => {
  const query = search.value.trim().toLowerCase()
  if (!query) return requests.value
  return requests.value.filter((item) =>
    [item.number, item.title, String(item.siteId), statusLabels[item.status]]
      .some((value) => value.toLowerCase().includes(query)),
  )
})

function showScreen(value) {
  screen.value = value
  notice.value = ''
}

function createRequest() {
  const title = form.value.title.trim()
  const description = form.value.description.trim()
  const siteId = Number(form.value.siteId)

  if (title.length < 5 || title.length > 80) {
    notice.value = 'Тема должна содержать от 5 до 80 символов.'
    return
  }
  if (!Number.isInteger(siteId) || siteId < 1) {
    notice.value = 'Укажите корректный номер участка.'
    return
  }
  if (description.length < 10 || description.length > 500) {
    notice.value = 'Описание должно содержать от 10 до 500 символов.'
    return
  }

  requests.value.unshift({
    id: nextId.value,
    number: `VR-${String(nextId.value).padStart(5, '0')}`,
    title,
    siteId,
    description,
    status: 'New',
  })
  nextId.value += 1
  form.value = { title: '', siteId: 2, description: '' }
  notice.value = 'Заявка добавлена в демонстрационный список. Данные пока не отправляются на сервер.'
  screen.value = 'list'
}

function signIn() {
  if (!login.value.trim() || !password.value) {
    notice.value = 'Заполните логин и пароль.'
    return
  }
  notice.value = 'Экран входа работает в демонстрационном режиме: серверная авторизация будет добавлена позже.'
}
</script>

<template>
  <div class="app-shell">
    <aside class="sidebar">
      <a class="brand" href="#" @click.prevent="showScreen('list')" aria-label="На главную">
        <span class="brand-mark"><span></span><span></span><span></span></span>
        <span class="brand-copy"><strong>ПланГрафик</strong><small>Система управления</small></span>
      </a>

      <div class="workspace-label">РАБОЧЕЕ ПРОСТРАНСТВО</div>
      <nav class="navigation" aria-label="Главная навигация">
        <button class="nav-item" :class="{ active: screen === 'list' }" type="button" @click="showScreen('list')">
          <span class="nav-icon">▤</span><span>Заявки</span><span class="nav-count">{{ requests.length }}</span>
        </button>
        <button class="nav-item" :class="{ active: screen === 'new' }" type="button" @click="showScreen('new')">
          <span class="nav-icon plus-icon">＋</span><span>Создать заявку</span>
        </button>
      </nav>

      <div class="sidebar-bottom">
        <div class="help-card">
          <div class="help-icon">?</div>
          <strong>Нужна помощь?</strong>
          <p>Рабочее пространство для планирования отпусков.</p>
        </div>
        <button class="profile-button" type="button" @click="showScreen('login')">
          <span class="avatar">МК</span>
          <span class="profile-copy"><strong>Мария Крец</strong><small>Кадровый специалист</small></span>
          <span class="profile-more">···</span>
        </button>
      </div>
    </aside>

    <main class="main-content">
      <header class="topbar">
        <div class="breadcrumbs"><span>Рабочее пространство</span><span class="crumb-separator">/</span><strong>{{ screen === 'list' ? 'Заявки' : screen === 'new' ? 'Создание заявки' : 'Вход в систему' }}</strong></div>
        <div class="topbar-right"><span class="environment"><i></i> Демо-режим</span><button class="top-avatar" type="button" aria-label="Открыть вход" @click="showScreen('login')">МК</button></div>
      </header>

      <section v-if="screen === 'list'" class="page-content">
        <div class="page-heading">
          <div><div class="eyebrow">ПЛАНИРОВАНИЕ ПЕРСОНАЛА</div><h1>Заявки на планирование</h1><p class="page-subtitle">Управляйте заявками на отпуск и контролируйте готовность графика.</p></div>
          <button class="primary-button" type="button" @click="showScreen('new')"><span>＋</span> Создать заявку</button>
        </div>

        <div class="summary-grid">
          <article class="summary-card"><div class="summary-top"><span>Всего заявок</span><span class="summary-icon blue">▤</span></div><div class="summary-value">{{ requests.length }}</div><div class="summary-note">В текущем списке</div></article>
          <article class="summary-card"><div class="summary-top"><span>Новые</span><span class="summary-icon violet">✳</span></div><div class="summary-value">{{ requests.filter((item) => item.status === 'New').length }}</div><div class="summary-note">Ожидают обработки</div></article>
          <article class="summary-card"><div class="summary-top"><span>В работе</span><span class="summary-icon orange">◷</span></div><div class="summary-value">{{ requests.filter((item) => item.status === 'InProgress').length }}</div><div class="summary-note">На этапе планирования</div></article>
          <article class="summary-card"><div class="summary-top"><span>Завершены</span><span class="summary-icon green">✓</span></div><div class="summary-value">{{ requests.filter((item) => item.status === 'Closed').length }}</div><div class="summary-note">Готовые результаты</div></article>
        </div>

        <section class="panel">
          <div class="panel-heading"><div><h2>Все заявки</h2><p>Список заявок на планирование отпусков</p></div><label class="search-box"><span>⌕</span><input v-model="search" type="search" placeholder="Поиск по заявкам…" aria-label="Поиск по заявкам" /></label></div>
          <div class="table-wrap">
            <table>
              <thead><tr><th>НОМЕР</th><th>ТЕМА ЗАЯВКИ</th><th>УЧАСТОК</th><th>СТАТУС</th><th></th></tr></thead>
              <tbody>
                <tr v-for="item in filteredRequests" :key="item.id">
                  <td><span class="request-number">{{ item.number }}</span></td>
                  <td><div class="request-title">{{ item.title }}</div><div class="request-description">{{ item.description }}</div></td>
                  <td><span class="site-chip">Участок {{ item.siteId }}</span></td>
                  <td><span class="status-pill" :class="`status-${item.status.toLowerCase()}`"><i></i>{{ statusLabels[item.status] }}</span></td>
                  <td><button class="row-more" type="button" :aria-label="`Подробнее о заявке ${item.number}`" @click="notice = `${item.number}: ${item.description}`">···</button></td>
                </tr>
                <tr v-if="filteredRequests.length === 0"><td colspan="5" class="empty-state">Заявки не найдены. Попробуйте изменить запрос.</td></tr>
              </tbody>
            </table>
          </div>
          <div class="table-footer"><span>Показано {{ filteredRequests.length }} из {{ requests.length }} заявок</span><span class="demo-label"><i></i> Тестовые данные</span></div>
        </section>
        <p v-if="notice" class="notice" role="status">{{ notice }}</p>
      </section>

      <section v-else-if="screen === 'new'" class="page-content form-page">
        <div class="page-heading"><div><div class="eyebrow">НОВАЯ ЗАПИСЬ</div><h1>Создать заявку</h1><p class="page-subtitle">Заполните основные сведения для планирования отпусков.</p></div><button class="secondary-button" type="button" @click="showScreen('list')">← К списку</button></div>
        <div class="form-layout">
          <section class="panel form-panel"><div class="panel-heading"><div><h2>Основная информация</h2><p>Поля со звёздочкой обязательны для заполнения</p></div></div>
            <form class="request-form" @submit.prevent="createRequest">
              <div class="field-group"><label for="title">Тема заявки <span>*</span></label><input id="title" v-model="form.title" name="title" type="text" minlength="5" maxlength="80" placeholder="Например, планирование отпусков участка связи" autofocus required /><small>От 5 до 80 символов</small></div>
              <div class="field-group"><label for="siteId">Номер участка <span>*</span></label><input id="siteId" v-model.number="form.siteId" name="siteId" type="number" min="1" step="1" required /><small>Идентификатор участка из справочника</small></div>
              <div class="field-group"><label for="description">Описание <span>*</span></label><textarea id="description" v-model="form.description" name="description" minlength="10" maxlength="500" rows="5" placeholder="Опишите задачу и ожидаемый результат…" required></textarea><small>{{ form.description.length }} / 500 символов · минимум 10</small></div>
              <p v-if="notice" class="notice" role="status">{{ notice }}</p>
              <div class="form-actions"><button class="secondary-button" type="button" @click="showScreen('list')">Отмена</button><button class="primary-button" type="submit">Создать заявку <span>→</span></button></div>
            </form>
          </section>
          <aside class="hint-card"><div class="hint-symbol">✦</div><h3>Перед созданием</h3><p>Проверьте, что тема отражает цель заявки, а номер участка соответствует справочнику предприятия.</p><div class="hint-divider"></div><div class="hint-line"><span>Статус после создания</span><strong class="status-pill status-new"><i></i>Новая</strong></div><div class="hint-line"><span>Номер заявки</span><strong>Назначается автоматически</strong></div><div class="hint-caption">В этой лабораторной данные добавляются только в демонстрационный список браузера.</div></aside>
        </div>
      </section>

      <section v-else class="page-content login-page">
        <div class="login-card panel"><div class="login-brand"><span class="brand-mark"><span></span><span></span><span></span></span></div><div class="eyebrow">РАБОЧЕЕ ПРОСТРАНСТВО</div><h1>С возвращением</h1><p class="page-subtitle">Войдите, чтобы продолжить работу с заявками.</p>
          <form class="request-form" @submit.prevent="signIn"><div class="field-group"><label for="login">Логин <span>*</span></label><input id="login" v-model="login" name="login" autocomplete="username" placeholder="Введите логин" required autofocus /></div><div class="field-group"><label for="password">Пароль <span>*</span></label><input id="password" v-model="password" name="password" type="password" autocomplete="current-password" placeholder="Введите пароль" required /></div><button class="primary-button full-width" type="submit">Войти в систему <span>→</span></button><p v-if="notice" class="notice" role="status">{{ notice }}</p></form>
          <button class="back-link" type="button" @click="showScreen('list')">← Вернуться к заявкам</button><p class="login-disclaimer">Демонстрационный экран. Проверка учётных данных пока не подключена.</p>
        </div>
      </section>
      <footer class="page-footer"><span>ПланГрафик · Учебный проект</span><span>Лабораторная работа № 3 · Vue 3</span></footer>
    </main>
  </div>
</template>
