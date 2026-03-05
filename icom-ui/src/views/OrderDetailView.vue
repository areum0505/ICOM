<template>
  <div class="p-6">
    <!-- 헤더 -->
    <div class="flex items-center gap-4 mb-6">
      <button
        @click="router.push({ name: 'OrderList' })"
        class="text-sm text-gray-500 hover:text-gray-800"
      >
        ← 발주 목록
      </button>

      <template v-if="isNew">
        <h1 class="text-2xl font-bold text-gray-800">새 발주</h1>
        <input
          type="date"
          v-model="orderDate"
          class="border border-gray-300 rounded px-3 py-1.5 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
      </template>

      <h1 v-else class="text-2xl font-bold text-gray-800">{{ orderDateLabel }} 발주</h1>
    </div>

    <!-- 테이블 -->
    <div class="bg-white border border-gray-200 rounded-lg overflow-hidden">
      <table class="w-full text-sm">
        <thead class="bg-gray-50 border-b border-gray-200">
          <tr>
            <th class="px-4 py-3 text-left font-semibold text-gray-700 w-1/4">제품명</th>
            <th class="px-4 py-3 text-right font-semibold text-gray-700 w-1/5">매입(1BOX)</th>
            <th class="px-4 py-3 text-center font-semibold text-gray-700 w-1/5">매입수량</th>
            <th class="px-4 py-3 text-right font-semibold text-gray-700 w-1/5">매입금액</th>
            <th class="px-4 py-3 text-center font-semibold text-gray-700 w-12"></th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="rows.length === 0">
            <td colspan="5" class="px-4 py-6 text-center text-gray-400">
              아래 입력 영역에서 품목을 추가하세요.
            </td>
          </tr>
          <tr
            v-for="(row, index) in rows"
            :key="row._key"
            :class="row.status === 'deleted' ? 'bg-red-50 opacity-50' : row.status === 'added' ? 'bg-blue-50' : row.status === 'modified' ? 'bg-yellow-50' : ''"
            class="border-b border-gray-100 last:border-0"
          >
            <td class="px-4 py-2 text-gray-800" :class="{ 'line-through text-gray-400': row.status === 'deleted' }">
              {{ row.productName }}
            </td>
            <td class="px-4 py-2 text-right">
              <input
                v-model.number="row.boxPrice"
                @input="markModified(row)"
                type="number"
                min="0"
                :disabled="row.status === 'deleted'"
                class="w-full border border-gray-300 rounded px-2 py-1.5 text-sm text-right focus:outline-none focus:ring-2 focus:ring-blue-500 disabled:bg-gray-100"
              />
            </td>
            <td class="px-4 py-2 text-center">
              <input
                v-model.number="row.quantity"
                @input="markModified(row)"
                type="number"
                min="1"
                :disabled="row.status === 'deleted'"
                class="w-24 mx-auto border border-gray-300 rounded px-2 py-1.5 text-sm text-center focus:outline-none focus:ring-2 focus:ring-blue-500 disabled:bg-gray-100"
              />
            </td>
            <td class="px-4 py-2 text-right text-gray-800" :class="{ 'line-through text-gray-400': row.status === 'deleted' }">
              {{ formatNumber(row.boxPrice * row.quantity) }}원
            </td>
            <td class="px-4 py-2 text-center">
              <!-- 삭제 예약된 행: 되돌리기 -->
              <button
                v-if="row.status === 'deleted'"
                @click="row.status = null"
                class="text-gray-400 hover:text-gray-600 text-sm"
                title="되돌리기"
              >
                ↩
              </button>
              <!-- 일반 행: 삭제 -->
              <button
                v-else
                @click="removeRow(index, row)"
                class="text-red-400 hover:text-red-600 font-bold text-lg leading-none"
                title="삭제"
              >
                &times;
              </button>
            </td>
          </tr>
        </tbody>

        <!-- 합계 -->
        <tfoot v-if="rows.length > 0" class="bg-gray-50 border-t border-gray-200">
          <tr>
            <td colspan="3" class="px-4 py-3 text-right font-semibold text-gray-700">합계</td>
            <td class="px-4 py-3 text-right font-bold text-gray-900">
              {{ formatNumber(totalAmount) }}원
            </td>
            <td></td>
          </tr>
        </tfoot>
      </table>

      <!-- 고정 입력 영역 -->
      <div class="border-t-2 border-blue-200 bg-blue-50 px-4 py-3 flex items-center gap-2">
        <select
          v-model="draft.productId"
          @change="onDraftProductSelect"
          class="flex-1 border border-gray-300 rounded px-2 py-1.5 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
        >
          <option value="" disabled>제품 선택</option>
          <option v-for="p in products" :key="p.id" :value="p.id">{{ p.name }}</option>
        </select>
        <input
          v-model.number="draft.boxPrice"
          type="number"
          min="0"
          placeholder="매입단가"
          class="w-28 border border-gray-300 rounded px-2 py-1.5 text-sm text-right focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
        <input
          v-model.number="draft.quantity"
          type="number"
          min="1"
          placeholder="수량"
          class="w-20 border border-gray-300 rounded px-2 py-1.5 text-sm text-center focus:outline-none focus:ring-2 focus:ring-blue-500"
        />
        <span class="w-28 text-sm text-right text-gray-600 shrink-0">
          {{ formatNumber(draft.boxPrice * draft.quantity) }}원
        </span>
        <button
          @click="commitDraft"
          :disabled="!draft.productId"
          class="px-4 py-1.5 bg-blue-600 text-white text-sm rounded hover:bg-blue-700 disabled:opacity-40 disabled:cursor-not-allowed shrink-0"
        >
          추가
        </button>
      </div>
    </div>

    <!-- 저장 버튼 -->
    <div class="mt-4 flex justify-end gap-3">
      <span v-if="saveError" class="text-sm text-red-600 self-center">{{ saveError }}</span>
      <button
        @click="saveAll"
        :disabled="saving || !hasPendingChanges"
        class="px-6 py-2 bg-green-600 text-white text-sm rounded hover:bg-green-700 disabled:opacity-50 disabled:cursor-not-allowed"
      >
        {{ saving ? '저장 중...' : '저장' }}
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { getOrder, getOrderItems, createOrder, createOrderItem, updateOrderItem, deleteOrderItem } from '../api/orders'
import { getProducts } from '../api/products'

