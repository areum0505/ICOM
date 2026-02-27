<template>
  <form @submit.prevent="handleSubmit" class="space-y-6">

    <!-- 기본 정보 -->
    <section class="bg-white rounded-lg border border-gray-200 p-6">
      <h2 class="text-sm font-semibold text-gray-500 uppercase tracking-wide mb-4">기본 정보</h2>
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">

        <div class="md:col-span-2">
          <label class="block text-sm font-medium text-gray-700 mb-1">
            상품명 <span class="text-red-500">*</span>
          </label>
          <input
            v-model="form.name"
            type="text"
            placeholder="예) 메로나"
            :class="fieldClass(errors.name)"
          />
          <p v-if="errors.name" class="mt-1 text-xs text-red-500">{{ errors.name }}</p>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">
            카테고리 <span class="text-red-500">*</span>
          </label>
          <select
            v-model="form.category"
            :class="fieldClass(errors.category)"
            :disabled="categoriesLoading"
          >
            <option value="" disabled>
              {{ categoriesLoading ? '불러오는 중...' : '선택하세요' }}
            </option>
            <option v-for="cat in categories" :key="cat.code" :value="cat.code">
              {{ cat.name }}
            </option>
          </select>
          <p v-if="errors.category" class="mt-1 text-xs text-red-500">{{ errors.category }}</p>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">공급업체</label>
          <input
            v-model="form.supplier"
            type="text"
            placeholder="예) 빙그레"
            :class="fieldClass()"
          />
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">바코드</label>
          <input
            v-model="form.barcode"
            type="text"
            placeholder="예) 8801234567890"
            :class="fieldClass()"
          />
        </div>

      </div>
    </section>

    <!-- 가격 정보 -->
    <section class="bg-white rounded-lg border border-gray-200 p-6">
      <h2 class="text-sm font-semibold text-gray-500 uppercase tracking-wide mb-4">가격 정보</h2>
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4">

        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">
            박스당 매입가 <span class="text-red-500">*</span>
          </label>
          <div class="relative">
            <input
              v-model.number="form.boxPrice"
              type="number" min="1" step="1"
              placeholder="0"
              :class="fieldClass(errors.boxPrice) + ' pr-6'"
            />
            <span class="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 text-sm">원</span>
          </div>
          <p v-if="errors.boxPrice" class="mt-1 text-xs text-red-500">{{ errors.boxPrice }}</p>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">
            입수 (박스당 개수) <span class="text-red-500">*</span>
          </label>
          <div class="relative">
            <input
              v-model.number="form.packageSize"
              type="number" min="1" step="1"
              placeholder="0"
              :class="fieldClass(errors.packageSize) + ' pr-8'"
            />
            <span class="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 text-sm">개</span>
          </div>
          <p v-if="errors.packageSize" class="mt-1 text-xs text-red-500">{{ errors.packageSize }}</p>
        </div>

        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">
            판매가 <span class="text-red-500">*</span>
          </label>
          <div class="relative">
            <input
              v-model.number="form.retailPrice"
              type="number" min="1" step="1"
              placeholder="0"
              :class="fieldClass(errors.retailPrice) + ' pr-6'"
            />
            <span class="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 text-sm">원</span>
          </div>
          <p v-if="errors.retailPrice" class="mt-1 text-xs text-red-500">{{ errors.retailPrice }}</p>
        </div>

      </div>

      <!-- 자동 계산 미리보기 -->
      <div v-if="unitPrice !== null" class="mt-4 flex gap-6 p-3 bg-gray-50 rounded-lg text-sm">
        <div>
          <span class="text-gray-500">낱개 매입가</span>
          <span class="ml-2 font-semibold text-gray-800">{{ formatCurrency(unitPrice) }}</span>
        </div>
        <div>
          <span class="text-gray-500">마진율</span>
          <span
            class="ml-2 font-semibold"
            :class="margin >= 0.3 ? 'text-green-600' : 'text-orange-500'"
          >
            {{ formatMargin(margin) }}
          </span>
        </div>
      </div>
    </section>

    <!-- 재고 -->
    <section class="bg-white rounded-lg border border-gray-200 p-6">
      <h2 class="text-sm font-semibold text-gray-500 uppercase tracking-wide mb-4">재고</h2>
      <div class="w-40">
        <label class="block text-sm font-medium text-gray-700 mb-1">초기 재고 수량</label>
        <div class="relative">
          <input
            v-model.number="form.stockQuantity"
            type="number" min="0" step="1"
            placeholder="0"
            :class="fieldClass() + ' pr-8'"
          />
          <span class="absolute right-3 top-1/2 -translate-y-1/2 text-gray-400 text-sm">개</span>
        </div>
      </div>
    </section>

    <!-- 액션 버튼 -->
    <div class="flex justify-end gap-3">
      <button
        type="button"
        class="px-5 py-2 text-sm font-medium text-gray-600 bg-white border border-gray-300 rounded-lg hover:bg-gray-50 transition-colors"
        @click="$emit('cancel')"
      >
        취소
      </button>
      <button
        type="submit"
        :disabled="submitting"
        class="px-5 py-2 text-sm font-medium text-white bg-blue-600 rounded-lg hover:bg-blue-700 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
      >
        {{ submitting ? '저장 중...' : isEditMode ? '수정 완료' : '상품 등록' }}
      </button>
    </div>

  </form>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { createProduct, updateProduct } from '../api/products'
