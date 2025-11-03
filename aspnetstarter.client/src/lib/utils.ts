import type {ClassValue} from "clsx"
import {clsx} from "clsx"
import {twMerge} from "tailwind-merge"

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs))
}

export function getBackendBaseUrl() {
  return import.meta.env['ASPNETCORE_URLS'] ?? 'https://localhost:5001';
}
