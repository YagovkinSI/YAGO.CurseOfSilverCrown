import React from 'react';
import { HelpCircle } from 'lucide-react';

interface GameParameterInfoButtonProps {
    onClick?: () => void;
}

const GameParameterInfoButton: React.FC<GameParameterInfoButtonProps> = ({ onClick }) => {
    const handleClick = (e: React.MouseEvent) => {
        e.stopPropagation();
        onClick?.();
    };

    return (
        <button
            onClick={handleClick}
            className="flex-shrink-0 p-1 rounded-md text-muted hover:text-bright hover:bg-bright/10 transition-colors"
            aria-label="Справка"
        >
            <HelpCircle className="w-4 h-4" />
        </button>
    );
};

export default GameParameterInfoButton;