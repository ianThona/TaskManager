/* -----------------------------------------------------------
   Content/Login/js/login.js · v2.6
   -----------------------------------------------------------*/
(function () {
    'use strict';

    /* ---------- referencias ---------- */
    const txtUser = document.getElementById('Username');
    const txtPass = document.getElementById('Password');
    const btnSubmit = document.getElementById('login-submit');
    const errorBox = document.getElementById('creds-error');
    const dict = window.userPass || {};     // { usuario : contraseña }

    /* ---------- helpers ---------- */
    const sameCreds = () => dict[txtUser.value.trim()] === txtPass.value;
    const hideError = () => errorBox?.classList.add('d-none');
    const showError = () => errorBox?.classList.remove('d-none');

    /* ---------- 1) Rellenar password **una sola vez** al cargar ---------- */
    document.addEventListener('DOMContentLoaded', () => {
        const stored = dict[txtUser.value.trim()];
        if (stored) {
            txtPass.value = stored;
            txtPass.dispatchEvent(new Event('input', { bubbles: true }));
        }
        hideError();
    }, { once: true });

    /* ---------- 2) Si el usuario toca el username, limpiar password ---------- */
    function clearPassword() {
        if (txtPass.value) {
            txtPass.value = '';
            txtPass.dispatchEvent(new Event('input', { bubbles: true }));
        }
        hideError();
    }
    ['input', 'change', 'keyup'].forEach(evt =>
        txtUser?.addEventListener(evt, clearPassword));

    /* ---------- floating label ---------- */
    function toggleFloat(e) {
        const box = e.target.closest('.form-outline'); if (!box) return;
        if (e.type === 'input') box.classList.toggle('filled', !!e.target.value);
        else if (e.type === 'focus') box.classList.add('filled');
        else if (e.type === 'blur' && !e.target.value) box.classList.remove('filled');
    }
    document.querySelectorAll('.form-outline > input').forEach(ctrl => {
        if (ctrl.value) ctrl.closest('.form-outline').classList.add('filled');
        ['input', 'focus', 'blur'].forEach(evt => ctrl.addEventListener(evt, toggleFloat));
    });

    /* ---------- pulsaciones ---------- */
    function handleInput() { hideError(); }
    txtUser?.addEventListener('input', handleInput);
    txtPass?.addEventListener('input', handleInput);

    /* ---------- submit con ENTER en password ---------- */
    if (txtPass?.form) {
        txtPass.addEventListener('keydown', e => {
            if (e.key === 'Enter') {
                e.preventDefault();
                (txtPass.form.requestSubmit && txtPass.form.requestSubmit()) || txtPass.form.submit();
            }
        });

        /* ---------- validación final ---------- */
        txtPass.form.addEventListener('submit', e => {
            if (!sameCreds()) {
                e.preventDefault();   // no envía al servidor
                showError();          // muestra el mensaje
            }
        });
    }
})();
