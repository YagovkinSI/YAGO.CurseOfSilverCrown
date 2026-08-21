import { ChevronRight, HelpCircle } from "lucide-react";
import { useNavigate } from "react-router-dom";
import GameIcon from "../../../shared/ui/icons/GameIcon";
import type { GameRequirement } from "./gameRequirement.types";

export type RequirementParameterType = 'default';

export interface GameRequirementProps {
    requirement: GameRequirement
}

const GameRequirementUI: React.FC<GameRequirementProps> = ({ requirement }) => {
    const navigate = useNavigate();

    const handleRowClick = () => {
        if (requirement.url) navigate(requirement.url);
    };

    const handleInfoClick = (e: React.MouseEvent) => {
        e.stopPropagation();
        if (requirement.infoUrl) navigate(requirement.infoUrl);
    };

    const color = requirement.isMet ? '#22c55e' : '#ef4444';
    return (
        <div
            className={`
                relative flex items-center gap-2 px-3 py-2 rounded-lg
                transition-all duration-200
                ${requirement.url ? 'cursor-pointer hover:bg-bright/5 hover:scale-[1.01]' : 'cursor-default'}
                bg-dark/40 border border-bright/5
                shadow-gray-500/10
            `}
            onClick={handleRowClick}
            role={requirement.url ? 'button' : 'article'}
            tabIndex={requirement.url ? 0 : undefined}
            onKeyDown={(e) => {
                if (requirement.url && (e.key === 'Enter' || e.key === ' ')) {
                    e.preventDefault();
                    handleRowClick();
                }
            }}
        >
            <div className="flex-shrink-0 w-7 h-7 flex items-center justify-center">
                {requirement.isMet ? '✅' : '❌'}
            </div>

            {/* Иконка */}
            <div className="flex-shrink-0 w-7 h-7 flex items-center justify-center">
                <GameIcon iconType={requirement.iconType} className="w-4 h-4 text-muted" />
            </div>

            {/* Название (обрезается если длинное) */}
            {requirement.isLabelFirst
                ? <span className='flex-1 min-w-0 text-sm truncate text-light/80'
                >
                    <span>{requirement.label} </span>
                    <span style={{ color }}>{requirement.value}</span>
                </span>
                : <span className='flex-1 min-w-0 text-sm truncate text-light/80'
                >
                    <span style={{ color }}>{requirement.value}</span>
                    <span>{requirement.label} </span>
                </span>}


            {/* Кнопка "?" — справка */}
            {requirement.infoUrl && (
                <button
                    onClick={handleInfoClick}
                    className="flex-shrink-0 p-1 rounded-md text-muted hover:text-bright hover:bg-bright/10 transition-colors"
                    aria-label="Справка"
                >
                    <HelpCircle className="w-4 h-4" />
                </button>
            )}

            {/* Стрелка → если есть подменю */}
            {requirement.url && <ChevronRight className="flex-shrink-0 w-4 h-4 text-muted/50" />}
        </div>
    );
};

export default GameRequirementUI;