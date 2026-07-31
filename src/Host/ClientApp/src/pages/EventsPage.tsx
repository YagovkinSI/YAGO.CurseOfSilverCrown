import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    Search,
    Zap,
    ArrowLeft,
} from 'lucide-react';
import Text from '../shared/ui/Text';
import { QuestType, type MyQuest } from '../entities/events/MyQuest';
import { useGetMyColonyQuery } from '../entities/colonies/MyColony';
import PageHeader from '../features/PageHeader';
import Page from '../widgets/Page';
import { FlexContainer } from '../shared/ui/FlexContainer';
import EventCard from '../entities/events/EventCard';
import Surface from '../shared/ui/Surface';

const EventsPage: React.FC = () => {
    const navigate = useNavigate();

    const getMyColonyResult = useGetMyColonyQuery();

    const isLoading = getMyColonyResult.isLoading;
    const error = getMyColonyResult.error;

    const eventsFromServer = getMyColonyResult.data?.data?.quests;

    const [events, setEvents] = useState<MyQuest[]>([]);

    useEffect(() => {
        if (eventsFromServer && Array.isArray(eventsFromServer)) {
            setEvents(eventsFromServer);
        }
    }, [eventsFromServer]);

    useEffect(() => {
        if (!getMyColonyResult.isFetching && getMyColonyResult.isSuccess && eventsFromServer != undefined) {
            const autoRunQuest = eventsFromServer.find(x => x.type == QuestType.Autostart);
            if (autoRunQuest)
                navigate(`/me/events/${autoRunQuest.id}`);
        }
    }, [getMyColonyResult, eventsFromServer, navigate]);

    const renderIllustration = () => (
        <div className="relative rounded-xl overflow-hidden h-32 md:h-48 mb-4">
            <img
                src="/images/pictures/register_colony.jpg"
                className="w-full h-full object-cover"
                alt="События"
            />
            <div className="absolute inset-0 bg-gradient-to-t from-dark via-dark/50 to-transparent" />
            <div className="absolute bottom-4 left-4">
                <h2 className="text-lg font-bold text-light">События колонии</h2>
                <p className="text-sm text-muted">Все события и дилеммы</p>
            </div>
        </div>
    );

    const renderEventsList = () => {
        // Группировка по ходам
        const sortedEvents = [...events].sort((a, b) =>
            new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime()
        );

        if (events.length === 0) {
            return (
                <div className="flex flex-col items-center justify-center py-12">
                    <Zap className="w-12 h-12 text-muted/30" />
                    <Text variant="secondary" size="sm" className="mt-3">
                        Событий пока нет
                    </Text>
                </div>
            );
        }

        return (
            <div className="space-y-1">
                {sortedEvents.map((event) => <EventCard event={event} />)}
            </div>
        );
    };

    const renderLoadMore = () => {
        // TODO: пагинация
        return (
            <button className="w-full py-3 mt-4 text-sm text-muted hover:text-light transition-colors border border-bright/10 rounded-lg hover:bg-bright/5">
                Загрузить ещё
            </button>
        );
    };

    const renderContent = () => (
        <div className='h-full overflow-y-auto scrollbar-hide'>
            <FlexContainer justify='start'>
                <div className="w-full max-w-2xl mx-auto px-4 py-4">
                    <PageHeader
                        title={'События'}
                        leftButton={{ icon: ArrowLeft, onClick: () => navigate(-1), label: 'Назад' }}
                        rightButton={{ icon: Search, onClick: () => undefined, disabled: true }} />
                    {renderIllustration()}
                    <Surface rounded='md' variant='default' className='max-h-[60vh] w-full p-3 flex flex-col gap-2 overflow-y-auto'>
                        {renderEventsList()}
                    </Surface>

                    {events.length > 10 && renderLoadMore()}
                </div>
            </FlexContainer>
        </div>
    );

    return (
        <Page backgroundImage='captain_hall' darkenBackground isLoading={isLoading} error={error}>
            {renderContent()}
        </Page>
    );
};

export default EventsPage;