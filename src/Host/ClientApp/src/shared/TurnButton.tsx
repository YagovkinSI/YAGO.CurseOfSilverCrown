import { Clock, Hourglass } from "lucide-react";
import { useEffect, useState } from "react";
import { useGetMyCycleQuery, useRunCycleMutation } from "../entities/MyCycle";

const TurnButton: React.FC = () => {
    const getMyCycleResult = useGetMyCycleQuery();
    
    const [turnTimer, setTurnTimer] = useState<number>(0);
    const [isTurnAvailable, setIsTurnAvailable] = useState<boolean>(false);
    const [runCycleMutation, runCycleResult] = useRunCycleMutation();

    const cycle = getMyCycleResult.data?.data;

    useEffect(() => {
        if (!cycle) return;

        updateTimer(cycle.startAtUtc);
        const interval = setInterval(() => updateTimer(cycle.startAtUtc), 1000);
        return () => clearInterval(interval);
    }, [cycle]);

    const handleTurn = async () => {
        await runCycleMutation().unwrap();
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

    const renderTurnButtonGlow = () => (
        <>
            <div className="absolute -inset-1 rounded-2xl bg-bright/20 blur-xl animate-pulse" />
            <div className="absolute -inset-1 rounded-2xl bg-bright/10 blur-2xl animate-pulse" style={{ animationDelay: '0.5s' }} />
        </>
    );

    const renderTurnButtonMainContent = () => (
        <div className="flex items-center gap-3">
            {isTurnAvailable
                ? (<Hourglass className="w-6 h-6 md:w-7 md:h-7 fill-current drop-shadow-[0_2px_4px_rgba(0,0,0,0.2)]" />)
                : (<Clock className="w-6 h-6 md:w-7 md:h-7" />)}
            <span
                className={`text-base md:text-lg font-bold uppercase tracking-wider
                        ${isTurnAvailable ? 'drop-shadow-[0_2px_4px_rgba(0,0,0,0.2)]' : ''}
                    `}
            >
                {isTurnAvailable
                    ? 'Вперёд'
                    : getMyCycleResult.isLoading ? 'Загрузка...' : formatTime(turnTimer)}
            </span>
        </div>
    )

    const renderTurnButtonAdditionContent = () => (
        <span
            className={`text-[0.55rem] md:text-xs font-medium uppercase tracking-widest w-full
                        ${isTurnAvailable ? 'text-dark/60' : 'text-muted/70'}
                    `}
        >
            {isTurnAvailable
                ? 'Следующий ход'
                : runCycleResult.isLoading ? 'Обработка хода' : 'до следующего хода'}
        </span>
    )

    return (
            <button
                onClick={handleTurn} disabled={!isTurnAvailable || runCycleResult.isLoading}
                className={`relative group flex items-center justify-center w-full w-full 
                    px-6 py-4 md:px-8 md:py-5 rounded-2xl transition-all duration-300
                    ${isTurnAvailable || runCycleResult.isLoading
                        ? `bg-gradient-to-br from-bright to-[#d4ca4a] text-dark shadow-[0_0_40px_rgba(240,230,92,0.2)]
                            hover:scale-105 hover:shadow-[0_0_60px_rgba(240,230,92,0.4)] active:scale-95 cursor-pointer`
                        : ` bg-[#1a1a2e] text-muted/50 border border-muted/20 cursor-not-allowed`
                    }
                `}
            >
                {isTurnAvailable && renderTurnButtonGlow()}
                <div className="relative z-10 flex flex-col items-center w-full gap-1">
                    {renderTurnButtonMainContent()}
                    {renderTurnButtonAdditionContent()}
                </div>
            </button>
        );
}

export default TurnButton;