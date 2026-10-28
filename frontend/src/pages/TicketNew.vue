<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useTicketsStore } from '../ticketsStore'

const router = useRouter()
const { createTicket } = useTicketsStore()
const form = ref({ title: '', siteId: 2, description: '' })
const notice = ref('')

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
  createTicket({ title, siteId, description })
  router.push({ name: 'tickets', query: { created: '1' } })
}
</script>

<template>
  <section class="page-content form-page">
    <div class="page-heading"><div><div class="eyebrow">НОВАЯ ЗАПИСЬ</div><h1>Создать заявку</h1><p class="page-subtitle">Заполните основные сведения для планирования отпусков.</p></div><RouterLink class="secondary-button" to="/">← К списку</RouterLink></div>
    <div class="form-layout">
      <section class="panel form-panel"><div class="panel-heading"><div><h2>Основная информация</h2><p>Поля со звёздочкой обязательны для заполнения</p></div></div>
        <form class="request-form" @submit.prevent="createRequest">
          <div class="field-group"><label for="title">Тема заявки <span>*</span></label><input id="title" v-model="form.title" name="title" type="text" minlength="5" maxlength="80" placeholder="Например, планирование отпусков участка связи" required /><small>От 5 до 80 символов</small></div>
          <div class="field-group"><label for="siteId">Номер участка <span>*</span></label><input id="siteId" v-model.number="form.siteId" name="siteId" type="number" min="1" step="1" required /><small>Идентификатор участка из справочника предприятия</small></div>
          <div class="field-group"><label for="description">Описание <span>*</span></label><textarea id="description" v-model="form.description" name="description" minlength="10" maxlength="500" rows="5" placeholder="Опишите задачу и ожидаемый результат…" required></textarea><small>{{ form.description.length }} / 500 символов · минимум 10</small></div>
          <p v-if="notice" class="notice" role="status">{{ notice }}</p>
          <div class="form-actions"><RouterLink class="secondary-button" to="/">Отмена</RouterLink><button class="primary-button" type="submit">Создать заявку <span>→</span></button></div>
        </form>
      </section>
      <aside class="hint-card"><div class="hint-symbol">✦</div><h3>Перед созданием</h3><p>Проверьте, что тема отражает цель заявки, а номер участка соответствует справочнику предприятия.</p><div class="hint-divider"></div><div class="hint-line"><span>Статус после создания</span><strong class="status-pill status-new"><i></i>Новая</strong></div><div class="hint-line"><span>Номер заявки</span><strong>Назначается автоматически</strong></div><div class="hint-caption">Данные хранятся в общем реактивном массиве на время работы приложения. Сервер ещё не подключён.</div></aside>
    </div>
  </section>
</template>
