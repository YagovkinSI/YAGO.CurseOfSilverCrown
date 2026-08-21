import React from 'react';
import GameParameterRow from '../common/gameParameterRow/GameParameterRow';
import type { GameParameterValueStatus } from './colony.types';

export interface ColonyParameterRowProps {
    icon: React.ElementType;
    label: string;
    value: string;
    status?: GameParameterValueStatus;
    url?: string;
    infoUrl?: string;
}

const ColonyParameterRow: React.FC<ColonyParameterRowProps> = ({
    icon: Icon,
    label,
    value,
    status = 'neutral',
    url,
    infoUrl,
}) => {
    const renderIcon = () => (
        <Icon className="w-4 h-4 text-muted" />
    );

    return (
        <GameParameterRow
            iconNode={renderIcon()}
            label={label}
            value={value}
            valueStatus= {status}
            url={url}
            infoUrl={infoUrl}
        />
    );
};

export default ColonyParameterRow;