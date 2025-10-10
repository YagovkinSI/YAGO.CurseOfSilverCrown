import { Box, Paper, Typography, useMediaQuery, useTheme } from '@mui/material';
import React from 'react';
import type { StateItem } from '../entities/StateItem';
import { ArrowForwardIos } from '@mui/icons-material';

import './stateList.css'

interface StateListProps {
    items: StateItem[]
}

const StateList: React.FC<StateListProps> = ({ items }) => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));

    const handleItemClick = (stat: StateItem) => {
        if (stat.url) {
            window.location.href = stat.url;
        }
    };

    const renderLeftLine = (stat: StateItem) => (
        <Box
            className="state-item-left-line"
            style={{
                '--accent-color-start': `${stat.color}00`,
                '--accent-color': stat.color,
                '--accent-color-end': `${stat.color}00`,
            } as React.CSSProperties}
        />
    );

    const renderIcon = (stat: StateItem) => (
        <Box className="state-item-icon-container">
            <stat.icon
                className={'state-item-icon'}
                style={{ color: stat.color }}
            />
        </Box>
    );

    const renderLabel = (stat: StateItem) => (
        <Typography
            className={`state-item-label ${!isMobile ? 'state-item-label--desktop' : ''}`}
            style={{
                '--text-primary': theme.palette.text.primary,
                '--text-secondary': theme.palette.text.secondary,
            } as React.CSSProperties}
        >
            {stat.label}
        </Typography>
    );

    const renderStatValueWithArrow = (stat: StateItem) => (
        <Box className={`state-item-value-with-arrow ${!isMobile ? 'state-item-value-with-arrow--desktop' : ''}`}>
            <Box
                className={'state-item-value-container'}
                style={{
                    '--value-bg-start': `${stat.color}08`,
                    '--value-border-color': `${stat.color}15`,
                } as React.CSSProperties}
            >
                <Typography
                    className={`state-item-value ${!isMobile ? 'state-item-value--desktop' : ''}`}
                    style={{
                        color: stat.color,
                        '--value-glow': `${stat.color}40`,
                    } as React.CSSProperties}
                >
                    {stat.value}
                </Typography>
            </Box>
            {renderStatArrow(stat)}
        </Box>
    );

    const renderStatArrow = (stat: StateItem) => {
        if (!stat.url) return null;

        return (
            <Box
                className="state-item-arrow"
                style={{ '--accent-color': stat.color } as React.CSSProperties}
            >
                <ArrowForwardIos sx={{ fontSize: 16 }} />
            </Box>
        );
    };

    const renderStatContent = (stat: StateItem) => (
        <Box className="state-item-content">
            {renderIcon(stat)}
            {renderLabel(stat)}
        </Box>
    );

    const renderStat = (stat: StateItem) => (
        <Paper
            elevation={0}
            className={`state-item ${!isMobile ? 'state-item--desktop' : ''}`}
            style={{
                background: `linear-gradient(135deg, ${theme.palette.background.paper} 0%, ${theme.palette.background.default} 50%, ${theme.palette.background.paper} 100%)`,
                borderColor: theme.palette.divider,
                '--accent-color': `${stat.color}40`,
                '--accent-color-hover': `${stat.color}30`,
                '--accent-color-shadow': `${stat.color}15`,
            } as React.CSSProperties}
            onClick={() => handleItemClick(stat)}
        >
            {renderLeftLine(stat)}
            {renderStatContent(stat)}
            {renderStatValueWithArrow(stat)}
        </Paper>
    );

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