import React from 'react';
import GameParameterRow from '../common/gameParameterRow/GameParameterRow';
import { categoryIcons } from './categoryIcons';
import type { StatisticField } from './statistics.types';

const StatisticRow: React.FC<{ field: StatisticField }> = ({ field }) => {
    const Icon = categoryIcons[field.category] ?? categoryIcons.Info;
    const renderIcon = () => <Icon className="w-4 h-4 text-muted" />;

    return (
        <GameParameterRow
            iconNode={renderIcon()}
            label={field.label}
            value={field.value}
            valueStatus={field.status}
            url={field.childrenCode ? `/me/statistics/${field.childrenCode}` : undefined}
        />
    );
};

export default StatisticRow;
