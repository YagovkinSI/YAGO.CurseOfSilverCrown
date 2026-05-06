import { Box, Paper, Typography, useMediaQuery, useTheme, type SvgIconTypeMap } from '@mui/material';
import React from 'react';
import { ArrowForwardIos } from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';
import type { OverridableComponent } from '@mui/material/OverridableComponent';

import './stateList.css'

export interface RowDataProps {
    color: string,
    icon: OverridableComponent<SvgIconTypeMap<Record<string, unknown>, "svg">> & { muiName: string; },
    label: string,
    value: string,
    url?: string | undefined
}

const RowData: React.FC<RowDataProps> = (props) => {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down('md'));
    const navigate = useNavigate();

    const handleItemClick = () => {
        if (props.url) {
            navigate(props.url);
        }
    };

    const renderLeftLine = () => (
        <Box
            className="state-item-left-line"
            style={{
                '--accent-color-start': `${props.color}00`,
                '--accent-color': props.color,
                '--accent-color-end': `${props.color}00`,
            } as React.CSSProperties}
        />
    );

    const renderIcon = () => (
        <Box className="state-item-icon-container">
            <props.icon
                className={'state-item-icon'}
                style={{ color: props.color }}
            />
        </Box>
    );

    const renderLabel = () => (
        <Typography
            className={`state-item-label ${!isMobile ? 'state-item-label--desktop' : ''}`}
            style={{
                '--text-primary': theme.palette.text.primary,
                '--text-secondary': theme.palette.text.secondary,
            } as React.CSSProperties}
        >
            {props.label}
        </Typography>
    );

    const renderStatValueWithArrow = () => (
        <Box className={`state-item-value-with-arrow ${!isMobile ? 'state-item-value-with-arrow--desktop' : ''}`}>
            <Box
                className={'state-item-value-container'}
                style={{
                    '--value-bg-start': `${props.color}08`,
                    '--value-border-color': `${props.color}15`,
                } as React.CSSProperties}
            >
                <Typography
                    className={`state-item-value ${!isMobile ? 'state-item-value--desktop' : ''}`}
                    style={{
                        color: props.color,
                        '--value-glow': `${props.color}40`,
                    } as React.CSSProperties}
                >
                    {props.value}
                </Typography>
            </Box>
            {renderStatArrow()}
        </Box>
    );

    const renderStatArrow = () => {
        if (!props.url) return null;

        return (
            <Box
                className="state-item-arrow"
                style={{ '--accent-color': props.color } as React.CSSProperties}
            >
                <ArrowForwardIos sx={{ fontSize: 16 }} />
            </Box>
        );
    };

    const renderStatContent = () => (
        <Box className="state-item-content">
            {renderIcon()}
            {renderLabel()}
        </Box>
    );

    return (
        <Paper
            elevation={0}
            className={`state-item ${!isMobile ? 'state-item--desktop' : ''}`}
            style={{
                background: `linear-gradient(135deg, ${theme.palette.background.paper} 0%, ${theme.palette.background.default} 50%, ${theme.palette.background.paper} 100%)`,
                borderColor: theme.palette.divider,
                '--accent-color': `${props.color}40`,
                '--accent-color-hover': `${props.color}30`,
                '--accent-color-shadow': `${props.color}15`,
            } as React.CSSProperties}
            onClick={() => handleItemClick()}
        >
            {renderLeftLine()}
            {renderStatContent()}
            {renderStatValueWithArrow()}
        </Paper>
    )
}

export default RowData