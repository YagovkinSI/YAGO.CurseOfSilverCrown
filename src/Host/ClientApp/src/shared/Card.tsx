import React from 'react';
import Surface from './Surface';

interface CardProps {
    children?: React.ReactNode;
    className?: string;
    variant?: 'default' | 'glow' | 'error' | 'success';
    size?: 'md' | 'lg';
}

const Card: React.FC<CardProps> = ({
    children,
    className = '',
    variant = 'default',
    size = 'lg',
}) => {

    const sizeClasses = {
        md: 'p-4 gap-2',
        lg: 'p-8 md:p-12 gap-6 md:gap-10',
    };

    return (
        <Surface 
            variant={variant} 
            rounded={size}
            className={`
                w-full max-w-md
                flex flex-col items-center mx-auto 
                ${sizeClasses[size]} 
                ${className}
            `}
        >
            {children}
        </Surface>
    );
};

export default Card;