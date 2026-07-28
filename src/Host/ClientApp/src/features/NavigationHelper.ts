import {
    Zap,
    Building2,
    Gavel,
    BarChart3,
    Settings,
    Trophy,
    BookOpen,
    type LucideIcon,
    Home,
    MoreHorizontal,
    LogIn,
    LogOut,
} from 'lucide-react';
import type { MyColony } from '../entities/colonies/MyColony';

export type NavItemType =
    'home' | 'colony' | 'events' | 'construction' | 'reforms' | 'statistics' | 'settings' |
    'rating' | 'wiki' | 'more' | 'registration' | 'logout'

export interface NavItem {
    id: NavItemType;
    icon: LucideIcon;
    label: string;
    path: string;
    badge?: number;
    isActive?: boolean;
}

export const SetNavItemData = (item: NavItem, colony: MyColony | undefined) => {
    
    switch (item.id) {
        case 'events':
            item.badge = colony?.quests.length ?? 0;
    }

    item.isActive = true;
    switch (item.id) {
        case 'settings':
            item.isActive = false;
            break;
    }

    return item;
}
export const HomeNavItem: NavItem = { id: 'home', icon: Home, label: 'Главная', path: '/' }
export const GameNavItem: NavItem = { id: 'colony', icon: Home, label: 'Главная', path: '/me/colony' }
export const RatingNavItem: NavItem = { id: 'rating', icon: Trophy, label: 'Рейтинг', path: '/rating' }
export const WikiNavItem: NavItem = { id: 'wiki', icon: BookOpen, label: 'Wiki', path: '/wiki' }
export const MoreNavItem: NavItem = { id: 'more', icon: MoreHorizontal, label: 'Ещё', path: '/more' }

export const GameNavItemsList: NavItem[] = [
    { id: 'events', icon: Zap, label: 'События', path: '/me/events' },
    { id: 'construction', icon: Building2, label: 'Строительство', path: '/me/construction' },
    { id: 'reforms', icon: Gavel, label: 'Реформы', path: '/me/reforms' },
    { id: 'statistics', icon: BarChart3, label: 'Статистика', path: '/me/statistics' },
    { id: 'settings', icon: Settings, label: 'Настройки', path: '/me/settings' },
]

export const LogInNavItem: NavItem = { id: 'registration', icon: LogIn, label: 'Авторизация', path: '/registration' }

export const LogOutNavItem: NavItem = { id: 'logout', icon: LogOut, label: 'Выход', path: '/logout' }