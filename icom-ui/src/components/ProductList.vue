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
          @click="onCategoryChange(cat.code)"
        >{{ cat.name }}</button>
      </div>

      <!-- 검색 -->
      <div class="relative ml-auto">
        <span class="absolute left-3 top-1/2 -translate-y-1/2 text-gray-400 text-xs">🔍</span>
        <input
          v-model="searchInput"
          type="text"
          placeholder="상품명 검색"
          class="pl-8 pr-3 py-1.5 text-sm border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-200 w-44"
        />
      </div>
    </div>

    <!-- 로딩 -->
    <div v-if="loading" class="flex justify-center py-12">
      <span class="text-gray-500">불러오는 중...</span>
    </div>

    <!-- 에러 -->
    <div
      v-else-if="error"
      class="p-4 bg-red-50 border border-red-200 rounded-lg text-red-600 text-sm"
    >
      {{ error }}
    </div>

    <template v-else>
      <!-- 결과 없음 -->
      <div v-if="pagedResult.totalCount === 0" class="text-center py-12 text-gray-400">
        {{ selectedCategory !== '__all__' || searchInput ? '검색 결과가 없습니다.' : '등록된 상품이 없습니다.' }}
      </div>

      <!-- 테이블 -->
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
              v-for="product in pagedResult.items"
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
                >{{ formatMargin(product.margin) }}</span>
              </td>
            </tr>
          </tbody>
        </table>

        <!-- 하단: 카운트 + 페이지네이션 -->
        <div class="flex items-center justify-between px-4 py-3 border-t border-gray-100 bg-gray-50">
          <span class="text-xs text-gray-400">
            총 {{ pagedResult.totalCount }}개 중
            {{ (currentPage - 1) * PAGE_SIZE + 1 }}–{{ Math.min(currentPage * PAGE_SIZE, pagedResult.totalCount) }}
          </span>

          <div class="flex items-center gap-1">
            <button
              :disabled="currentPage === 1"
              class="px-2 py-1 text-xs rounded border border-gray-300 bg-white text-gray-600
                     hover:bg-gray-100 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
              @click="goToPage(currentPage - 1)"
            >‹</button>

            <template v-for="p in pageNumbers" :key="p">
              <span v-if="p === '...'" class="px-1 text-xs text-gray-400">…</span>
              <button
                v-else
                :class="p === currentPage
                  ? 'bg-blue-600 text-white border-blue-600'
                  : 'bg-white text-gray-600 border-gray-300 hover:bg-gray-100'"
                class="min-w-[28px] px-2 py-1 text-xs rounded border transition-colors"
                @click="goToPage(p)"
              >{{ p }}</button>
            </template>

            <button
              :disabled="currentPage === pagedResult.totalPages"
              class="px-2 py-1 text-xs rounded border border-gray-300 bg-white text-gray-600
                     hover:bg-gray-100 disabled:opacity-40 disabled:cursor-not-allowed transition-colors"
              @click="goToPage(currentPage + 1)"
            >›</button>
          </div>
        </div>
      </div>
    </template>
  </div>
</template>

<script setup>
import { ref, computed, watch, onMounted } from 'vue'
import { getProducts } from '../api/products'
import { getDetailCodes } from '../api/codes'

const PAGE_SIZE = 20

const categories = ref([])
const selectedCategory = ref('__all__')
const searchInput = ref('')
const currentPage = ref(1)
const loading = ref(false)
const error = ref(null)
const pagedResult = ref({ items: [], totalCount: 0, totalPages: 0 })

let searchTimer = null

const categoryTabs = computed(() => [
  { code: '__all__', name: '전체' },
  ...categories.value,
])

/** 페이지 번호 목록 (최대 7개, 양끝 + 현재 주변만 표시) */
const pageNumbers = computed(() => {
  const total = pagedResult.value.totalPages
  const cur = currentPage.value
  if (total <= 7) return Array.from({ length: total }, (_, i) => i + 1)

  const pages = []
  pages.push(1)
  if (cur > 3) pages.push('...')
  for (let i = Math.max(2, cur - 1); i <= Math.min(total - 1, cur + 1); i++) pages.push(i)
  if (cur < total - 2) pages.push('...')
  pages.push(total)
  return pages
})

async function fetchProducts() {
  loading.value = true
  error.value = null
  try {
    const params = {
      page: currentPage.value,
      pageSize: PAGE_SIZE,
      ...(selectedCategory.value !== '__all__' && { category: selectedCategory.value }),
      ...(searchInput.value.trim() && { search: searchInput.value.trim() }),
    }
    const { data } = await getProducts(params)
    pagedResult.value = data
  } catch {
    error.value = '상품 목록을 불러오지 못했습니다.'
  } finally {
    loading.value = false
  }
}

function onCategoryChange(code) {
  selectedCategory.value = code
  currentPage.value = 1
  fetchProducts()
}

function goToPage(page) {
  currentPage.value = page
  fetchProducts()
}

/** 검색어 입력 시 300ms 디바운스 후 page=1 재조회 */
watch(searchInput, () => {
  clearTimeout(searchTimer)
  searchTimer = setTimeout(() => {
    currentPage.value = 1
    fetchProducts()
  }, 300)
})

async function init() {
  loading.value = true
  error.value = null
  try {
    const params = { page: 1, pageSize: PAGE_SIZE }
    const [{ data: productData }, { data: categoryData }] = await Promise.all([
      getProducts(params),
      getDetailCodes('ProductCategory'),
    ])
    pagedResult.value = productData
    categories.value = categoryData
  } catch {
    error.value = '상품 목록을 불러오지 못했습니다.'
  } finally {
    loading.value = false
  }
}

/** 금액 포맷 */
function formatCurrency(value) {
  if (value == null) return '-'
  return Number(value).toLocaleString('ko-KR') + '원'
}

/** 마진율 포맷 */
function formatMargin(value) {
  if (value == null) return '-'
  return (Number(value) * 100).toFixed(2) + '%'
}

onMounted(init)
</script>
