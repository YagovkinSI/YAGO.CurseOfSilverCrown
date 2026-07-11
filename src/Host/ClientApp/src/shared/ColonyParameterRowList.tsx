import React from 'react';
import ColonyParameterRow, { type ColonyParameterRowProps } from './ColonyParameterRow';

interface ColonyParameterRowListProps {
    items: ColonyParameterRowProps[];
    className?: string;
    dense?: boolean;
    maxWidth?: 'sm' | 'md' | 'lg' | 'full';
}

const maxWidthMap = {
    sm: 'max-w-sm',
    md: 'max-w-md',
    lg: 'max-w-lg',
    full: 'max-w-full',
};

const ColonyParameterRowList: React.FC<ColonyParameterRowListProps> = ({ 
    items, 
    className = '',
    dense = false,
    maxWidth = 'md',
}) => {
    if (items.length === 0) {
        return null;
    }

    return (
        <div
            className={`
                flex flex-col
                mx-auto w-full
                ${maxWidthMap[maxWidth]}
                ${dense ? 'gap-0.5' : 'gap-1'}
                ${className}
            `}
        >
            {items.map((rowData, index) => (
                <ColonyParameterRow 
                    key={rowData.label + index} 
                    {...rowData} 
                />
            ))}
        </div>
    );
};

export default ColonyParameterRowList;