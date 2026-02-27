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

    <template v-else>
      <!-- 필터 / 검색 -->
      <div class="flex flex-wrap items-center gap-3 mb-4">
        <!-- 카테고리 탭 -->
        <div class="flex flex-wrap gap-1">
          <button
            v-for="cat in categoryTabs"
            :key="cat.code"
            :class="selectedCategory === cat.code
              ? 'bg-blue-600 text-white border-blue-600'
              : 'bg-white text-gray-600 border-gray-300 hover:border-blue-400 hover:text-blue-600'"
            class="px-3 py-1.5 text-xs font-medium border rounded-full transition-colors"
            @click="selectedCategory = cat.code"
          >{{ cat.name }}</button>
        </div>

        <!-- 검색 -->
        <div class="relative ml-auto">
          <span class="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400 text-xs">🔍</span>
          <input
            v-model="searchQuery"
            type="text"
            placeholder="상품명 검색"
            class="pl-8 pr-3 py-1.5 text-sm border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-200 w-44"
          />
        </div>
      </div>

      <!-- 데이터 없음 (전체) -->
      <div
        v-if="products.length === 0"
        class="text-center py-12 text-gray-400"
      >
        등록된 상품이 없습니다.
      </div>

      <!-- 데이터 없음 (필터 결과) -->
      <div
        v-else-if="filteredProducts.length === 0"
        class="text-center py-12 text-gray-400"
      >
        검색 결과가 없습니다.
      </div>

      <!-- 상품 테이블 -->
      <div v-else class="overflow-x-auto rounded-lg border border-gray-200">
        <table class="min-w-full divide-y divide-gray-200 text-sm">
          <thead class="bg-gray-50">
            <tr>
              <th class="px-4 py-3 text-left font-medium text-gray-600 w-56">상품명</th>
              <th class="px-4 py-3 text-left font-medium text-gray-600">바코드</th>
              <th class="px-4 py-3 text-right font-medium text-gray-600">박스가</th>
              <th class="px-4 py-3 text-right font-medium text-gray-600">낱개 매입가</th>
              <th class="px-4 py-3 text-right font-medium text-gray-600">판매가</th>
              <th class="px-4 py-3 text-right font-medium text-gray-600">마진율</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-gray-100 bg-white">
            <tr
              v-for="product in filteredProducts"
              :key="product.id"
              class="hover:bg-gray-50 transition-colors"
            >
              <td class="px-4 py-3 font-medium">
                <RouterLink
                  :to="{ name: 'ProductEdit', params: { id: product.id } }"
                  class="text-blue-600 hover:text-blue-800 hover:underline"
                >{{ product.name }}</RouterLink>
              </td>
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
            </tr>
          </tbody>
        </table>
        <div class="px-4 py-2 text-xs text-gray-400 border-t border-gray-100 bg-gray-50">
          {{ filteredProducts.length }}개 상품
        </div>
      </div>
    </template>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { getProducts } from '../api/products'
import { getDetailCodes } from '../api/codes'

const products = ref([])
const loading = ref(false)
const error = ref(null)

const categories = ref([])
const selectedCategory = ref('__all__')
const searchQuery = ref('')

/** 전체 탭 + 카테고리 탭 목록 */
const categoryTabs = computed(() => [
  { code: '__all__', name: '전체' },
  ...categories.value,
])

/** 카테고리 + 검색어 필터 적용 */
const filteredProducts = computed(() => {
  let list = products.value
  if (selectedCategory.value !== '__all__') {
    list = list.filter(p => p.category === selectedCategory.value)
  }
  const q = searchQuery.value.trim().toLowerCase()
  if (q) {
    list = list.filter(p => p.name.toLowerCase().includes(q))
  }
  return list
})

async function fetchProducts() {
  loading.value = true
  error.value = null
  try {
    const [{ data: productData }, { data: categoryData }] = await Promise.all([
      getProducts(),
      getDetailCodes('ProductCategory'),
    ])
    products.value = productData
    categories.value = categoryData
  } catch {
    error.value = '상품 목록을 불러오지 못했습니다.'
  } finally {
    loading.value = false
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
