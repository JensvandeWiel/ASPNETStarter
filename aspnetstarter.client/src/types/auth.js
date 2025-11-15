/**
 * @typedef {Object} LoginRequestBody
 * @property {string} email
 * @property {string} password
 */

/**
 * @typedef {Object} LoginOptions
 * @property {boolean} [useCookies]
 * @property {boolean} [useSessionCookies]
 */

/**
 * @typedef {Object} AccessTokenResponse
 * @property {string} tokenType
 * @property {string} accessToken
 * @property {string} refreshToken
 * @property {number} expiresIn
 */

/**
 * @typedef {Object} RefreshRequestBody
 * @property {string} refreshToken
 */

export {}

