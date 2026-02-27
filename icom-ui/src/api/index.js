import axios from 'axios'

// ICOM API 기본 클라이언트
const api = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000/api',
  headers: {
    'Content-Type': 'application/json',
  },
})

// 요청 인터셉터: 공통 전처리 (토큰 등 필요 시 추가)
api.interceptors.request.use(
  (config) => config,
  (error) => Promise.reject(error)
)

// 응답 인터셉터: 공통 에러 처리
api.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error('[API Error]', error.response?.status, error.response?.data)
    return Promise.reject(error)
  }
)

export default api
