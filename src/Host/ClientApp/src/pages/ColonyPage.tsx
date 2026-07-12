import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Zap, Target, } from 'lucide-react';
import { useGetMyUserQuery } from '../entities/MyUser';
import { useGetMyColonyQuery } from '../entities/MyColony';
import TurnButton from '../features/TurnButton';
import { GameNavItemsList, SetNavItemData } from '../features/NavigationHelper';
import ButtonNavigation from '../shared/ButtonNavigation';
import { QuestType, type MyQuest } from '../entities/MyQuest';
import WidgetCard from '../widgets/WidgetCard';
import Page from '../widgets/Page';
import { FlexContainer } from '../shared/FlexContainer';

const ColonyPage: React.FC = () => {
    const navigate = useNavigate();
    const getMyUserResult = useGetMyUserQuery();
    const getMyColonyResult = useGetMyColonyQuery();
    const [isDesktop, setIsDesktop] = useState(window.innerWidth >= 768);

    const user = getMyUserResult.data?.data;
    const colony = getMyColonyResult.data?.data;
    const allQuests = getMyColonyResult.data?.data?.quests ?? [];

    const isLoading = getMyUserResult.isLoading || getMyColonyResult.isLoading;
    const error = getMyUserResult.error ?? getMyColonyResult.error;

    useEffect(() => {
        if (!getMyUserResult.isFetching && !isLoading && !user) {
            navigate('/');
        }
    }, [isLoading, user, navigate]);

    useEffect(() => {
        const handleResize = () => setIsDesktop(window.innerWidth >= 768);
        window.addEventListener('resize', handleResize);
        return () => window.removeEventListener('resize', handleResize);
    }, []);

    const events = allQuests
        .filter((q: MyQuest) => q.type !== QuestType.Default)
        .sort((a: MyQuest, b: MyQuest) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
        .slice(0, 5);

    const quests = allQuests
        .filter((q: MyQuest) => q.type === QuestType.Default)
        .sort((a: MyQuest, b: MyQuest) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
        .slice(0, 5);

    const handleNavClick = (path: string) => {
        navigate(path);
    };

    const renderNavLine = (isLeft: boolean) => {
        const half = Math.ceil(GameNavItemsList.length / 2);
        const startIndex = isLeft ? 0 : half;
        const endIndex = isLeft ? half : GameNavItemsList.length;
        return (
            <div className="flex flex-col gap-4 md:gap-6">
                {GameNavItemsList.slice(startIndex, endIndex).map((item) => {
                    const data = SetNavItemData(item, colony);
                    return (
                        <ButtonNavigation
                            key={data.id}
                            icon={<data.icon className="w-5 h-5" />}
                            label={data.label}
                            onClick={() => handleNavClick(data.path)}
                            badge={data.badge}
                            isActive={data.isActive}
                        />
                    );
                })}
            </div>
        );
    };

    const renderMobileNav = () => (
        <FlexContainer direction='row' items='end' justify='between' className="pb-8 px-3 md:px-6 gap-6">
            <div className="flex flex-col gap-6">{renderNavLine(true)}</div>
            <TurnButton />
            <div className="flex flex-col gap-6">{renderNavLine(false)}</div>
        </FlexContainer>
    );

    const renderDesktopContent = () => {
        return (
            <FlexContainer direction='row' items='end' justify='between' className="pb-10 max-w-7xl mx-auto px-6 gap-6 pt-4">
                {/* Левый виджет: События */}
                <WidgetCard
                    title={'События'}
                    icon={<Zap className="w-4 h-4 text-bright" />}
                    items={events}
                    emptyText={'Нет событий'}
                    colorClass={'bg-bright/5 border-bright/10'}/>

                {/* Центр (пусто) */}
                <div className="flex-1" />

                {/* Правый виджет: Квесты */}
                <WidgetCard
                    title={'Квесты'}
                    icon={<Target className="w-4 h-4 text-blue-400" />}
                    items={quests}
                    emptyText={'Нет активных квестов'}
                    colorClass={'bg-blue-500/5 border-blue-500/20'}/>
            </FlexContainer>
        );
    };

    const renderContent = () => (
        <FlexContainer justify='end' >
            {isDesktop ? renderDesktopContent() : renderMobileNav()}
        </FlexContainer>
    )

    return (
        <Page backgroundImage='captain_hall' isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default ColonyPage;