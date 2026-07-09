import React from 'react';

interface YagoTitleProps {
    children: React.ReactNode;
    className?: string;
}

const YagoTitle: React.FC<YagoTitleProps> = ({ children, className = '' }) => (
    <h1 className={`text-xl md:text-2xl font-bold text-light tracking-wider uppercase ${className}`}>
        {children}
    </h1>
);

export default YagoTitle;