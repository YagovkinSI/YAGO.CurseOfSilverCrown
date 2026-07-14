import React, { useState } from 'react';
import { ChevronRight, HelpCircle } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

export type ParameterStatus = 'critical' | 'bad' | 'neutral' | 'good' | 'excellent';

export interface ColonyParameterRowProps {
    icon: React.ElementType;
    label: string;
    value: string;
    status?: ParameterStatus;
    url?: string;
    infoUrl?: string;
}

const statusColors: Record<ParameterStatus, string> = {
    critical: '#ef4444',    // red-500
    bad: '#f59e0b',         // amber-500
    neutral: '#6b7280',     // gray-500
    good: '#22c55e',        // green-500
    excellent: '#22d3ee',   // cyan-400
};

const statusGlow: Record<ParameterStatus, string> = {
    critical: 'shadow-red-500/20',
    bad: 'shadow-amber-500/20',
    neutral: 'shadow-gray-500/10',
    good: 'shadow-green-500/20',
    excellent: 'shadow-cyan-400/20',
};

const ColonyParameterRow: React.FC<ColonyParameterRowProps> = ({
    icon: Icon,
    label,
    value,
    status = 'neutral',
    url,
    infoUrl,
}) => {
    const navigate = useNavigate();
    const [_, setShowTooltip] = useState(false);
    const color = statusColors[status] || statusColors.neutral;

    const handleRowClick = () => {
        if (url) navigate(url);
    };

    const handleInfoClick = (e: React.MouseEvent) => {
        e.stopPropagation();
        if (infoUrl) navigate(infoUrl);
    };

    return (
        <div
            className={`
                relative flex items-center gap-2 px-3 py-2 rounded-lg
                transition-all duration-200
                ${url ? 'cursor-pointer hover:bg-bright/5 hover:scale-[1.01]' : 'cursor-default'}
                bg-dark/40 border border-bright/5
                ${statusGlow[status]}
            `}
            onClick={handleRowClick}
            role={url ? 'button' : 'article'}
            tabIndex={url ? 0 : undefined}
            onKeyDown={(e) => {
                if (url && (e.key === 'Enter' || e.key === ' ')) {
                    e.preventDefault();
                    handleRowClick();
                }
            }}
        >
            {/* Иконка */}
            <div className="flex-shrink-0 w-7 h-7 flex items-center justify-center">
                <Icon className="w-4 h-4 text-muted" />
            </div>

            {/* Название (обрезается если длинное) */}
            <span className="flex-1 min-w-0 text-sm text-light/80 truncate">
                {label}
            </span>

            {/* Значение с цветом статуса */}
            <span 
                className="text-sm font-medium px-2 py-0.5 rounded"
                style={{ color }}
            >
                {value}
            </span>

            {/* Кнопка "?" — справка */}
            {infoUrl && (
                <button
                    onClick={handleInfoClick}
                    className="flex-shrink-0 p-1 rounded-md text-muted hover:text-bright hover:bg-bright/10 transition-colors"
                    aria-label="Справка"
                    onMouseEnter={() => setShowTooltip(true)}
                    onMouseLeave={() => setShowTooltip(false)}
                >
                    <HelpCircle className="w-4 h-4" />
                </button>
            )}

            {/* Стрелка → если есть подменю */}
            {url && <ChevronRight className="flex-shrink-0 w-4 h-4 text-muted/50" />}
        </div>
    );
};

export default ColonyParameterRow;