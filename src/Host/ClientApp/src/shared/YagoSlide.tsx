import { useNavigate } from 'react-router-dom';
import { X, ArrowLeft } from 'lucide-react';
import YagoCard from './YagoCard';
import YagoTitle from './YagoTitle';

interface YagoCardProps {
    children?: React.ReactNode;
    title: string;
    path?: string;
    isLinkToRazor?: boolean;
    image?: string;
    headerButtonsAccess?: boolean;
}

const YagoSlide: React.FC<YagoCardProps> = ({ 
    children, 
    title, 
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
                <YagoTitle>
                    {title}
                </YagoTitle>
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
        <YagoCard>
            {renderCardHeader()}
            {renderImage()}
            {renderCardContent()}
        </YagoCard>
    );
};

export default YagoSlide;