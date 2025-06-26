/***************************************************************************
 * Content/KanbanBoard/js/kanbanBoard.js
 * jQuery 3.7 + Bootstrap 5.3
 * ------------------------------------------------------------------------
 *  ▸ Modal “Ver tarea” (read-only)     → verTarea(...)
 *  ▸ Click handler en tarjetas Kanban
 ***************************************************************************/
(() => {
    'use strict';

    /**
     * Abre el modal 'Ver tarea' y carga sus campos.
     * @param {string} titulo          – Título de la tarea.
     * @param {string} descripcion     – Descripción de la tarea.
     * @param {string} fechaAsignacion – Fecha de asignación (yyyy-MM-dd).
     * @param {string} fechaVencimiento– Fecha de vencimiento (yyyy-MM-dd).
     * @param {string} estado          – Estado de la tarea.
     */
    window.verTarea = function (titulo, descripcion, fechaAsignacion, fechaVencimiento, estado) {
        $('#verTitulo').val(titulo);
        $('#verDescripcion').val(descripcion);
        $('#verFechaAsig').val(fechaAsignacion);
        $('#verFechaVto').val(fechaVencimiento);
        $('#verEstado').val(estado);
        bootstrap.Modal.getOrCreateInstance('#verTareaModal').show();
    };

    /**
     * Asigna el handler de clic a cada tarjeta Kanban cuando el DOM esté listo.
     * Cada .kanban-card debe llevar estos atributos data-:
     *   data-titulo, data-descripcion, data-asignacion,
     *   data-vencimiento, data-estado
     */
    $(function () {
        $('.kanban-card').on('click', function () {
            const $card = $(this);
            const titulo = $card.data('titulo') || '';
            const descripcion = $card.data('descripcion') || '';
            const fechaAsignacion = $card.data('asignacion') || '';
            const fechaVencimiento = $card.data('vencimiento') || '';
            const estado = $card.data('estado') || '';
            verTarea(titulo, descripcion, fechaAsignacion, fechaVencimiento, estado);
        });
    });
})();
