import { ref } from 'vue'

// Единый источник заявок для списка, боковой навигации и формы.
// Данные существуют только в памяти браузера и сбрасываются после F5.
export const items = ref([
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

export let nextId = 3

export function addItem({ title, siteId, description }) {
  const item = {
    id: nextId,
    number: `TMP-${nextId}`,
    title: title.trim(),
    siteId: Number(siteId),
    description: description.trim(),
    status: 'New',
  }

  items.value.unshift(item)
  nextId += 1
  return item
}

// Сохраняем совместимость с компонентами, созданными в ЛР4.
export function useTicketsStore() {
  return {
    requests: items,
    createTicket: addItem,
  }
}

export const statusLabels = {
  New: 'Новая',
  InProgress: 'В работе',
  Closed: 'Завершена',
  Cancelled: 'Отменена',
}
