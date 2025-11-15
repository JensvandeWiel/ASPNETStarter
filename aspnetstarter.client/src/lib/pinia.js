import {getActivePinia} from "pinia";

/**
 * @typedef {import('pinia').Pinia & {_s: Map<string, import('pinia').Store>}} ExtendedPinia
 */

/**
 * Creates reset utility for Pinia stores
 * @returns {Record<string, () => void>} Object with store reset methods
 */
export const $ResetPinia = () => {
  const pinia = getActivePinia();

  if (!pinia) {
    throw new Error("There is no stores");
  }

  const resetStores = {};

  pinia._s.forEach((store, name) => {
    resetStores[name] = () => store.$reset();
  });

  // Add 'all' method to reset all stores
  resetStores.all = () => pinia._s.forEach((store) => store.$reset());
  return resetStores;
};

