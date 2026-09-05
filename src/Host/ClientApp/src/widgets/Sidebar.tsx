import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { useGetUserPrivateQuery, useLogoutMutation } from "../entities/users/user.api";
import TurnButton from '../features/TurnButton';
import { GameNavItemsList, LogOutNavItem, type NavItem, HomeNavItem, RatingNavItem, SetNavItemData, GameNavItem } from '../features/NavigationHelper';
import { useGetMyColonyQuery } from '../entities/colonies/colony.api';

export interface SidebarProps {
    isOpen?: boolean;
    onClose?: () => void;
    className?: string;
}

const Sidebar: React.FC<SidebarProps> = ({ isOpen, onClose, className }) => {
    const navigate = useNavigate();
    const location = useLocation();

    const getUserPrivateResult = useGetUserPrivateQuery();
    const getMyColonyResult = useGetMyColonyQuery();
    const [logout] = useLogoutMutation();

    const isDrawer = isOpen !== undefined;
    const user = getUserPrivateResult.data?.data;
    const colony = getMyColonyResult.data?.data;

    const isActive = (path: string) => location.pathname === path || location.pathname.startsWith(path + '/');
    const isEventPage = () => location.pathname.startsWith('/me/events/');

    const handleNavigate = (path: string) => {
        navigate(path);
        onClose?.();
    };

    const handleLogout = async () => {
        await logout().unwrap();
        onClose?.();
        navigate('/');
    };

    const renderMainNavItem = (item: NavItem) => {
        item = SetNavItemData(item, colony)
        return <button
            key={item.id}
            disabled={!item.isActive}
            onClick={() => item.id == 'logout' ? handleLogout() : handleNavigate(item.path)}
            className={`
                w-full flex items-center gap-3 px-3 py-2.5 rounded-lg
                transition-all duration-200
                ${!item.isActive
                    ? 'opacity-40 cursor-not-allowed text-muted/50 hover:bg-transparent'
                    : isActive(item.path)
                        ? 'bg-bright/10 text-bright hover:bg-bright/15'
                        : 'text-muted hover:text-light hover:bg-bright/5'
                }
            `}
        >
            {<item.icon className="w-5 h-5" />}
            <span className="text-sm font-medium">{item.label}</span>
            {!!item.badge && item.badge > 0 && item.isActive && (
                <span className={`ml-auto w-2 h-2 ${item.badgeColor === 'success' ? 'bg-emerald-500' : 'bg-danger'} rounded-full animate-pulse`} />
            )}
        </button>
    }

    const renderDivider = () => (
        <div className="my-4 border-t border-bright/10" />
    )

    const renderSidebarBackground = () => (
        <div
            className="fixed inset-0 z-[1150] bg-black/60"
            onClick={onClose}
        />
    )

    const renderMainPart = () => (
        <nav className="flex-1 overflow-y-auto px-3 py-2">
            <div className="space-y-1">
                {renderMainNavItem(colony ? GameNavItem : HomeNavItem)}
                {colony && GameNavItemsList.map((item) => renderMainNavItem(item))}
                {renderDivider()}

                {renderMainNavItem(RatingNavItem)}
                {renderDivider()}
            </div>
        </nav>
    )

    const renderFooterPart = () => (
        <div className="border-t border-bright/10 px-3 py-3">
            {user && renderMainNavItem(LogOutNavItem)}
        </div>
    )

    return (
        <>
            {isDrawer && isOpen && renderSidebarBackground()}
            <aside className={
                    `py-[3px] w-64 bg-dark/95 backdrop-blur-sm border-r border-bright/20 flex flex-col
                    ${isDrawer 
                        ? `${isOpen ? 'translate-x-0' : '-translate-x-full'}` 
                        : ''}
                    ${className}`}
            >
                {colony && !isEventPage() && <div className="px-3 pt-2 pb-3 border-b border-bright/10">
                    <TurnButton />
                </div>}
                {renderMainPart()}
                {renderFooterPart()}
            </aside>
        </>
    );
};

export default Sidebar;