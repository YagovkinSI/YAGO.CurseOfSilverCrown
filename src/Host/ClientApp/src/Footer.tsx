import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { FooterNavItemsList, type NavItem, type NavItemType } from './shared/NavItem';

const Footer: React.FC = () => {
    const navigate = useNavigate();
    const location = useLocation();

    const navItemTypes : NavItemType[] = ['game', 'rating', 'wiki', 'more']
    const links = FooterNavItemsList.filter(x => navItemTypes.includes(x.id))
    const activeTab = links.slice(1, 4).findIndex(link => location.pathname === link.path || location.pathname.startsWith(link.path + '/'));
    const currentTab = activeTab !== -1 ? activeTab + 1 : 0;

    const handleChange = (_event: React.SyntheticEvent, newValue: number) => {
        navigate(links[newValue].path);
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
            {links.map((link, index) => renderNavItem(link, index))}
        </nav>
    );

    return (
        <footer className="fixed bottom-0 left-0 right-0 z-[1100] bg-[#0a0a1a] border-t-2 border-bright shadow-[0_-4px_10px_rgba(0,0,0,0.5)]">
            {renderNavigation()}
        </footer>
    );
};

export default Footer;