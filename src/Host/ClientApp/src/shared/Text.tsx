import React from 'react';

interface TextProps {
    children: React.ReactNode;
    className?: string;
    variant?: 'primary' | 'secondary' | 'muted' | 'glass-dim'; // Заменили dim на glass-dim
    size?: 'xs' | 'sm' | 'base' | 'lg' | 'xl';
    align?: 'left' | 'center' | 'right';
    as?: 'p' | 'span' | 'div';
    maxWidth?: 'none' | 'sm' | 'md' | 'lg';
}

const variantMap = {
    primary: 'text-light',
    secondary: 'text-muted',
    muted: 'text-muted/70',
    'glass-dim': 'text-white/60',
};

const sizeMap = {
    xs: 'text-xs',
    sm: 'text-sm',
    base: 'text-base',
    lg: 'text-lg',
    xl: 'text-xl',
};

const alignMap = {
    left: 'text-left',
    center: 'text-center',
    right: 'text-right',
};

const maxWidthMap = {
    none: '',
    sm: 'max-w-sm',
    md: 'max-w-md',
    lg: 'max-w-lg',
};

const Text: React.FC<TextProps> = ({ 
    children, 
    className = '', 
    variant = 'secondary',
    size = 'sm',
    align = 'center',
    maxWidth = 'none',
    as: Component = 'p',
}) => (
    <Component className={`
            leading-relaxed
            ${variantMap[variant]} 
            ${sizeMap[size]} 
            ${alignMap[align]}
            ${maxWidthMap[maxWidth]}
            ${className}
        `}>
        {children}
    </Component>
);

export default Text;