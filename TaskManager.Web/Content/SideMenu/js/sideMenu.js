/* ------------------------------------------------------------------
   File:  /Content/SideMenu/js/sideMenu.js
   Controla:
       · El menú lateral (#sidebar)     – Backlog & Kanban board
       · El popup de usuario (#user-menu)
   Mark-up:
       #menu-toggle   #sidebar   #topbar
       #user-icon     #user-menu
   ------------------------------------------------------------------*/
(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', () => {

        /* ───────────────────────────  referencias  ───────────────────────── */
        const toggleBtn = document.getElementById('menu-toggle'); // ☰
        const sidebar = document.getElementById('sidebar');
        const topbar = document.getElementById('topbar');

        const userIcon = document.getElementById('user-icon');   // foto + nombre
        const userMenu = document.getElementById('user-menu');   // popup

        if (!toggleBtn || !sidebar) return;   // no hay menú → salir

        /* ─────────────────────────────  helpers  ─────────────────────────── */
        const openSidebar = () => {
            sidebar.classList.add('active');
            topbar?.classList.add('menu-open');   // mueve logo al centro opcional
        };
        const closeSidebar = () => {
            sidebar.classList.remove('active');
            topbar?.classList.remove('menu-open');
        };
        const isSidebarOpen = () => sidebar.classList.contains('active');

        const openUserMenu = () => userMenu?.classList.add('show');
        const closeUserMenu = () => userMenu?.classList.remove('show');
        const isUserMenuOpen = () => userMenu?.classList.contains('show');

        const closeEverything = () => {
            closeSidebar();
            closeUserMenu();
        };

        /* ───────────── 1 · toggle con botón hamburguesa ───────────── */
        toggleBtn.addEventListener('click', e => {
            e.stopPropagation();
            isSidebarOpen() ? closeSidebar() : (closeUserMenu(), openSidebar());
        });

        /* ───────────── 2 · toggle con icono de usuario ────────────── */
        if (userIcon && userMenu) {
            userIcon.addEventListener('click', e => {
                e.stopPropagation();
                isUserMenuOpen() ? closeUserMenu() : (closeSidebar(), openUserMenu());
            });
        }

        /* ───────────── 3 · click fuera → cerrar menús ─────────────── */
        document.addEventListener('click', e => {
            const clickInsideSidebar = sidebar.contains(e.target) || toggleBtn.contains(e.target);
            const clickInsideMenu = userMenu && (userMenu.contains(e.target) || userIcon.contains(e.target));

            if (!clickInsideSidebar && !clickInsideMenu) closeEverything();
        });

        /* ───────────── 4 · <Esc> cierra lo que esté abierto ────────── */
        document.addEventListener('keydown', e => {
            if (e.key === 'Escape') closeEverything();
        });

        /* ───────────── 5 · resaltar link activo en sidebar ─────────── */
        const current = location.pathname.toLowerCase();
        sidebar.querySelectorAll('a').forEach(a => {
            if (a.pathname.toLowerCase() === current) {
                a.classList.add('active');        // define .active en sideMenu.css
            }
        });
    });

})();
