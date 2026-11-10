
import { ref } from 'vue'

// Единый источник заявок для списка,
// боковой навигации и формы.
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

// Заменяем содержимое общего списка данными API.
export function setItems(tickets) {
  items.value = Array.isArray(tickets) ? tickets : []
}

// Добавляем заявку, полученную от API или mock.
export function addItem(ticket) {
  const existingIndex = items.value.findIndex(
    (item) => item.id === ticket.id
  )

  if (existingIndex !== -1) {
    items.value.splice(existingIndex, 1)
  }

  items.value.unshift(ticket)
  return ticket
}

// Сохраняем совместимость с компонентами ЛР4 и ЛР5.
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
