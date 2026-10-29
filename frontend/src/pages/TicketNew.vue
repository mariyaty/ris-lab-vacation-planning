<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { addItem } from '../ticketsStore'

const router = useRouter()
const form = ref({
  title: '',
  siteId: 2,
  description: '',
})
const errors = ref({
  title: '',
  siteId: '',
  description: '',
})

function validateForm() {
  errors.value = { title: '', siteId: '', description: '' }

  const title = form.value.title.trim()
  const description = form.value.description.trim()
  const siteId = form.value.siteId

  if (title.length < 5 || title.length > 80) {
    errors.value.title = 'Тема: от 5 до 80 символов.'
  }

  if (!Number.isInteger(siteId) || siteId <= 0) {
    errors.value.siteId = 'Укажите целый номер участка больше нуля.'
  }

  if (description.length < 10 || description.length > 500) {
    errors.value.description = 'Описание: от 10 до 500 символов.'
  }

  return !Object.values(errors.value).some(Boolean)
}

function onCreate() {
  if (!validateForm()) return

  addItem({
    title: form.value.title.trim(),
    siteId: form.value.siteId,
    description: form.value.description.trim(),
  })

  errors.value = { title: '', siteId: '', description: '' }
  form.value = { title: '', siteId: 2, description: '' }
  router.push('/')
}
</script>

<template>
  <section class="page-content form-page">
    <div class="page-heading"><div><div class="eyebrow">НОВАЯ ЗАПИСЬ</div><h1>Создать заявку</h1><p class="page-subtitle">Заполните основные сведения для планирования отпусков.</p></div><RouterLink class="secondary-button" to="/">← К списку</RouterLink></div>
    <div class="form-layout">
      <section class="panel form-panel">
        <div class="panel-heading"><div><h2>Основная информация</h2><p>Поля со звёздочкой обязательны для заполнения</p></div></div>
        <form class="request-form" @submit.prevent="onCreate" novalidate>
          <div class="field-group">
            <label for="title">Тема заявки <span>*</span></label>
            <input id="title" v-model="form.title" name="title" type="text" minlength="5" maxlength="80" placeholder="Например, планирование отпусков участка связи" required :aria-invalid="Boolean(errors.title)" aria-describedby="title-error" />
            <small>От 5 до 80 символов</small>
            <p v-if="errors.title" id="title-error" class="field-error" role="alert">{{ errors.title }}</p>
          </div>
          <div class="field-group">
            <label for="siteId">Номер участка <span>*</span></label>
            <input id="siteId" v-model.number="form.siteId" name="siteId" type="number" min="1" step="1" required :aria-invalid="Boolean(errors.siteId)" aria-describedby="siteId-error" />
            <small>Идентификатор участка из справочника предприятия</small>
            <p v-if="errors.siteId" id="siteId-error" class="field-error" role="alert">{{ errors.siteId }}</p>
          </div>
          <div class="field-group">
            <label for="description">Описание <span>*</span></label>
            <textarea id="description" v-model="form.description" name="description" minlength="10" maxlength="500" rows="5" placeholder="Опишите задачу и ожидаемый результат…" required :aria-invalid="Boolean(errors.description)" aria-describedby="description-error"></textarea>
            <small>{{ form.description.length }} / 500 символов · минимум 10</small>
            <p v-if="errors.description" id="description-error" class="field-error" role="alert">{{ errors.description }}</p>
          </div>
          <div class="form-actions"><RouterLink class="secondary-button" to="/">Отмена</RouterLink><button class="primary-button" type="submit">Создать заявку <span>→</span></button></div>
        </form>
      </section>
      <aside class="hint-card"><div class="hint-symbol">✦</div><h3>Перед созданием</h3><p>Проверьте, что тема отражает цель заявки, а номер участка соответствует справочнику предприятия.</p><div class="hint-divider"></div><div class="hint-line"><span>Статус после создания</span><strong class="status-pill status-new"><i></i>Новая</strong></div><div class="hint-line"><span>Номер заявки</span><strong>Назначается автоматически</strong></div><div class="hint-caption">Заявки хранятся в общем реактивном массиве только до перезагрузки страницы. Сервер ещё не подключён.</div></aside>
    </div>
  </section>
</template>
