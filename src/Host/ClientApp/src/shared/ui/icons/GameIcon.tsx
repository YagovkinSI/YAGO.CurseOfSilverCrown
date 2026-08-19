import { Coins, Info, LayoutGrid, Smile, Zap } from 'lucide-react';
import React from 'react';

export type IconType = 
    'Default' | 'Solars' | 'ActionPoints' | 'Mood' | 'Modules';

interface GameIconProps {
    iconType: IconType,
    className?: string;
}

const GameIcon: React.FC<GameIconProps> = ({
    iconType,
    className = '',
}) => {

    const getIcon = (): React.ElementType => {
        switch (iconType) {
            case 'ActionPoints':
                return Zap;
            case 'Solars':
                return Coins;
            case 'Modules':
                return LayoutGrid;
            case 'Mood':
                return Smile;
            case 'Default':
            default:
                return Info;
        }
    }
    const Icon = getIcon();

    return (
        <Icon className={className} />
    );
}

export default GameIcon;