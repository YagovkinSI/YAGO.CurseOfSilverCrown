import { useNavigate } from 'react-router-dom';
import { X, ArrowLeft } from 'lucide-react';
import YagoStringLine from './YagoStringLine';

interface YagoCardProps {
    children?: React.ReactNode;
    title: string;
    path?: string;
    isLinkToRazor?: boolean;
    image?: string;
    headerButtonsAccess?: boolean;
}

const YagoCard: React.FC<YagoCardProps> = ({ 
    children, 
    title, 
    path, 
    isLinkToRazor, 
    image, 
    headerButtonsAccess = true 
}) => {
    const navigate = useNavigate();

    const renderBackButton = () => (
        <button
            onClick={() => navigate(-1)}
            className="absolute left-2 sm:left-3 p-1.5 rounded-full hover:bg-bright/10 transition-colors text-light/70 hover:text-bright"
            aria-label="Назад"
        >
            <ArrowLeft className="w-5 h-5" />
        </button>
    );

    const renderTitle = () => (
        <YagoStringLine
            name={title}
            path={path}
            isLinkToRazor={isLinkToRazor}
            isTitleH1={true}
        />
    );

    const renderCloseButton = () => (
        <button
            onClick={() => navigate('/')}
            className="absolute right-2 sm:right-3 p-1.5 rounded-full hover:bg-bright/10 transition-colors text-light/70 hover:text-bright"
            aria-label="Закрыть"
        >
            <X className="w-5 h-5" />
        </button>
    );

    const renderCardHeader = () => (
        <header className="flex items-center justify-between px-2 sm:px-4 pt-2 sm:pt-3 min-h-[32px] relative">
            {headerButtonsAccess && renderBackButton()}
            <div className="flex-1 flex justify-center">
                {renderTitle()}
            </div>
            {headerButtonsAccess && renderCloseButton()}
        </header>
    );

    const renderImage = () => {
        if (!image) return null;
        return (
            <div className="relative w-full pt-[56.25%]">
                <img
                    src={image}
                    alt="YAGO picture"
                    className="absolute top-0 left-0 w-full h-full object-cover"
                />
            </div>
        );
    };

    const renderCardContent = () => (
        <div className="p-2 sm:p-4 [&:last-child]:pb-2">
            {children}
        </div>
    );

    return (
        <div 
            className="bg-[#fafaf8]/90 rounded-lg shadow-[5px_5px_5px_rgba(0,0,0,0.5)] max-w-[80vh] mx-auto flex flex-col border border-bright/10"
            style={{ backgroundColor: 'rgba(250, 250, 248, 0.9)' }}
        >
            {renderCardHeader()}
            {renderImage()}
            {renderCardContent()}
        </div>
    );
};

export default YagoCard;