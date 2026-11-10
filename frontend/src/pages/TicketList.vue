<script setup>
import { computed, onMounted, ref } from 'vue'
import { getTickets } from '../api/tickets'
import { items, setItems } from '../ticketsStore'
import TicketTable from '../components/TicketTable.vue'

const screen = ref('loading')
const errorMessage = ref('')

const newCount = computed(() => items.value.filter((item) => item.status === 'New').length)
const inProgressCount = computed(() => items.value.filter((item) => item.status === 'InProgress').length)
const closedCount = computed(() => items.value.filter((item) => item.status === 'Closed').length)

async function loadTickets() {
  screen.value = 'loading'
  errorMessage.value = ''

  try {
    const tickets = await getTickets()
    setItems(tickets)
    screen.value = tickets.length === 0 ? 'empty' : 'data'
  } catch (error) {
    screen.value = 'error'
    errorMessage.value = error.status
      ? `Сервер вернул ошибку ${error.status}. Попробуйте повторить запрос позже.`
      : 'Нет связи с сервером. Проверьте подключение и адрес API.'
  }
}

onMounted(loadTickets)
</script>

<template>
  <section class="page-content">
    <div class="page-heading">
      <div><div class="eyebrow">ПЛАНИРОВАНИЕ ПЕРСОНАЛА</div><h1>Заявки на планирование</h1><p class="page-subtitle">Управляйте заявками на отпуск и контролируйте готовность графика.</p></div>
      <RouterLink class="primary-button" to="/new"><span>＋</span> Создать заявку</RouterLink>
    </div>

    <div v-if="screen === 'loading'" class="panel api-state" role="status">
      <span class="state-spinner" aria-hidden="true"></span>
      <strong>Загрузка заявок…</strong>
      <p>Получаем данные из API</p>
    </div>

    <div v-else-if="screen === 'error'" class="panel api-state api-error" role="alert">
      <div class="state-symbol">!</div>
      <strong>Не удалось загрузить заявки</strong>
      <p>{{ errorMessage }}</p>
      <button class="secondary-button" type="button" @click="loadTickets">Повторить запрос</button>
    </div>

    <div v-else-if="screen === 'empty'" class="panel api-state" role="status">
      <div class="state-symbol">▤</div>
      <strong>Записей пока нет</strong>
      <p>Создайте первую заявку, чтобы она появилась в этом списке.</p>
      <RouterLink class="primary-button" to="/new">Создать заявку</RouterLink>
    </div>

    <template v-else>
      <div class="summary-grid">
        <article class="summary-card"><div class="summary-top"><span>Всего заявок</span><span class="summary-icon blue">▤</span></div><div class="summary-value">{{ items.length }}</div><div class="summary-note">В текущем списке</div></article>
        <article class="summary-card"><div class="summary-top"><span>Новые</span><span class="summary-icon violet">✳</span></div><div class="summary-value">{{ newCount }}</div><div class="summary-note">Ожидают обработки</div></article>
        <article class="summary-card"><div class="summary-top"><span>В работе</span><span class="summary-icon orange">◷</span></div><div class="summary-value">{{ inProgressCount }}</div><div class="summary-note">На этапе планирования</div></article>
        <article class="summary-card"><div class="summary-top"><span>Завершены</span><span class="summary-icon green">✓</span></div><div class="summary-value">{{ closedCount }}</div><div class="summary-note">Готовые результаты</div></article>
      </div>
      <TicketTable :items="items" />
    </template>
  </section>
</template>
