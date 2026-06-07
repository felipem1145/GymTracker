import { useAuthStore } from '@/stores/auth'

export interface ValidationErrors {
  [field: string]: string[]
}

export class ApiError extends Error {
  status: number
  statusText: string
  url: string
  details: unknown
  validationErrors?: ValidationErrors

  constructor(params: {
    message: string
    status: number
    statusText: string
    url: string
    details: unknown
    validationErrors?: ValidationErrors
  }) {
    super(params.message)
    this.name = 'ApiError'
    this.status = params.status
    this.statusText = params.statusText
    this.url = params.url
    this.details = params.details
    this.validationErrors = params.validationErrors
  }
}

interface RequestOptions<TBody> {
  method?: 'GET' | 'POST' | 'PUT' | 'DELETE'
  body?: TBody
  headers?: Record<string, string>
  signal?: AbortSignal
  query?: Record<string, string | number | boolean | null | undefined>
}

function trimTrailingSlash(value: string): string {
  return value.endsWith('/') ? value.slice(0, -1) : value
}

const configuredBaseUrl = import.meta.env.VITE_API_BASE_URL?.trim()
const API_BASE_URL = trimTrailingSlash(configuredBaseUrl || 'http://localhost:5161/api')

function buildUrl(path: string, query?: RequestOptions<never>['query']): string {
  const normalizedPath = path.startsWith('/') ? path : `/${path}`
  const url = new URL(`${API_BASE_URL}${normalizedPath}`)

  if (query) {
    for (const [key, value] of Object.entries(query)) {
      if (value === null || value === undefined) {
        continue
      }

      url.searchParams.set(key, String(value))
    }
  }

  return url.toString()
}

function parseJsonSafely(value: string): unknown {
  try {
    return JSON.parse(value)
  } catch {
    return null
  }
}

function extractErrorData(data: unknown, fallbackMessage: string): {
  message: string
  validationErrors?: ValidationErrors
} {
  if (!data || typeof data !== 'object') {
    return { message: fallbackMessage }
  }

  const candidate = data as {
    title?: unknown
    detail?: unknown
    message?: unknown
    errors?: unknown
  }

  const validationErrors =
    candidate.errors && typeof candidate.errors === 'object' && !Array.isArray(candidate.errors)
      ? (candidate.errors as ValidationErrors)
      : undefined

  const validationMessage = validationErrors
    ? Object.values(validationErrors)
        .flat()
        .join(' ')
        .trim()
    : ''

  const message =
    (typeof candidate.detail === 'string' && candidate.detail.trim()) ||
    (typeof candidate.message === 'string' && candidate.message.trim()) ||
    (typeof candidate.title === 'string' && candidate.title.trim()) ||
    validationMessage ||
    fallbackMessage

  return { message, validationErrors }
}

async function request<TResponse, TBody = never>(
  path: string,
  options: RequestOptions<TBody> = {},
): Promise<TResponse> {
  const { method = 'GET', body, headers, signal, query } = options
  const url = buildUrl(path, query)
  const authStore = useAuthStore()
  const accessToken = authStore.accessToken

  const response = await fetch(url, {
    method,
    headers: {
      Accept: 'application/json',
      ...(body !== undefined ? { 'Content-Type': 'application/json' } : {}),
      ...(accessToken ? { Authorization: `Bearer ${accessToken}` } : {}),
      ...headers,
    },
    body: body === undefined ? undefined : JSON.stringify(body),
    signal,
  })

  const rawText = await response.text()
  const parsed = rawText ? parseJsonSafely(rawText) : null

  if (!response.ok) {
    const fallbackMessage = `HTTP ${response.status} ${response.statusText}`
    const { message, validationErrors } = extractErrorData(parsed, fallbackMessage)

    throw new ApiError({
      message,
      status: response.status,
      statusText: response.statusText,
      url,
      details: parsed ?? rawText,
      validationErrors,
    })
  }

  if (!rawText || response.status === 204) {
    return undefined as TResponse
  }

  return (parsed ?? rawText) as TResponse
}

export const apiClient = {
  get<TResponse>(path: string, options?: Omit<RequestOptions<never>, 'method' | 'body'>) {
    return request<TResponse>(path, { ...options, method: 'GET' })
  },
  post<TResponse, TBody>(path: string, body: TBody, options?: Omit<RequestOptions<TBody>, 'method' | 'body'>) {
    return request<TResponse, TBody>(path, { ...options, method: 'POST', body })
  },
  put<TResponse, TBody>(path: string, body: TBody, options?: Omit<RequestOptions<TBody>, 'method' | 'body'>) {
    return request<TResponse, TBody>(path, { ...options, method: 'PUT', body })
  },
  delete<TResponse>(path: string, options?: Omit<RequestOptions<never>, 'method' | 'body'>) {
    return request<TResponse>(path, { ...options, method: 'DELETE' })
  },
}
