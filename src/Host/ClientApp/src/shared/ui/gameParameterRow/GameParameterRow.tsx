import React from 'react';
import { ChevronRight } from 'lucide-react';
import GameParameterRowContainer from './GameParameterRowContainer';
import GameParameterInfoButton from './GameParameterInfoButton';
import InfoTooltip from '../InfoTooltip';
import { statusColors, type GameParameterValueStatus } from '../../../entities/colonies/colony.types';

export interface GameParameterInfo {
    name: string;
    imageName: string | undefined;
    description: string[];
}

export interface GameParameterRowProps {
    iconNode: React.ReactNode;
    label: string;
    value: string;
    valueStatus: GameParameterValueStatus;
    leading?: React.ReactNode;
    url?: string;
    info?: GameParameterInfo | undefined;
}

const GameParameterRow: React.FC<GameParameterRowProps> = ({
    iconNode,
    label,
    value,
    valueStatus,
    leading,
    url,
    info,
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

    const renderLabel = () => (
        <span className="min-w-0 text-sm text-light/80 truncate">
            {label}
        </span>
    );

    const renderValue = () => {
        return <span
            className="text-sm font-medium px-2 py-0.5 rounded"
            style={{ color: statusColors[valueStatus] || statusColors.neutral }}
        >
            {value}
        </span>
    }

    const renderArrow = () => {
        if (!url) return null;
        return <ChevronRight className="flex-shrink-0 w-4 h-4 text-muted/50" />;
    };

    const renderInfoButton = () => {
        if (!info) return null;
        return (
            <InfoTooltip content={info}>
                <GameParameterInfoButton />
            </InfoTooltip>
        );
    };

    return (
        <GameParameterRowContainer url={url}>
            <div className="flex items-center gap-2 flex-1 min-w-0">
                {renderLeading()}
                {renderIcon()}
                {renderLabel()}
                {renderInfoButton()}
            </div>
            <div className="flex items-center gap-2 flex-shrink-0">
                {renderValue()}
                {renderArrow()}
            </div>
        </GameParameterRowContainer>
    );
};

export default GameParameterRow;