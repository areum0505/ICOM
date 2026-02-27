# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## 프로젝트 개요

**ICOM (Ice Cream Order Manager)** — 아이스크림 외 기타 품목의 매입 내역을 통합 관리하고, 단가/마진 자동 계산 및 발주서 생성을 제공하는 관리 시스템.

## 개발 서버 실행

```bash
# 백엔드 API (http://localhost:5186)
cd src/ICOM.Api && dotnet run --launch-profile http

# 프론트엔드 UI (http://localhost:5173)
cd icom-ui && npm run dev
```

Swagger: http://localhost:5186/swagger

## 주요 명령

```bash
# EF Core 마이그레이션 (src/ICOM.Api 에서)
dotnet ef migrations add <MigrationName> --project ../ICOM.Infrastructure
dotnet ef database update
```

## 아키텍처

### 백엔드 — Clean Architecture

```
src/
├── ICOM.Domain/          # 엔티티 (외부 의존성 없음)
├── ICOM.Application/     # DTOs, 서비스 인터페이스 및 구현
├── ICOM.Infrastructure/  # AppDbContext, 리포지토리 구현, DI 등록
└── ICOM.Api/             # 컨트롤러, Program.cs
```

의존 방향: `Api → Infrastructure → Application → Domain`

각 레이어에 `DependencyInjection.cs`가 있으며 `Program.cs`에서 `.AddApplicationServices()` / `.AddInfrastructureServices()`로 등록.

### 프론트엔드 — Vue 3 + Vite

```
icom-ui/src/
├── api/          # Axios 인스턴스(index.js), products.js, codes.js
├── components/   # 재사용 컴포넌트
├── views/        # 라우트별 페이지
└── router/       # Vue Router 4 설정
```

API base URL은 `icom-ui/.env.development`의 `VITE_API_BASE_URL`로 주입.

## 핵심 도메인 로직

### 가격 계산

`Product` 엔티티의 두 프로퍼티는 `[NotMapped]`로 DB에 저장되지 않고 실시간 계산된다.

- `UnitPrice = BoxPrice / PackageSize` (낱개 매입가)
- `Margin = (RetailPrice - UnitPrice) / RetailPrice` (마진율)

### 코드 테이블

카테고리 등 Enum 대신 `GroupCode` / `DetailCode` 테이블로 범용 코드를 관리한다.
조회 시 항상 `IsUse = true`, `DeleteDate = null` 필터를 적용한다.
프론트엔드에서는 `/api/codes/{groupCode}/details`로 드롭다운 목록을 로드한다.

## API 엔드포인트

| Method | Path | Query 파라미터 | 설명 |
|--------|------|----------------|------|
| GET | `/api/products` | `category`, `search`, `page`, `pageSize` | 상품 목록 |
| GET | `/api/products/{id}` | | 단건 조회 |
| POST | `/api/products` | | 상품 등록 |
| PUT | `/api/products/{id}` | | 상품 수정 |
| DELETE | `/api/products/{id}` | | 상품 삭제 |
| GET | `/api/codes/{groupCode}/details` | | 코드 목록 조회 |

## 데이터베이스

- **LocalDB (SQL Server)**: `Server=(localdb)\mssqllocaldb;Database=ICOM_DB`
- EF Core Code First; 마이그레이션은 `ICOM.Infrastructure/Migrations/`
- 컬럼 타입 규칙: 금액 → `decimal(18,2)`, 코드값 → `varchar(ASCII)`, 명칭 → `nvarchar`
- 소프트 삭제: `DeleteDate` nullable 컬럼으로 관리 (`IsUse = true`, `DeleteDate = null` 조건으로 조회)

## 네이밍 규칙

### C# 백엔드

| 대상 | 규칙 | 예시 |
|------|------|------|
| 클래스 / 인터페이스 | PascalCase | `ProductService`, `IProductRepository` |
| 인터페이스 | `I` 접두사 | `IProductService`, `ICodeRepository` |
| 메서드 | PascalCase + 비동기는 `Async` 접미사 | `GetByIdAsync`, `CreateAsync` |
| 프로퍼티 | PascalCase | `BoxPrice`, `TotalCount` |
| private 필드 | `_camelCase` | `_context`, `_repository` |
| DTO | 용도 + `Dto` 접미사 | `ProductDto`, `CreateProductDto`, `PagedResultDto<T>` |
| 컨트롤러 | 복수형 리소스 + `Controller` | `ProductsController` |
| 서비스 / 리포지토리 | 단수형 리소스 + 역할명 | `ProductService`, `ProductRepository` |
| 네임스페이스 | `ICOM.레이어명` | `ICOM.Application.DTOs` |

### Vue / JavaScript 프론트엔드

| 대상 | 규칙 | 예시 |
|------|------|------|
| 컴포넌트 파일 | PascalCase `.vue` | `ProductForm.vue`, `ProductList.vue` |
| View 파일 | PascalCase + `View` 접미사 | `ProductListView.vue`, `ProductEditView.vue` |
| API 모듈 파일 | camelCase `.js` | `products.js`, `codes.js` |
| 변수 / 함수 | camelCase | `fetchProducts`, `searchInput` |
| 상수 | UPPER_SNAKE_CASE | `PAGE_SIZE` |
| Vue Router 이름 | PascalCase | `ProductList`, `ProductCreate`, `ProductEdit` |
| emit 이벤트명 | kebab-case | `@success`, `@cancel`, `@delete` |

### DB / EF Core

| 대상 | 규칙 | 예시 |
|------|------|------|
| 테이블명 | 단수 PascalCase | `Product`, `GroupCode`, `DetailCode` |
| 컬럼명 | PascalCase | `BoxPrice`, `PackageSize` |
| PK | `Id` | `int Id` |
| FK (int) | 참조 엔티티명 + `Id` | `GroupCodeId` |
| FK (string 코드) | 참조 엔티티명 그대로 | `GroupCode` 컬럼 in `DetailCode` |

## 인증 설계 고려사항

추후 Phase 5에서 사용자 로그인 및 권한 관리를 도입할 예정이다. 기능 추가 시 아래 원칙을 따른다.

- **인증 방식**: JWT Bearer Token (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- **미들웨어**: `Program.cs`에 `UseAuthentication()` → `UseAuthorization()` 순서로 등록
- **컨트롤러 보호**: 모든 컨트롤러에 `[Authorize]` 적용, 공개 엔드포인트만 `[AllowAnonymous]`
- **역할(Role)**: 최소 `Admin` / `User` 분리
- **프론트엔드**: Axios 인터셉터(`src/api/index.js`)에서 Authorization 헤더 주입 및 401 처리 중앙 관리
- **라우터 가드**: Vue Router `beforeEach`에서 인증 상태 확인 후 미로그인 시 로그인 페이지로 리다이렉트

## 개발 로드맵

| Phase | 내용 |
|-------|------|
| 1 | 상품 CRUD + 단가/마진 관리 (완료) |
| 2 | 아이스크림 발주 리스트 및 이미지(PNG) 내보내기 |
| 3 | 과자/음료 엑셀/CSV 업로드 파이프라인 |
| 4 | 통합 통계 대시보드 및 지출 리포트 |
| 5 | 사용자 로그인 및 권한 관리 |
