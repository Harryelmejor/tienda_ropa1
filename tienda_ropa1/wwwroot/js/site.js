/* ══════════════════════════════════════
   STORE — Interactive Features
   ══════════════════════════════════════ */

// ── Toast Notifications ──
function showToast(message, type = 'success') {
    let container = document.querySelector('.toast-container');
    if (!container) {
        container = document.createElement('div');
        container.className = 'toast-container';
        document.body.appendChild(container);
    }

    const icons = {
        success: '&#10003;',
        error: '&#10007;',
        info: '&#9432;'
    };

    const toast = document.createElement('div');
    toast.className = `toast toast-${type}`;
    toast.innerHTML = `
        <span class="toast-icon">${icons[type] || icons.info}</span>
        <span>${message}</span>
    `;
    toast.onclick = () => dismissToast(toast);
    container.appendChild(toast);

    setTimeout(() => dismissToast(toast), 4000);
}

function dismissToast(toast) {
    if (!toast || toast.classList.contains('toast-out')) return;
    toast.classList.add('toast-out');
    setTimeout(() => toast.remove(), 300);
}

// ── Auto-show toasts from TempData ──
document.addEventListener('DOMContentLoaded', function () {
    const successAlert = document.querySelector('.alert-success');
    const errorAlert = document.querySelector('.alert-error');

    if (successAlert) {
        showToast(successAlert.textContent.trim(), 'success');
        successAlert.style.display = 'none';
    }
    if (errorAlert) {
        showToast(errorAlert.textContent.trim(), 'error');
        errorAlert.style.display = 'none';
    }

    animateCounters();
});

// ── Animated Stat Counters ──
function animateCounters() {
    const statValues = document.querySelectorAll('.stat-value');
    statValues.forEach(el => {
        const target = parseInt(el.textContent.replace(/[^0-9]/g, ''), 10);
        if (isNaN(target) || target === 0) return;

        const suffix = el.textContent.includes('$') ? '$' : '';
        const duration = 1200;
        const start = performance.now();

        el.textContent = suffix + '0';

        function update(now) {
            const elapsed = now - start;
            const progress = Math.min(elapsed / duration, 1);
            const eased = 1 - Math.pow(1 - progress, 3);
            const current = Math.round(target * eased);
            el.textContent = suffix + current.toLocaleString();
            if (progress < 1) requestAnimationFrame(update);
        }
        requestAnimationFrame(update);
    });
}

// ── Confirm Delete Modal ──
function confirmDelete(url, entityName) {
    const overlay = document.createElement('div');
    overlay.className = 'modal-overlay';
    overlay.innerHTML = `
        <div class="modal-box">
            <div class="modal-icon modal-icon-danger">!</div>
            <h3>Confirmar eliminación</h3>
            <p>¿Está seguro que desea eliminar <strong>${entityName}</strong>? Esta acción no se puede deshacer.</p>
            <div class="modal-actions">
                <button class="btn btn-secondary" onclick="this.closest('.modal-overlay').remove()">Cancelar</button>
                <a href="${url}" class="btn btn-danger">Eliminar</a>
            </div>
        </div>
    `;
    overlay.onclick = function (e) {
        if (e.target === overlay) overlay.remove();
    };
    document.body.appendChild(overlay);
}

// ── Live Subtotal Calculation ──
function updateSaleSubtotal() {
    const prendas = window._prendasData || [];
    const container = document.getElementById('detalle-items');
    if (!container) return;

    let total = 0;
    const rows = container.querySelectorAll('.detalle-row');

    rows.forEach(row => {
        const select = row.querySelector('select');
        const input = row.querySelector('input[type="number"]');
        if (!select || !input) return;

        const prendaId = parseInt(select.value, 10);
        const cantidad = parseInt(input.value, 10) || 0;
        const prenda = prendas.find(p => p.id === prendaId);

        if (prenda && cantidad > 0) {
            const subtotal = prenda.precio * cantidad;
            total += subtotal;

            let subtotalEl = row.querySelector('.subtotal-indicator');
            if (!subtotalEl) {
                subtotalEl = document.createElement('span');
                subtotalEl.className = 'subtotal-indicator';
                subtotalEl.style.cssText = 'font-size:.75rem;color:var(--success);font-weight:600;margin-top:.25rem;display:block;';
                input.parentElement.appendChild(subtotalEl);
            }
            subtotalEl.textContent = `$${subtotal.toLocaleString('en-US', { minimumFractionDigits: 2 })}`;

            const stockMax = prenda.stock;
            if (cantidad > stockMax) {
                input.style.borderColor = 'var(--danger)';
                input.title = `Stock máximo: ${stockMax}`;
            } else if (cantidad > stockMax * 0.8) {
                input.style.borderColor = 'var(--warning)';
                input.title = `Stock disponible: ${stockMax}`;
            } else {
                input.style.borderColor = '';
                input.title = '';
            }
        } else {
            const subtotalEl = row.querySelector('.subtotal-indicator');
            if (subtotalEl) subtotalEl.remove();
            input.style.borderColor = '';
        }
    });

    let totalEl = document.getElementById('venta-subtotal-live');
    if (!totalEl) {
        totalEl = document.createElement('div');
        totalEl.id = 'venta-subtotal-live';
        totalEl.className = 'subtotal-live';
        const form = container.closest('form');
        if (form) {
            const actions = form.querySelector('.form-actions');
            if (actions) {
                form.insertBefore(totalEl, actions);
            }
        }
    }

    totalEl.innerHTML = `
        <span class="subtotal-live-label">Total estimado</span>
        <span>$${total.toLocaleString('en-US', { minimumFractionDigits: 2 })}</span>
    `;
}

