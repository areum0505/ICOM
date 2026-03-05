<template>
  <div class="p-6">
    <!-- 헤더 -->
    <div class="flex items-center justify-between mb-6">
      <h1 class="text-2xl font-bold text-gray-800">발주 관리</h1>
      <button
        @click="router.push({ name: 'OrderCreate' })"
        class="px-4 py-2 bg-blue-600 text-white text-sm rounded hover:bg-blue-700"
      >
        + 발주 등록
      </button>
    </div>

    <!-- 오류 메시지 -->
    <p v-if="error" class="mb-4 text-sm text-red-600">{{ error }}</p>

    <!-- 테이블 -->
    <div class="bg-white border border-gray-200 rounded-lg overflow-hidden">
      <table class="w-full text-sm">
        <thead class="bg-gray-50 border-b border-gray-200">
          <tr>
            <th class="px-4 py-3 text-left font-semibold text-gray-700">날짜</th>
            <th class="px-4 py-3 text-center font-semibold text-gray-700">품목 수</th>
            <th class="px-4 py-3 text-right font-semibold text-gray-700">합계</th>
            <th class="px-4 py-3 text-center font-semibold text-gray-700 w-36"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="orders.length === 0">
            <td colspan="4" class="px-4 py-8 text-center text-gray-400">
              발주 내역이 없습니다. 발주 등록 버튼을 눌러 발주를 생성하세요.
            </td>
          </tr>
          <tr
            v-for="order in orders"
            :key="order.id"
            class="border-b border-gray-100 last:border-0 hover:bg-gray-50"
          >
            <td class="px-4 py-3 text-gray-800">{{ formatDate(order.orderDate) }}</td>
            <td class="px-4 py-3 text-center text-gray-600">{{ order.itemCount }}</td>
            <td class="px-4 py-3 text-right text-gray-800">{{ formatNumber(order.totalAmount) }}원</td>
            <td class="px-4 py-3 text-center">
              <div class="flex items-center justify-center gap-2">
                <button
                  @click="goDetail(order)"
                  class="px-3 py-1 text-xs bg-blue-50 text-blue-700 border border-blue-200 rounded hover:bg-blue-100"
                >
                  상세
                </button>
                <button
                  @click="removeOrder(order)"
                  class="px-3 py-1 text-xs bg-red-50 text-red-700 border border-red-200 rounded hover:bg-red-100"
                >
                  삭제
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
      <!-- 페이징 -->
      <div class="flex items-center justify-between px-4 py-3 border-t border-gray-100 bg-gray-50">
        <span class="text-xs text-gray-400">
          총 {{ totalCount }}개 중
          {{ totalCount === 0 ? 0 : (page - 1) * PAGE_SIZE + 1 }}–{{ Math.min(page * PAGE_SIZE, totalCount) }}
        </span>
        <div class="flex items-center gap-1">
          <button
            @click="changePage(page - 1)"
            :disabled="page === 1"
            class="px-2 py-1 text-sm border border-gray-300 rounded hover:bg-gray-100 disabled:opacity-40 disabled:cursor-not-allowed"
          >‹</button>
          <template v-for="p in pageNumbers" :key="p">
            <span v-if="p === '...'" class="px-1 text-xs text-gray-400">…</span>
            <button
              v-else
              @click="changePage(p)"
              :class="p === page ? 'bg-blue-600 text-white border-blue-600' : 'bg-white text-gray-700 border-gray-300 hover:bg-gray-50'"
              class="px-3 py-1 text-sm border rounded"
            >{{ p }}</button>
          </template>
          <button
            @click="changePage(page + 1)"
            :disabled="page === totalPages"
            class="px-2 py-1 text-sm border border-gray-300 rounded hover:bg-gray-100 disabled:opacity-40 disabled:cursor-not-allowed"
          >›</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { getOrders, deleteOrder } from '../api/orders'

const PAGE_SIZE = 20

const router = useRouter()
const orders = ref([])
const error = ref('')
const page = ref(1)
const totalCount = ref(0)

const totalPages = computed(() => Math.ceil(totalCount.value / PAGE_SIZE))

const pageNumbers = computed(() => {
  const total = totalPages.value
  const cur = page.value
  if (total <= 7) return Array.from({ length: total }, (_, i) => i + 1)
  const pages = []
  pages.push(1)
  if (cur > 3) pages.push('...')
  for (let i = Math.max(2, cur - 1); i <= Math.min(total - 1, cur + 1); i++) pages.push(i)
  if (cur < total - 2) pages.push('...')
  pages.push(total)
  return pages
})

function formatDate(dateStr) {
  return dateStr ? String(dateStr).slice(0, 10) : ''
}

function formatNumber(val) {
  return Number(val || 0).toLocaleString('ko-KR')
}

async function loadOrders() {
  error.value = ''
  try {
    const res = await getOrders({ page: page.value, pageSize: PAGE_SIZE })
    orders.value = res.data.items
    totalCount.value = res.data.totalCount
  } catch {
    error.value = '발주 목록을 불러오지 못했습니다.'
  }
}

function changePage(p) {
  if (p < 1 || p > totalPages.value) return
  page.value = p
  loadOrders()
}

function goDetail(order) {
  router.push({ name: 'OrderDetail', params: { id: order.id } })
}

async function removeOrder(order) {
  if (!confirm(`${formatDate(order.orderDate)} 발주를 삭제하시겠습니까?\n품목도 모두 삭제됩니다.`)) return
  error.value = ''
  try {
    await deleteOrder(order.id)
    orders.value = orders.value.filter((o) => o.id !== order.id)
    totalCount.value -= 1
  } catch {
    error.value = '삭제 중 오류가 발생했습니다.'
  }
}

onMounted(loadOrders)
</script>
