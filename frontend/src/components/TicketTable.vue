<script setup>
import { computed, ref } from 'vue'
import { statusLabels } from '../ticketsStore'

const props = defineProps({
  items: { type: Array, required: true },
})
const search = ref('')
const notice = ref('')
const filteredItems = computed(() => {
  const query = search.value.trim().toLowerCase()
  if (!query) return props.items
  return props.items.filter((item) =>
    [item.number, item.title, String(item.siteId), statusLabels[item.status]]
      .some((value) => value.toLowerCase().includes(query)),
  )
})
</script>

<template>
  <section class="panel">
    <div class="panel-heading">
      <div><h2>Все заявки</h2><p>Список заявок на планирование отпусков</p></div>
      <label class="search-box"><span>⌕</span><input v-model="search" type="search" placeholder="Поиск по заявкам…" aria-label="Поиск по заявкам" /></label>
    </div>
    <div class="table-wrap">
      <table>
        <thead><tr><th>НОМЕР</th><th>ТЕМА ЗАЯВКИ</th><th>УЧАСТОК</th><th>СТАТУС</th><th></th></tr></thead>
        <tbody>
          <tr v-for="item in filteredItems" :key="item.id">
            <td><span class="request-number">{{ item.number }}</span></td>
            <td><div class="request-title">{{ item.title }}</div><div class="request-description">{{ item.description }}</div></td>
            <td><span class="site-chip">Участок {{ item.siteId }}</span></td>
            <td><span class="status-pill" :class="`status-${item.status.toLowerCase()}`"><i></i>{{ statusLabels[item.status] }}</span></td>
            <td><button class="row-more" type="button" :aria-label="`Подробнее о заявке ${item.number}`" @click="notice = `${item.number}: ${item.description}`">···</button></td>
          </tr>
          <tr v-if="filteredItems.length === 0"><td colspan="5" class="empty-state">Заявки не найдены. Попробуйте изменить запрос.</td></tr>
        </tbody>
      </table>
    </div>
    <div class="table-footer"><span>Показано {{ filteredItems.length }} из {{ items.length }} заявок</span><span class="demo-label"><i></i> Тестовые данные</span></div>
    <p v-if="notice" class="notice table-notice" role="status">{{ notice }}</p>
  </section>
</template>
