<template>
  <div class="p-6">
    <div class="mb-6">
      <button
        class="text-sm text-gray-500 hover:text-gray-700 mb-2 flex items-center gap-1"
        @click="router.back()"
      >
        ← 목록으로
      </button>
      <h1 class="text-2xl font-bold text-gray-800">상품 등록</h1>
    </div>

    <!-- 서버 오류 -->
    <div
      v-if="form?.errors?._server"
      class="mb-4 p-4 bg-red-50 border border-red-200 rounded-lg text-red-600 text-sm"
    >
      {{ form.errors._server }}
    </div>

    <ProductForm @success="onSuccess" @cancel="router.back()" ref="form" />
  </div>
</template>

<script setup>
import { useRouter } from 'vue-router'
import ProductForm from '../components/ProductForm.vue'

const router = useRouter()

/** 등록 성공 시 목록으로 이동 */
function onSuccess() {
  router.push({ name: 'ProductList' })
}
</script>
