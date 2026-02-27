import api from './index'

// 코드(Code) 관련 API 호출 함수 모음

/**
 * 특정 그룹의 상세 코드 목록 조회
 * @param {string} groupCode - 그룹 코드 (예: 'ProductCategory')
 */
export const getDetailCodes = (groupCode) => api.get(`/codes/${groupCode}/details`)
