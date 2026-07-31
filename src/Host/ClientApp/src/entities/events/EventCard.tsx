import { useNavigate } from "react-router-dom";
import { QuestType, useSetReadMutation, type MyQuest } from "./MyQuest";
import { formatTimeAgo } from "../../features/TimeHelper";
import { AlertCircle, Clock, Target, Zap } from "lucide-react";
import { useState } from "react";

interface EventCardProps {
    event: MyQuest,
}

const EventCard: React.FC<EventCardProps> = ({ event }) => {
    const navigate = useNavigate();
    const isUrgent = event.type === QuestType.Immediately;
    const [setRead] = useSetReadMutation();
    const [isRead, setIsread] = useState(event.isRead ?? false);

    const handleEventClick = async () => {
        if (!event.isRead) {
            await setRead({eventId: event.id}).unwrap();
            setIsread(true);
        }
        navigate(`/me/events/${event.id}`);
    };

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
            case QuestType.Autostart:
                return typeColors.dilemma;
            case QuestType.News:
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
            case QuestType.Autostart:
                return 'dilemma';
            case QuestType.News:
                return 'news';
        }
    }

    const typeColor = getTypeColors(event.type);
    const stringType = getType(event.type);

    const renderIcon = () => (
        <div className="mt-0.5 flex-shrink-0">
            {stringType === 'dilemma' && <AlertCircle className={`w-5 h-5 ${typeColors.dilemma.icon}`} />}
            {stringType === 'news' && <Zap className={`w-5 h-5 ${typeColors.news.icon}`} />}
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
                <span className={`text-sm font-medium truncate text-light`}>
                    {event.title}
                </span>
                {stringType === 'dilemma' && renderTag('Дилемма', false)}
                {isUrgent && renderTag('Важное', true)}
            </div>
            <div className="flex items-center gap-3 mt-0.5">
                <span className="text-xs text-muted/50">{formatTimeAgo(event.createdAtUtc)}</span>
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
            max-w-2xl relative flex items-start gap-3 p-3 rounded-lg cursor-pointer
            transition-all duration-200
            ${typeColor.bg} border ${typeColor.border} hover:brightness-110
            ${isUrgent ? 'border-danger/30' : ''}
        `}
            onClick={() => handleEventClick()}
        >
            {renderIcon()}
            {renderContent()}
            {!isRead && <div className={`w-2 h-2 mt-1.5 rounded-full flex-shrink-0 ${typeColor.dot}`} />}
        </div>
    );
}

export default EventCard;