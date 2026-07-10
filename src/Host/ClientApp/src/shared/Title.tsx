import React from 'react';

interface TitleProps {
    children: React.ReactNode;
    className?: string;
}

const Title: React.FC<TitleProps> = ({ children, className = '' }) => (
    <h1 className={`text-xl md:text-2xl font-bold text-light tracking-wider uppercase ${className}`}>
        {children}
    </h1>
);

export default Title;