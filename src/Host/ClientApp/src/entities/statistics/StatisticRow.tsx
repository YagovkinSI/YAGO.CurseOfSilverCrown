import React from 'react';
import GameParameterRow from '../common/gameParameterRow/GameParameterRow';
import { categoryIcons } from './categoryIcons';
import type { StatisticField } from './statistics.types';

const StatisticRow: React.FC<{ field: StatisticField; rank?: number }> = ({ field, rank }) => {
    const Icon = categoryIcons[field.category] ?? categoryIcons.Info;
    const renderIcon = () => <Icon className="w-4 h-4 text-muted" />;
    const renderRank = () => <span className="w-4 text-center text-sm text-muted tabular-nums">{rank}</span>;

    return (
        <GameParameterRow
            iconNode={rank !== undefined ? renderRank() : renderIcon()}
            label={field.label}
            value={field.value}
            valueStatus={field.status}
            url={field.childrenCode ? `/me/statistics/${field.childrenCode}` : undefined}
            info={field.info ?? undefined}
        />
    );
};

export default StatisticRow;
