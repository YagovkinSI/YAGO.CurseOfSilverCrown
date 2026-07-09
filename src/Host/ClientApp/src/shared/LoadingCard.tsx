import YagoCard from './YagoCard';

export const LoadingCard: React.FC = () => {
    return (
        <YagoCard variant="default" className="flex flex-col items-center gap-4">
            <div className="w-12 h-12 border-4 border-bright/20 border-t-bright rounded-full animate-spin" />
            <p className="text-muted text-sm">Загрузка...</p>
        </YagoCard>
    );
};

export default LoadingCard