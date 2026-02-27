<template>
  <div class="p-6">
    <div class="mb-6">
      <button
        class="text-sm text-gray-500 hover:text-gray-700 mb-2"
        @click="router.back()"
      >
        ← 목록으로
      </button>
      <h1 class="text-2xl font-bold text-gray-800">상품 수정</h1>
    </div>

    <!-- 로딩 -->
    <div v-if="loading" class="py-12 text-center text-gray-400">불러오는 중...</div>

    <!-- 에러 -->
    <div
      v-else-if="fetchError"
      class="p-4 bg-red-50 border border-red-200 rounded-lg text-red-600 text-sm"
    >
      {{ fetchError }}
    </div>

    <!-- 폼 -->
    <ProductForm
      v-else-if="product"
      :product-id="product.id"
      :initial-data="product"
      @success="onSuccess"
      @delete="onSuccess"
      @cancel="router.back()"
    />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import ProductForm from '../components/ProductForm.vue'
import { getProduct } from '../api/products'

const route = useRoute()
const router = useRouter()

const product = ref(null)
const loading = ref(true)
const fetchError = ref(null)

/** 기존 상품 데이터 조회 */
async function fetchProduct() {
  try {
    const { data } = await getProduct(Number(route.params.id))
    product.value = data
  } catch {
    fetchError.value = '상품 정보를 불러오지 못했습니다.'
  } finally {
    loading.value = false
  }
}

/** 수정 성공 시 목록으로 이동 */
function onSuccess() {
  router.push({ name: 'ProductList' })
}

onMounted(fetchProduct)
</script>
