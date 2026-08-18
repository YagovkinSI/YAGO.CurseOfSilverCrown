import { useNavigate } from "react-router-dom";
import { type EventType, type ColonyEventSummary } from "./colonyEvent.types";
import { useSetReadMutation } from "./colonyEvent.api";
import { formatTimeAgo } from "../../features/TimeHelper";
import { AlertCircle, Clock, Target, Zap } from "lucide-react";
import { useState } from "react";

interface EventCardProps {
    event: ColonyEventSummary,
}

interface TypeColors {
    border: string,
    bg: string,
    icon: string,
    dot: string,
    label: string,
}

const EventCard: React.FC<EventCardProps> = ({ event }) => {
    const navigate = useNavigate();
    const isUrgent = event.type === 'Urgent';
    const [setRead] = useSetReadMutation();
    const [isRead, setIsRead] = useState(event.isRead ?? false);

    const handleEventClick = async () => {
        if (!event.isRead) {
            await setRead({colonyEventId: event.id}).unwrap();
            setIsRead(true);
        }
        navigate(`/me/events/${event.id}`);
    };

    const typeColors : Record<EventType, TypeColors> = {
        Default: {
            border: 'border-bright/10',
            bg: 'bg-bright/5',
            icon: 'text-bright',
            dot: 'bg-bright',
            label: '',
        },
        Urgent: {
            border: 'border-violet-500/30',
            bg: 'bg-violet-500/5',
            icon: 'text-violet-400',
            dot: 'bg-violet-400',
            label: 'text-violet-400 bg-violet-500/20',
        },
        Quest: {
            border: 'border-blue-500/30',
            bg: 'bg-blue-500/5',
            icon: 'text-blue-400',
            dot: 'bg-blue-400',
            label: 'text-blue-400 bg-blue-500/20',
        },
        Autostart: {
            border: 'border-bright/10',
            bg: 'bg-bright/5',
            icon: 'text-bright',
            dot: 'bg-bright',
            label: '',
        },
    };

    const typeColor = typeColors[event.type];

    const renderIcon = () => (
        <div className="mt-0.5 flex-shrink-0">
            {event.type === 'Urgent' && <AlertCircle className={`w-5 h-5 ${typeColor.icon}`} />}
            {event.type === 'Default' && <Zap className={`w-5 h-5 ${typeColor.icon}`} />}
            {event.type === 'Quest' && <Target className={`w-5 h-5 ${typeColor.icon}`} />}
        </div>
    )

    const renderTag = (name: string, isDanger: boolean) => (
        <span className={`
                text-[0.5rem] px-1.5 py-0.5 rounded-full uppercase font-bold 
                ${isDanger ? 'bg-danger/20 text-danger animate-pulse' : typeColor.label}`}
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
                {isUrgent && renderTag('Важное', true)}
            </div>
            <div className="flex items-center gap-3 mt-0.5">
                <span className="text-xs text-muted/50">{formatTimeAgo(event.createdAtUtc)}</span>
                {event.turnsLeft !== undefined && (
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