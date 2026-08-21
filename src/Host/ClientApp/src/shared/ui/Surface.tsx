import React from 'react';

interface SurfaceProps {
    children?: React.ReactNode;
    className?: string;
    variant?: 'default' | 'glow' | 'error' | 'success';
    rounded?: 'md' | 'lg';
    ref?: React.RefObject<HTMLDivElement | null>;
}

const Surface: React.FC<SurfaceProps> = ({
    children,
    className = '',
    variant = 'default',
    rounded = 'md',
    ref = null
}) => {
    
    const roundedClasses = {
        'md': 'rounded-xl',
        'lg': 'rounded-3xl',
    };

    const variantClasses = {
        default: 'shadow-[0_8px_32px_rgba(0,0,0,0.4)]',
        glow: 'shadow-[0_8px_40px_rgba(240,230,92,0.15)]',
        error: 'shadow-[0_8px_40px_rgba(211,47,47,0.15)]',
        success: 'shadow-[0_8px_40px_rgba(76,175,80,0.15)]',
    };

    return (
        <div ref={ref} className={`
            bg-[#0a0f1a]/80 backdrop-blur-xl ring-1 ring-white/10 
            border border-white/5
            ${roundedClasses[rounded]}
            ${variantClasses[variant]}
            ${className}
        `}>
            {children}
        </div>
    );
};

export default Surface;