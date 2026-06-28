import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import {
    BottomNavigation,
    BottomNavigationAction,
    Paper,
} from '@mui/material';
import {
    Home as HomeIcon,
    EmojiEvents as TrophyIcon,
    MenuBook as BookIcon,
    MoreHoriz as MoreIcon,
} from '@mui/icons-material';
import './Footer.css';

interface YagoLink {
    name: string;
    path: string;
    icon: React.ReactNode;
}

const links: YagoLink[] = [
    { name: 'Главная', path: '/', icon: <HomeIcon /> },
    { name: 'Колония', path: '/me/colony', icon: <MoreIcon /> },
    { name: 'Рейтинг', path: '/colonyRaiting', icon: <TrophyIcon /> },
    { name: 'Wiki', path: '/wiki', icon: <BookIcon /> }
];

const Footer: React.FC = () => {
    const navigate = useNavigate();
    const location = useLocation();

    // Находим индекс активного пункта
    const activeTab = links.findIndex(link => link.path === location.pathname);
    const currentTab = activeTab !== -1 ? activeTab : 0;

    const handleChange = (_event: React.SyntheticEvent, newValue: number) => {
        navigate(links[newValue].path);
    };

    const renderBottomNavigation = () => {
        return <BottomNavigation
            value={currentTab}
            onChange={handleChange}
            showLabels
            className="footer-navigation"
            sx={{
                height: '56px',
                bgcolor: 'transparent',
                '@media (min-width: 768px)': {
                    height: '64px',
                },
            }}
        >
            {links.map((link, index) => (
                <BottomNavigationAction
                    key={index}
                    className="footer-action"
                    icon={link.icon}
                    label={link.name}
                />
            ))}
        </BottomNavigation>
    }

    return (
        <Paper
            className="footer-paper"
            elevation={8}
            sx={{
                position: 'fixed',
                bottom: 0,
                left: 0,
                right: 0,
                zIndex: 1100,
                bgcolor: '#0a0a1a',
                borderTop: '2px solid #f0e65c',
                boxShadow: '0 -4px 10px rgba(0,0,0,0.5)',
                borderRadius: 0,
            }}
        >
            {renderBottomNavigation()}
        </Paper>
    );
};

export default Footer;