<script setup>
import AppHeader from './components/AppHeader.vue'
</script>

<template>
  <div class="app-shell">
    <AppHeader />
    <main class="main-content">
      <header class="topbar">
        <div class="breadcrumbs">
          <span>Рабочее пространство</span><span class="crumb-separator">/</span>
          <strong>{{ $route.meta.title || ({ '/': 'Заявки', '/new': 'Создание заявки', '/login': 'Вход в систему' }[$route.path] || 'Заявки') }}</strong>
        </div>
        <div class="topbar-right">
          <span class="environment"><i></i> Демо-режим</span>
          <RouterLink class="top-avatar" to="/login" aria-label="Открыть вход">МК</RouterLink>
        </div>
      </header>

      <RouterView />

      <footer class="page-footer">
        <span>ПланГрафик · Учебный проект</span>
        <span>Лабораторная работа № 4 · Vue 3</span>
      </footer>
    </main>
  </div>
</template>
