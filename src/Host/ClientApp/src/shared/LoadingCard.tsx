import { Loader2 } from 'lucide-react';
import ModalCard from './ModalCard';

export const LoadingCard: React.FC = () => {
    const renderIcon = () => (
        <Loader2 className="w-6 h-6 text-bright animate-spin" />
    );

    return (
        <ModalCard
            severity="info"
            title="Загрузка..."
            text="Пожалуйста, подождите..."
            icon={renderIcon()}
        />
    );
};

export default LoadingCard;