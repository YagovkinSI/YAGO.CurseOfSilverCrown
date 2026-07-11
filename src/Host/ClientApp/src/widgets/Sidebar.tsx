import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { useGetMyUserQuery, useLogoutMutation } from '../entities/MyUser';
import TurnButton from '../features/TurnButton';
import { GameNavItemsList, LogInNavItem, LogOutNavItem, type NavItem, HomeNavItem, RatingNavItem, WikiNavItem, SetNavItemData, GameNavItem } from '../features/NavigationHelper';
import { useGetMyColonyQuery } from '../entities/MyColony';

const Sidebar: React.FC = () => {
    const navigate = useNavigate();
    const location = useLocation();

    const getMyUserResult = useGetMyUserQuery();
    const getMyColonyResult = useGetMyColonyQuery();
    const [logout] = useLogoutMutation();

    const user = getMyUserResult.data?.data;
    const colony = getMyColonyResult.data?.data;

    const isActive = (path: string) => location.pathname === path || location.pathname.startsWith(path + '/');

    const handleLogout = async () => {
        await logout().unwrap();
        navigate('/');
    };

    const renderMainNavItem = (item: NavItem) => {
        item = SetNavItemData(item, colony)
        return <button
            key={item.id}
            disabled={!item.isActive}
            onClick={() => item.id == 'logout' ? handleLogout() : navigate(item.path)}
            className={`
                w-full flex items-center gap-3 px-3 py-2.5 rounded-lg
                transition-all duration-200
                ${!item.isActive
                    ? 'opacity-40 cursor-not-allowed text-muted/50 hover:bg-transparent'
                    : isActive(item.path)
                        ? 'bg-bright/10 text-bright border border-bright/30 hover:bg-bright/15'
                        : 'text-muted hover:text-light hover:bg-bright/5'
                }
            `}
        >
            {<item.icon className="w-5 h-5" />}
            <span className="text-sm font-medium">{item.label}</span>
            {item.badge && item.badge > 0 && (
                <span className="ml-auto w-2 h-2 bg-danger rounded-full animate-pulse" />
            )}
        </button>
    }

    const renderDivider = () => (
        <div className="my-4 border-t border-bright/10" />
    )

    return (
        <aside
            className="sticky left-0 top-0 py-[3px] z-[1000] w-64 bg-dark/95 backdrop-blur-sm border-r border-bright/20 flex flex-col"
        >
            {user && <div className="px-3 pt-2 pb-3 border-b border-bright/10">
                <TurnButton />
            </div>}

            {/* Основная часть */}
            <nav className="flex-1 overflow-y-auto px-3 py-2">
                <div className="space-y-1">
                    {renderMainNavItem(user ? GameNavItem : HomeNavItem)}
                    {!user && renderMainNavItem(LogInNavItem)}
                    {user && GameNavItemsList.map((item) => renderMainNavItem(item))}
                    {renderDivider()}

                    {renderMainNavItem(RatingNavItem)}
                    {renderMainNavItem(WikiNavItem)}
                    {renderDivider()}
                </div>
            </nav>

            {/* Нижняя часть */}
            <div className="border-t border-bright/10 px-3 py-3">
                {user && renderMainNavItem(LogOutNavItem)}
            </div>
        </aside>
    );
};

export default Sidebar;