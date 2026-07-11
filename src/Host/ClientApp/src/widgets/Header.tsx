//import vk_logo from '../assets/images/links/vk_logo.svg'
import React from 'react';
import { useGetMyUserQuery } from '../entities/MyUser';
import { useGetMyColonyQuery } from '../entities/MyColony';
import { GetStateItems } from '../features/GetColonyParameterList';
import LoginIconMenu from '../features/LoginIconMenu';
import type { ColonyParameterRowProps } from '../shared/ColonyParameterRow';
import { AlertCircle } from 'lucide-react';

export interface HeaderStat {
    id: string;
    icon: React.ReactNode;
    value: string;
    iconColor?: string;
    valueColor?: string;
}

// Компонент спиннера
const Spinner = () => (
    <div className="inline-block w-5 h-5 border-2 border-bright/20 border-t-bright rounded-full animate-spin mr-1" />
);

export interface HeaderProps {
    className?: string;
}

const Header: React.FC<HeaderProps> = ({className}) => {
    const getMyUserResult = useGetMyUserQuery();
    const getMyColonyResult = useGetMyColonyQuery();

    const user = getMyUserResult.data?.data;
    const isAuthenticated = user != undefined;
    const colony = getMyColonyResult.data?.data;
    const colonyName = colony?.name ?? "Мир YAGO";
    const colonyParameters = colony?.colonyParameters?.filter(x => x.parrentType == undefined) ?? [];
    const stats = GetStateItems(colonyParameters);

    const isLoading = getMyUserResult.isLoading || getMyColonyResult.isLoading;
    const error = getMyUserResult.error ?? getMyColonyResult.error;

    const renderLeftPart = () => (
        <div className="flex items-center gap-2 min-w-0">
            <LoginIconMenu />
            <span className="
                text-light font-semibold tracking-wide whitespace-nowrap overflow-hidden text-ellipsis max-w-[100px]
                text-sm md:text-base md:max-w-[300px]
                max-[480px]:text-[0.85rem] max-[480px]:max-w-[100px]
            ">
                {isAuthenticated ? colonyName : 'Мир YAGO'}
            </span>
        </div>
    );

    const renderRightPart = () => (
        <div className="flex items-center gap-1 flex-shrink-0">
            {isLoading && <Spinner />}
            {error && (
                <div title="Ошибка загрузки данных">
                    <AlertCircle className="text-danger w-5 h-5 mr-1" />
                </div>
            )}
        </div>
    );

    const renderStat = (stat: ColonyParameterRowProps) => {
        return <div
            key={stat.label}
            className="flex items-center gap-1 flex-shrink-0 px-1.5 border-r border-bright/15 last:border-r-0 max-[480px]:px-1 md:gap-1.5 md:px-2 lg:px-3"
        >
            <span
                className="flex items-center text-[0.8rem] leading-none max-[480px]:text-[0.65rem] md:text-[0.9rem]"
                style={{ color: 'var(--color-muted)' }}
            >
                <span className="state-item-icon-container">
                    <stat.icon
                        className="state-item-icon"
                    />
                </span>
            </span>
            <span className="text-[0.75rem] font-semibold tracking-wide leading-tight text-light max-[480px]:text-[0.6rem] md:text-[0.85rem]">
                {stat.value}
            </span>
        </div>
    }

    const renderStats = () => (
        <div className="h-7 px-2 overflow-hidden flex items-center md:h-8 md:px-3 max-[480px]:h-6 max-[480px]:px-1">
            <div className="
                flex items-center gap-2 overflow-x-auto overflow-y-hidden px-1 w-full whitespace-nowrap
                scrollbar-thin scrollbar-thumb-bright scrollbar-track-transparent
                [&::-webkit-scrollbar]:h-0.5
                [&::-webkit-scrollbar-track]:bg-transparent
                [&::-webkit-scrollbar-thumb]:bg-bright [&::-webkit-scrollbar-thumb]:rounded-full
                max-[768px]:[&::-webkit-scrollbar]:h-0
                md:gap-3 md:px-2
                lg:gap-4
                max-[480px]:gap-1 max-[480px]:px-0.5
            ">
                {stats.map((stat) => renderStat(stat))}
            </div>
        </div>
    );

    return (
        <header className={
            `bg-dark border-b-2 border-bright shadow-[0_4px_10px_rgba(0,0,0,0.5)] 
            ${className}`}
        >
            <div className="flex items-center justify-between h-10 px-3 md:h-12 md:px-4">
                {renderLeftPart()}
                {renderRightPart()}
            </div>
            <div className="h-px bg-gradient-to-r from-transparent via-bright to-transparent opacity-50" />
            {isAuthenticated && stats.length > 0 && renderStats()}
        </header>
    );
};

export default Header;