// ── Bind live subtotal events ──
document.addEventListener('DOMContentLoaded', function () {
    const container = document.getElementById('detalle-items');
    if (!container) return;

    container.addEventListener('change', updateSaleSubtotal);
    container.addEventListener('input', updateSaleSubtotal);

    // Initial calculation
    setTimeout(updateSaleSubtotal, 200);
});

// ── Sale Detail Row Functions ──
function buildPrendasOptions() {
    if (window._prendasData && window._prendasData.length > 0) {
        let html = '<option value="">-- Seleccione --</option>';
        window._prendasData.forEach(p => {
            html += `<option value="${p.id}">${p.nombre} ($${p.precio.toFixed(2)}) — Stock: ${p.stock}</option>`;
        });
        return html;
    }
    return '<option value="">-- Seleccione --</option>';
}

function addDetalleRow() {
    const container = document.getElementById('detalle-items');
    if (!container) return;

    const rows = container.querySelectorAll('.detalle-row');
    const index = rows.length;
    const firstSelect = container.querySelector('select');
    const optionsHtml = firstSelect ? firstSelect.innerHTML : buildPrendasOptions();

    const row = document.createElement('div');
    row.className = 'detalle-row';
    row.innerHTML = `
        <div class="form-group">
            <label>Prenda</label>
            <select name="Items[${index}].PrendaId" class="form-control">${optionsHtml}</select>
        </div>
        <div class="form-group">
            <label>Cantidad</label>
            <input type="number" name="Items[${index}].Cantidad" value="1" min="1" class="form-control" />
        </div>
        <button type="button" class="btn-remove" onclick="removeDetalleRow(this)" title="Eliminar">&times;</button>
    `;
    container.appendChild(row);

    row.querySelector('select').addEventListener('change', updateSaleSubtotal);
    row.querySelector('input').addEventListener('input', updateSaleSubtotal);

    updateSaleSubtotal();
}

function removeDetalleRow(btn) {
    const container = document.getElementById('detalle-items');
    const rows = container.querySelectorAll('.detalle-row');
    if (rows.length <= 1) return;
    btn.closest('.detalle-row').remove();
    reindexDetalleRows();
    updateSaleSubtotal();
}

function reindexDetalleRows() {
    const container = document.getElementById('detalle-items');
    if (!container) return;

    container.querySelectorAll('.detalle-row').forEach((row, index) => {
        const select = row.querySelector('select');
        const input = row.querySelector('input[type="number"]');
        if (select) select.name = `Items[${index}].PrendaId`;
        if (input) input.name = `Items[${index}].Cantidad`;
    });
}

// ── Search debounce ──
let searchTimeout;
document.addEventListener('DOMContentLoaded', function () {
    const searchInput = document.querySelector('.search-bar input[type="text"]');
    if (!searchInput) return;

    searchInput.addEventListener('input', function () {
        clearTimeout(searchTimeout);
        searchTimeout = setTimeout(() => {
            const form = searchInput.closest('form');
            if (form) form.submit();
        }, 600);
    });
});

// ── Keyboard shortcuts ──
document.addEventListener('keydown', function (e) {
    if (e.key === 'Escape') {
        const modal = document.querySelector('.modal-overlay');
        if (modal) modal.remove();
    }
});

// ── Print invoice with animation ──
function printInvoice() {
    document.querySelector('.invoice').classList.add('printing');
    setTimeout(() => window.print(), 300);
}