import { getDetailCodes } from '../api/codes'

const props = defineProps({
  /** 수정 모드일 때 상품 ID */
  productId: { type: Number, default: null },
  /** 수정 모드일 때 초기 데이터 */
  initialData: { type: Object, default: null },
})

const emit = defineEmits(['success', 'cancel'])

const isEditMode = computed(() => props.productId !== null)

const form = ref({
  name:          props.initialData?.name          ?? '',
  category:      props.initialData?.category      ?? '',
  supplier:      props.initialData?.supplier      ?? '',
  barcode:       props.initialData?.barcode       ?? '',
  boxPrice:      props.initialData?.boxPrice      ?? null,
  packageSize:   props.initialData?.packageSize   ?? null,
  retailPrice:   props.initialData?.retailPrice   ?? null,
  stockQuantity: props.initialData?.stockQuantity ?? 0,
})

const categories = ref([])
const categoriesLoading = ref(false)

const errors = ref({})
const submitting = ref(false)

/** ProductCategory 상세 코드 로드 */
onMounted(async () => {
  categoriesLoading.value = true
  try {
    const { data } = await getDetailCodes('ProductCategory')
    categories.value = data
  } finally {
    categoriesLoading.value = false
  }
})

/** 낱개 매입가 실시간 계산 */
const unitPrice = computed(() => {
  const { boxPrice, packageSize } = form.value
  if (boxPrice > 0 && packageSize > 0) return boxPrice / packageSize
  return null
})

/** 마진율 실시간 계산 */
const margin = computed(() => {
  const { retailPrice } = form.value
  if (unitPrice.value !== null && retailPrice > 0)
    return (retailPrice - unitPrice.value) / retailPrice
  return null
})

/** 입력 필드 CSS 클래스 */
function fieldClass(error) {
  const base = 'w-full px-3 py-2 text-sm border rounded-lg focus:outline-none focus:ring-2 transition-colors'
  return error
    ? `${base} border-red-300 focus:ring-red-200`
    : `${base} border-gray-300 focus:ring-blue-200`
}

/** 폼 유효성 검사 */
function validate() {
  const e = {}
  if (!form.value.name.trim()) e.name = '상품명은 필수입니다.'
  if (!form.value.category.trim()) e.category = '카테고리는 필수입니다.'
  if (!form.value.boxPrice || form.value.boxPrice <= 0) e.boxPrice = '박스당 매입가를 입력하세요.'
  if (!form.value.packageSize || form.value.packageSize < 1) e.packageSize = '입수는 1 이상이어야 합니다.'
  if (!form.value.retailPrice || form.value.retailPrice <= 0) e.retailPrice = '판매가를 입력하세요.'
  errors.value = e
  return Object.keys(e).length === 0
}

/** 등록/수정 제출 */
async function handleSubmit() {
  if (!validate()) return
  submitting.value = true
  try {
    const { data } = isEditMode.value
      ? await updateProduct(props.productId, form.value)
      : await createProduct(form.value)
    emit('success', data)
  } catch (e) {
    const msg = e.response?.data?.title || (isEditMode.value ? '상품 수정에 실패했습니다.' : '상품 등록에 실패했습니다.')
    errors.value._server = msg
  } finally {
    submitting.value = false
  }
}

/** 금액 포맷 */
function formatCurrency(value) {
  if (value == null) return '-'
  return Number(value).toLocaleString('ko-KR', { maximumFractionDigits: 0 }) + '원'
}

/** 마진율 포맷 */
function formatMargin(value) {
  if (value == null) return '-'
  return (Number(value) * 100).toFixed(2) + '%'
}
</script>
