import { useNavigate } from "react-router-dom";
import { ChevronRight } from "lucide-react";
import type { WikiSummary } from "./wiki.types";

interface WikiCardProps {
    summary: WikiSummary,
}

const WikiCard: React.FC<WikiCardProps> = ({ summary }) => {
    const navigate = useNavigate();

    const handleArticleClick = () => navigate(`/wiki/${summary.code}`);

    return (
        <div
            className="
                flex items-center gap-3 px-3 py-2.5 rounded-lg cursor-pointer
                bg-bright/5 border border-bright/10
                transition-all duration-200 hover:bg-bright/10 hover:brightness-110
            "
            onClick={handleArticleClick}
            role="button"
            tabIndex={0}
            onKeyDown={(e) => {
                if (e.key === 'Enter' || e.key === ' ') {
                    e.preventDefault();
                    handleArticleClick();
                }
            }}
        >
            <span className="flex-1 min-w-0 text-sm font-medium break-words line-clamp-2 text-light/80">
                {summary.name}
            </span>

            {!summary.isRead && (
                <span className="flex-shrink-0 w-2 h-2 rounded-full bg-green-400" />
            )}

            <ChevronRight className="flex-shrink-0 w-4 h-4 text-muted/50" />
        </div>
    );
};

export default WikiCard;
