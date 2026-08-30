import GameIcon from "../../../shared/ui/icons/GameIcon";
import type { GameParameterValueStatus } from "../../colonies/colony.types";
import GameParameterRow from "../gameParameterRow/GameParameterRow";
import type { GameVisibleEffect } from "./gameVisibleEffect.types";

export interface GameVisibleEffectProps {
    visibleEffect: GameVisibleEffect
}

const GameVisibleEffectUI: React.FC<GameVisibleEffectProps> = ({ visibleEffect: effect }) => {
    const getValueStatus = () : GameParameterValueStatus => {
        switch (effect.color)
        {
            case 'Negative': return 'critical';
            case 'Neutral': return 'neutral';
            case 'Positive': return 'good';
            default: return 'neutral'
        }
    }
    
    const renderIcon = () => (
        <GameIcon iconType={effect.iconType} className="w-4 h-4 text-muted" />
    );

    return (
        <GameParameterRow
            iconNode={renderIcon()}
            label={effect.label}
            value={effect.value}
            valueStatus={getValueStatus()}
            url={effect.url}
        />
    );
};

export default GameVisibleEffectUI;