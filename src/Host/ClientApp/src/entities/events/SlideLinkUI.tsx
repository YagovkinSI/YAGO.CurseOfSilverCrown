import React from 'react';
import { Trophy } from 'lucide-react';
import GameParameterRow from '../../shared/ui/gameParameterRow/GameParameterRow';
import type { SlideLink } from '../events/colonyEvent.types';

export interface SlideLinkProps {
    link: SlideLink;
}

const SlideLinkUI: React.FC<SlideLinkProps> = ({ link }) => {
    return (
        <GameParameterRow
            iconNode={<Trophy className="w-4 h-4 text-muted" />}
            label={link.label}
            value=""
            valueStatus="neutral"
            url={link.url}
        />
    );
};

export default SlideLinkUI;