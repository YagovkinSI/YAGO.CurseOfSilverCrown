import * as React from 'react';
import { useNavigate } from 'react-router-dom';
import { User, LogOut, LogIn, Edit } from 'lucide-react';
import YagoAvatar from '../shared/Avatar';
import type YagoLink from '../entities/YagoLink';
import { useGetMyUserQuery, useLogoutMutation } from '../entities/MyUser';
import { IsDesktop } from './MediaHelper';

const userTemporaryProfileLinks: YagoLink[] = [
    { name: 'Изменить', path: '/registration' },
    { name: 'Выход', path: 'logout' },
];

const userProfileLinks: YagoLink[] = [
    { name: 'Выход', path: 'logout' },
];

const guestProfileLinks: YagoLink[] = [
    { name: 'Авторизация', path: '/registration' },
];

const LoginIconMenu: React.FC = () => {
    const isDesktop = IsDesktop();
    const getMyUserResult = useGetMyUserQuery();
    const [logout, { isLoading: isLoggingOut }] = useLogoutMutation();
    const [isMenuOpen, setIsMenuOpen] = React.useState(false);
    const navigate = useNavigate();

    const user = getMyUserResult?.data?.data;

    const handleOpenUserMenu = () => {
        setIsMenuOpen(true);
    };

    const handleCloseUserMenu = () => {
        setIsMenuOpen(false);
    };

    const handleLogout = async () => {
        try {
            await logout().unwrap();
            navigate('/');
        } catch (err) {
            console.error('Logout failed:', err);
        }
    };

    const onLinkClick = (path: string) => {
        if (path === 'logout')
            handleLogout();
        else
            navigate(path);
        handleCloseUserMenu();
    };

    const renderAvatar = () => {
        if (user != undefined) {
            return <YagoAvatar name={user.userName} />;
        }
        return (
            <div className="w-[30px] h-[30px] sm:w-[40px] sm:h-[40px] rounded-full bg-bright/10 border border-bright/20 flex items-center justify-center text-bright hover:bg-bright/20 transition-colors duration-200">
                <User className="w-4 h-4 sm:w-5 sm:h-5" />
            </div>
        );
    };

    const renderTooltip = () => (
        <button
            onClick={handleOpenUserMenu}
            className="p-0 focus:outline-none"
            title="Меню управления аккаунтом"
        >
            {renderAvatar()}
        </button>
    );

    const renderUserName = (userName: string) => (
        <>
            <div className="px-4 py-2 text-muted text-center text-sm font-medium cursor-default">
                {userName}
            </div>
            <div className="h-px bg-bright/10 mx-2" />
        </>
    );

    const getIcon = (linkName: string) => {
        if (linkName === 'Выход') return <LogOut className="w-4 h-4" />;
        if (linkName === 'Авторизация') return <LogIn className="w-4 h-4" />;
        if (linkName === 'Изменить') return <Edit className="w-4 h-4" />;
        return null;
    };

    const renderMenuItem = (link: YagoLink) => {
        const isLogout = link.name === 'Выход';
        return (
            <button
                key={link.name}
                disabled={isLogout && isLoggingOut}
                onClick={() => onLinkClick(link.path!)}
                className="w-full flex items-center gap-3 px-4 py-2.5 text-left text-light hover:bg-bright/10 transition-colors duration-150 text-sm"
            >
                {isLogout && isLoggingOut ? (
                    <div className="w-4 h-4 border-2 border-danger/20 border-t-danger rounded-full animate-spin" />
                ) : (
                    getIcon(link.name)
                )}
                <span>{link.name}</span>
            </button>
        );
    };

    const renderMenuLinks = () => {
        const userMenuLinks = user != undefined
            ? user.isTemporary
                ? userTemporaryProfileLinks
                : userProfileLinks
            : guestProfileLinks;

        return userMenuLinks.map((link) => renderMenuItem(link));
    };

    const renderOverlay = () => {
        return <div
            className="fixed inset-0 z-[1200]"
            onClick={handleCloseUserMenu}
        />
    }

    const renderMenu = () => {
        if (!isMenuOpen || isDesktop) return null;
        return (
            <>
                {renderOverlay()}
                <div className="
                    fixed top-[52px] left-3 z-[1201] min-w-[180px]
                    bg-[#0a0a1a] border border-bright/20 rounded-lg shadow-[0_4px_30px_rgba(0,0,0,0.7)]
                    overflow-hidden
                    md:top-[60px] md:left-4
                    max-[480px]:top-[44px] max-[480px]:left-2 max-[480px]:min-w-[160px]
                ">
                    <div className="py-1">
                        {user != undefined && renderUserName(user.userName)}
                        {renderMenuLinks()}
                    </div>
                </div>
            </>
        );
    };

    return (
        <div className="flex-grow-0 relative">
            {renderTooltip()}
            {renderMenu()}
        </div>
    );
};

export default LoginIconMenu;