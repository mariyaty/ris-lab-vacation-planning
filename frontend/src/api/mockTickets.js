// Учебная имитация API. Возвращает те же данные, которые ожидаются от backend.
const delay = (ms = 500) => new Promise((resolve) => setTimeout(resolve, ms))

let tickets = [
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
]

let nextId = 3

export async function mockGetTickets() {
  await delay()
  return { ok: true, status: 200, data: tickets.map((ticket) => ({ ...ticket })) }
}

export async function mockCreateTicket(body) {
  await delay()

  if (body.force500) {
    return { ok: false, status: 500, data: { message: 'Учебная ошибка сервера.' } }
  }

  if (!body.title || body.title.trim().length < 5) {
    return { ok: false, status: 400, data: { message: 'Тема заявки должна содержать не менее 5 символов.' } }
  }

  const ticket = {
    id: nextId,
    number: `VR-${String(nextId).padStart(5, '0')}`,
    title: body.title.trim(),
    siteId: Number(body.siteId),
    description: body.description.trim(),
    status: 'New',
  }

  nextId += 1
  tickets = [ticket, ...tickets]
  return { ok: true, status: 201, data: { ...ticket } }
}
