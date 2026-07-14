import React from 'react';

interface TitleProps {
    children: React.ReactNode;
    className?: string;
    uppercase?: boolean;
    size?: 'h1' | 'h2' | 'h3';
}

const sizeMap = {
    h1: 'text-2xl md:text-3xl',
    h2: 'text-xl md:text-2xl',
    h3: 'text-lg md:text-xl',
};

const Title: React.FC<TitleProps> = ({ 
    children, 
    className = '', 
    uppercase = true,
    size = 'h2'
}) => (
    <h1 className={`
        font-bold text-light 
        tracking-wide 
        ${uppercase ? 'uppercase' : 'normal-case'}
        ${sizeMap[size]}
        ${className}
    `}>
        {children}
    </h1>
);

export default Title;