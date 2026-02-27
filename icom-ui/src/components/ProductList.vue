<template>
  <div class="p-6">
    <div class="flex items-center justify-between mb-6">
      <h1 class="text-2xl font-bold text-gray-800">상품 목록</h1>
      <RouterLink
        :to="{ name: 'ProductCreate' }"
        class="px-4 py-2 bg-blue-600 text-white text-sm font-medium rounded-lg hover:bg-blue-700 transition-colors"
      >
        + 상품 등록
      </RouterLink>
    </div>

    <!-- 로딩 상태 -->
    <div v-if="loading" class="flex justify-center py-12">
      <span class="text-gray-500">불러오는 중...</span>
    </div>

    <!-- 에러 상태 -->
    <div
      v-else-if="error"
      class="p-4 bg-red-50 border border-red-200 rounded-lg text-red-600 text-sm"
    >
      {{ error }}
    </div>

    <!-- 데이터 없음 -->
    <div
      v-else-if="products.length === 0"
      class="text-center py-12 text-gray-400"
    >
      등록된 상품이 없습니다.
    </div>

    <!-- 상품 테이블 -->
    <div v-else class="overflow-x-auto rounded-lg border border-gray-200">
      <table class="min-w-full divide-y divide-gray-200 text-sm">
        <thead class="bg-gray-50">
          <tr>
            <th class="px-4 py-3 text-left font-medium text-gray-600">상품명</th>
            <th class="px-4 py-3 text-left font-medium text-gray-600">바코드</th>
            <th class="px-4 py-3 text-right font-medium text-gray-600">박스가</th>
            <th class="px-4 py-3 text-right font-medium text-gray-600">낱개 매입가</th>
            <th class="px-4 py-3 text-right font-medium text-gray-600">판매가</th>
            <th class="px-4 py-3 text-right font-medium text-gray-600">마진율</th>
            <th class="px-4 py-3 text-center font-medium text-gray-600">관리</th>
          </tr>
        </thead>
        <tbody class="divide-y divide-gray-100 bg-white">
          <tr
            v-for="product in products"
            :key="product.id"
            class="hover:bg-gray-50 transition-colors"
          >
            <td class="px-4 py-3 font-medium text-gray-800">{{ product.name }}</td>
            <td class="px-4 py-3 text-gray-500 font-mono text-xs">{{ product.barcode || '-' }}</td>
            <td class="px-4 py-3 text-right text-gray-600">{{ formatCurrency(product.boxPrice) }}</td>
            <td class="px-4 py-3 text-right text-gray-600">{{ formatCurrency(product.unitPrice) }}</td>
            <td class="px-4 py-3 text-right text-gray-600">{{ formatCurrency(product.retailPrice) }}</td>
            <td class="px-4 py-3 text-right">
              <span
                :class="product.margin >= 0.3 ? 'text-green-600' : 'text-orange-500'"
                class="font-medium"
              >
                {{ formatMargin(product.margin) }}
              </span>
            </td>
            <td class="px-4 py-3 text-center">
              <!-- 삭제 확인 상태 -->
              <template v-if="deletingId === product.id">
                <span class="text-xs text-gray-500 mr-1">삭제할까요?</span>
                <button
                  class="text-red-500 hover:text-red-700 text-xs font-medium mr-1"
                  :disabled="deleteLoading"
                  @click="confirmDelete(product.id)"
                >확인</button>
                <button
                  class="text-gray-400 hover:text-gray-600 text-xs"
                  @click="deletingId = null"
                >취소</button>
              </template>
              <!-- 기본 상태 -->
              <template v-else>
                <RouterLink
                  :to="{ name: 'ProductEdit', params: { id: product.id } }"
                  class="text-blue-500 hover:text-blue-700 text-xs mr-3"
                >수정</RouterLink>
                <button
                  class="text-red-400 hover:text-red-600 text-xs"
                  @click="deletingId = product.id"
                >삭제</button>
              </template>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { getProducts, deleteProduct } from '../api/products'

const products = ref([])
const loading = ref(false)
const error = ref(null)
const deletingId = ref(null)
const deleteLoading = ref(false)

/** 상품 목록을 API에서 불러옴 */
async function fetchProducts() {
  loading.value = true
  error.value = null
  try {
    const { data } = await getProducts()
    products.value = data
  } catch (e) {
    error.value = '상품 목록을 불러오지 못했습니다.'
  } finally {
    loading.value = false
  }
}

/** 삭제 확정 */
async function confirmDelete(id) {
  deleteLoading.value = true
  try {
    await deleteProduct(id)
    products.value = products.value.filter(p => p.id !== id)
    deletingId.value = null
  } catch {
    error.value = '삭제에 실패했습니다.'
  } finally {
    deleteLoading.value = false
  }
}

/** 금액 포맷 (예: 1,500원) */
function formatCurrency(value) {
  if (value == null) return '-'
  return Number(value).toLocaleString('ko-KR') + '원'
}

/** 마진율 포맷 (예: 32.50%) */
function formatMargin(value) {
  if (value == null) return '-'
  return (Number(value) * 100).toFixed(2) + '%'
}

onMounted(fetchProducts)
</script>
