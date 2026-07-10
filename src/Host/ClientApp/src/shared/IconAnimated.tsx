import React from 'react';
import type { LucideIcon } from 'lucide-react';

export interface AnimatedIconProps {
    icon: LucideIcon;
    className?: string;
    color?: 'bright' | 'danger' | 'good' | 'info' | 'muted' | 'light';
    size?: 'sm' | 'md' | 'lg' | 'xl';
    pingOpacity?: number;  // 0-1, по умолчанию 0.2
}

const sizeMap = {
    sm: 'w-8 h-8',
    md: 'w-10 h-10',
    lg: 'w-12 h-12',
    xl: 'w-16 h-16',
};

const colorMap = {
    bright: 'text-bright',
    danger: 'text-danger',
    good: 'text-good',
    info: 'text-info',
    muted: 'text-muted',
    light: 'text-light',
};

const IconAnimated: React.FC<AnimatedIconProps> = ({
    icon: Icon,
    className = '',
    color = 'bright',
    size = 'lg',
    pingOpacity = 0.2,
}) => {
    const sizeClass = sizeMap[size];
    const colorClass = colorMap[color];

    return (
        <div className={`relative ${className}`}>
            {/* Ping-слой */}
            <div 
                className={`absolute inset-0 animate-ping ${sizeClass} ${colorClass}`}
                style={{ opacity: pingOpacity }}
            >
                <Icon className={`${sizeClass} ${colorClass}`} />
            </div>
            
            {/* Основная иконка */}
            <Icon 
                className={`
                    ${sizeClass} ${colorClass} relative}
                `}
            />
        </div>
    );
};

export default IconAnimated;