import {clsx} from "clsx"
import {twMerge} from "tailwind-merge"

/**
 * Combines class names using clsx and tailwind-merge
 * @param {...any[]} inputs - Class names or objects to merge
 * @returns {string} Merged class names
 */
export function cn(...inputs) {
  return twMerge(clsx(inputs))
}

/**
 * Gets the backend base URL from environment variables
 * @returns {string} The backend base URL
 */
export function getBackendBaseUrl() {
  return import.meta.env['ASPNETCORE_URLS'] ?? 'https://localhost:5001';
}

