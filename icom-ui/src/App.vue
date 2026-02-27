<template>
  <div class="min-h-screen bg-gray-100">
    <!-- 헤더 / GNB -->
    <header class="bg-white border-b border-gray-200 px-6 py-3 flex items-center gap-8">
      <span class="text-lg font-bold text-blue-700 tracking-wide">ICOM</span>
      <nav class="flex gap-4 text-sm font-medium">
        <RouterLink
          to="/products"
          class="text-gray-600 hover:text-blue-600 transition-colors"
          active-class="text-blue-600 border-b-2 border-blue-600 pb-0.5"
        >
          상품 관리
        </RouterLink>
        <RouterLink
          to="/orders"
          class="text-gray-600 hover:text-blue-600 transition-colors"
          active-class="text-blue-600 border-b-2 border-blue-600 pb-0.5"
        >
          발주 관리
        </RouterLink>
      </nav>
    </header>

    <!-- 페이지 렌더링 영역 -->
    <main class="max-w-5xl mx-auto mt-6">
      <RouterView />
    </main>
  </div>
</template>
