
const TokenKey = 'JC-Token'

export function getToken() {
  console.log(localStorage.getItem(TokenKey))
  return localStorage.getItem(TokenKey)
}

export function setToken(token) {
  return localStorage.setItem(TokenKey, token)
}

export function removeToken() {
  return localStorage.setItem(TokenKey)
}
