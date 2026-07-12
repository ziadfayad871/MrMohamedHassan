(function () {
    'use strict';

    function initApp() {
        var alerts = document.querySelectorAll('.alert-dismissible');
        alerts.forEach(function (alert) {
            setTimeout(function () { alert.remove(); }, 5000);
        });

        var deleteForms = document.querySelectorAll('form[data-confirm]');
        deleteForms.forEach(function (form) {
            form.addEventListener('submit', function (e) {
                if (!confirm(form.getAttribute('data-confirm') || 'هل أنت متأكد من الحذف؟')) {
                    e.preventDefault();
                }
            });
        });

        var dataLoadingForms = document.querySelectorAll('form[data-loading]');
        dataLoadingForms.forEach(function (form) {
            form.addEventListener('submit', function () {
                var btn = form.querySelector('button[type="submit"]');
                if (btn) {
                    btn.disabled = true;
                    btn.innerHTML = '<span class="spinner-border spinner-border-sm ms-2"></span> جاري الحفظ...';
                }
            });
        });

        window.printReceipt = function () { window.print(); };

        window.generateQR = function (content, elementId) {
            fetch('/QrCode/Generate?content=' + encodeURIComponent(content))
                .then(function (r) { return r.json(); })
                .then(function (d) { document.getElementById(elementId).src = d.qrCode; });
        };

        window.confirmDelete = function (message, callback) {
            if (confirm(message || 'هل أنت متأكد من الحذف؟')) { callback(); }
        };

        window.formatCurrency = function (amount) {
            return new Intl.NumberFormat('ar-EG', { style: 'decimal' }).format(amount) + ' ج.م';
        };

        window.debounce = function (func, wait) {
            var timeout;
            return function () {
                var ctx = this, args = arguments;
                clearTimeout(timeout);
                timeout = setTimeout(function () { func.apply(ctx, args); }, wait);
            };
        };

        var searchInput = document.querySelector('[data-search]');
        if (searchInput) {
            searchInput.addEventListener('input', window.debounce(function () {
                var q = this.value.toLowerCase();
                document.querySelectorAll('[data-search-item]').forEach(function (item) {
                    item.style.display = item.textContent.toLowerCase().includes(q) ? '' : 'none';
                });
            }, 300));
        }

        window.selectAllPresent = function () {
            document.querySelectorAll('select[id*="Status"]').forEach(function (s) { s.value = 'Present'; });
        };
        window.selectAllAbsent = function () {
            document.querySelectorAll('select[id*="Status"]').forEach(function (s) { s.value = 'Absent'; });
        };

        // Scroll reveal animation
        var revealElements = document.querySelectorAll('[data-reveal]');
        if ('IntersectionObserver' in window && revealElements.length) {
            var observer = new IntersectionObserver(function (entries) {
                entries.forEach(function (entry) {
                    if (entry.isIntersecting) {
                        entry.target.style.opacity = '1';
                        entry.target.style.transform = 'translateY(0)';
                        observer.unobserve(entry.target);
                    }
                });
            }, { threshold: 0.1 });
            revealElements.forEach(function (el) { observer.observe(el); });
        }
    }

    if (document.readyState === 'loading') {
        document.addEventListener('DOMContentLoaded', initApp);
    } else {
        initApp();
    }
})();
