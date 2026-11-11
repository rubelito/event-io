/** Clear both the current ID key and the legacy logout key. */
export function clearStoredSession(): void {
  for (const key of ['id', 'userId', 'username', 'isLogin', 'loginStatus', 'accessToken', 'role']) {
    localStorage.removeItem(key);
  }
}
