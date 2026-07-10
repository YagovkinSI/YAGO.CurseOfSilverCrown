import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import {
    ArrowLeft,
    Search,
    Zap,
    AlertCircle,
    CheckCircle,
    Clock,
    Target,
} from 'lucide-react';
import Text from '../shared/Text';
import PageContainer from '../widgets/ContainerPage';
import { QuestType, type MyQuest } from '../entities/MyQuest';
import { useGetMyColonyQuery } from '../entities/MyColony';
import { formatTimeAgo } from '../features/TimeHelper';

const EventsPage: React.FC = () => {
    const navigate = useNavigate();

    const getMyColonyResult = useGetMyColonyQuery();

    const isLoading = getMyColonyResult.isLoading;
    const error = getMyColonyResult.error;

    const eventsFromServer = getMyColonyResult.data?.data?.quests;

    const [events, setEvents] = useState<MyQuest[]>([]);

    const typeColors = {
        news: {
            border: 'border-bright/10',
            bg: 'bg-bright/5',
            icon: 'text-bright',
            dot: 'bg-bright',
            label: '',
        },
        dilemma: {
            border: 'border-violet-500/30',
            bg: 'bg-violet-500/5',
            icon: 'text-violet-400',
            dot: 'bg-violet-400',
            label: 'text-violet-400 bg-violet-500/20',
        },
        quest: {
            border: 'border-blue-500/30',
            bg: 'bg-blue-500/5',
            icon: 'text-blue-400',
            dot: 'bg-blue-400',
            label: 'text-blue-400 bg-blue-500/20',
        },
    };

    const getTypeColors = (type: QuestType) => {
        switch (type) {
            case QuestType.Unknown:
            case QuestType.Default:
                return typeColors.quest;
            case QuestType.Ready:
            case QuestType.Immediately:
                return typeColors.dilemma;
            case QuestType.News:
            case QuestType.Mute:
            default:
                return typeColors.news;
        }
    }

    const getType = (type: QuestType) => {
        switch (type) {
            case QuestType.Unknown:
            case QuestType.Default:
                return 'quest';
            case QuestType.Ready:
            case QuestType.Immediately:
                return 'dilemma';
            case QuestType.News:
            case QuestType.Mute:
                return 'news';
        }
    }

    useEffect(() => {
        if (eventsFromServer && Array.isArray(eventsFromServer)) {
            setEvents(eventsFromServer);
        }
    }, [eventsFromServer]);

    const handleEventClick = (event: MyQuest) => {
        // Если не прочитано — отметить
        if (!event.isRead) {
            // TODO: API вызов
            setEvents(prev => prev.map(e =>
                e.id === event.id ? { ...e, isRead: true } : e
            ));
        }
        navigate(`/event/${event.id}`);
    };

    const renderHeader = () => (
        <div className="flex items-center justify-between w-full mb-4">
            <button
                onClick={() => navigate(-1)}
                className="p-2 text-muted hover:text-light transition-colors"
            >
                <ArrowLeft className="w-5 h-5" />
            </button>
            <h1 className="text-lg font-bold text-light">События</h1>
            <button
                className="p-2 text-muted hover:text-light transition-colors"
                disabled
            >
                <Search className="w-5 h-5 opacity-50" />
            </button>
        </div>
    );

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

    const renderEventCard = (event: MyQuest) => {
        const isRead = event.isRead ?? false;
        const isUrgent = event.type === QuestType.Immediately;
        const typeColor = getTypeColors(event.type);
        const stringType = getType(event.type);

        const renderIcon = () => (
            <div className="mt-0.5 flex-shrink-0">
                {stringType === 'dilemma' && <AlertCircle className={`w-5 h-5 ${typeColors.dilemma.icon}`} />}
                {stringType === 'news' && !isRead && <Zap className={`w-5 h-5 ${typeColors.news.icon}`} />}
                {stringType === 'news' && isRead && <CheckCircle className="w-5 h-5 text-muted" />}
                {stringType === 'quest' && <Target className={`w-5 h-5 ${typeColors.quest.icon}`} />}
            </div>
        )

        const renderTag = (name: string, isDanger: boolean) => (
            <span className={`
                text-[0.5rem] px-1.5 py-0.5 rounded-full uppercase font-bold 
                ${isDanger ? 'bg-danger/20 text-danger animate-pulse' : typeColors.dilemma.label}`}
            >
                {name}
            </span>
        )

        const renderContent = () => (
            <div className="flex-1 min-w-0">
                <div className="flex items-center gap-2 flex-wrap">
                    <span className={`text-sm font-medium truncate ${isRead ? 'text-muted' : 'text-light'}`}>
                        {event.title}
                    </span>
                    {stringType === 'dilemma' && renderTag('Дилемма', false)}
                    {isUrgent && renderTag('Важное', true)}
                </div>
                <div className="flex items-center gap-3 mt-0.5">
                    <span className="text-xs text-muted/50">{formatTimeAgo(event.createdAt)}</span>
                    {stringType === 'dilemma' && event.turnsLeft !== undefined && (
                        <span className="text-xs text-violet-400/70 flex items-center gap-1">
                            <Clock className="w-3 h-3" />
                            {event.turnsLeft} ходов
                        </span>
                    )}
                </div>
            </div>
        )

        return (
            <div className={`
                    flex items-start gap-3 p-3 rounded-lg cursor-pointer
                    transition-all duration-200
                    ${isRead
                    ? 'opacity-60 hover:opacity-80'
                    : `${typeColor.bg} border ${typeColor.border} hover:brightness-110`
                }
                    ${isUrgent ? 'border-danger/30' : ''}
                `}
                onClick={() => handleEventClick(event)}
            >
                {renderIcon()}
                {renderContent()}
                {!isRead && <div className={`w-2 h-2 mt-1.5 rounded-full flex-shrink-0 ${typeColor.dot}`} />}
            </div>
        );
    };

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
                {sortedEvents.map((event) => renderEventCard(event))}
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
            <div className="w-full max-w-2xl mx-auto px-4 py-4">
                {renderHeader()}
                {renderIllustration()}
                {renderEventsList()}
                {events.length > 10 && renderLoadMore()}
            </div>
    );

    return (
        <PageContainer backgroundImage='space' darkenBackground isLoading={isLoading} error={error} justifyContent='start'>
            {renderContent()}
        </PageContainer>
    );
};

export default EventsPage;