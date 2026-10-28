<script setup>
import { useRoute } from 'vue-router'
import { useTicketsStore } from '../ticketsStore'

const route = useRoute()
const { requests } = useTicketsStore()
</script>

<template>
  <aside class="sidebar">
    <RouterLink class="brand" to="/" aria-label="На главную">
      <span class="brand-mark"><span></span><span></span><span></span></span>
      <span class="brand-copy"><strong>ПланГрафик</strong><small>Система управления</small></span>
    </RouterLink>

    <div class="workspace-label">РАБОЧЕЕ ПРОСТРАНСТВО</div>
    <nav class="navigation" aria-label="Главная навигация">
      <RouterLink class="nav-item" :class="{ active: route.path === '/' }" to="/">
        <span class="nav-icon">▤</span><span>Заявки</span><span class="nav-count">{{ requests.length }}</span>
      </RouterLink>
      <RouterLink class="nav-item" :class="{ active: route.path === '/new' }" to="/new">
        <span class="nav-icon plus-icon">＋</span><span>Создать заявку</span>
      </RouterLink>
    </nav>

    <div class="sidebar-bottom">
      <div class="help-card">
        <div class="help-icon">?</div>
        <strong>Нужна помощь?</strong>
        <p>Рабочее пространство для планирования отпусков.</p>
      </div>
      <RouterLink class="profile-button" to="/login">
        <span class="avatar">МК</span>
        <span class="profile-copy"><strong>Мария Крец</strong><small>Кадровый специалист</small></span>
        <span class="profile-more">···</span>
      </RouterLink>
    </div>
  </aside>
</template>
