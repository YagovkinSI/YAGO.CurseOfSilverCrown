import React, { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useGetMyUserQuery } from '../entities/MyUser';
import { useGetMyColonyQuery } from '../entities/MyColony';
import PageContainer from '../shared/PageContainer';
import NavButton from '../shared/NavButton';
import ErrorCard from '../shared/ErrorCard';
import LoadingCard from '../shared/LoadingCard';
import TurnButton from '../shared/TurnButton';
import { GameNavItemsList, SetNavItemData } from '../shared/NavItem';

const Game: React.FC = () => {
    const navigate = useNavigate();
    const getMyUserResult = useGetMyUserQuery();
    const getMyColonyResult = useGetMyColonyQuery();

    const user = getMyUserResult.data?.data;
    const colony = getMyColonyResult.data?.data;

    const isLoading = getMyUserResult.isLoading || getMyColonyResult.isLoading;
    const error = getMyUserResult.error ?? getMyColonyResult.error;

    useEffect(() => {
        if (!isLoading && !user) {
            navigate('/');
        }
    }, [isLoading, user, navigate]);

    const handleNavClick = (path: string) => {
        navigate(path);
    };

    const renderNavLine = (isLeft: boolean) => {
        const half = Math.ceil(GameNavItemsList.length / 2);
        const startIndex = isLeft ? 0 : half;
        const endIndex = isLeft ? half : GameNavItemsList.length;
        return <div className="flex flex-col gap-4 md:gap-6">
            {GameNavItemsList.slice(startIndex, endIndex).map((item) => {
                item = SetNavItemData(item, colony)
                return <NavButton
                    key={item.id}
                    icon={<item.icon className="w-5 h-5" />}
                    label={item.label}
                    onClick={() => handleNavClick(item.path)}
                    badge={item.badge}
                    isActive={item.isActive}
                />
            })}
        </div>
    };

    const renderContent = () => (
        <>
            <div className="flex pb-8 md:pb-10 items-end justify-between w-full w-full mx-auto px-3 md:px-6 flex-1">
                {/* Левая навигация */}
                <div className="flex flex-col gap-2">
                    {renderNavLine(true)}
                </div>

                {/* Центр (пустое место для модалок) */}
                <div className="flex-1">
                </div>

                {/* Правая навигация */}
                <div className="flex flex-col gap-2">
                    {renderNavLine(false)}
                </div>
            </div>

            {/* Кнопка хода внизу */}
            <div className="absolute bottom-8 left-1/2 -translate-x-1/2 md:bottom-10">
                <TurnButton />
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