<script setup>
import { computed } from 'vue'
import { useTicketsStore } from '../ticketsStore'
import TicketTable from '../components/TicketTable.vue'

const { requests } = useTicketsStore()
const newCount = computed(() => requests.value.filter((item) => item.status === 'New').length)
const inProgressCount = computed(() => requests.value.filter((item) => item.status === 'InProgress').length)
const closedCount = computed(() => requests.value.filter((item) => item.status === 'Closed').length)
</script>

<template>
  <section class="page-content">
    <div class="page-heading">
      <div><div class="eyebrow">ПЛАНИРОВАНИЕ ПЕРСОНАЛА</div><h1>Заявки на планирование</h1><p class="page-subtitle">Управляйте заявками на отпуск и контролируйте готовность графика.</p></div>
      <RouterLink class="primary-button" to="/new"><span>＋</span> Создать заявку</RouterLink>
    </div>

    <div class="summary-grid">
      <article class="summary-card"><div class="summary-top"><span>Всего заявок</span><span class="summary-icon blue">▤</span></div><div class="summary-value">{{ requests.length }}</div><div class="summary-note">В текущем списке</div></article>
      <article class="summary-card"><div class="summary-top"><span>Новые</span><span class="summary-icon violet">✳</span></div><div class="summary-value">{{ newCount }}</div><div class="summary-note">Ожидают обработки</div></article>
      <article class="summary-card"><div class="summary-top"><span>В работе</span><span class="summary-icon orange">◷</span></div><div class="summary-value">{{ inProgressCount }}</div><div class="summary-note">На этапе планирования</div></article>
      <article class="summary-card"><div class="summary-top"><span>Завершены</span><span class="summary-icon green">✓</span></div><div class="summary-value">{{ closedCount }}</div><div class="summary-note">Готовые результаты</div></article>
    </div>

    <TicketTable :items="requests" />
  </section>
</template>