// 'added'  : draft에서 추가됐으나 아직 미저장
// 'modified': DB에 있고 수정됨
// 'deleted' : DB에 있고 삭제 예약
// null     : DB에 있고 변경 없음

const route = useRoute()
const router = useRouter()

const isNew = route.name === 'OrderCreate'
const orderId = isNew ? null : Number(route.params.id)

const rows = ref([])
const products = ref([])
const saving = ref(false)
const saveError = ref('')
let _keyCounter = 0

const orderDate = ref(new Date().toISOString().slice(0, 10))
const orderDateLabel = ref('')

const draft = ref({ productId: '', productName: '', boxPrice: 0, quantity: 1 })

const totalAmount = computed(() =>
  rows.value.reduce((sum, r) => sum + (r.boxPrice || 0) * (r.quantity || 0), 0)
)

const hasPendingChanges = computed(() => {
  if (isNew) return rows.value.some((r) => r.productId)
  return rows.value.some((r) => r.status !== null)
})

function formatNumber(val) {
  return Number(val || 0).toLocaleString('ko-KR')
}

function onDraftProductSelect() {
  const product = products.value.find((p) => p.id === draft.value.productId)
  if (product) {
    draft.value.productName = product.name
    draft.value.boxPrice = product.boxPrice
  }
}

function commitDraft() {
  if (!draft.value.productId) return
  rows.value.push({
    _key: ++_keyCounter,
    id: null,
    productId: draft.value.productId,
    productName: draft.value.productName,
    boxPrice: draft.value.boxPrice,
    quantity: draft.value.quantity,
    status: 'added',
  })
  draft.value = { productId: '', productName: '', boxPrice: 0, quantity: 1 }
}

async function loadOrder() {
  try {
    const res = await getOrder(orderId)
    orderDateLabel.value = String(res.data.orderDate).slice(0, 10)
  } catch {
    // 날짜 표시 실패는 무시
  }
}

async function loadItems() {
  try {
    const res = await getOrderItems(orderId)
    rows.value = res.data.map((item) => ({
      _key: ++_keyCounter,
      id: item.id,
      productId: item.productId,
      productName: item.productName,
      boxPrice: item.boxPrice,
      quantity: item.quantity,
      status: null,
    }))
  } catch {
    rows.value = []
  }
}

async function loadProducts() {
  try {
    const res = await getProducts({ pageSize: 1000 })
    products.value = res.data.items
  } catch {
    products.value = []
  }
}

function markModified(row) {
  if (row.status === null) row.status = 'modified'
}

function removeRow(index, row) {
  if (row.status === 'added') {
    rows.value.splice(index, 1)
  } else {
    row.status = 'deleted'
  }
}

async function saveAll() {
  saveError.value = ''
  saving.value = true
  try {
    if (isNew) {
      const orderRes = await createOrder({ orderDate: orderDate.value })
      const newOrderId = orderRes.data.id
      for (const row of rows.value) {
        if (row.productId) {
          await createOrderItem(newOrderId, {
            productId: row.productId,
            boxPrice: row.boxPrice,
            quantity: row.quantity,
          })
        }
      }
      router.replace({ name: 'OrderDetail', params: { id: newOrderId } })
    } else {
      for (const row of rows.value) {
        if (row.status === 'deleted' && row.id) {
          await deleteOrderItem(orderId, row.id)
        } else if (row.status === 'added' && row.productId) {
          const res = await createOrderItem(orderId, {
            productId: row.productId,
            boxPrice: row.boxPrice,
            quantity: row.quantity,
          })
          row.id = res.data.id
          row.status = null
        } else if (row.status === 'modified' && row.id) {
          await updateOrderItem(orderId, row.id, {
            boxPrice: row.boxPrice,
            quantity: row.quantity,
          })
          row.status = null
        }
      }
      rows.value = rows.value.filter((r) => r.status !== 'deleted')
    }
  } catch {
    saveError.value = '저장 중 오류가 발생했습니다.'
    setTimeout(() => (saveError.value = ''), 3000)
  } finally {
    saving.value = false
  }
}

onMounted(() => {
  loadProducts()
  if (!isNew) {
    loadOrder()
    loadItems()
  }
})
</script>
