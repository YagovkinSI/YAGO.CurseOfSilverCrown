import React from 'react';

interface ButtonLinkProps {
    children: React.ReactNode;
    onClick?: () => void;
    variant?: 'primary' | 'secondary';
    disabled?: boolean;
    className?: string;
}

const ButtonLink: React.FC<ButtonLinkProps> = ({
    children,
    onClick,
    variant = 'primary',
    disabled = false,
    className
}) => {

    const variantMap = {
        primary: 'text-bright/80 hover:text-bright',
        secondary: 'text-muted/50 hover:text-bright',
    };

    return (
        <button
            onClick={onClick}
            disabled={disabled}
            className={`text-sm transition-colors 
            ${variantMap[variant]}
            ${className}`}
        >
            {children}
        </button>
    );
};

export default ButtonLink;