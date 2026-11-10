import { mockCreateTicket, mockGetTickets } from './mockTickets'

const baseUrl = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000'
const USE_MOCK = import.meta.env.VITE_USE_MOCK !== 'false'

async function readResponse(response) {
  let data = {}
  try {
    data = await response.json()
  } catch {
    data = {}
  }

  if (!response.ok) {
    const error = new Error(data.message || `Ошибка HTTP ${response.status}`)
    error.status = response.status
    throw error
  }

  return data
}

function unwrapMockResponse(response) {
  if (!response.ok) {
    const error = new Error(response.data?.message || `Ошибка HTTP ${response.status}`)
    error.status = response.status
    throw error
  }
  return response.data
}

// Страницы приложения работают только с этими двумя функциями.
export async function getTickets() {
  if (USE_MOCK) {
    return unwrapMockResponse(await mockGetTickets())
  }

  const response = await fetch(`${baseUrl}/api/tickets`)
  return readResponse(response)
}

export async function createTicket(body) {
  if (USE_MOCK) {
    return unwrapMockResponse(await mockCreateTicket(body))
  }

  const response = await fetch(`${baseUrl}/api/tickets`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(body),
  })
  return readResponse(response)
}
