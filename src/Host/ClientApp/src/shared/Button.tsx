import React from 'react';
import type { LucideIcon } from 'lucide-react';

interface ButtonProps {
    children: React.ReactNode;
    onClick?: () => void;
    variant?: 'primary' | 'secondary' | 'danger' | 'ghost';
    size?: 'sm' | 'md' | 'lg';
    sizeMd?: 'sm' | 'md' | 'lg';
    icon?: LucideIcon;
    iconPosition?: 'left' | 'right';
    disabled?: boolean;
    loading?: boolean;
    className?: string;
    type?: 'button' | 'submit' | 'reset';
}

const variantMap = {
    primary: 'bg-bright text-dark hover:bg-[#d4ca4a] active:scale-95',
    secondary: 'border border-bright/30 text-light hover:bg-bright/10 active:scale-95',
    danger: 'bg-danger text-light hover:bg-[#b71c1c] active:scale-95',
    ghost: 'text-muted hover:text-light hover:bg-bright/5 active:scale-95',
};

const sizeMap = {
    sm: 'px-4 py-2 text-xs',
    md: 'px-6 py-3 text-sm',
    lg: 'px-8 py-4 text-base',
};

const sizeMdMap = {
    sm: 'md:px-4 md:py-2 md:text-xs',
    md: 'md:px-6 md:py-3 md:text-sm',
    lg: 'md:px-8 md:py-4 md:text-base',
};

const Button: React.FC<ButtonProps> = ({
    children,
    onClick,
    variant = 'primary',
    size = 'md',
    sizeMd = 'sm',
    icon: Icon,
    iconPosition = 'left',
    disabled = false,
    loading = false,
    className = '',
    type = 'button',
}) => {
    const isDisabled = disabled || loading;

    const renderLoading = () => {
        return <div className="w-5 h-5 border-2 border-current border-t-transparent rounded-full animate-spin" />
    }

    const renderButtonContent = () => {
        return <>
            {Icon && iconPosition === 'left' && <Icon className="w-4 h-4" />}
            <span>{children}</span>
            {Icon && iconPosition === 'right' && <Icon className="w-4 h-4" />}
        </>
    }

    return (
        <button
            type={type}
            onClick={onClick}
            disabled={isDisabled}
            className={`
                flex items-center justify-center gap-2 w-full
                font-semibold uppercase tracking-wide rounded-lg
                transition-all duration-200
                ${variantMap[variant]}
                ${sizeMap[size]}
                ${sizeMdMap[sizeMd]}
                ${isDisabled ? 'opacity-50 cursor-not-allowed active:scale-100 hover:!bg-opacity-100' : ''}
                ${className}
            `}
        >
            {loading ? renderLoading() : renderButtonContent()}
        </button>
    );
};

export default Button;