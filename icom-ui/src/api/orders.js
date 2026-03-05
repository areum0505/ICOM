import api from './index'

export const getOrders = (params) => api.get('/orders', { params })
export const getOrder = (id) => api.get(`/orders/${id}`)
export const createOrder = (payload) => api.post('/orders', payload)
export const deleteOrder = (id) => api.delete(`/orders/${id}`)

export const getOrderItems = (orderId) => api.get(`/orders/${orderId}/items`)
export const createOrderItem = (orderId, payload) => api.post(`/orders/${orderId}/items`, payload)
export const updateOrderItem = (orderId, itemId, payload) => api.put(`/orders/${orderId}/items/${itemId}`, payload)
export const deleteOrderItem = (orderId, itemId) => api.delete(`/orders/${orderId}/items/${itemId}`)
