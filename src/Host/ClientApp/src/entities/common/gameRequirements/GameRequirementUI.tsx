import GameIcon from "../../../shared/ui/icons/GameIcon";
import type { GameParameterValueStatus } from "../../colonies/colony.types";
import GameParameterRow from "../../../shared/ui/gameParameterRow/GameParameterRow";
import type { GameRequirement } from "./gameRequirement.types";

export interface GameRequirementProps {
    requirement: GameRequirement
}

const GameRequirementUI: React.FC<GameRequirementProps> = ({ requirement }) => {
    const valueStatus : GameParameterValueStatus = requirement.isMet ? 'good' : 'critical';

    const renderStatusIcon = () => requirement.isMet ? '✅' : '❌';

    const renderIcon = () => (
        <GameIcon iconType={requirement.iconType} className="w-4 h-4 text-muted" />
    );

    return (
        <GameParameterRow
            iconNode={renderIcon()}
            label={requirement.label}
            value={requirement.value}
            valueStatus={valueStatus}
            leading={renderStatusIcon()}
            url={requirement.url}
        />
    );
};

export default GameRequirementUI;