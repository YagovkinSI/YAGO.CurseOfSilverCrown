import React from 'react';
import type { RowDataProps } from './RowData';
import RowData from './RowData';

interface StateListProps {
    items: RowDataProps[];
    className?: string;
}

const StateList: React.FC<StateListProps> = ({ items, className = '' }) => {
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
                    <RowData key={rowData.label} {...rowData} />
                </React.Fragment>
            ))}
        </div>
    );
};

export default StateList;