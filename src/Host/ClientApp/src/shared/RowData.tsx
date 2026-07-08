import React from 'react';
import { ChevronRight } from 'lucide-react';
import { useNavigate } from 'react-router-dom';

export interface RowDataProps {
    color: string;
    icon: React.ElementType;
    label: string;
    value: string;
    url?: string;
}

const RowData: React.FC<RowDataProps> = (props) => {
    const navigate = useNavigate();
    const { color, icon: Icon, label, value, url } = props;

    const handleItemClick = () => {
        if (url) {
            navigate(url);
        }
    };

    const renderLeftLine = () => (
        <div 
            className="absolute left-0 top-1/2 -translate-y-1/2 w-1 h-12 rounded-r-full opacity-60"
            style={{ 
                background: `linear-gradient(to bottom, ${color}00, ${color}, ${color}00)` 
            }}
        />
    );

    const renderIcon = () => (
        <div className="flex-shrink-0 w-10 h-10 flex items-center justify-center">
            <Icon 
                className="w-6 h-6"
                style={{ color }}
            />
        </div>
    );

    const renderLabel = () => (
        <span className="text-light/80 text-sm font-medium">
            {label}
        </span>
    );

    const renderValue = () => (
        <div 
            className="px-3 py-1 rounded-full border"
            style={{
                backgroundColor: `${color}08`,
                borderColor: `${color}15`,
                color: color
            }}
        >
            <span className="text-sm font-medium">
                {value}
            </span>
        </div>
    );

    const renderArrow = () => {
        if (!url) return null;
        return (
            <ChevronRight 
                className="w-4 h-4 flex-shrink-0 ml-1 opacity-60"
                style={{ color }}
            />
        );
    };

    const renderContent = () => (
        <div className="flex items-center gap-3 flex-1 min-w-0">
            {renderIcon()}
            {renderLabel()}
        </div>
    );

    const renderValueWithArrow = () => (
        <div className="flex items-center gap-2 flex-shrink-0">
            {renderValue()}
            {renderArrow()}
        </div>
    );

    return (
        <div
            className={`
                relative flex items-center justify-between gap-4 px-4 py-3 rounded-lg cursor-pointer
                transition-all duration-200 hover:scale-[1.02] hover:shadow-lg
                bg-gradient-to-br from-dark/80 via-dark/60 to-dark/80 border border-muted/10
            `}
            onClick={handleItemClick}
            role={url ? "button" : "article"}
            tabIndex={url ? 0 : undefined}
            onKeyDown={(e) => {
                if (url && (e.key == 'Enter' || e.key == ' ')) {
                    e.preventDefault();
                    handleItemClick();
                }
            }}
            style={{
                boxShadow: `0 4px 15px ${color}15`,
                '--hover-shadow': `${color}25`,
            } as React.CSSProperties}
        >
            {renderLeftLine()}
            {renderContent()}
            {renderValueWithArrow()}
        </div>
    );
};

export default RowData;