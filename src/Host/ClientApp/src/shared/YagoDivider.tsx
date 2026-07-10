import React from 'react';

interface YagoDividerProps {
    className?: string;
}

const YagoDivider: React.FC<YagoDividerProps> = ({ 
    className = '', 
}) => (
    <div className={`
        absolute bottom-24 left-1/2 -translate-x-1/2
        w-32 h-px bg-gradient-to-r from-transparent via-bright/20 to-transparent
        ${className}
    `} />
);

export default YagoDivider;