<template>
  <div class="p-6">
    <button
      class="mb-4 text-sm text-blue-600 hover:underline"
      @click="router.back()"
    >
      ← 목록으로
    </button>
    <p class="text-gray-500">상품 상세 페이지 (준비 중) — ID: {{ route.params.id }}</p>
  </div>
</template>

<script setup>
import { useRoute, useRouter } from 'vue-router'

const route = useRoute()
const router = useRouter()
</script>
