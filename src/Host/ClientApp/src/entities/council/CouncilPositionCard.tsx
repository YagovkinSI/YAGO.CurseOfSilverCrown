import { BookOpen } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import type { CouncilPosition } from './council.types';
import { COUNCIL_AVATARS } from './council.avatars';

interface CouncilPositionCardProps {
    position: CouncilPosition;
}

const CouncilPositionCard: React.FC<CouncilPositionCardProps> = ({ position }) => {
    const navigate = useNavigate();
    const member = position.member;

    return (
        <div className="p-3 rounded-lg bg-bright/5 border border-bright/10 transition-all duration-200 hover:bg-bright/10">
            <div className="flex items-center gap-2">
                <div className="flex-shrink-0 w-10 h-10 rounded-lg overflow-hidden bg-bright/10">
                    <img
                        src={COUNCIL_AVATARS[position.code]}
                        alt={member.name}
                        className="w-full h-full object-cover"
                    />
                </div>
                <div className="flex flex-col min-w-0">
                    <span className="text-sm font-medium text-light">{position.title}</span>
                    <span className="text-xs text-muted/80">{member.name}</span>
                </div>
            </div>
            <p className="mt-1.5 text-xs text-muted/80 leading-relaxed">{position.description}</p>
            <button
                type="button"
                onClick={() => navigate(`/wiki/${member.wikiArticleCode}`)}
                className="mt-1 flex items-center gap-1 text-xs text-bright/80 hover:text-bright transition-colors"
            >
                <BookOpen className="w-3.5 h-3.5" />
                Статья Wiki
            </button>
        </div>
    );
};

export default CouncilPositionCard;