import { Clock, Hourglass, Zap } from "lucide-react";
import { useEffect, useState } from "react";
import { useGetMyCycleQuery } from "../entities/cycles/MyCycle";
import { useGetMyColonyQuery } from "../entities/colonies/MyColony";
import { useNavigate } from "react-router-dom";

const TurnButton: React.FC = () => {
    const navigate = useNavigate();
    const getMyCycleResult = useGetMyCycleQuery();

    const [turnTimer, setTurnTimer] = useState<number>(0);
    const [isTurnAvailable, setIsTurnAvailable] = useState<boolean>(false);
    const getMyColonyResult = useGetMyColonyQuery();

    const isLoading = getMyCycleResult.isLoading;

    const urgentEvents = getMyColonyResult.data?.data?.quests
        ?.find(q => q.type === 'Urgent');
    const cycle = getMyCycleResult.data?.data;

    useEffect(() => {
        if (!cycle) return;

        updateTimer(cycle.startAtUtc);
        const interval = setInterval(() => updateTimer(cycle.startAtUtc), 1000);
        return () => clearInterval(interval);
    }, [cycle]);

    const handleTurn = async () => {
        if (urgentEvents) {
            navigate(`/me/events/${urgentEvents.id}`);
            return;
        }
        navigate(`/me/turnResult`);
    }

    const updateTimer = (turnStartAtUtc: string) => {
        const startAt = Date.parse(turnStartAtUtc);
        const now = Date.now();
        const isReady = startAt < now;
        const difference = startAt - now;
        if (isReady || difference <= 0) {
            setIsTurnAvailable(true);
            setTurnTimer(0);
        } else {
            setIsTurnAvailable(false);
            setTurnTimer(difference);
        }
    };

    const formatTime = (milliseconds: number): string => {
        if (milliseconds <= 0) return '00:00';
        const seconds = Math.floor((milliseconds / 1000) % 60);
        const minutes = Math.floor((milliseconds / (1000 * 60)) % 60);
        return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
    };

    // --- ЛОГИКА СТИЛЕЙ ---
    const isActive = isTurnAvailable && !isLoading;
    const isCrisis = isActive && urgentEvents;

    // Основные стили кнопки в зависимости от состояния
    const getButtonStyles = () => {
        if (!isActive) {
            return 'bg-[#1a1a2e] text-muted border border-muted/20 cursor-not-allowed';
        }
        if (isCrisis) {
            return `bg-gradient-to-br from-violet-600 to-purple-700 text-white 
                    shadow-[0_0_40px_rgba(124,58,237,0.25)]
                    hover:scale-105 hover:shadow-[0_0_60px_rgba(124,58,237,0.4)] active:scale-95 cursor-pointer`;
        }
        // Обычный ход (желтый)
        return `bg-gradient-to-br from-bright to-[#d4ca4a] text-dark 
                shadow-[0_0_40px_rgba(240,230,92,0.2)]
                hover:scale-105 hover:shadow-[0_0_60px_rgba(240,230,92,0.4)] active:scale-95 cursor-pointer`;
    };

    // Текст для подсказки снизу
    const getSubtext = () => {
        if (!isActive) return isLoading ? 'Обработка хода' : formatTime(turnTimer);
        if (isCrisis) return 'Важное событие';
        return 'Следующий ход';
    };

    const renderTurnButtonGlow = () => {
        if (!isActive) return null;

        const glowColor = isCrisis 
            ? 'bg-violet-500/20 blur-xl' 
            : 'bg-bright/20 blur-xl';

        return (
            <>
                <div className={`absolute -inset-1 rounded-2xl ${glowColor} animate-pulse`} />
                <div className={`absolute -inset-1 rounded-2xl ${glowColor.replace('/20', '/10')} blur-2xl animate-pulse`} style={{ animationDelay: '0.5s' }} />
            </>
        );
    };

    return (
        <button
            onClick={handleTurn} disabled={!isActive}
            className={`
                relative group
                w-full flex flex-col items-center justify-center gap-1
                px-6 py-4 md:px-8 md:py-5 rounded-2xl
                transition-all duration-300
                ${getButtonStyles()}
            `}
        >
            {isActive && renderTurnButtonGlow()}
            
            {/* Основной контент (иконка + текст) */}
            <div className="relative z-10 flex items-center gap-3">
                {isActive
                    ? isCrisis
                        ? (<Zap className="w-6 h-6 md:w-7 md:h-7 fill-current drop-shadow-[0_2px_4px_rgba(0,0,0,0.2)]" />)
                        : (<Hourglass className="w-6 h-6 md:w-7 md:h-7 fill-current drop-shadow-[0_2px_4px_rgba(0,0,0,0.2)]" />)
                    : (<Clock className="w-6 h-6 md:w-7 md:h-7" />)
                }
                <span className="text-base md:text-lg font-bold uppercase tracking-wider drop-shadow-[0_2px_4px_rgba(0,0,0,0.2)]">
                    {isActive ? 'Вперёд' : isLoading ? 'Загрузка...' : 'Ожидание'}
                </span>
            </div>

            {/* Дополнительный текст снизу */}
            <span className={
                `relative z-10 text-[0.55rem] md:text-xs font-medium uppercase tracking-widest w-full 
                ${!isActive ? 'text-muted/70' : isCrisis ? 'text-white/85' : 'text-dark/85'}`}
            >
                {getSubtext()}
            </span>
        </button>
    );
}

export default TurnButton;