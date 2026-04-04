import React from "react";
import './yagoButton.css';

interface ButtonOnClickProps {
    onClick: (() => void) | undefined;
    children?: React.ReactNode;
    isDisabled?: boolean;
    type?: 'navigation' | 'mutation' | 'delete-warning' | 'delete-confirm' | 'secondary';
    fullWidth?: boolean;
    icon?: string | null;
    className?: string;
}

const YagoButton: React.FC<ButtonOnClickProps> = ({
    onClick,
    children,
    isDisabled = false,
    type = 'navigation',
    fullWidth = false,
    icon = null,
    className = ''
}) => {
    const getButtonClass = () => {
        const baseClass = 'game-button';
        const typeClass = `game-button--${type}`;
        const disabledClass = isDisabled ? 'game-button--disabled' : '';
        const fullWidthClass = fullWidth ? 'game-button--full-width' : '';
        return `${baseClass} ${typeClass} ${disabledClass} ${fullWidthClass} ${className}`;
    };

    return (
        <button
            className={getButtonClass()}
            onClick={onClick}
            disabled={isDisabled}
        >
            {/* Верхняя линия */}
            <div className="game-button__line game-button__line--top" />

            {/* Нижняя линия */}
            <div className="game-button__line game-button__line--bottom" />

            {/* Левая цветная полоска */}
            <div className="game-button__left-line" />

            {/* Правая цветная полоска */}
            <div className="game-button__right-line" />

            {/* Контент */}
            <div className="game-button__content">
                {icon && <span className="game-button__icon">{icon}</span>}
                <span className="game-button__text">{children}</span>
            </div>
        </button>
    );
};

export default YagoButton;