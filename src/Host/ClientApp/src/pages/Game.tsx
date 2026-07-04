import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Building2, Zap, Settings, Clock, Rocket, Gavel, BarChart3 } from 'lucide-react';
import { useGetMyUserQuery } from '../entities/MyUser';
import { useGetMyColonyQuery } from '../entities/MyColony';
import PageContainer from '../shared/PageContainer';
import NavButton from '../shared/NavButton';
import { useGetMyCycleQuery, useRunCycleMutation } from '../entities/MyCycle';
import ErrorCard from '../shared/ErrorCard';
import LoadingCard from '../shared/LoadingCard';

type NavItemName = 'events' | 'construction' | 'reforms' | 'statistics' | 'settings';

interface NavItem {
    id: NavItemName,
    icon: React.ReactNode,
    label: string,
    path: string,
    bagde?: boolean,
    isActive?: boolean
}

const Game: React.FC = () => {
    const navItems: NavItem[] = [
        { id: 'events', icon: <Zap className="w-5 h-5" />, label: 'События', path: '/me/events' },
        { id: 'construction', icon: <Building2 className="w-5 h-5" />, label: 'Строительство', path: '/me/construction' },
        { id: 'reforms', icon: <Gavel className="w-5 h-5" />, label: 'Реформы', path: '/me/reforms' },
        { id: 'statistics', icon: <BarChart3 className="w-5 h-5" />, label: 'Статистика', path: '/me/statistics' },
        { id: 'settings', icon: <Settings className="w-5 h-5" />, label: 'Настройки', path: '/me/settings' },
    ];

    const navigate = useNavigate();
    const getMyUserResult = useGetMyUserQuery();
    const getMyColonyResult = useGetMyColonyQuery();
    const getMyCycleResult = useGetMyCycleQuery();

    const user = getMyUserResult.data?.data;
    const colony = getMyColonyResult.data?.data;
    const cycle = getMyCycleResult.data?.data;
    const [runCycleMutation, runCycleResult] = useRunCycleMutation();

    const isLoading = getMyUserResult.isLoading || getMyColonyResult.isLoading || getMyCycleResult.isLoading;
    const error = getMyUserResult.error ?? getMyColonyResult.error ?? getMyCycleResult.error ?? runCycleResult.error;

    const [turnTimer, setTurnTimer] = useState<number>(0);
    const [isTurnAvailable, setIsTurnAvailable] = useState<boolean>(false);
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

    useEffect(() => {
        if (!isLoading && !user) {
            navigate('/');
        }
    }, [isLoading, user, navigate]);

    useEffect(() => {
        if (!cycle) return;

        updateTimer(cycle.startAtUtc);
        const interval = setInterval(() => updateTimer(cycle.startAtUtc), 1000);
        return () => clearInterval(interval);
    }, [cycle]);

    const handleNavClick = (path: string) => {
        navigate(path);
    };

    const handleTurn = async () => {
        await runCycleMutation().unwrap();
    }

    const getNavItemBadge = (id: NavItemName): boolean => {
        switch (id) {
            case 'events':
                return (colony?.quests.length ?? 0) > 0;
            default:
                return false;
        }
    };

    // Активность кнопок (для disabled)
    const getNavItemIsActive = (id: NavItemName): boolean => {
        switch (id) {
            case 'construction':
            case 'settings':
                return false;
            default:
                return true;
        }
    };

    const formatTime = (milliseconds: number): string => {
        if (milliseconds <= 0) return '00:00';
        const seconds = Math.floor((milliseconds / 1000) % 60);
        const minutes = Math.floor((milliseconds / (1000 * 60)) % 60);
        return `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`;
    };

    const renderNavLine = (startIndex: number, endIndex: number) => (
        <div className="flex flex-col gap-4 md:gap-6">
            {navItems.slice(startIndex, endIndex).map((item) => (
                <NavButton
                    key={item.id}
                    icon={item.icon}
                    label={item.label}
                    onClick={() => handleNavClick(item.path)}
                    badge={getNavItemBadge(item.id)}
                    isActive={getNavItemIsActive(item.id)}
                />
            ))}
        </div>
    );

    const renderTurnButtonGlow = () => (
        <>
            <div className="absolute -inset-1 rounded-2xl bg-bright/20 blur-xl animate-pulse" />
            <div className="absolute -inset-1 rounded-2xl bg-bright/10 blur-2xl animate-pulse" style={{ animationDelay: '0.5s' }} />
        </>
    );

    const renderTurnButtonMainContent = () => (
        <div className="flex items-center gap-3">
            {isTurnAvailable
                ? (<Rocket className="w-6 h-6 md:w-7 md:h-7 fill-current drop-shadow-[0_2px_4px_rgba(0,0,0,0.2)]" />)
                : (<Clock className="w-6 h-6 md:w-7 md:h-7" />)}
            <span
                className={`text-base md:text-lg font-bold uppercase tracking-wider
                        ${isTurnAvailable ? 'drop-shadow-[0_2px_4px_rgba(0,0,0,0.2)]' : ''}
                    `}
            >
                {isTurnAvailable
                    ? 'Вперёд'
                    : runCycleResult.isLoading ? 'Загрузка...' : formatTime(turnTimer)}
            </span>
        </div>
    )

    const renderTurnButtonAdditionContent = () => (
        <span
            className={`text-[0.55rem] md:text-xs font-medium uppercase tracking-widest
                        ${isTurnAvailable ? 'text-dark/60' : 'text-muted/70'}
                    `}
        >
            {isTurnAvailable
                ? 'Следующий ход'
                : runCycleResult.isLoading ? 'Обработка хода' : 'до следующего хода'}
        </span>
    )

    const renderTurnButton = () => {
        return (
            <button
                onClick={handleTurn} disabled={!isTurnAvailable || runCycleResult.isLoading}
                className={`relative group flex items-center justify-center w-full max-w-xs 
                    mx-auto px-6 py-4 md:px-8 md:py-5 rounded-2xl transition-all duration-300
                    ${isTurnAvailable || runCycleResult.isLoading
                        ? `bg-gradient-to-br from-bright to-[#d4ca4a] text-dark shadow-[0_0_40px_rgba(240,230,92,0.2)]
                            hover:scale-105 hover:shadow-[0_0_60px_rgba(240,230,92,0.4)] active:scale-95 cursor-pointer`
                        : ` bg-[#1a1a2e] text-muted/50 border border-muted/20 cursor-not-allowed`
                    }
                `}
            >
                {isTurnAvailable && renderTurnButtonGlow()}
                <div className="relative z-10 flex flex-col items-center gap-1">
                    {renderTurnButtonMainContent()}
                    {renderTurnButtonAdditionContent()}
                </div>
            </button>
        );
    };

    const renderContent = () => (
        <>
            <div className="flex pb-8 md:pb-10 items-end justify-between w-full w-full mx-auto px-3 md:px-6 flex-1">
                {/* Левая навигация */}
                <div className="flex flex-col gap-2">
                    {renderNavLine(0, 3)}
                </div>

                {/* Центр (пустое место для модалок) */}
                <div className="flex-1">
                </div>

                {/* Правая навигация */}
                <div className="flex flex-col gap-2">
                    {renderNavLine(3, 6)}
                </div>
            </div>

            {/* Кнопка хода внизу */}
            <div className="absolute bottom-8 left-1/2 -translate-x-1/2 md:bottom-10">
                {renderTurnButton()}
            </div>
        </>
    )

    return (
        <PageContainer backgroundImage="captain_hall">
            {isLoading && <LoadingCard />}
            {!isLoading && error && <ErrorCard error={error!} />}
            {!isLoading && !error && renderContent()}
        </PageContainer>
    );
};

export default Game;