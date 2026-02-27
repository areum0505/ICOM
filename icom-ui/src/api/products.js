import api from './index'

// 상품(Product) 관련 API 호출 함수 모음

/**
 * 상품 목록 조회 (필터 + 페이지네이션)
 * @param {{ category?: string, search?: string, page?: number, pageSize?: number }} params
 */
export const getProducts = (params) => api.get('/products', { params })

/**
 * 상품 단건 조회
 * @param {number} id - 상품 ID
 */
export const getProduct = (id) => api.get(`/products/${id}`)

/**
 * 상품 등록
 * @param {object} payload - 상품 데이터
 */
export const createProduct = (payload) => api.post('/products', payload)

/**
 * 상품 수정
 * @param {number} id - 상품 ID
 * @param {object} payload - 수정 데이터
 */
export const updateProduct = (id, payload) => api.put(`/products/${id}`, payload)

/**
 * 상품 삭제
 * @param {number} id - 상품 ID
 */
export const deleteProduct = (id) => api.delete(`/products/${id}`)
