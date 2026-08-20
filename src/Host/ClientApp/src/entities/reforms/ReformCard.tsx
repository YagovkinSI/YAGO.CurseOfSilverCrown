import { useNavigate } from "react-router-dom";
import { ChevronRight, Lock, Unlock } from "lucide-react";
import type { ReformSummary } from "./reform.types";

interface ReformCardProps {
    reform: ReformSummary,
}

const ReformCard: React.FC<ReformCardProps> = ({ reform }) => {
    const navigate = useNavigate();

    const handleReformClick = () => navigate(`/me/reforms/${reform.code}`);

    return (
        <div
            className={`
                flex items-center gap-3 px-3 py-2.5 rounded-lg cursor-pointer
                transition-all duration-200
                ${reform.isAvailable
                    ? 'bg-bright/5 border border-bright/10 hover:bg-bright/10 hover:brightness-110'
                    : 'bg-dark/40 border border-bright/5 opacity-60 hover:opacity-80'
                }
            `}
            onClick={handleReformClick}
            role="button"
            tabIndex={0}
            onKeyDown={(e) => {
                if (e.key === 'Enter' || e.key === ' ') {
                    e.preventDefault();
                    handleReformClick();
                }
            }}
        >
            <div className="flex-shrink-0 w-7 h-7 flex items-center justify-center">
                {reform.isAvailable
                    ? <Unlock className="w-4 h-4 text-bright" />
                    : <Lock className="w-4 h-4 text-muted/40" />}
            </div>

            <span className="flex-1 min-w-0 text-sm font-medium truncate text-light/80">
                {reform.name}
            </span>

            <ChevronRight className="flex-shrink-0 w-4 h-4 text-muted/50" />
        </div>
    );
};

export default ReformCard;