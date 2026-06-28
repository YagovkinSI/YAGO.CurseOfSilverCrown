import React from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import {
    BottomNavigation,
    BottomNavigationAction,
    Badge,
    Paper,
    Box,
} from '@mui/material';
import {
    Home as HomeIcon,
    EmojiEvents as TrophyIcon,
    MenuBook as BookIcon,
    MoreHoriz as MoreIcon,
} from '@mui/icons-material';
import type YagoLink from './entities/YagoLink';
import './footer.css'

const links: YagoLink[] =
    [
        { name: 'Главная', path: '/', icon: <HomeIcon /> },
        { name: 'Управление', path: '/me/colony', icon: <MoreIcon /> },
        { name: 'Колонии', path: '/colonyRaiting', icon: <TrophyIcon /> },
        { name: 'Случайная статья', path: '/wiki', icon: <BookIcon /> }
    ];

interface FooterProps {
    notifications?: {
        main?: number;
        rating?: number;
        encyclopedia?: number;
        more?: number;
    };
}

const Footer: React.FC<FooterProps> = ({ notifications = {} }) => {
    const navigate = useNavigate();
    const location = useLocation();

    const activeLink = links.find(x => x.path == location.pathname);
    const activeTab = activeLink != undefined ? links.indexOf(activeLink) : 0;

    const handleChange = (_event: React.SyntheticEvent, newValue: number) => {
        const path = links[newValue].path;
        if (path != undefined)
            navigate(path);
    };

    const renderAction = (icon : React.ReactNode, name: string, notificationCount: number) => {
        return <BottomNavigationAction
            icon={
                <Badge
                    color="error"
                    variant="dot"
                    invisible={notificationCount === 0}
                    sx={{
                        '& .MuiBadge-dot': {
                            top: 4,
                            right: 4,
                            width: 8,
                            height: 8,
                            boxShadow: '0 0 6px rgba(244, 67, 54, 0.5)',
                        },
                    }}
                >
                    {icon}
                </Badge>
            }
            label={name}
        />
    }

    const renderBottomNavigation = () => {
        return <BottomNavigation
            value={activeTab}
            onChange={handleChange}
            showLabels
            sx={{
                height: '56px',
                bgcolor: 'transparent',
                '@media (min-width: 768px)': {
                    height: '64px',
                },
                '& .MuiBottomNavigationAction-root': {
                    color: '#6c757d',
                    padding: '6px 12px 4px',
                    minWidth: 'auto',
                    '@media (min-width: 768px)': {
                        padding: '8px 16px 6px',
                    },
                    '& .MuiBottomNavigationAction-label': {
                        fontSize: '0.65rem',
                        fontWeight: 500,
                        letterSpacing: '0.3px',
                        '@media (min-width: 768px)': {
                            fontSize: '0.75rem',
                        },
                    },
                    '&.Mui-selected': {
                        color: '#f0e65c',
                        '& .MuiBottomNavigationAction-label': {
                            fontSize: '0.65rem',
                            fontWeight: 600,
                            '@media (min-width: 768px)': {
                                fontSize: '0.75rem',
                            },
                        },
                        '& .MuiSvgIcon-root': {
                            filter: 'drop-shadow(0 0 6px rgba(240, 230, 92, 0.3))',
                        },
                    },
                    '& .MuiSvgIcon-root': {
                        fontSize: '26px',
                        '@media (min-width: 768px)': {
                            fontSize: '30px',
                        },
                        transition: 'all 0.2s ease',
                    },
                },
            }}
        >
            {renderAction(<HomeIcon />, 'Главная', notifications.main ?? 0)}
            {renderAction(<MoreIcon />, 'Колония', notifications.more ?? 0)}
            {renderAction(<TrophyIcon />, 'Рейтинг', notifications.rating ?? 0)}
            {renderAction(<BookIcon />, 'Wiki', notifications.encyclopedia ?? 0)}
        </BottomNavigation>
    }

    return (
        <Box sx={{ pb: '56px' }}>
      <Paper
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
                '@media (min-width: 768px)': {
                    height: '64px',
                },
            }}
        >
            {renderBottomNavigation()}
        </Paper>
    </Box>
        
    );
};

export default Footer;