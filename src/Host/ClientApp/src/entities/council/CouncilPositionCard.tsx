import { BookOpen, Coins, UserCog, Users, Wrench, type LucideIcon } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import type { CouncilPosition, CouncilPositionCode } from './council.types';

interface CouncilPositionCardProps {
    position: CouncilPosition;
}

const POSITION_ICONS: Record<CouncilPositionCode, LucideIcon> = {
    administrator: UserCog,
    engineer: Wrench,
    financier: Coins,
    social: Users,
};

const CouncilPositionCard: React.FC<CouncilPositionCardProps> = ({ position }) => {
    const navigate = useNavigate();
    const Icon = POSITION_ICONS[position.code];
    const member = position.member;

    const renderAvatar = () => (
        <div className="flex-shrink-0 w-20 h-10 rounded-lg overflow-hidden bg-bright/10">
            <div
                className="w-full h-full bg-cover bg-center"
                style={{ backgroundImage: `url('/images/pictures/${member.avatar}.jpg')` }}
            />
        </div>
    );

    const renderLoyalty = () => (
        <div className="flex items-center gap-2">
            <span className="text-xs text-muted">Лояльность</span>
            <div className="flex-1 h-1.5 bg-bright/10 rounded-full overflow-hidden">
                <div className="h-full rounded-full bg-emerald-500" style={{ width: `${member.loyalty}%` }} />
            </div>
            <span className="text-xs font-medium text-light">{member.loyalty}</span>
        </div>
    );

    const renderMemberInfo = () => (
        <div className="mt-3 pt-3 border-t border-bright/10 flex items-start gap-3">
            {renderAvatar()}
            <div className="flex-1 min-w-0 flex flex-col gap-1.5">
                <span className="text-sm font-medium text-light">{member.name}</span>
                {renderLoyalty()}
                <button
                    type="button"
                    onClick={() => navigate(`/wiki/${member.wikiArticleCode}`)}
                    className="flex items-center gap-1 text-xs text-bright/80 hover:text-bright transition-colors"
                >
                    <BookOpen className="w-3.5 h-3.5" />
                    Статья Wiki
                </button>
            </div>
        </div>
    );

    return (
        <div className="p-3 rounded-lg bg-bright/5 border border-bright/10 transition-all duration-200 hover:bg-bright/10">
            <div className="flex items-center gap-2">
                <Icon className="w-5 h-5 text-bright/80" />
                <span className="text-sm font-medium text-light">{position.title}</span>
            </div>
            <p className="mt-1 text-xs text-muted/80 leading-relaxed">{position.description}</p>
            {renderMemberInfo()}
        </div>
    );
};

export default CouncilPositionCard;