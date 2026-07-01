import React from 'react';
import { Box, Typography, IconButton, Badge, Tooltip } from '@mui/material';
import './ActionButton.css';

export interface ActionButtonProps {
    icon: React.ReactNode;
    label: string;
    isActive?: boolean;
    hasNotification?: boolean;
    badgeContent?: number;
    timer?: string;
    onClick?: () => void;
    tooltip?: string;
    color?: string;
    size?: 'small' | 'medium' | 'large';
}

const ActionButton: React.FC<ActionButtonProps> = ({
    icon,
    label,
    isActive = true,
    hasNotification = false,
    badgeContent,
    timer,
    onClick,
    tooltip,
    color = '#f0e65c',
    size = 'medium',
}) => {
    // Определяем размеры
    const sizeMap = {
        small: { button: 48, icon: 24, font: '0.55rem' },
        medium: { button: 60, icon: 30, font: '0.6rem' },
        large: { button: 72, icon: 36, font: '0.65rem' },
    };

    const currentSize = sizeMap[size];

    const buttonContent = (
        <Box className="action-button-wrapper">
            <Badge
                color="error"
                variant="dot"
                invisible={!hasNotification && (!badgeContent || badgeContent === 0)}
                badgeContent={badgeContent}
                sx={{
                    '& .MuiBadge-badge': {
                        backgroundColor: '#d32f2f',
                        boxShadow: '0 0 8px rgba(211, 47, 47, 0.5)',
                        width: 10,
                        height: 10,
                        minWidth: 10,
                        borderRadius: '50%',
                        top: 2,
                        right: 2,
                    },
                    '& .MuiBadge-badge[data-badge-content]': {
                        fontSize: '0.6rem',
                        minWidth: 16,
                        height: 16,
                        borderRadius: 8,
                        padding: '0 4px',
                    },
                }}
            >
                <IconButton
                    className={`action-button ${!isActive ? 'action-button--inactive' : ''}`}
                    onClick={onClick}
                    disabled={!isActive}
                    sx={{
                        width: currentSize.button,
                        height: currentSize.button,
                        bgcolor: '#050515',
                        border: `2px solid ${isActive ? color : 'rgba(108, 117, 125, 0.3)'}`,
                        boxShadow: isActive ? `0 0 20px ${color}15` : 'none',
                        transition: 'all 0.3s ease',
                        position: 'relative',
                        '&::after': {
                            content: '""',
                            position: 'absolute',
                            inset: -4,
                            borderRadius: '50%',
                            border: `2px solid ${isActive ? color : 'transparent'}`,
                            opacity: 0,
                            transition: 'all 0.3s ease',
                        },
                        '&:hover': {
                            bgcolor: isActive ? 'rgba(240, 230, 92, 0.08)' : '#050515',
                            transform: isActive ? 'scale(1.05)' : 'none',
                            boxShadow: isActive ? `0 0 30px ${color}25` : 'none',
                            '&::after': {
                                opacity: isActive ? 0.3 : 0,
                            },
                        },
                        '&:active': {
                            transform: isActive ? 'scale(0.95)' : 'none',
                        },
                        '& .MuiSvgIcon-root': {
                            fontSize: currentSize.icon,
                            color: isActive ? color : '#6c757d',
                            filter: isActive ? 'none' : 'grayscale(1) opacity(0.4)',
                            transition: 'all 0.3s ease',
                        },
                    }}
                >
                    {icon}
                </IconButton>
            </Badge>
            {timer && (
                <Typography
                    className="action-button-timer"
                    sx={{
                        fontSize: '0.5rem',
                        color: '#6c757d',
                        textAlign: 'center',
                        mt: 0.5,
                        fontFamily: 'monospace',
                        letterSpacing: '0.5px',
                    }}
                >
                    {timer}
                </Typography>
            )}
            <Typography
                className="action-button-label"
                sx={{
                    fontSize: currentSize.font,
                    color: isActive ? '#fafaf8' : '#6c757d',
                    textAlign: 'center',
                    mt: 0.5,
                    fontWeight: isActive ? 500 : 400,
                    opacity: isActive ? 1 : 0.5,
                    letterSpacing: '0.3px',
                    textTransform: 'uppercase',
                    lineHeight: 1.2,
                    maxWidth: currentSize.button + 20,
                    overflow: 'hidden',
                    textOverflow: 'ellipsis',
                    whiteSpace: 'nowrap',
                }}
            >
                {label}
            </Typography>
        </Box>
    );

    if (tooltip) {
        return (
            <Tooltip title={tooltip} placement="top">
                <span>{buttonContent}</span>
            </Tooltip>
        );
    }

    return buttonContent;
};

export default ActionButton;