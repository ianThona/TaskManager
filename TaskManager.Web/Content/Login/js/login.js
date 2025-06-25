/* -----------------------------------------------------------
   Content/Login/js/login.js · v2.5
   -----------------------------------------------------------*/
(function () {
    'use strict';

    /* ---------- refs ---------- */
    const txtUser = document.getElementById('Username');
    const txtPass = document.getElementById('Password');
    const btnSubmit = document.getElementById('login-submit');
    const errorBox = document.getElementById('creds-error');
    const dict = window.userPass || {};      // { usuario : contraseña }

    /* ---------- helpers ---------- */
    const sameCreds = () =>
        dict[txtUser.value.trim()] === txtPass.value;

    const hideError = () => errorBox?.classList.add('d-none');
    const showError = () => errorBox?.classList.remove('d-none');

    /* ---------- autocompletar ---------- */
    function fillPassword() {
        const stored = dict[txtUser.value.trim()];
        txtPass.value = stored ?? '';
        txtPass.dispatchEvent(new Event('input', { bubbles: true }));
    }
    txtUser?.addEventListener('input', fillPassword);
    document.addEventListener('DOMContentLoaded', fillPassword, { once: true });

    /* ---------- manejar pulsaciones ---------- */
    function handleInput() { hideError(); }
    txtUser?.addEventListener('input', handleInput);
    txtPass?.addEventListener('input', handleInput);

    /* ---------- floating label (sin cambios) ---------- */
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

    /* ---------- submit con ENTER en password ---------- */
    if (txtPass?.form) {
        txtPass.addEventListener('keydown', e => {
            if (e.key === 'Enter') {
                e.preventDefault();
                (txtPass.form.requestSubmit && txtPass.form.requestSubmit()) || txtPass.form.submit();
            }
        });
    }

    /* ---------- validación final ---------- */
    const form = txtPass?.form;
    if (form) {
        form.addEventListener('submit', e => {
            if (!sameCreds()) {
                e.preventDefault();   // no envía al servidor
                showError();          // muestra el mensaje
            }
        });
    }

    /* ---------- estado inicial ---------- */
    document.addEventListener('DOMContentLoaded', hideError, { once: true });

})();
