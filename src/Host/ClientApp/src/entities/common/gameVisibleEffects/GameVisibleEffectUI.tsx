import { ChevronRight, HelpCircle } from "lucide-react";
import { useNavigate } from "react-router-dom";
import GameIcon from "../../../shared/ui/icons/GameIcon";
import type { EffectColor, GameVisibleEffect } from "./gameVisibleEffect.types";

export type RequirementParameterType = 'default';

export interface GameVisibleEffectProps {
    visibleEffect: GameVisibleEffect
}

const effectColors : Record<EffectColor, string> = {
        Negative: '#ef4444',
        Neutral: '#b8b9bb',
        Positive: '#22c55e'
    };

const GameVisibleEffectUI: React.FC<GameVisibleEffectProps> = ({ visibleEffect: effect }) => {
    const navigate = useNavigate();

    const handleRowClick = () => {
        if (effect.url) navigate(effect.url);
    };

    const handleInfoClick = (e: React.MouseEvent) => {
        e.stopPropagation();
        if (effect.infoUrl) navigate(effect.infoUrl);
    };

    const color = effectColors[effect.color];
    return (
        <div
            className={`
                relative flex items-center gap-2 px-3 py-2 rounded-lg
                transition-all duration-200
                ${effect.url ? 'cursor-pointer hover:bg-bright/5 hover:scale-[1.01]' : 'cursor-default'}
                bg-dark/40 border border-bright/5
                shadow-gray-500/10
            `}
            onClick={handleRowClick}
            role={effect.url ? 'button' : 'article'}
            tabIndex={effect.url ? 0 : undefined}
            onKeyDown={(e) => {
                if (effect.url && (e.key === 'Enter' || e.key === ' ')) {
                    e.preventDefault();
                    handleRowClick();
                }
            }}
        >
            <div className="flex-shrink-0 w-7 h-7 flex items-center justify-center">
                ℹ️
            </div>

            {/* Иконка */}
            <div className="flex-shrink-0 w-7 h-7 flex items-center justify-center">
                <GameIcon iconType={effect.iconType} className="w-4 h-4 text-muted" />
            </div>

            {/* Название (обрезается если длинное) */}
            <span className='flex-1 min-w-0 text-sm truncate text-light/80'
            >
                <span style={{ color }}>{effect.value}</span>
                <span>{effect.label} </span>
            </span>

            {/* Кнопка "?" — справка */}
            {effect.infoUrl && (
                <button
                    onClick={handleInfoClick}
                    className="flex-shrink-0 p-1 rounded-md text-muted hover:text-bright hover:bg-bright/10 transition-colors"
                    aria-label="Справка"
                >
                    <HelpCircle className="w-4 h-4" />
                </button>
            )}

            {/* Стрелка → если есть подменю */}
            {effect.url && <ChevronRight className="flex-shrink-0 w-4 h-4 text-muted/50" />}
        </div>
    );
};

export default GameVisibleEffectUI;