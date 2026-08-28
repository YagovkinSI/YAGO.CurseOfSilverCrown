import type { ElementType } from 'react';
import { Info, Zap, Coins, Boxes, Smile, ScrollText, Users, Building2 } from 'lucide-react';
import type { StatisticCategory } from './statistics.types';

export const categoryIcons: Record<StatisticCategory, ElementType> = {
    Info: Info,
    ActionPoints: Zap,
    Solars: Coins,
    Modules: Boxes,
    Mood: Smile,
    Reforms: ScrollText,
    Population: Users,
    PrivateCapital: Building2,
};
