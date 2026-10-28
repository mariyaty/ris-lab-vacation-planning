import { ref } from 'vue'

// Единый источник тестовых данных для всех страниц приложения.
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

let nextId = 3

export function useTicketsStore() {
  function createTicket({ title, siteId, description }) {
    const ticket = {
      id: nextId,
      number: `VR-${String(nextId).padStart(5, '0')}`,
      title: title.trim(),
      siteId: Number(siteId),
      description: description.trim(),
      status: 'New',
    }
    nextId += 1
    requests.value.unshift(ticket)
    return ticket
  }

  return { requests, createTicket }
}

export const statusLabels = {
  New: 'Новая',
  InProgress: 'В работе',
  Closed: 'Завершена',
  Cancelled: 'Отменена',
}
