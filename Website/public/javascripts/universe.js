$(document).ready(function() {
  function classToggle() { 
    const navs = document.querySelectorAll('.u-nav__items')
    navs.forEach(nav => nav.classList.toggle('u-nav__link-toggle-show'));
  }
  document.querySelector('.u-nav__link-toggle')
    .addEventListener('click', classToggle);

});
