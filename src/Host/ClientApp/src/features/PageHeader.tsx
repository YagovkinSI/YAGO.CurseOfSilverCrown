import React from 'react';
import type { LucideIcon } from 'lucide-react';

export interface PageHeaderButton {
    icon: LucideIcon;
    label?: string;
    onClick: () => void;
    disabled?: boolean;
    className?: string;
}

interface PageHeaderProps {
    title: string;
    leftButton?: PageHeaderButton;
    rightButton?: PageHeaderButton;
    className?: string;
}

const PageHeader: React.FC<PageHeaderProps> = ({ 
    title, 
    leftButton, 
    rightButton, 
    className = '' 
}) => {
    const renderButton = (button?: PageHeaderButton) => {
        if (!button) 
            return <div className="w-9 flex-shrink-0" />;
        const Icon = button.icon;
        return (
            <button
                onClick={button.onClick}
                disabled={button.disabled}
                className={`
                    p-2 text-muted hover:text-light transition-colors
                    ${button.disabled ? 'opacity-50 cursor-not-allowed' : ''}
                    ${button.className || ''}
                `}
                aria-label={button.label}
            >
                <Icon className="w-5 h-5" />
            </button>
        );
    };

    return (
        <div className={`flex items-center justify-between w-full mb-4 ${className}`}>
            {renderButton(leftButton)}
            <h1 className="text-lg font-bold text-light">
                {title}
            </h1>
            {renderButton(rightButton)}
        </div>
    );
};

export default PageHeader;