import { useNavigate } from 'react-router-dom';
import { type MouseEvent } from 'react';
import { ArrowRight } from 'lucide-react';

export interface YagoStringLineProps {
    isTitleH1?: boolean;
    name: string;
    path?: string;
    isLinkToRazor?: boolean;
}

const YagoStringLine: React.FC<YagoStringLineProps> = ({ 
    name, 
    path, 
    isLinkToRazor, 
    isTitleH1 
}) => {
    const navigate = useNavigate();

    const handleClick = (e: MouseEvent<HTMLSpanElement>) => {
        if (!path) return;
        e.preventDefault();
        if (!isLinkToRazor) {
            navigate(path);
        } else {
            window.location.href = path;
        }
    };

    const renderLinkOrText = () => {
        if (path) {
            return (
                <span
                    onClick={handleClick}
                    className="text-bright hover:underline cursor-pointer transition-colors duration-200"
                >
                    {name}
                </span>
            );
        }
        return <span className="text-light">{name}</span>;
    };

    const renderArrowIcon = () => {
        if (!path) return null;
        return (
            <ArrowRight 
                className="w-4 h-4 text-bright/60 ml-2 flex-shrink-0" 
                strokeWidth={2}
            />
        );
    };

    const getTypographyStyles = () => {
        const baseStyles = 'flex items-center justify-center gap-1 mx-6';
        const titleStyles = isTitleH1 ? 'text-xl font-bold' : 'text-base font-normal';
        const cursorStyles = path ? 'cursor-pointer' : 'cursor-default';
        return `${baseStyles} ${titleStyles} ${cursorStyles}`;
    };

    const Component = isTitleH1 ? 'h1' : 'p';

    return (
        <Component className={getTypographyStyles()}>
            {renderLinkOrText()}
            {renderArrowIcon()}
        </Component>
    );
};

export default YagoStringLine;