import React from 'react';
import { useNavigate } from 'react-router-dom';

interface GameParameterRowContainerProps {
    url?: string;
    shadowClassName?: string;
    children: React.ReactNode;
}

const GameParameterRowContainer: React.FC<GameParameterRowContainerProps> = ({
    url,
    children,
}) => {
    const navigate = useNavigate();

    const handleRowClick = () => {
        if (url) navigate(url);
    };

    return (
        <div
            className={`
                relative flex items-center gap-2 px-3 py-2 rounded-lg
                transition-all duration-200
                ${url ? 'cursor-pointer hover:bg-bright/5 hover:scale-[1.01]' : 'cursor-default'}
                bg-dark/40 border border-bright/5
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
            {children}
        </div>
    );
};

export default GameParameterRowContainer;