import { createRouter, createWebHistory } from 'vue-router'

const routes = [
  {
    path: '/',
    redirect: '/products',
  },
  {
    path: '/products',
    name: 'ProductList',
    component: () => import('../views/ProductListView.vue'),
    meta: { title: '상품 목록' },
  },
  {
    path: '/products/new',
    name: 'ProductCreate',
    component: () => import('../views/ProductCreateView.vue'),
    meta: { title: '상품 등록' },
  },
  {
    path: '/products/:id/edit',
    name: 'ProductEdit',
    component: () => import('../views/ProductEditView.vue'),
    meta: { title: '상품 수정' },
  },
  {
    path: '/products/:id',
    name: 'ProductDetail',
    component: () => import('../views/ProductDetailView.vue'),
    meta: { title: '상품 상세' },
  },
  {
    path: '/orders',
    name: 'OrderList',
    component: () => import('../views/OrderListView.vue'),
    meta: { title: '발주 목록' },
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'NotFound',
    component: () => import('../views/NotFoundView.vue'),
  },
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes,
})

// 페이지 이동 시 document.title 자동 업데이트
router.afterEach((to) => {
  document.title = to.meta.title ? `${to.meta.title} | ICOM` : 'ICOM'
})

export default router
