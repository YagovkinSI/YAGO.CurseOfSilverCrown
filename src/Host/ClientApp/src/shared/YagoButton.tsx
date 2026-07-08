import React from 'react';
import { ArrowRight, Trash2, Check, X, AlertTriangle } from 'lucide-react';

interface YagoButtonProps {
    onClick?: () => void;
    children?: React.ReactNode;
    isDisabled?: boolean;
    type?: 'navigation' | 'mutation' | 'delete-warning' | 'delete-confirm' | 'secondary';
    fullWidth?: boolean;
    icon?: React.ReactNode;
    className?: string;
}

const YagoButton: React.FC<YagoButtonProps> = ({
    onClick,
    children,
    isDisabled = false,
    type = 'navigation',
    fullWidth = false,
    icon = null,
    className = ''
}) => {

    const getButtonStyles = () => {
        const baseStyles = 'relative px-6 py-2.5 font-medium transition-all duration-200 rounded-md focus:outline-none focus:ring-2 focus:ring-bright/50 disabled:opacity-50 disabled:cursor-not-allowed overflow-hidden';
        const typeStyles = {
            navigation: 'bg-bright/10 text-bright border border-bright/30 hover:bg-bright/20 hover:border-bright/50',
            mutation: 'bg-good/10 text-good border border-good/30 hover:bg-good/20 hover:border-good/50',
            'delete-warning': 'bg-warning/10 text-warning border border-warning/30 hover:bg-warning/20 hover:border-warning/50',
            'delete-confirm': 'bg-danger/10 text-danger border border-danger/30 hover:bg-danger/20 hover:border-danger/50',
            secondary: 'bg-muted/10 text-muted border border-muted/30 hover:bg-muted/20 hover:border-muted/50'
        };
        const widthStyles = fullWidth ? 'w-full' : 'w-auto';
        const disabledStyles = isDisabled ? 'pointer-events-none' : '';
        return `${baseStyles} ${typeStyles[type]} ${widthStyles} ${disabledStyles} ${className}`;
    };

    const getDefaultIcon = () => {
        if (icon) return icon;
        switch (type) {
            case 'navigation':
                return <ArrowRight className="w-4 h-4" />;
            case 'mutation':
                return <Check className="w-4 h-4" />;
            case 'delete-warning':
                return <AlertTriangle className="w-4 h-4" />;
            case 'delete-confirm':
                return <Trash2 className="w-4 h-4" />;
            case 'secondary':
                return <X className="w-4 h-4" />;
            default:
                return null;
        }
    };

    const renderDecorationLines = () => (
        <>
            {/* Верхняя линия - декоративная */}
            <div className="absolute top-0 left-0 right-0 h-px bg-gradient-to-r from-transparent via-bright/30 to-transparent" />
            
            {/* Нижняя линия - декоративная */}
            <div className="absolute bottom-0 left-0 right-0 h-px bg-gradient-to-r from-transparent via-bright/30 to-transparent" />
            
            {/* Левая цветная полоска */}
            <div className="absolute left-0 top-1/2 -translate-y-1/2 w-0.5 h-6 bg-bright/40 rounded-full" />
            
            {/* Правая цветная полоска */}
            <div className="absolute right-0 top-1/2 -translate-y-1/2 w-0.5 h-6 bg-bright/40 rounded-full" />
        </>
    );

    const renderContent = () => (
        <div className="flex items-center justify-center gap-2 relative z-10">
            {getDefaultIcon()}
            <span>{children}</span>
        </div>
    );

    return (
        <button
            className={getButtonStyles()}
            onClick={onClick}
            disabled={isDisabled}
        >
            {renderDecorationLines()}
            {renderContent()}
        </button>
    );
};

export default YagoButton;