import { ChevronRight, HelpCircle } from "lucide-react";
import { useNavigate } from "react-router-dom";

export type RequirementParameterType = 'default';

export interface RequirementParameterProps {
    icon: React.ElementType;
    label: string;
    value: string;
    status: boolean;
    url?: string;
    infoUrl?: string;
}

const RequirementParameter: React.FC<RequirementParameterProps> = ({
    icon: Icon,
    label,
    value,
    status,
    url,
    infoUrl,
}) => {
    const navigate = useNavigate();
    
    const handleRowClick = () => {
        if (url) navigate(url);
    };

    const handleInfoClick = (e: React.MouseEvent) => {
        e.stopPropagation();
        if (infoUrl) navigate(infoUrl);
    };

    const color = status ? '#22c55e' : '#ef4444';
    return (
        <div
            className={`
                relative flex items-center gap-2 px-3 py-2 rounded-lg
                transition-all duration-200
                ${url ? 'cursor-pointer hover:bg-bright/5 hover:scale-[1.01]' : 'cursor-default'}
                bg-dark/40 border border-bright/5
                shadow-gray-500/10
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
            <div className="flex-shrink-0 w-7 h-7 flex items-center justify-center">
                {status ? '✅' : '❌'}
            </div>

            {/* Иконка */}
            <div className="flex-shrink-0 w-7 h-7 flex items-center justify-center">
                <Icon className="w-4 h-4 text-muted" />
            </div>

            {/* Название (обрезается если длинное) */}
            <span className='flex-1 min-w-0 text-sm truncate text-light/80'
            >
                <span>{label} </span>
                <span style={{ color }}>{value}</span>
            </span>

            {/* Кнопка "?" — справка */}
            {infoUrl && (
                <button
                    onClick={handleInfoClick}
                    className="flex-shrink-0 p-1 rounded-md text-muted hover:text-bright hover:bg-bright/10 transition-colors"
                    aria-label="Справка"
                >
                    <HelpCircle className="w-4 h-4" />
                </button>
            )}

            {/* Стрелка → если есть подменю */}
            {url && <ChevronRight className="flex-shrink-0 w-4 h-4 text-muted/50" />}
        </div>
    );
};

export default RequirementParameter;