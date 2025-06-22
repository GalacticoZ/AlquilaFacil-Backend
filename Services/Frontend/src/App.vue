<script setup>
  const savedTheme = localStorage.getItem("theme");
  if (savedTheme) {
    document.documentElement.setAttribute("data-theme", savedTheme);
  } else {
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
    document.documentElement.setAttribute("data-theme", prefersDark ? "dark" : "light");
  }
</script>

<template>
  <router-view />
</template>