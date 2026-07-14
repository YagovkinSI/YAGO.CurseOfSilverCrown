import React, { type CSSProperties } from 'react';

interface BaseContainerProps {
    children: React.ReactNode;
    className?: string;
    fullHeight?: boolean;
    fullWidth?: boolean;
    style?: CSSProperties;
}

export const BaseContainer: React.FC<BaseContainerProps> = ({
    children,
    className = '',
    fullHeight = true,
    fullWidth = true,
    style,
}) => {
    const heightClass = fullHeight ? 'min-h-full' : '';
    const widthClass = fullWidth ? 'w-full' : '';

    return (
        <div 
            className={`${heightClass} ${widthClass} ${className}`}
            style={style}
        >
            {children}
        </div>
    );
};