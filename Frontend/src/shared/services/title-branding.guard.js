export const titleBrandingGuard = (to, from, next) => {
  const baseTitle = 'AlquilaFacil';
  document.title = `${baseTitle} | ${to.meta['title']}`;
  next();
}