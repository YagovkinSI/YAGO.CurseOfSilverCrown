import React from 'react';
import type { ColonyParameterRowProps } from './ColonyParameterRow';
import ColonyParameterRow from './ColonyParameterRow';

interface ColonyParameterRowListProps {
    items: ColonyParameterRowProps[];
    className?: string;
}

const ColonyParameterRowList: React.FC<ColonyParameterRowListProps> = ({ items, className = '' }) => {
    const getMaxWidth = () => {
        // Используем медиа-запрос через Tailwind классы
        return 'w-full max-w-[350px] md:max-w-[700px]';
    };

    return (
        <div
            className={`
                flex flex-col gap-1
                mx-auto
                ${getMaxWidth()}
                ${className}
            `}
        >
            {items.map((rowData, index) => (
                <React.Fragment key={index}>
                    <ColonyParameterRow key={rowData.label} {...rowData} />
                </React.Fragment>
            ))}
        </div>
    );
};

export default ColonyParameterRowList;