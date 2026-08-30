import React from 'react';
import { useNavigate } from 'react-router-dom';
import { HelpCircle } from 'lucide-react';

interface GameParameterInfoButtonProps {
    infoUrl?: string;
    onClick?: () => void;
}

const GameParameterInfoButton: React.FC<GameParameterInfoButtonProps> = ({ infoUrl, onClick }) => {
    const navigate = useNavigate();

    const handleClick = (e: React.MouseEvent) => {
        e.stopPropagation();
        if (onClick) {
            onClick();
        } else if (infoUrl) {
            navigate(infoUrl);
        }
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