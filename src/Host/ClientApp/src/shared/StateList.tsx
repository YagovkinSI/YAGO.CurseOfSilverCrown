import { Box, Paper, Typography, useMediaQuery, useTheme } from '@mui/material';
import React from 'react';
import type { StateItem } from '../entities/StateItem';

import './stateList.css'

interface StateListProps {
    items: StateItem[]
}

const StateList: React.FC<StateListProps> = ({ items }) => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    const renderLeftLine = (stat: StateItem) => {
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

    const renderIcon = (stat: StateItem) => {
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

    const renderLabel = (stat: StateItem) => {
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

    const renderStatValue = (stat: StateItem) => {
        return (
            <Box
                className={`state-item-value ${isMobile ? 'state-item-value--mobile' : 'state-item-value--desktop'}`}
                style={{
                    background: `linear-gradient(90deg, ${stat.color}08 0%, transparent 100%)`,
                    borderColor: `${stat.color}15`,
                } as React.CSSProperties}
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

    const renderStat = (stat: StateItem) => {
        return (
            <Paper
                elevation={0}
                className={`state-item ${isMobile ? 'state-item--mobile' : 'state-item--desktop'}`}
                style={{
                    '--accent-color': `${stat.color}40`,
                    '--accent-color-hover': `${stat.color}30`,
                    '--accent-color-shadow': `${stat.color}15`,
                } as React.CSSProperties}
                data-accent-color={stat.color}
            >
                {renderLeftLine(stat)}
                <Box className="state-item-content">
                    {renderIcon(stat)}
                    {renderLabel(stat)}
                </Box>
                {renderStatValue(stat)}
            </Paper>
        )
    }
    
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
            {items.map((stat, index) => (
                <React.Fragment key={index}>
                    {renderStat(stat)}
                </React.Fragment>
            ))}
        </Box>
    )
}

export default StateList