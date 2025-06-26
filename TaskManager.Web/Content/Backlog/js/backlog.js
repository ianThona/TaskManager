/***************************************************************************
 * backlog.js · jQuery 3.7 + Bootstrap 5.3
 * ------------------------------------------------------------------------
 *  ▸ Dropdown (⋮)                       → toggleDropdown(...)
 *  ▸ Modal “Ver tarea”  (read-only)     → verTarea(...)
 *  ▸ Modal “Editar tarea”               → editarTarea(...)
 *  ▸ Modal “Crear tarea” (validación)   → gestionado en $(document).ready
 *  ▸ Eliminar tarea                     → deleteRow(...)
 *  ▸ Filtrado cliente                   → filterTasks(...)
 *  ▸ Modal “Reasignar folio”            → showReassignModal(...)
 ***************************************************************************/
(() => {
    'use strict';

    // — URL al método Delete en BacklogController (inyectado desde la vista) —
    if (typeof deleteUrl === 'undefined') {
        console.error('deleteUrl no está definido. Asegúrate de inyectarlo en la vista.');
    }

    /**
     * Eliminar una tarea por AJAX y, si va bien, quitar la fila de la UI.
     * @param {number} id – Id de la tarea a borrar.
     */
    function deleteRow(id) {
        $.post(deleteUrl, {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val(),
            id: id
        })
            .done(res => {
                if (res.ok) {
                    $('#row-' + id).remove();
                } else {
                    alert('No se pudo eliminar: ' + res.msg);
                }
            })
            .fail(() => {
                alert('Error al comunicarse con el servidor.');
            });
    }
    window.deleteRow = deleteRow;


    /* =========================================================
       1 · DROPDOWN ⋮
    ========================================================= */
    function closeAllDropdowns() {
        document.querySelectorAll('.dropdown-menu.show')
            .forEach(m => m.classList.remove('show'));
    }
    window.closeAllDropdowns = closeAllDropdowns;

    window.toggleDropdown = (e, icon) => {
        e.stopPropagation();
        const menu = icon.nextElementSibling;
        if (!menu) return;

        const wasOpen = menu.classList.contains('show');
        closeAllDropdowns();
        if (wasOpen) return;

        const r = icon.getBoundingClientRect();
        const GAP = 8;
        const minW = menu.offsetWidth || 150;
        const tooWide = r.left + minW > innerWidth;

        Object.assign(menu.style, {
            position: 'fixed',
            top: `${r.bottom + GAP}px`,
            left: tooWide ? 'auto' : `${r.left}px`,
            right: tooWide ? `${GAP}px` : 'auto'
        });
        menu.classList.add('show');
    };

    document.addEventListener('click', e => {
        if (!e.target.closest('.dropdown-container'))
            closeAllDropdowns();
    });


    /* =========================================================
       2 · MODAL “VER TAREA”
    ========================================================= */
    window.verTarea = (titulo, desc, fAsig, fVto, estado) => {
        $('#verTitulo').val(titulo);
        $('#verDescripcion').val(desc);
        $('#verFechaAsig').val(fAsig);
        $('#verFechaVto').val(fVto);
        $('#verEstado').val(estado);
        bootstrap.Modal.getOrCreateInstance('#verTareaModal').show();
        closeAllDropdowns();
    };


    /* =========================================================
       3 · MODAL “EDITAR TAREA”
    ========================================================= */
    window.editarTarea = (id, titulo, desc, fAsig, fVto, estado) => {
        $('#IdEditar').val(id);
        $('#TituloEditar').val(titulo);
        $('#DescripcionEditar').val(desc);
        $('#FechaAsignacionEditar').val(fAsig);
        $('#FechaVencimientoEditar').val(fVto);
        $('#EstadoEditar').val(estado);
        bootstrap.Modal.getOrCreateInstance('#editarTareaModal').show();
        closeAllDropdowns();
    };


    /* =========================================================
       4 · DOCUMENT READY (Alta, Edición, Filtrado, Reasignar folio)
    ========================================================= */
    $(function () {
        // Fecha base para nuevos registros
        const today = new Date().toISOString().slice(0, 10); // yyyy-MM-dd
        $('#FechaAsignacion').val(today).prop('disabled', true);

        /* — 4.1 · Alta (“Crear tarea”) — */
        $('#formCrearTarea').on('submit', function (e) {
            e.preventDefault();
            const $f = $(this);
            const url = $f.data('action');
            const $err = $('#crearTareaError').addClass('d-none');
            const estadoText = $('#EstadoNueva option:selected').text() || 'Por hacer';
            const payload = {
                __RequestVerificationToken: $('[name="__RequestVerificationToken"]').val(),
                Titulo: $('#TituloNueva').val().trim(),
                Descripcion: $('#DescripcionNueva').val(),
                Vencimiento: $('#FechaVencimientoNueva').val(),
                Estado: estadoText
            };

            if (!payload.Titulo)
                return showErr('El título es obligatorio.');
            if (!payload.Vencimiento || payload.Vencimiento <= today)
                return showErr('La fecha de vencimiento debe ser mayor a hoy.');

            $.post(url, payload)
                .done(res => {
                    if (!res.ok) return showErr(res.msg);
                    addRow(res.task);
                    bootstrap.Modal.getInstance('#crearTareaModal').hide();
                    $f[0].reset();
                    $('#FechaAsignacion').val(today);
                })
                .fail(() => showErr('Error inesperado al guardar.'));

            function showErr(msg) {
                $err.text(msg).removeClass('d-none');
                setTimeout(() => $err.addClass('d-none'), 4000);
            }
        });

        /* — 4.2 · Edición (“Guardar” en modal Editar) — */
        $('#formEditarTarea').on('submit', function (e) {
            e.preventDefault();
            const $f = $(this);
            const url = $f.data('action');
            const $err = $('#editarTareaError').addClass('d-none');
            const estadoText = $('#EstadoEditar option:selected').text() || 'Por hacer';
            const payload = {
                __RequestVerificationToken: $('[name="__RequestVerificationToken"]').val(),
                IdTask: $('#IdEditar').val(),
                Titulo: $('#TituloEditar').val().trim(),
                Descripcion: $('#DescripcionEditar').val(),
                Vencimiento: $('#FechaVencimientoEditar').val(),
                Estado: estadoText
            };

            if (!payload.Titulo)
                return showErr('El título es obligatorio.');
            if (!payload.Vencimiento || payload.Vencimiento <= today)
                return showErr('La fecha de vencimiento debe ser mayor a hoy.');

            $.post(url, payload)
                .done(res => {
                    if (!res.ok) return showErr(res.msg);
                    updateRow(res.task);
                    bootstrap.Modal.getInstance('#editarTareaModal').hide();
                })
                .fail(() => showErr('Error inesperado al actualizar.'));

            function showErr(msg) {
                $err.text(msg).removeClass('d-none');
                setTimeout(() => $err.addClass('d-none'), 4000);
            }
        });

        /* — 4.3 · Filtrado cliente — */
        function filterTasks() {
            const titleInput = $('#Titulo').val().trim();
            const status = $('#Estatus').val();
            const dateFilter = $('#Fecha').val();
            let titleRx = null;

            if (titleInput) {
                try { titleRx = new RegExp(titleInput, 'i'); }
                catch { return alert('Expresión regular inválida en Título'); }
            }

            $('#tbodyTasks tr').each(function () {
                const $tr = $(this);
                const title = $tr.find('td:eq(1)').text();
                const estado = $tr.data('estado');
                const venc = $tr.data('vencimiento');
                let show = true;

                if (titleRx && !titleRx.test(title)) show = false;
                if (status && estado !== status) show = false;
                if (dateFilter && venc !== dateFilter) show = false;

                $tr.toggle(show);
            });
        }
        $('#btnFilter').on('click', e => {
            e.preventDefault();
            filterTasks();
        });

        /* — 4.4 · Helpers para la tabla — */
        function esc(s) {
            return (s || '').replace(/'/g, "\\'");
        }
        function addRow(t) {
            $('#tbodyTasks').append(`
<tr id="row-${t.id}" data-estado="${esc(t.estado)}" data-vencimiento="${t.vencimiento}">
  <td>${t.id}</td>
  <td>${t.titulo}</td>
  <td class="text-center">
    <div class="dropdown-container">
      <i class="fa-solid fa-ellipsis-vertical icon-applied"
         onclick="toggleDropdown(event,this)"></i>
      <ul class="dropdown-menu">
        <li><a href="#" onclick="verTarea('${esc(t.titulo)}','${esc(t.descripcion || '')}','${t.asignacion}','${t.vencimiento}','${t.estado}')">Ver tarea</a></li>
        <li><a href="#" onclick="editarTarea(${t.id},'${esc(t.titulo)}','${esc(t.descripcion || '')}','${t.asignacion}','${t.vencimiento}','${t.estado}')">Editar</a></li>
        <li><a href="#" onclick="deleteRow(${t.id})">Eliminar</a></li>
      </ul>
    </div>
  </td>
</tr>`);
        }
        function updateRow(t) {
            const $tr = $(`#row-${t.id}`);
            if (!$tr.length) return addRow(t);
            $tr.find('td:eq(1)').text(t.titulo);
            $tr.data('estado', t.estado).data('vencimiento', t.vencimiento);
        }

        /* — 4.5 · Reasignar folio (sin cambios) — */
        // … tu código original …

    });
})();
