import YagoCard from '../shared/YagoCard';
import ErrorField from '../shared/ErrorField';
import LoadingCard from '../shared/LoadingCard';
import { Box, Paper, Typography, useMediaQuery, useTheme } from '@mui/material';
import DefaultErrorCard from '../shared/DefaultErrorCard';
import { WorkspacePremium, AttachMoney, Grade, ViewModule, RocketLaunch, People } from '@mui/icons-material';
import type { MyState } from '../entities/MyState';
import React from 'react';

const StatePage: React.FC = () => {
    const myState: MyState = {
        id: 0,
        name: '-',
        iserId: 0,
        income: -10,
        solars: 10000,
        reputation: 0,
        population: 0,
        freeZones: 5,
        ship: 'Рассвет-782'
    };

    const isLoading = false;
    const error = undefined;

    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    const stats = [
        {
            icon: WorkspacePremium,
            label: 'Колония',
            value: `${myState.name}`,
            color: '#9C27B0',
        },
        {
            icon: Grade,
            label: 'Репутация',
            value: myState.reputation,
            color: '#4FC3F7',
        },
        {
            icon: AttachMoney,
            label: 'Солары',
            value: `${myState.solars} (${myState.income}/ч)`,
            color: '#FFD700',
        },
        {
            icon: RocketLaunch,
            label: 'Корабль',
            value: myState.ship,
            color: '#FF8A65'
        },
        {
            icon: ViewModule,
            label: 'Свободные зоны',
            value: myState.freeZones,
            color: '#757575'
        },
        {
            icon: People,
            label: 'Население',
            value: myState.population,
            color: '#81C784'
        }
    ];

    const renderLeftLine = (stat: typeof stats[0]) => {
        return (
            <Box
                sx={{
                    position: 'absolute',
                    left: 0,
                    top: 0,
                    bottom: 0,
                    width: 3,
                    background: `linear-gradient(180deg, 
                        ${stat.color}00 0%, 
                        ${stat.color} 50%, 
                        ${stat.color}00 100%)`,
                    opacity: 0.8
                }}
            />
        )
    }

    const renderIcon = (stat: typeof stats[0]) => {
        return (
            <Box
                sx={{
                    p: 0.8,
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                }}
            >
                <stat.icon sx={{ color: stat.color, fontSize: isMobile ? 20 : 24 }} />
            </Box>
        )
    }

    const renderLabel = (stat: typeof stats[0]) => {
        return (
            <Typography
                variant={isMobile ? "body2" : "body1"}
                fontWeight="600"
                color="text.primary"
                noWrap
                sx={{
                    background: `linear-gradient(135deg, ${theme.palette.text.primary} 0%, ${theme.palette.text.secondary} 100%)`,
                    backgroundClip: 'text',
                    WebkitBackgroundClip: 'text',
                    WebkitTextFillColor: 'transparent',
                    textTransform: 'uppercase',
                    fontSize: isMobile ? '0.75rem' : '0.85rem',
                    letterSpacing: '0.5px'
                }}
            >
                {stat.label}
            </Typography>
        )
    }

    const renderStatValue = (stat: typeof stats[0]) => {
        return (
            <Box 
                sx={{
                    background: `linear-gradient(90deg, ${stat.color}08 0%, transparent 100%)`,
                    px: 2,
                    py: 0.5,
                    borderRadius: '12px 0 0 12px',
                    border: `1px solid ${stat.color}15`,
                    borderRight: 'none',
                    minWidth: isMobile ? 120 : 150
                }}
            >
                <Typography
                    variant={isMobile ? "body1" : "h6"}
                    fontWeight="bold"
                    color={stat.color}
                    noWrap
                    sx={{
                        textAlign: 'right',
                        textShadow: `0 0 10px ${stat.color}40`,
                        fontSize: isMobile ? '0.9rem' : '1rem',
                        letterSpacing: '0.5px',
                    }}
                >
                    {stat.value}
                </Typography>
            </Box>
        )
    }

    const renderStat = (stat: typeof stats[0]) => {
        return (
            <Paper
                elevation={0}
                sx={{
                    p: isMobile ? 1 : 1.5,
                    borderRadius: 3,
                    background: `
                    linear-gradient(135deg, 
                        ${theme.palette.background.paper} 0%, 
                        ${theme.palette.background.default} 50%,
                        ${theme.palette.background.paper} 100%
                    )`,
                    border: `1px solid ${theme.palette.divider}`,
                    position: 'relative',
                    minWidth: isMobile ? 300 : 400,
                    maxWidth: isMobile ? 350 : 700,
                    height: 40,
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'space-between',
                    overflow: 'hidden',
                    '&::before': {
                        content: '""',
                        position: 'absolute',
                        top: 0,
                        left: 0,
                        right: 0,
                        height: '1px',
                        background: `linear-gradient(90deg, 
                        transparent 0%, 
                        ${stat.color}40 50%, 
                        transparent 100%)`,
                    },
                    '&::after': {
                        content: '""',
                        position: 'absolute',
                        bottom: 0,
                        left: 0,
                        right: 0,
                        height: '1px',
                        background: `linear-gradient(90deg, 
                        transparent 0%, 
                        ${stat.color}40 50%, 
                        transparent 100%)`,
                    },
                    '&:hover': {
                        border: `1px solid ${stat.color}30`,
                        boxShadow: `0 0 20px ${stat.color}15, 0 4px 12px rgba(0,0,0,0.1)`,
                        transform: 'translateY(-1px)',
                        transition: 'all 0.3s ease'
                    }
                }}
            >
                {renderLeftLine(stat)}
                <Box display="flex" alignItems="center" flex={1} minWidth={0} >
                    {renderIcon(stat)}
                    {renderLabel(stat)}
                </Box>
                {renderStatValue(stat)}
            </Paper>
        )
    }

    const renderContent = () => {
        return (
            <Box
                display="flex"
                flexDirection="column"
                gap={1}
                sx={{
                    width: '100%',
                    maxWidth: isMobile ? 350 : 700,
                    margin: '0 auto'
                }}
            >
                {stats.map((stat, index) => (
                    <React.Fragment key={index}>
                        {renderStat(stat)}
                    </React.Fragment>
                ))}
            </Box>
        )
    }

    const renderCard = () => {
        return (
            <YagoCard
                title={myState.name}
                image={`/assets/images/pictures/captain_hall.jpg`}
            >
                {renderContent()}
            </YagoCard>
        )
    }

    return (
        <>
            <ErrorField title='Ошибка' error={error} />
            {isLoading
                ? <LoadingCard />
                : error != undefined
                    ? <DefaultErrorCard />
                    : renderCard()}
        </>
    )
}

export default StatePage