import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Zap, Target, } from 'lucide-react';
import { useCreateColonyMutation, useGetMyColonyQuery } from '../entities/colonies/colony.api';
import TurnButton from '../features/TurnButton';
import { GameNavItemsList, SetNavItemData } from '../features/NavigationHelper';
import ButtonNavigation from '../shared/ui/buttons/ButtonNavigation';
import { type ColonyEventSummary } from '../entities/events/colonyEvent.types';
import WidgetCard from '../widgets/WidgetCard';
import Page from '../widgets/Page';
import { FlexContainer } from '../shared/ui/FlexContainer';

const ColonyPage: React.FC = () => {
    const navigate = useNavigate();
    
    const getMyColonyResult = useGetMyColonyQuery();
    const [createColony, createColonyResult] = useCreateColonyMutation();
    const createColonyUsed = React.useRef<boolean>(false);
    const [autostartEventChecked, setAutostartEventChecked] = useState(false);
    const [isDesktop, setIsDesktop] = useState(window.innerWidth >= 768);

    const colony = getMyColonyResult.data?.data;
    const allQuests = getMyColonyResult.data?.data?.quests ?? [];

    const isLoading = getMyColonyResult.data?.data == undefined || !autostartEventChecked;
    const error = getMyColonyResult.error ?? createColonyResult.error;

    useEffect(() => {
        if (getMyColonyResult.data?.data != undefined || getMyColonyResult.isFetching || createColonyUsed.current)
            return;
        createColonyUsed.current = true;
        createColony().unwrap();
    }, [getMyColonyResult, createColonyResult, createColony]);

    useEffect(() => {
        const handleResize = () => setIsDesktop(window.innerWidth >= 768);
        window.addEventListener('resize', handleResize);
        return () => window.removeEventListener('resize', handleResize);
    }, []);

    useEffect(() => {
        if (autostartEventChecked)
            return;
        if (!getMyColonyResult.isFetching && getMyColonyResult.isSuccess && colony != undefined) {
            const autostartEvent = colony.quests.find(x => x.type == 'Autostart');
            if (autostartEvent)
                navigate(`/me/events/${autostartEvent.id}`);
            else
                setAutostartEventChecked(true);
        }
    }, [autostartEventChecked, getMyColonyResult, colony, navigate]);

    const events = allQuests
        .filter((q: ColonyEventSummary) => q.type != 'Quest')
        .sort((a: ColonyEventSummary, b: ColonyEventSummary) => new Date(b.createdAtUtc).getTime() - new Date(a.createdAtUtc).getTime())
        .slice(0, 5);

    const quests = allQuests
        .filter((q: ColonyEventSummary) => q.type == 'Quest')
        .sort((a: ColonyEventSummary, b: ColonyEventSummary) => new Date(b.createdAtUtc).getTime() - new Date(a.createdAtUtc).getTime())
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