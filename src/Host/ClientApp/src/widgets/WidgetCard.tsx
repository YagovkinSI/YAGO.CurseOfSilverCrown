import { useNavigate } from "react-router-dom";
import { QuestType, type MyQuest } from "../entities/MyQuest";
import { formatTimeAgo } from "../features/TimeHelper";

interface WidgetCardProps {
    title: string,
    icon: React.ReactNode,
    items: MyQuest[],
    emptyText: string,
    colorClass: string,
    onItemClick: (id: string) => void
}

const WidgetCard: React.FC<WidgetCardProps> = ({ title, icon, items, emptyText, colorClass, onItemClick }) => {
    const navigate = useNavigate();

    const renderItemContent = (item: MyQuest) => (
        <div className="flex-1 min-w-0">
            <div className="flex items-center gap-1.5">
                <span className={`text-xs font-medium truncate ${item.isRead ? 'text-muted' : 'text-light'}`}>
                    {item.title}
                </span>
                {item.type == QuestType.Immediately && (
                    <span className="text-[0.45rem] px-1 py-0.5 bg-danger/20 text-danger rounded-full uppercase font-bold flex-shrink-0">
                        Важное
                    </span>
                )}
            </div>
            <span className="text-[0.55rem] text-muted/50">
                {formatTimeAgo(item.createdAt)}
            </span>
        </div>
    )

    const renderItem = (item: MyQuest) => (
        <div
            key={item.id}
            onClick={() => onItemClick(item.id)}
            className={`
                flex items-center gap-2 p-2 rounded-lg cursor-pointer
                transition-all duration-200
                ${item.isRead
                    ? 'opacity-60 hover:opacity-80'
                    : `${colorClass} border ${colorClass.replace('text', 'border')}/20 hover:brightness-110`
                }
            `}
        >
            {renderItemContent(item)}
            {!item.isRead && <div className="w-1.5 h-1.5 rounded-full bg-bright flex-shrink-0" />}
        </div>
    )

    return <div className="bg-dark/50 backdrop-blur-sm border border-bright/10 rounded-xl p-4 w-full max-w-sm">
        <div className="flex items-center gap-2 mb-3">
            {icon}
            <h3 className="text-sm font-bold text-light uppercase tracking-wider">{title}</h3>
            <span className="ml-auto text-xs text-muted/50">{items.length}</span>
        </div>
        <div className="space-y-2">
            {items.length === 0 ? (
                <p className="text-xs text-muted/50 text-center py-4">{emptyText}</p>
            ) : (
                items.map((item) => renderItem(item))
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
    </div>
};

export default WidgetCard;