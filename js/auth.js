window.auth = (function () {
  async function getMe(apiBase) {
    try {
      const res = await fetch(apiBase + '/auth/me', {
        method: 'GET',
        credentials: 'include'
      });
      if (res.status === 401) return null;
      if (!res.ok) return null;
      return await res.json();
    } catch (e) {
      return null;
    }
  }

  async function logout(apiBase) {
    await fetch(apiBase + '/auth/logout', {
      method: 'GET',
      credentials: 'include'
    });
  }

  return { getMe, logout };
})();

