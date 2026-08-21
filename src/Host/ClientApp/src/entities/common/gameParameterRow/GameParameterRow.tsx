import React from 'react';
import { ChevronRight } from 'lucide-react';
import GameParameterRowContainer from './GameParameterRowContainer';
import GameParameterValueBadge from './GameParameterValueBadge';
import GameParameterInfoButton from './GameParameterInfoButton';
import type { GameParameterValueStatus } from '../../colonies/colony.types';

const statusColors: Record<GameParameterValueStatus, string> = {
    critical: '#ef4444',    // red-500
    bad: '#f59e0b',         // amber-500
    neutral: '#6b7280',     // gray-500
    good: '#22c55e',        // green-500
    excellent: '#22d3ee',   // cyan-400
};

export interface GameParameterRowProps {
    iconNode: React.ReactNode;
    label: string;
    value: string;
    valueStatus: GameParameterValueStatus;
    leading?: React.ReactNode;
    url?: string;
    infoUrl?: string;
}

const GameParameterRow: React.FC<GameParameterRowProps> = ({
    iconNode,
    label,
    value,
    valueStatus,
    leading,
    url,
    infoUrl,
}) => {
    const renderLeading = () => {
        if (!leading) return null;
        return (
            <div className="flex-shrink-0 w-7 h-7 flex items-center justify-center">
                {leading}
            </div>
        );
    };

    const renderIcon = () => (
        <div className="flex-shrink-0 w-7 h-7 flex items-center justify-center">
            {iconNode}
        </div>
    );

    const renderArrow = () => {
        if (!url) return null;
        return <ChevronRight className="flex-shrink-0 w-4 h-4 text-muted/50" />;
    };

    return (
        <GameParameterRowContainer
            url={url}
        >
            {renderLeading()}
            {renderIcon()}
            <GameParameterValueBadge
                label={label} value={value}
                color={statusColors[valueStatus] || statusColors.neutral} />
            <GameParameterInfoButton infoUrl={infoUrl} />
            {renderArrow()}
        </GameParameterRowContainer>
    );
};

export default GameParameterRow;