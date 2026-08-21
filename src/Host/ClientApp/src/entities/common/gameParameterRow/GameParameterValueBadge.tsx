import React from 'react';

interface GameParameterValueBadgeProps {
    label: string;
    value: string;
    color: string;
}

const GameParameterValueBadge: React.FC<GameParameterValueBadgeProps> = ({
    label,
    value,
    color,
}) => (
    <>
        <span className="flex-1 min-w-0 text-sm text-light/80 truncate">
            {label}
        </span>
        <span
            className="text-sm font-medium px-2 py-0.5 rounded"
            style={{ color }}
        >
            {value}
        </span>
    </>
);

export default GameParameterValueBadge;