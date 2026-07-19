import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { GameNavItem, HomeNavItem, MoreNavItem, RatingNavItem, WikiNavItem, type NavItem } from '../features/NavigationHelper';
import { useGetMyUserQuery } from '../entities/users/MyUser';

interface FooterProps {
    className?: string;
}

const Footer: React.FC<FooterProps> = ({className}) => {
    const navigate = useNavigate();
    const location = useLocation();
    
    const getMyUserResult = useGetMyUserQuery();

    const user = getMyUserResult.data?.data;

    const navItems : NavItem[] = user
        ? [ HomeNavItem, RatingNavItem, WikiNavItem, MoreNavItem]
        : [ GameNavItem, RatingNavItem, WikiNavItem, MoreNavItem]
    const activeTab = navItems.slice(1, 4).findIndex(link => location.pathname === link.path || location.pathname.startsWith(link.path + '/'));
    const currentTab = activeTab !== -1 ? activeTab + 1 : 0;

    const handleChange = (_event: React.SyntheticEvent, newValue: number) => {
        navigate(navItems[newValue].path);
    };

    const renderNavItemContent = (link: NavItem, isActive: boolean) => {
        return <>
            <span className="mui-icon text-[26px] transition-all duration-200 md:text-[30px]">
                {<link.icon/>}
            </span>
            <span className={`
                    text-[0.65rem] font-medium tracking-wide uppercase transition-all duration-200
                    ${isActive ? 'font-semibold' : ''}
                    md:text-[0.75rem]
                `}>
                {link.label}
            </span>
        </>
    }

    const renderNavItem = (link: NavItem, index: number) => {
        const isActive = index === currentTab;
        return (
            <button
                key={index}
                onClick={() => handleChange({} as React.SyntheticEvent, index)}
                className={`
                    flex flex-col items-center justify-center gap-0.5 flex-1 py-1 px-2
                    transition-all duration-200
                    ${isActive
                        ? 'text-bright [&_.mui-icon]:text-bright [&_.mui-icon]:drop-shadow-[0_0_6px_rgba(240,230,92,0.3)]'
                        : 'text-muted hover:text-light'
                    }
                    md:py-1.5 md:px-3
                `}
            >
                {renderNavItemContent(link, isActive)}
            </button>
        );
    };

    const renderNavigation = () => (
        <nav className="flex items-center justify-around h-14 bg-[#0a0a1a] md:h-16">
            {navItems.map((link, index) => renderNavItem(link, index))}
        </nav>
    );

    return (
        <footer className={
            `bg-[#0a0a1a] border-t-2 border-bright shadow-[0_-4px_10px_rgba(0,0,0,0.5)]
            ${className}`}
        >
            {renderNavigation()}
        </footer>
    );
};

export default Footer;