/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        // Ваши цвета из игры
        dark: '#050515',
        bright: '#f0e65c',
        muted: '#9ca3af',
        light: '#fafaf8',
        // Для статусов
        good: '#4caf50',
        warning: '#f0e65c',
        danger: '#d32f2f',
        info: '#2196f3',
      },
      // Ваши шрифты
      fontFamily: {
        sans: ['Roboto', 'sans-serif'],
      },
      // Кастомные анимации
      animation: {
        'pulse-glow': 'pulse-glow 2s ease-in-out infinite',
        'border-pulse': 'border-pulse 2.5s ease-in-out infinite', // ← добавляем вторую
      },
      keyframes: {
        'pulse-glow': {
          '0%, 100%': { boxShadow: '0 0 20px rgba(240, 230, 92, 0.1)' },
          '50%': { boxShadow: '0 0 40px rgba(240, 230, 92, 0.2)' },
        },
        'border-pulse': {  // ← добавляем
          '0%, 100%': { borderColor: 'rgba(240, 230, 92, 0.2)' },
          '50%': { borderColor: 'rgba(240, 230, 92, 0.6)' },
        },
      }
    },
  },
  plugins: [],
}