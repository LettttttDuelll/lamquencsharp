// ====== Login Page Script ======
document.addEventListener('DOMContentLoaded', function () {

    // ---- Floating Particles ----
    const particlesContainer = document.getElementById('particles');
    if (particlesContainer) {
        const particleCount = 30;
        for (let i = 0; i < particleCount; i++) {
            const particle = document.createElement('div');
            particle.classList.add('particle');
            particle.style.left = Math.random() * 100 + '%';
            particle.style.width = particle.style.height = (Math.random() * 3 + 1) + 'px';
            particle.style.animationDuration = (Math.random() * 15 + 10) + 's';
            particle.style.animationDelay = (Math.random() * 10) + 's';
            particle.style.opacity = Math.random() * 0.5 + 0.1;
            particlesContainer.appendChild(particle);
        }
    }

    // ---- Toggle Password Visibility ----
    const togglePassword = document.getElementById('togglePassword');
    const passwordInput = document.getElementById('password');

    if (togglePassword && passwordInput) {
        togglePassword.addEventListener('click', function () {
            const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
            passwordInput.setAttribute('type', type);

            const icon = this.querySelector('i');
            icon.classList.toggle('fa-eye');
            icon.classList.toggle('fa-eye-slash');

            this.style.transform = 'translateY(-50%) scale(0.85)';
            setTimeout(() => {
                this.style.transform = 'translateY(-50%) scale(1)';
            }, 120);
        });
    }

    // ---- Form Submission Loading State ----
    const loginForm = document.getElementById('loginForm');

    if (loginForm) {
        loginForm.addEventListener('submit', function (e) {
            const username = document.getElementById('username');
            const password = document.getElementById('password');

            if (!username.value || !password.value) {
                e.preventDefault();
                return;
            }

            const btnLogin = document.querySelector('.btn-login');
            const btnText = document.querySelector('.btn-text');

            if (btnLogin && btnText) {
                btnLogin.disabled = true;
                btnText.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Đang xử lý...';
            }
        });
    }

    // ---- Input Focus Animation ----
    const inputs = document.querySelectorAll('.field-input');

    inputs.forEach(input => {
        input.addEventListener('focus', function () {
            const wrapper = this.closest('.field-wrapper');
            if (wrapper) {
                wrapper.style.transform = 'scale(1.02)';
                wrapper.style.transition = 'transform 0.3s ease';
            }
        });

        input.addEventListener('blur', function () {
            const wrapper = this.closest('.field-wrapper');
            if (wrapper) {
                wrapper.style.transform = 'scale(1)';
            }
        });
    });

    // ---- Ripple Effect on Button ----
    function createRipple(event) {
        const button = event.currentTarget;
        const ripple = document.createElement('span');
        const diameter = Math.max(button.clientWidth, button.clientHeight);
        const radius = diameter / 2;

        ripple.style.cssText = `
            position: absolute;
            width: ${diameter}px;
            height: ${diameter}px;
            border-radius: 50%;
            background: rgba(255, 255, 255, 0.4);
            left: ${event.clientX - button.getBoundingClientRect().left - radius}px;
            top: ${event.clientY - button.getBoundingClientRect().top - radius}px;
            animation: ripple 0.6s ease-out;
            pointer-events: none;
        `;

        button.appendChild(ripple);
        setTimeout(() => ripple.remove(), 600);
    }

    // Inject ripple keyframes
    const style = document.createElement('style');
    style.textContent = `
        @keyframes ripple {
            0% { transform: scale(0); opacity: 1; }
            100% { transform: scale(4); opacity: 0; }
        }
    `;
    document.head.appendChild(style);

    const btnLogin = document.querySelector('.btn-login');
    if (btnLogin) {
        btnLogin.addEventListener('click', createRipple);
    }

    // ---- Keyboard Shortcuts ----
    document.addEventListener('keydown', function (e) {
        if (e.altKey && e.key === 'l') {
            e.preventDefault();
            const username = document.getElementById('username');
            if (username) username.focus();
        }
        if (e.key === 'Escape' && loginForm) {
            loginForm.reset();
        }
    });

    // ---- Auto-focus username ----
    setTimeout(() => {
        const username = document.getElementById('username');
        if (username) username.focus();
    }, 500);

    // ---- Checkbox animation ----
    const checkbox = document.getElementById('remember');
    if (checkbox) {
        checkbox.addEventListener('change', function () {
            const label = this.closest('.remember-me');
            if (label && this.checked) {
                label.style.transform = 'scale(1.05)';
                setTimeout(() => {
                    label.style.transform = 'scale(1)';
                }, 200);
            }
        });
    }
});

// ====== Notification System ======
function showNotification(message, type = 'info') {
    const notification = document.createElement('div');
    notification.className = `notification notification-${type}`;

    const icon = type === 'success' ? 'fa-check-circle' :
        type === 'error' ? 'fa-exclamation-circle' : 'fa-info-circle';

    notification.innerHTML = `
        <i class="fas ${icon}"></i>
        <span>${message}</span>
    `;

    document.body.appendChild(notification);
    setTimeout(() => notification.remove(), 3000);
}

window.showNotification = showNotification;
