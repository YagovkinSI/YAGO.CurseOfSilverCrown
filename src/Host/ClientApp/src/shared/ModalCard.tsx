import { useEffect, useState } from 'react';
import { AlertCircle, Info, X } from 'lucide-react';

export interface ModalCardProps {
    severity: "info" | "error";
    title: string;
    text: string;
    icon?: React.ReactNode;
    backgroundColor?: string;
}

const ModalCard: React.FC<ModalCardProps> = ({ 
    severity, 
    title, 
    text, 
    icon, 
    backgroundColor = '#fafaf8' 
}) => {
    const [isVisible, setIsVisible] = useState(false);

    useEffect(() => {
        setIsVisible(true);
    }, []);

    const getSeverityStyles = () => {
        const baseStyles = 'fixed bottom-10 left-10 z-50 max-w-md p-4 rounded-lg shadow-2xl border transition-all duration-500';
        const severityStyles = {
            info: 'bg-light/95 border-info/30 text-light',
            error: 'bg-danger/10 border-danger/30 text-danger'
        };
        const visibilityStyles = isVisible 
            ? 'opacity-100 translate-y-0' 
            : 'opacity-0 translate-y-4';
        return `${baseStyles} ${severityStyles[severity]} ${visibilityStyles}`;
    };

    const getDefaultIcon = () => {
        if (icon) return icon;
        return severity == 'info' 
            ? <Info className="w-5 h-5 text-info flex-shrink-0" />
            : <AlertCircle className="w-5 h-5 text-danger flex-shrink-0" />;
    };

    const renderHeader = () => (
        <div className="flex items-start gap-3">
            <div className="mt-0.5">
                {getDefaultIcon()}
            </div>
            <div className="flex-1 min-w-0">
                <h4 className="font-semibold text-base mb-1">
                    {title}
                </h4>
                <p className="text-sm opacity-90">
                    {text}
                </p>
            </div>
            <button
                onClick={() => setIsVisible(false)}
                className="flex-shrink-0 -mt-1 -mr-1 p-1 rounded-full hover:bg-black/5 transition-colors"
                aria-label="Закрыть"
            >
                <X className="w-4 h-4 opacity-60 hover:opacity-100" />
            </button>
        </div>
    );

    if (!isVisible) 
        return null;

    return (
        <div 
            className={getSeverityStyles()}
            style={{ backgroundColor }}
            role="alert"
        >
            {renderHeader()}
        </div>
    );
};

export default ModalCard;