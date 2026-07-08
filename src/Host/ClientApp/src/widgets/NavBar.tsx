import * as React from 'react';
import { Menu as MenuIcon } from 'lucide-react';
import { useNavigate } from 'react-router-dom';
import LoginIconMenu from '../features/LoginIconMenu';
import type YagoLink from '../entities/YagoLink';

const links: YagoLink[] = [
    { name: 'Главная', path: '/' },
    { name: 'Управление', path: '/me/colony' },
    { name: 'Колонии', path: '/colonyRaiting' },
    { name: 'Случайная статья', path: '/wiki' }
];

const NavBar: React.FC = () => {
    const [anchorElNav, setAnchorElNav] = React.useState<null | HTMLElement>(null);
    const navigate = useNavigate();

    const onLinkClick = (path: string) => {
        navigate(path);
        setAnchorElNav(null);
    };

    const renderMenuIcon = () => (
        <div className="flex sm:hidden">
            <button
                onClick={(event) => setAnchorElNav(event.currentTarget)}
                className="p-2 rounded-md hover:bg-bright/10 transition-colors text-light"
                aria-label="main menu"
            >
                <MenuIcon className="w-6 h-6" />
            </button>
            {anchorElNav && (
                <div className="absolute top-full left-0 right-0 mt-1 bg-dark/95 border border-bright/20 rounded-lg shadow-xl py-2 z-50">
                    {links.map((link: YagoLink) => (
                        <button
                            key={link.path}
                            onClick={() => onLinkClick(link.path!)}
                            className="w-full px-6 py-3 text-left text-light/80 hover:text-bright hover:bg-bright/5 transition-colors text-sm"
                        >
                            {link.name}
                        </button>
                    ))}
                </div>
            )}
        </div>
    );

    const renderLogo = () => (
        <div
            onClick={() => onLinkClick('/')}
            className="text-light font-mono font-bold tracking-[0.3rem] text-xl sm:text-lg cursor-pointer hover:text-bright transition-colors whitespace-nowrap"
        >
            YAGO World
        </div>
    );

    const renderLinks = () => (
        <div className="hidden sm:flex items-center gap-1">
            {links.map((link) => (
                <button
                    key={link.path}
                    onClick={() => onLinkClick(link.path!)}
                    className="px-4 py-2 text-light/80 hover:text-bright hover:bg-bright/10 rounded-md transition-colors text-sm font-medium"
                >
                    {link.name}
                </button>
            ))}
        </div>
    );

    return (
        <div className="flex items-center justify-between h-16 sm:h-[66px]">
            <div className="flex items-center gap-2">
                {renderMenuIcon()}
                {renderLogo()}
            </div>
            {renderLinks()}
            <LoginIconMenu />
        </div>
    );
};

export default NavBar;