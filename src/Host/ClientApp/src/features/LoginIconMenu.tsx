import * as React from 'react';
import { User } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import YagoAvatar from '../shared/YagoAvatar';
import type YagoLink from '../entities/YagoLink';
import { useGetMyUserQuery } from '../entities/MyUser';

const userTemporaryProfileLinks: YagoLink[] = [
    { name: 'Изменить', path: '/registration' },
    { name: 'Выход', path: '/logout' },
];

const userProfileLinks: YagoLink[] = [
    { name: 'Выход', path: '/logout' },
];

const guestProfileLinks: YagoLink[] = [
    { name: 'Авторизация', path: '/registration' },
];

const LoginIconMenu: React.FC = () => {
    const getMyUserResult = useGetMyUserQuery();
    const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(null);
    const navigate = useNavigate();

    const user = getMyUserResult?.data?.data;

    const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElUser(event.currentTarget);
    };

    const handleCloseUserMenu = () => {
        setAnchorElUser(null);
    };

    const onLinkClick = (path: string) => {
        navigate(path);
    };

    const renderAvatar = () => {
        if (user != undefined) {
            return <YagoAvatar name={user.userName} />;
        }
        return (
            <div className="w-[30px] h-[30px] sm:w-[40px] sm:h-[40px] rounded-full bg-dark/50 border border-muted/30 flex items-center justify-center">
                <User className="w-4 h-4 sm:w-5 sm:h-5 text-muted" />
            </div>
        );
    };

    const renderLoginMenuTooltip = () => (
        <button
            onClick={handleOpenUserMenu}
            className="p-0 rounded-full hover:opacity-80 transition-opacity focus:outline-none focus:ring-2 focus:ring-bright/50"
            aria-label="Меню управления аккаунтом"
        >
            {renderAvatar()}
        </button>
    );

    const renderUserName = (userName: string) => (
        <>
            <div className="px-4 py-2 text-center text-muted text-sm font-medium cursor-default">
                {userName}
            </div>
            <hr className="border-muted/20" />
        </>
    );

    const renderMenuLinks = () => {
        const userMenuLinks = user != undefined
            ? user.isTemporary
                ? userTemporaryProfileLinks
                : userProfileLinks
            : guestProfileLinks;
        return userMenuLinks.map((link) => (
            <button
                key={link.name}
                onClick={() => {
                    onLinkClick(link.path!);
                    handleCloseUserMenu();
                }}
                className="w-full px-4 py-2 text-left text-light/80 hover:text-bright hover:bg-bright/10 transition-colors text-sm"
            >
                {link.name}
            </button>
        ));
    };

    const renderMenu = () => {
        if (!anchorElUser) return null;
        return (
            <div
                className="fixed mt-2 right-4 min-w-[180px] bg-dark/95 backdrop-blur-sm border border-bright/10 rounded-lg shadow-2xl py-1 z-50"
                style={{
                    top: 'calc(100% + 8px)',
                    transformOrigin: 'top right',
                }}
            >
                {user != undefined && renderUserName(user.userName)}
                {renderMenuLinks()}
            </div>
        );
    };

    return (
        <div className="relative flex-grow-0">
            {renderLoginMenuTooltip()}
            {renderMenu()}
        </div>
    );
};

export default LoginIconMenu;