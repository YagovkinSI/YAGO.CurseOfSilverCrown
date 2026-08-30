import React from 'react';
import StatisticRow from './StatisticRow';
import type { StatisticField } from './statistics.types';

interface StatisticRowListProps {
    fields: StatisticField[];
    className?: string;
    dense?: boolean;
    maxWidth?: 'sm' | 'md' | 'lg' | 'full';
    showRank?: boolean;
}

const maxWidthMap = {
    sm: 'max-w-sm',
    md: 'max-w-md',
    lg: 'max-w-lg',
    full: 'max-w-full',
};

const StatisticRowList: React.FC<StatisticRowListProps> = ({
    fields,
    className = '',
    dense = false,
    maxWidth = 'md',
    showRank = false,
}) => {
    if (fields.length === 0) return null;

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
            {fields.map((field, index) => (
                <StatisticRow key={field.label + index} field={field} rank={showRank ? index + 1 : undefined} />
            ))}
        </div>
    );
};

export default StatisticRowList;
