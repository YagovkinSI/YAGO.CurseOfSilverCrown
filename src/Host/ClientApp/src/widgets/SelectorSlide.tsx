import React from 'react';
import { ChevronLeft, ChevronRight } from 'lucide-react';

interface YagoCardContentSelectionProps {
    handlePrev: () => void;
    label: string;
    handleNext: () => void;
    disabledPrev?: boolean;
    disabledNext?: boolean;
}

const YagoCardContentSelection: React.FC<YagoCardContentSelectionProps> = ({
    handlePrev,
    label,
    handleNext,
    disabledPrev = false,
    disabledNext = false
}) => {
    const renderDecorationLines = () => (
        <>
            {/* Левая линия (декоративная) */}
            <div className="absolute left-0 top-1/2 -translate-y-1/2 w-0.5 h-8 bg-bright/20 rounded-full" />
            
            {/* Правая линия (декоративная) */}
            <div className="absolute right-0 top-1/2 -translate-y-1/2 w-0.5 h-8 bg-bright/20 rounded-full" />
            
            {/* Верхняя линия */}
            <div className="absolute top-0 left-1/2 -translate-x-1/2 w-3/4 h-px bg-gradient-to-r from-transparent via-bright/20 to-transparent" />
            
            {/* Нижняя линия */}
            <div className="absolute bottom-0 left-1/2 -translate-x-1/2 w-3/4 h-px bg-gradient-to-r from-transparent via-bright/20 to-transparent" />
        </>
    );

    const renderPrevButton = () => (
        <button
            onClick={handlePrev}
            disabled={disabledPrev}
            className={`
                relative group p-2 rounded-full transition-all duration-200
                ${disabledPrev 
                    ? 'opacity-40 cursor-not-allowed' 
                    : 'hover:bg-bright/10 hover:scale-110 active:scale-95'
                }
            `}
            aria-label="Назад"
        >
            <ChevronLeft className="w-6 h-6 text-bright" />
            <span className="absolute -bottom-8 left-1/2 -translate-x-1/2 text-xs text-muted opacity-0 group-hover:opacity-100 transition-opacity duration-200 whitespace-nowrap">
                Назад
            </span>
        </button>
    );

    const renderLabel = () => (
        <div className="flex-1 flex justify-center px-4">
            <h6 className="text-lg font-medium text-light text-center">
                {label}
            </h6>
        </div>
    );

    const renderNextButton = () => (
        <button
            onClick={handleNext}
            disabled={disabledNext}
            className={`
                relative group p-2 rounded-full transition-all duration-200
                ${disabledNext 
                    ? 'opacity-40 cursor-not-allowed' 
                    : 'hover:bg-bright/10 hover:scale-110 active:scale-95'
                }
            `}
            aria-label="Вперёд"
        >
            <ChevronRight className="w-6 h-6 text-bright" />
            <span className="absolute -bottom-8 left-1/2 -translate-x-1/2 text-xs text-muted opacity-0 group-hover:opacity-100 transition-opacity duration-200 whitespace-nowrap">
                Вперёд
            </span>
        </button>
    );

    return (
        <div className="relative py-4 px-2 my-2">
            {renderDecorationLines()}
            
            <div className="flex items-center justify-between gap-2 relative z-10">
                {renderPrevButton()}
                {renderLabel()}
                {renderNextButton()}
            </div>
        </div>
    );
};

export default YagoCardContentSelection;