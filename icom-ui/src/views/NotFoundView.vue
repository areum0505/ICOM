<template>
  <div class="flex flex-col items-center justify-center min-h-[60vh] text-center">
    <p class="text-6xl font-bold text-gray-300">404</p>
    <p class="mt-2 text-gray-500">페이지를 찾을 수 없습니다.</p>
    <RouterLink
      to="/products"
      class="mt-6 px-4 py-2 bg-blue-600 text-white text-sm rounded-lg hover:bg-blue-700"
    >
      홈으로
    </RouterLink>
  </div>
</template>
