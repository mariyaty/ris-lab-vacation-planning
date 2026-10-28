<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const login = ref('')
const password = ref('')
const notice = ref('')

function signIn() {
  if (!login.value.trim() || !password.value) {
    notice.value = 'Заполните логин и пароль.'
    return
  }
  notice.value = 'Экран входа работает в демонстрационном режиме: серверная авторизация будет добавлена позже.'
}
</script>

<template>
  <section class="page-content login-page">
    <div class="login-card panel"><div class="login-brand"><span class="brand-mark"><span></span><span></span><span></span></span></div><div class="eyebrow">РАБОЧЕЕ ПРОСТРАНСТВО</div><h1>С возвращением</h1><p class="page-subtitle">Войдите, чтобы продолжить работу с заявками.</p>
      <form class="request-form" @submit.prevent="signIn"><div class="field-group"><label for="login">Логин <span>*</span></label><input id="login" v-model="login" name="login" autocomplete="username" placeholder="Введите логин" required autofocus /></div><div class="field-group"><label for="password">Пароль <span>*</span></label><input id="password" v-model="password" name="password" type="password" autocomplete="current-password" placeholder="Введите пароль" required /></div><button class="primary-button full-width" type="submit">Войти в систему <span>→</span></button><p v-if="notice" class="notice" role="status">{{ notice }}</p></form>
      <button class="back-link" type="button" @click="router.push('/')">← Вернуться к заявкам</button><p class="login-disclaimer">Демонстрационный экран. Проверка учётных данных пока не подключена.</p>
    </div>
  </section>
</template>
