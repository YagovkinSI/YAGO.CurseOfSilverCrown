import { useNavigate } from "react-router-dom";
import { type ColonyEvent } from "../entities/events/ColonyEvent";
import Card from "../shared/ui/Card";
import EventCard from "../entities/events/EventCard";

interface WidgetCardProps {
    title: string,
    icon: React.ReactNode,
    items: ColonyEvent[],
    emptyText: string,
    colorClass: string,
}

const WidgetCard: React.FC<WidgetCardProps> = ({ title, icon, items, emptyText }) => {
    const navigate = useNavigate();

    return (
        <Card 
            size="md" variant="default"
            className="w-full max-w-sm shadow-lg items-stretch"
        >
            <div className="flex items-center gap-2 mb-3">
                {icon}
                <h3 className="text-sm font-bold text-light uppercase tracking-wider">{title}</h3>
                <span className="ml-auto text-xs text-muted/50">{items.length}</span>
            </div>
            
            <div className="space-y-2">
                {items.length === 0 ? (
                    <p className="text-xs text-light/50 text-center py-4">{emptyText}</p>
                ) : (
                    items.map((event) => <EventCard key={event.id} event={event} />)
                )}
            </div>
            
            {items.length > 0 && (
                <button
                    onClick={() => navigate('/me/events')}
                    className="w-full mt-3 text-xs text-muted/50 hover:text-bright transition-colors text-center"
                >
                    Все → {items.length}
                </button>
            )}
        </Card>
    );
};

export default WidgetCard;