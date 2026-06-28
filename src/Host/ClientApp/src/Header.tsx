import React from 'react';
import { Box, Typography, Tooltip, CircularProgress } from '@mui/material';
import { ErrorOutline } from '@mui/icons-material';
import './Header.css';
import { useGetMyUserQuery } from './entities/MyUser';
import { useGetMyColonyQuery } from './entities/MyColony';
import { GetStateItems } from './features/GetColonyParameterList';
import LoginIconMenu from './features/LoginIconMenu';

// Тип для ячейки статистики
export interface HeaderStat {
    id: string;
    icon: React.ReactNode;
    value: string;
    iconColor?: string;
    valueColor?: string;
}

// Цвета по умолчанию
const defaultColors = {
    icon: '#6c757d',
    value: '#fafaf8',
};

const Header: React.FC = () => {
    const getMyUserResult = useGetMyUserQuery();
    const getMyColonyResult = useGetMyColonyQuery();

    const user = getMyUserResult.data?.data;
    const isAuthenticated = user != undefined;
    const colony = getMyColonyResult.data?.data;
    const colonyName = colony?.name ?? "YAGO World";
    const colonyParameters = colony?.colonyParameters?.filter(x => x.parrentType == undefined) ?? [];
    const stats = GetStateItems(colonyParameters);

    const isLoading = getMyUserResult.isLoading || getMyColonyResult.isLoading;
    const error = getMyUserResult.error ?? getMyColonyResult.error;

    const renderHeaderMainLeft = () => {
        return <Box className="header-left">
            <LoginIconMenu />
            <Typography
                className="header-title"
                variant="h6"
                sx={{
                    fontSize: '0.95rem',
                    fontWeight: 600,
                    color: '#fafaf8',
                    letterSpacing: '0.5px',
                    '@media (min-width: 768px)': {
                        fontSize: '1.1rem',
                    },
                    '@media (max-width: 480px)': {
                        fontSize: '0.85rem',
                    },
                }}
            >
                {isAuthenticated ? colonyName : 'YAGO World'}
            </Typography>
        </Box>
    }

    const renderHeaderMainRight = () => {
        return <Box className="header-right">
            {isLoading && (
                <CircularProgress
                    size={20}
                    sx={{
                        color: '#f0e65c',
                        marginRight: 1,
                    }}
                />
            )}
            {error && (
                <Tooltip title="Ошибка загрузки данных">
                    <ErrorOutline
                        sx={{
                            color: '#d32f2f',
                            fontSize: 20,
                            marginRight: 1,
                        }}
                    />
                </Tooltip>
            )}
        </Box>
    }

    const renderHeaderColonyParameters = () => {
        return <Box className="header-bottom">
            <Box className="header-stats-scroll">
                {stats.map((stat) => (
                    <Box key={stat.label} className="header-stat-item">
                        <Box
                            component="span"
                            className="header-stat-icon"
                            sx={{
                                color: stat.color || defaultColors.icon,
                            }}
                        >
                            <Box className="state-item-icon-container">
                                <stat.icon
                                    className={'state-item-icon'}
                                    style={{ color: stat.color }}
                                />
                            </Box>
                        </Box>
                        <Typography
                            component="span"
                            className="header-stat-value"
                            sx={{
                                color: defaultColors.value,
                            }}
                        >
                            {stat.value}
                        </Typography>
                    </Box>))}
            </Box>
        </Box>
    }

    return (
        <Box className="header-container" sx={{ position: 'fixed', top: 0, left: 0, right: 0, zIndex: 1100 }}>
            <Box className="header-top">
                {renderHeaderMainLeft()}
                {renderHeaderMainRight()}
            </Box>
            <Box className="header-divider" />
            {isAuthenticated && stats.length > 0
                && renderHeaderColonyParameters()}
        </Box>
    );
};

export default Header;