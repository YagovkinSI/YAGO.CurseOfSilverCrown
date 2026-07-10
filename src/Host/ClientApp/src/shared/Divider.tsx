import React from 'react';

interface DividerProps {
    className?: string;
}

const Divider: React.FC<DividerProps> = ({ 
    className = '', 
}) => (
    <div className={`
        absolute bottom-24 left-1/2 -translate-x-1/2
        w-32 h-px bg-gradient-to-r from-transparent via-bright/20 to-transparent
        ${className}
    `} />
);

export default Divider;